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


    public partial class BranchManage : System.Web.UI.Page
    {
        public string actPage = "list";
        public StringBuilder outBuf = new StringBuilder();
        public StringBuilder outBuf2 = new StringBuilder();
        public String targetID = string.Empty;
        public Branch theBranch;
        public List<Branch> listBranch;

        public string errorText = "";
        public CultureInfo ci = new CultureInfo("en-US");
        public Logger log = Logger.GetLogger(Config.MAINLOG);
        //    protected System.Web.UI.HtmlControls.HtmlInputFile portrait;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Authentication
            string redirect = VerifyAA.Verify(Session
                , Request
                , Response
                , "NoRight.aspx");

            // Collect paramters
            actPage = Request.Form.Get("actPage");
            if (actPage == null) actPage = Request["actPage"];
            targetID = Request.Form.Get("targetID");
            if (targetID == null) targetID = Request["targetID"];

            // log
            log.StampLine(Logger.DETAILED, "Request [" + Request["ASP.NET_SessionId"] + "][" + Request.RawUrl + "][actPage=" + actPage + "&targetID=" + targetID + "]");
            log.StampLine(Logger.DEBUG, "Param [" + Request["ASP.NET_SessionId"] + "][" + Request.Params.ToString() + "]");

                if ((actPage == null) || (actPage.Trim().Length==0) || (actPage.Equals("list")))
                {
                    string qSearch = Request.Form.Get("qsearch");
                    bool isNewSearch = false;
                    if (qSearch != null)
                    {
                        isNewSearch = true;
                    }
                    else
                    {
                        qSearch = Request["qsearch"];
                    }
                    DoListBranch(qSearch, isNewSearch);
                }
                else if (actPage.Equals("add_submit"))
                {
                    DoAddSubmitBranch();
                    Response.Redirect("BranchManage.aspx");
                }
                else if (actPage.Equals("edit"))
                {
                    targetID = Request["targetID"];
                    DoEditBranch(targetID);
                }
                else if (actPage.Equals("edit_submit"))
                {
                    targetID = Request["targetID"];
                    DoEditSubmitBranch(targetID);
                    Response.Redirect("BranchManage.aspx");
                }
                else if (actPage.Equals("delete"))
                {
                    targetID = Request["targetID"];
                    DoDeleteBranch(targetID);
                    Response.Redirect("BranchManage.aspx");
                }
        }

        protected void listUser_Load(object sender, EventArgs e)
        {
            if ((actPage != null) && actPage.Equals("edit"))
            {
                List<string> list = GetListUser(targetID);
                for (int i = 0; i < list.Count(); i++)
                    listbox1.Items.Add(list.ElementAt(i));
            }
        }

        protected void DoListBranch(string searchStr, bool isNewSearch)
        {
            // get Page
            int pg = 1;
            if ((!isNewSearch) && (Request["pg"] != null)) pg = Int32.Parse(Request["pg"]);

            string[,] bgclass = new string[,] { { "class=\"spec\"", "class=\"td1\"" }, { "class=\"specalt\"", "class=\"alt\"" } };

            listBranch = new List<Branch>();
            DBManager db = new MySQLDBManager(Config.DB_SERVER, Config.DB_NAME, Config.DB_USER, Config.DB_PASSWORD);
            string qSearchSQL = Branch.GetQSearchSQL(searchStr);
            if (qSearchSQL.Trim().Length > 0) qSearchSQL = " WHERE " + qSearchSQL;

            int numRec = db.QueryCount("SELECT Count(*) FROM branch " + qSearchSQL);

            OdbcDataReader reader = db.Query("SELECT * FROM branch " + qSearchSQL + " LIMIT " + Config.TBRECORD_PER_PAGE + " OFFSET " + (((pg - 1) * Config.TBRECORD_PER_PAGE)));
            int i = 0;
            while (reader.Read())
            {
                Branch branch = Branch.CreateForm(reader);

                string divtxt = Config.URL_PIC_BRANCH + "/" + branch._img;

                outBuf.Append("<tr>");
                outBuf.Append("<th scope=\"row\" abbr=\"Model\" " + bgclass[i % 2, 0] + " align=center valign=top width=100px>" + Branch.GetBranchID(branch._branchID) + " &nbsp</th>");
                outBuf.Append("<td " + bgclass[i % 2, 1] + "  align=center>"+branch._branchCode+"</td>");
                outBuf.Append("<td " + bgclass[i % 2, 1] + "  align=left><p><b>สาขา: </b>" + branch._branchName + "</p>");                
                outBuf.Append("<p><b>ที่ตั้ง: </b>" + branch._address.Replace("\r\n", "<br>") + "&nbsp</p>");
                outBuf.Append("<p><b>เบอร์ติดต่อ: </b>" + branch._tel + "&nbsp</p>");
                outBuf.Append("<p><b>ชื่อผู้ดูแล: </b>" + branch._supervisor + "&nbsp</p>");
                
                outBuf.Append("&nbsp</td>");
                outBuf.Append("<td " + bgclass[i % 2, 1] + "  align=center><a href=\"" + Config.URL_PIC_BRANCH + "/" + branch._img + "\" ><img border=0 width=200px height=150px src=\"" + Config.URL_PIC_BRANCH + "/" + branch._img + "\" ></a></td>");

                outBuf.Append("<td " + bgclass[i % 2, 1] + "  align=center>&nbsp");
                outBuf.Append("<a href=\"javascript:setVal('actPage','edit');setVal('targetID','" + branch._branchID + "');doSubmit()\"><img src=\"img/sys/edit.gif\" border=0 alt=\"Edit\"></a>&nbsp");
                outBuf.Append("<a href=\"javascript:if (confirm('Delete this branch?')) { setVal('actPage','delete');setVal('targetID','" + branch._branchID + "');doSubmit(); }\"><img src=\"img/sys/delete.gif\" border=0 alt=\"Delete\"></a>&nbsp");

                outBuf.Append("</td>");
                outBuf.Append("</tr>\n");

                i++;
            }
            db.Close();

            // calculate max page            
            int maxpg = numRec / Config.TBRECORD_PER_PAGE;
            if (maxpg < 1) { maxpg = 1; }
            else if (numRec % Config.TBRECORD_PER_PAGE > 0) { maxpg++; }
            // Generate Page Navi HTML
            outBuf2.Append("<b>Page</b>  ");
            for (i = 1; i <= maxpg; i++)
            {
                if (i == pg) { outBuf2.Append("<b>" + i + "</b> "); }
                else
                {
                    outBuf2.Append(String.Format("<a href=\"BranchManage.aspx?pg={0}&qsearch={1}\">{0}</a> ", i.ToString(), searchStr));
                }

            }

            //  <a href="#">1</a> <b>2</b> <a href="#">3</a> <a href="#">4</a>


        }       
 
        protected void DoAddSubmitBranch()
        {
            Branch b = new Branch();
            
            // validate data
            b._branchName = Request["branch_name"];
            b._branchCode = Request["branch_code"];
            b._address = Request["address"];
            b._tel = Request["tel"];
            b._supervisor = Request["supervisor"];

            b._img = "noimg.jpg";
            
            if (portrait.PostedFile.FileName != "")
            {
                try
                {
                    string serverFileExt = Path.GetExtension(portrait.PostedFile.FileName);
                    Random rand = new Random((int)DateTime.Now.Ticks);
                    string fullpath = "";
                    string imgname = "";
                    do
                    {
                        string randomFName = rand.Next(Int32.MaxValue).ToString();
                        imgname = randomFName + serverFileExt;
                        fullpath = Config.PATH_APP_ROOT + "\\" + Config.URL_PIC_BRANCH + "\\" + imgname;
                    } while (File.Exists(fullpath));

                    portrait.PostedFile.SaveAs(fullpath);
                    b._img = imgname;
                }
                catch (Exception err)
                {
                    errorText = err.Message + err.StackTrace;
                }
            }

            // Save to DB
            DBManager db = new MySQLDBManager(Config.DB_SERVER, Config.DB_NAME, Config.DB_USER, Config.DB_PASSWORD);
            db.Connect();
            b.AddToDB(db);
            db.Close();
        }

        public void DoEditBranch(string branchID)
        {
            DBManager db = new MySQLDBManager(Config.DB_SERVER, Config.DB_NAME, Config.DB_USER, Config.DB_PASSWORD);
            theBranch = new Branch();
            if (!theBranch.LoadFromDB(db, "branch_id=" + branchID)) theBranch = null;

        }

        protected void DoEditSubmitBranch(string branchID)
        {
            Branch b = new Branch();

            // validate data
            b._branchID = Int32.Parse(branchID);
            b._branchName = Request["branch_name"];
            b._branchCode = Request["branch_code"];
            b._address = Request["address"];
            b._tel = Request["tel"];
            b._supervisor = Request["supervisor"];
            
            // default to old value
            b._img = Request["img_old"];
            if (portrait.PostedFile.FileName != "")
            {
                try
                {
                    string serverFileExt = Path.GetExtension(portrait.PostedFile.FileName);
                    Random rand = new Random((int)DateTime.Now.Ticks);
                    string fullpath = "";
                    string imgname = "";
                    do
                    {
                        string randomFName = rand.Next(Int32.MaxValue).ToString();
                        imgname = randomFName + serverFileExt;
                        fullpath = Config.PATH_APP_ROOT + "\\" + Config.URL_PIC_BRANCH + "\\" + imgname;
                    } while (File.Exists(fullpath));

                    portrait.PostedFile.SaveAs(fullpath);
                    b._img = imgname;
                }
                catch (Exception err)
                {
                    errorText = err.Message + err.StackTrace;
                }
            }

            // Save to DB
            DBManager db = new MySQLDBManager(Config.DB_SERVER, Config.DB_NAME, Config.DB_USER, Config.DB_PASSWORD);
            db.Connect();
            b.UpdateToDB(db);
            db.Close();
        }

        protected void DoDeleteBranch(string branchID)
        {
            Branch t = new Branch();
            t._branchID = Int32.Parse(branchID);

            DBManager db = new MySQLDBManager(Config.DB_SERVER, Config.DB_NAME, Config.DB_USER, Config.DB_PASSWORD);
            db.Connect();
            t.DeleteToDB(db);
            db.Close();
        }

        protected List<string> GetListUser(string branch_id)
        {
            List<string> list = new List<String>();
            if (string.IsNullOrEmpty(branch_id)) return list;
            string sql = "SELECT username,firstname,surname FROM user WHERE branch_id='" + branch_id + "'";
            string ret = String.Empty;
            DBManager db = new MySQLDBManager(Config.DB_SERVER, Config.DB_NAME, Config.DB_USER, Config.DB_PASSWORD);
            db.Connect();
            OdbcDataReader reader = db.Query(sql);

            int fCount = reader.FieldCount;
            while (reader.Read())
            {
                string tmp = "[" + reader.GetString(0).ToUpper() + "] " + reader.GetString(1) + " " + reader.GetString(2);
                list.Add(tmp);
            }

            db.Close();
            return list;
        }
    }
}
