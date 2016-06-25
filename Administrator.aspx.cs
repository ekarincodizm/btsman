using System;
using System.Data;
using System.Data.Odbc;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using BTS.DB;
using BTS.Entity;
using BTS.Util;

namespace BTS.Page
{

    public partial class Administrator : System.Web.UI.Page
    {

        public string actPage = "list";
        public StringBuilder outBuf = new StringBuilder();
        public StringBuilder outBuf2 = new StringBuilder();
        public String targetID;
        public AppUser theUser;
        public List<AppUser> listUser;
        public Role[] roleList;
        public Branch[] branchList;

        public CultureInfo ci = new CultureInfo("en-US");

        protected void Page_Load(object sender, EventArgs e)
        {
            // Authentication
            string redirect = VerifyAA.Verify(Session
                , Request
                , Response
                , "NoRight.aspx");

            actPage = Request["actPage"];

            if ((actPage == null) || (actPage.Trim().Length == 0) || (actPage.Equals("list")))
            {
                DoListUser(Request["qsearch"]);
            }
            else if (actPage.Equals("add_submit"))
            {
                DoAddSubmitUser();
                Response.Redirect("Administrator.aspx");
            }
            else if (actPage.Equals("add"))
            {
                DoAddUser();
            }
            else if (actPage.Equals("edit"))
            {
                targetID = Request["targetID"];
                DoEditUser(targetID);
            }
            else if (actPage.Equals("edit_submit"))
            {
                targetID = Request["targetID"];
                DoEditSubmitUser(targetID);
                Response.Redirect("Administrator.aspx");
            }
            else if (actPage.Equals("delete"))
            {
                targetID = Request["targetID"];
                DoDeleteUser(targetID);
                Response.Redirect("Administrator.aspx");
            }
        }

        protected string GetQSearchSQL(string qsearch)
        {
            if (qsearch == null) return "";
            return String.Format(" WHERE username LIKE '%{0}%' or firstname LIKE '%{0}%' or surname LIKE '%{0}%' ", qsearch);
        }

        protected void DoListUser(string searchStr)
        {
            // get Page
            int pg = 1;
            if (Request["pg"] != null) pg = Int32.Parse(Request["pg"]);

            string[,] bgclass = new string[,] { { "class=\"spec\"", "class=\"td1\"" }, { "class=\"specalt\"", "class=\"alt\"" } };

            listUser = new List<AppUser>();
            DBManager db = new MySQLDBManager(Config.DB_SERVER, Config.DB_NAME, Config.DB_USER, Config.DB_PASSWORD, Config.DB_CHAR_ENC);
            int numRec = db.QueryCount("SELECT Count(*) FROM user " + GetQSearchSQL(searchStr));

            OdbcDataReader reader = db.Query("SELECT * FROM user " + GetQSearchSQL(searchStr) + " LIMIT " + Config.TBRECORD_PER_PAGE + " OFFSET " + (((pg - 1) * Config.TBRECORD_PER_PAGE)));
            int i = 0;
            while (reader.Read())
            {
                AppUser user = BTS.Entity.AppUser.CreateForm(reader);
                // byte[] raw = Encoding.Default.GetBytes(user._firstname);
                // user._firstname = Encoding.GetEncoding("tis-620").GetString(raw);

                outBuf.Append("<tr>");
                outBuf.Append("<th scope=\"row\" abbr=\"Model\" " + bgclass[i % 2, 0] + ">" + user._userId + "</th>");
                outBuf.Append("<td " + bgclass[i % 2, 1] + " align=left ><a href=\"#\" >" + user._username + "</a>&nbsp</td>");
                outBuf.Append("<td " + bgclass[i % 2, 1] + " align=left ><a href=\"#\" >" + user._firstname + " " + user._surname + "</a>&nbsp</td>");
                outBuf.Append("<td " + bgclass[i % 2, 1] + "  >" + GetRoleName(user._roleId) + "&nbsp</td>");
                outBuf.Append("<td " + bgclass[i % 2, 1] + "  >" + GetBranchName(user._branchID) + "&nbsp</td>");
                outBuf.Append("<td " + bgclass[i % 2, 1] + "  align=center>&nbsp");
                outBuf.Append("<a href=\"javascript:setVal('actPage','edit');setVal('targetID','" + user._username + "');doSubmit()\"><img src=\"img/sys/edit.gif\" border=0 alt=\"Edit\"></a>&nbsp");
                outBuf.Append("<a href=\"javascript:if (confirm('Delete this user?')) { setVal('actPage','delete');setVal('targetID','" + user._username + "');doSubmit(); }\"><img src=\"img/sys/delete.gif\" border=0 alt=\"Delete\"></a>&nbsp");

                outBuf.Append("</td>");
                outBuf.Append("</tr>\n");

                i++;
            }
            db.Close();

            // calculate max page            
            int maxpg = numRec / Config.TBRECORD_PER_PAGE;
            if (maxpg < 1) { maxpg = 1; }
            else if (maxpg % Config.TBRECORD_PER_PAGE > 0) { maxpg++; }
            // Generate Page Navi HTML
            outBuf2.Append("<b>Page</b>  ");
            for (i = 1; i <= maxpg; i++)
            {
                if (i == pg) { outBuf2.Append("<b>" + i + "</b> "); }
                else
                {
                    outBuf2.Append(String.Format("<a href=\"Administrator.aspx?pg={0}\">{1}</a> ", i.ToString(), i.ToString()));
                }

            }

            //  <a href="#">1</a> <b>2</b> <a href="#">3</a> <a href="#">4</a>


        }

