using System;
using System.Text;
using System.Data;
using System.Data.Odbc;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using BTS.Constant;
using BTS.DB;
using BTS.Entity;
using BTS.Util;
using System.Collections.Generic;

namespace BTS.Page
{

    public partial class AppLogin : System.Web.UI.Page
    {
        public StringBuilder outBuf;
        public StringBuilder outBufTop;
        public Logger log = Logger.GetLogger(Config.MAINLOG);

        protected void Page_Load(object sender, EventArgs e)
        {
            string message = Request["message"];
            if (!String.IsNullOrEmpty(message))
            {
                outBufTop = new StringBuilder("<center><h1><font color=red>" + message + "</font></h1></center>");
            }


                AppUser loginUser;
                string action = Request.Params["action"];

                if (action == null)
                {
                    // new login
                    string username = Request.Params["username"];
                    string passwd = Request.Params["pwd"];

                    // log
                    log.StampLine(Logger.INFO, "Authen [user=" + username + "]");

                    loginUser = ValidateLogin(username, passwd);
                    if (loginUser != null)
                    {
                        // keep user info to Session
                        Session["USER"] = loginUser;
                        // preload all branches into Session
                        Session["BRANCHES"] = LoadAllBranches();

                        log.StampLine(Logger.INFO, "Login [user=" + username + "]");
                        Response.Redirect("Home.aspx");
                    }
                    // tried login but failed
                    if (username != null)
                    {
                        outBuf = new StringBuilder("<font color=red>Incorrect username or password</font>");
                    }
                }
                else
                {
                    // existing login
                    if (action.Equals("change_branch"))
                    {
                        string branch_id = Request.Params["branch_id"];
                        string return_page = Request.Params["return_page"];

                        loginUser = (AppUser)Session["USER"];
                        Branch[] branches = (Branch[])Session["BRANCHES"];
                        if (branches!=null)
                        foreach (Branch b in branches)
                        {
                            if (b._branchID.ToString().Equals(branch_id))
                            {
                                loginUser._branchName = b._branchName;
                                loginUser._branchID = b._branchID;
                                break;
                            }
                        }
                        // save back to Session
                        Session[SessionVar.USER] = loginUser;
                        Session[SessionVar.BRANCH_SELECTED] = branch_id;
                        Response.Redirect(return_page);
                    }

                }

        }

        public static List<String> GetSystemDriverList()
        {
            List<string> names = new List<string>();
            // get system dsn's
            Microsoft.Win32.RegistryKey reg = (Microsoft.Win32.Registry.LocalMachine).OpenSubKey("Software");
            if (reg != null)
            {
                reg = reg.OpenSubKey("ODBC");
                if (reg != null)
                {
                    reg = reg.OpenSubKey("ODBCINST.INI");
                    if (reg != null)
                    {

                        reg = reg.OpenSubKey("ODBC Drivers");
                        if (reg != null)
                        {
                            // Get all DSN entries defined in DSN_LOC_IN_REGISTRY.
                            foreach (string sName in reg.GetValueNames())
                            {
                                names.Add(sName);
                            }
                        }
                        try
                        {
                            reg.Close();
                        }
                        catch { /* ignore this exception if we couldn't close */ }
                    }
                }
            }

            return names;
        }
        protected AppUser ValidateLogin(string username, string passwd)
        {
            List<String> list = GetSystemDriverList();
            DBManager db = new MySQLDBManager(Config.DB_SERVER, Config.DB_NAME, Config.DB_USER, Config.DB_PASSWORD, Config.DB_CHAR_ENC);
            OdbcDataReader reader = null;
            try
            {
                reader = db.Query("SELECT u.*, b.branch_name as branch_name FROM user u, branch b  WHERE u.username='" + username + "' AND u.branch_id=b.branch_id");
                while (reader.Read())
                {
                    AppUser user = AppUser.CreateForm(reader);
                    if (user._encodedPassword == null) return null;
                    if (user._encodedPassword.Equals(AppUser.GetMD5Encoded(passwd) ))
                        return user;
                }
                return null;
            }
            catch (Exception e) { Console.WriteLine(e.StackTrace); return null; }
            finally { db.Close(reader); }                

        }

        protected Branch[] LoadAllBranches()
        {
            DBManager db = new MySQLDBManager(Config.DB_SERVER, Config.DB_NAME, Config.DB_USER, Config.DB_PASSWORD, Config.DB_CHAR_ENC);
            try
            {                
                Branch[] allBranches = Branch.LoadListFromDB(db, "");
                return allBranches;
            }
            catch (Exception e) { return null; }
            finally { db.Close(); }

        }

    }
}