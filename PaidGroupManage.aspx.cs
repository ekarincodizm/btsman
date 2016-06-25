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

    public partial class PaidGroupManage : System.Web.UI.Page
    {

        public string actPage = "list";
        public StringBuilder outBuf = new StringBuilder();
        public StringBuilder outBuf2 = new StringBuilder();
        public String targetID;
        public String target2ID;
        public PaidGroup thePaidGroup;
        public PaidGroup[] groupList;
        public Teacher[] teacherList;
        public Teacher[] memberList;

        public string errorText = "";
        public CultureInfo ci = new CultureInfo("en-US");
        public Logger log = Logger.GetLogger(Config.MAINLOG);

        protected void Page_Load(object sender, EventArgs e)
        {
            // Authentication
            string redirect = VerifyAA.Verify(Session
                , Request
                , Response
                , "NoRight.aspx");

            actPage = Request["actPage"];

            // log
            log.StampLine(Logger.DETAILED, "Request [" + Request["ASP.NET_SessionId"] + "][" + Request.RawUrl + "][actPage=" + actPage + "&targetID=" + targetID + "]");
            log.StampLine(Logger.DEBUG, "Param [" + Request["ASP.NET_SessionId"] + "][" + Request.Params.ToString() + "]");


            if ((actPage == null) || (actPage.Trim().Length == 0) || (actPage.Equals("list")))
            {
                DoListPaidGroup(Request["qsearch"]);
            }
            else if (actPage.Equals("add_submit"))
            {
                DoAddSubmitPaidGroup();
                Response.Redirect("PaidGroupManage.aspx");
            }
            else if (actPage.Equals("add"))
            {
                DoAddPaidGroup();
            }
            else if (actPage.Equals("edit"))
            {
                targetID = Request["targetID"];
                DoEditPaidGroup(targetID);
            }
            else if (actPage.Equals("edit_submit"))
            {
                targetID = Request["targetID"];
                DoEditSubmitPaidGroup(targetID);
                Response.Redirect("PaidGroupManage.aspx");
            }
            else if (actPage.Equals("add_teacher_submit"))
            {
                targetID = Request["targetID"];
                DoAddTeacherSubmit(targetID, Request["teacher_id"]);
                DoEditPaidGroup(targetID);
                actPage = "edit";
            }
            else if (actPage.Equals("remove_teacher_submit"))
            {
                targetID = Request["targetID"];
                target2ID = Request["target2ID"];
                DoRemoveTeacherSubmit(targetID, target2ID);
                DoEditPaidGroup(targetID);
                actPage = "edit";
            }
            else if (actPage.Equals("delete"))
            {
                targetID = Request["targetID"];
                DoDeletePaidGroup(targetID);
                Response.Redirect("PaidGroupManage.aspx");
            }
        }

        protected string GetQSearchSQL(string qsearch)
        {
            if (qsearch == null) return "";
            return String.Format(" WHERE paid_group_id LIKE '%{0}%' or name LIKE '%{0}%' ", qsearch);
        }

        protected void DoListPaidGroup(string searchStr)
        {
            // get Page
            int pg = 1;
            if (Request["pg"] != null) pg = Int32.Parse(Request["pg"]);

            string[,] bgclass = new string[,] { { "class=\"spec\"", "class=\"td1\"" }, { "class=\"specalt\"", "class=\"alt\"" } };

            DBManager db = new MySQLDBManager(Config.DB_SERVER, Config.DB_NAME, Config.DB_USER, Config.DB_PASSWORD);
            int numRec = db.QueryCount("SELECT Count(*) FROM paid_group " + GetQSearchSQL(searchStr));

            OdbcDataReader reader = db.Query("SELECT * FROM paid_group " + GetQSearchSQL(searchStr) + " LIMIT " + Config.TBRECORD_PER_PAGE + " OFFSET " + (((pg - 1) * Config.TBRECORD_PER_PAGE)));
            int i = 0;
            while (reader.Read())
            {
                PaidGroup group = BTS.Entity.PaidGroup.CreateForm(reader);

                // teacher member list
                Teacher[] memberList = group.LoadMemberTeachers(db);
                StringBuilder memberTxt = new StringBuilder();
                for (int j = 0; j < memberList.Length; j++)
                {
                    String txt = Teacher.GetTeacherID(memberList[j]._teacherID) + " " + memberList[j]._firstname + " " + memberList[j]._surname;
                    memberTxt.Append( "<a href=\"TeacherManage.aspx?actPage=view&targetID="+memberList[j]._teacherID+"\">"+ txt + "<br>");
                }

                outBuf.Append("<tr>");
                outBuf.Append("<th scope=\"row\" abbr=\"Model\" " + bgclass[i % 2, 0] + ">" + PaidGroup.GetPaidGroupID(group._paidGroupID) + "</th>");
                outBuf.Append("<td " + bgclass[i % 2, 1] + "  >" + group._name + "&nbsp</td>");
                outBuf.Append("<td " + bgclass[i % 2, 1] + "  >" + group._rawRateInfo + "&nbsp</td>");
                outBuf.Append("<td " + bgclass[i % 2, 1] + " align=left >" + memberTxt + "&nbsp</td>");
                outBuf.Append("<td " + bgclass[i % 2, 1] + "  align=center>&nbsp");
                outBuf.Append("<a href=\"javascript:setVal('actPage','edit');setVal('targetID','" + group._paidGroupID + "');doSubmit()\"><img src=\"img/sys/edit.gif\" border=0 alt=\"Edit\"></a>&nbsp");
                outBuf.Append("<a href=\"javascript:if (confirm('Delete this user?')) { setVal('actPage','delete');setVal('targetID','" + group._paidGroupID + "');doSubmit(); }\"><img src=\"img/sys/delete.gif\" border=0 alt=\"Delete\"></a>&nbsp");

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
                    outBuf2.Append(String.Format("<a href=\"PaidGroupManage.aspx?pg={0}\">{1}</a> ", i.ToString(), i.ToString()));
                }

            }

            //  <a href="#">1</a> <b>2</b> <a href="#">3</a> <a href="#">4</a>


        }

        protected void DoAddSubmitPaidGroup()
        {
            PaidGroup group = new PaidGroup();

            group._paidGroupID = Int32.Parse(Request["groupID"]);  
            group._name = Request["name"];
            StringBuilder sb = new StringBuilder();
            sb.Append(Request["bound1"]); sb.Append(":"); sb.Append(Request["rate1"]); sb.Append(";");
            sb.Append(Request["bound2"]); sb.Append(":"); sb.Append(Request["rate2"]); sb.Append(";");
            sb.Append(Request["bound3"]); sb.Append(":"); sb.Append(Request["rate3"]); sb.Append(";");
            sb.Append(Request["bound4"]); sb.Append(":"); sb.Append(Request["rate4"]); sb.Append(";");
            sb.Append(Request["bound5"]); sb.Append(":"); sb.Append(Request["rate5"]); sb.Append(";");
            sb.Append(Request["bound6"]); sb.Append(":"); sb.Append(Request["rate6"]); sb.Append(";");
            sb.Append(Request["bound7"]); sb.Append(":"); sb.Append(Request["rate7"]); sb.Append(";");
            sb.Append(Request["bound8"]); sb.Append(":"); sb.Append(Request["rate8"]); sb.Append(";");
            sb.Append(Request["bound9"]); sb.Append(":"); sb.Append(Request["rate9"]); sb.Append(";");
            sb.Append(Request["bound10"]); sb.Append(":"); sb.Append(Request["rate10"]);

            group._rawRateInfo = group.BuildRateInfoString(sb.ToString());
            group._rateInfo = PaidRateInfo.Parse(group._rawRateInfo);

            // Save to DB
            DBManager db = new MySQLDBManager(Config.DB_SERVER, Config.DB_NAME, Config.DB_USER, Config.DB_PASSWORD);
            db.Connect();
            group.AddToDB(db);
            db.Close();
        }

        public void DoAddPaidGroup()
        {
            DBManager db = new MySQLDBManager(Config.DB_SERVER, Config.DB_NAME, Config.DB_USER, Config.DB_PASSWORD);
            groupList = PaidGroup.LoadListFromDB(db, "");
            //teacherList = Teacher.LoadListFromDB(db, " WHERE is_active=1 ORDER BY firstname");
            db.Close();
        }
        public void DoEditPaidGroup(string groupID)
        {
            DBManager db = new MySQLDBManager(Config.DB_SERVER, Config.DB_NAME, Config.DB_USER, Config.DB_PASSWORD);
            thePaidGroup = new PaidGroup();
            if (!thePaidGroup.LoadFromDB(db, "paid_group_id='" + groupID + "'")) thePaidGroup = null;
            
            teacherList = thePaidGroup.LoadNonMemberTeachers(db);
            memberList = thePaidGroup.LoadMemberTeachers(db);

            db.Close();
        }

        protected void DoEditSubmitPaidGroup(string groupID)
        {
            PaidGroup group = new PaidGroup();

            // validate data
            group._paidGroupID = Int32.Parse(groupID);
            group._name = Request["name"];
            StringBuilder sb = new StringBuilder();
            sb.Append(Request["bound1"]); sb.Append(":"); sb.Append(Request["rate1"]); sb.Append(";");
            sb.Append(Request["bound2"]); sb.Append(":"); sb.Append(Request["rate2"]); sb.Append(";");
            sb.Append(Request["bound3"]); sb.Append(":"); sb.Append(Request["rate3"]); sb.Append(";");
            sb.Append(Request["bound4"]); sb.Append(":"); sb.Append(Request["rate4"]); sb.Append(";");
            sb.Append(Request["bound5"]); sb.Append(":"); sb.Append(Request["rate5"]); sb.Append(";");
            sb.Append(Request["bound6"]); sb.Append(":"); sb.Append(Request["rate6"]); sb.Append(";");
            sb.Append(Request["bound7"]); sb.Append(":"); sb.Append(Request["rate7"]); sb.Append(";");
            sb.Append(Request["bound8"]); sb.Append(":"); sb.Append(Request["rate8"]); sb.Append(";");
            sb.Append(Request["bound9"]); sb.Append(":"); sb.Append(Request["rate9"]); sb.Append(";");
            sb.Append(Request["bound10"]); sb.Append(":"); sb.Append(Request["rate10"]);

            group._rawRateInfo = group.BuildRateInfoString(sb.ToString());
            group._rateInfo = PaidRateInfo.Parse(group._rawRateInfo);

            // Save to DB
            DBManager db = new MySQLDBManager(Config.DB_SERVER, Config.DB_NAME, Config.DB_USER, Config.DB_PASSWORD);
            db.Connect();
            group.UpdateToDB(db);
            db.Close();
        }


        protected void DoAddTeacherSubmit(String paidGroupID, String teacherID)
        {
            PaidGroup group = new PaidGroup();
            group._paidGroupID = Int32.Parse(paidGroupID);
            // Save to DB
            DBManager db = new MySQLDBManager(Config.DB_SERVER, Config.DB_NAME, Config.DB_USER, Config.DB_PASSWORD);
            db.Connect();
            group.AddTeacherToDB(db, teacherID);
            db.Close();
        }

        protected void DoRemoveTeacherSubmit(String paidGroupID, String teacherID)
        {
            PaidGroup group = new PaidGroup();
            group._paidGroupID = Int32.Parse(paidGroupID);
            // Save to DB
            DBManager db = new MySQLDBManager(Config.DB_SERVER, Config.DB_NAME, Config.DB_USER, Config.DB_PASSWORD);
            db.Connect();
            group.RemoveTeacherToDB(db, teacherID);
            db.Close();
        }



        protected void DoDeletePaidGroup(string groupID)
        {
            PaidGroup group = new PaidGroup();
            group._paidGroupID = Int32.Parse(groupID);

            DBManager db = new MySQLDBManager(Config.DB_SERVER, Config.DB_NAME, Config.DB_USER, Config.DB_PASSWORD);
            db.Connect();
            group.DeleteToDB(db);
            db.Close();
        }
        protected string GetRoleName(int role_id)
        {
            if (role_id <= 0) return "";
            string sql = "SELECT name FROM role WHERE role_id='" + role_id + "'";
            string ret = String.Empty;
            DBManager db = new MySQLDBManager(Config.DB_SERVER, Config.DB_NAME, Config.DB_USER, Config.DB_PASSWORD);
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
            DBManager db = new MySQLDBManager(Config.DB_SERVER, Config.DB_NAME, Config.DB_USER, Config.DB_PASSWORD);
            db.Connect();
            OdbcDataReader reader = db.Query(sql);
            reader.Read();
            ret = reader.GetString(0);
            db.Close();
            return ret;
        }
    }
}