        protected void DoAddSubmitUser()
        {
            AppUser u = new AppUser();

            // validate data
            u._username = Request["username"];
            u._passwd = Request["passwd"];
            u._firstname = Request["firstname"];
            u._surname = Request["surname"];
            u._roleId = Int32.Parse(Request["role_id"]);
            u._branchID = Int32.Parse(Request["branch_id"]);

            // Save to DB
            DBManager db = new MySQLDBManager(Config.DB_SERVER, Config.DB_NAME, Config.DB_USER, Config.DB_PASSWORD, Config.DB_CHAR_ENC);
            db.Connect();
            try
            {
                u.AddToDB(db);
            }
            catch (Exception e)
            {
                // show message?
            }
            db.Close();
        }

        public void DoAddUser()
        {
            DBManager db = new MySQLDBManager(Config.DB_SERVER, Config.DB_NAME, Config.DB_USER, Config.DB_PASSWORD, Config.DB_CHAR_ENC);
            roleList = Role.LoadListFromDB(db, "");
            branchList = Branch.LoadListFromDB(db, "");
            db.Close();
        }
        public void DoEditUser(string username)
        {
            DBManager db = new MySQLDBManager(Config.DB_SERVER, Config.DB_NAME, Config.DB_USER, Config.DB_PASSWORD, Config.DB_CHAR_ENC);
            theUser = new AppUser();
            if (!theUser.LoadFromDB(db, "username='" + username+ "'")) theUser = null;

            roleList = Role.LoadListFromDB(db, "");
            branchList = Branch.LoadListFromDB(db, "");

            db.Close();
        }

        protected void DoEditSubmitUser(string username)
        {
            AppUser u = new AppUser();

            // validate data
            u._username = username;
            //FIX
            if (!String.IsNullOrEmpty(Request["passwd"]))  // Check validate???
                u._passwd = AppUser.GetMD5Encoded(Request["passwd"]);
            u._firstname = Request["firstname"];
            u._surname = Request["surname"];
            u._roleId = Int32.Parse(Request["role_id"]);
            u._branchID = Int32.Parse(Request["branch_id"]);

            // Save to DB
            DBManager db = new MySQLDBManager(Config.DB_SERVER, Config.DB_NAME, Config.DB_USER, Config.DB_PASSWORD, Config.DB_CHAR_ENC);
            db.Connect();
            u.UpdateToDB(db);
            db.Close();
        }

        protected void DoDeleteUser(string username)
        {
            AppUser u = new AppUser();
            u._username = username;

            DBManager db = new MySQLDBManager(Config.DB_SERVER, Config.DB_NAME, Config.DB_USER, Config.DB_PASSWORD, Config.DB_CHAR_ENC);
            db.Connect();
            u.DeleteToDB(db);
            db.Close();
        }
        protected string GetRoleName(int role_id)
        {
            if (role_id <= 0) return "";
            string sql = "SELECT name FROM role WHERE role_id='" + role_id + "'";
            string ret = String.Empty;
            DBManager db = new MySQLDBManager(Config.DB_SERVER, Config.DB_NAME, Config.DB_USER, Config.DB_PASSWORD, Config.DB_CHAR_ENC);
            db.Connect();
            OdbcDataReader reader = db.Query(sql);
            reader.Read();
            ret = reader.GetString(0);
            db.Close();
            return ret;
        }

        protected string GetBranchName(int branch_id)
        {
            if (branch_id <= 0) return "";
            string sql = "SELECT branch_name FROM branch WHERE branch_id='" + branch_id + "'";
            string ret = String.Empty;
            DBManager db = new MySQLDBManager(Config.DB_SERVER, Config.DB_NAME, Config.DB_USER, Config.DB_PASSWORD, Config.DB_CHAR_ENC);
            db.Connect();
            OdbcDataReader reader = db.Query(sql);
            reader.Read();
            ret = reader.GetString(0);
            db.Close();
            return ret;
        }
    }
}
