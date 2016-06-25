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


    public partial class TeacherManage : System.Web.UI.Page
    {
        public string actPage = "list";
        public StringBuilder outBuf = new StringBuilder();
        public StringBuilder outBuf2 = new StringBuilder();
        public StringBuilder outBuf3 = new StringBuilder();
        public String targetID;
        public Teacher theTeacher;
        public List<Teacher> listTeacher;
        public PaidGroup[] groupList;

        public string errorText = "";
//        public CultureInfo ci = new CultureInfo("en-US");
        public CultureInfo ci = new CultureInfo("th-TH");
        public Logger log = Logger.GetLogger(Config.MAINLOG);
    //    protected System.Web.UI.HtmlControls.HtmlInputFile portrait;
  

        protected void Page_Load(object sender, EventArgs e)
        {
            // Authentication Start
            string redirect = VerifyAA.Verify(Session
                , Request
                , Response
                , "NoRight.aspx");

            //if (!String.IsNullOrEmpty(redirect))
            //      Response.Redirect(redirect);
            // Authentication End

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
                    DoListTeacher(qSearch, isNewSearch);
                }
                else if (actPage.Equals("view"))
                {
                    targetID = Request["targetID"];
                    DoViewTeacher(targetID);
                }
                else if (actPage.Equals("add"))
                {
                    DoAddTeacher();
                }
                else if (actPage.Equals("add_submit"))
                {
                    DoAddSubmitTeacher();
                    Response.Redirect("TeacherManage.aspx");
                }
                else if (actPage.Equals("edit"))
                {
                    targetID = Request["targetID"];
                    DoEditTeacher(targetID);
                }
                else if (actPage.Equals("edit_submit"))
                {
                    targetID = Request["targetID"];
                    DoEditSubmitTeacher(targetID);
                    Response.Redirect("TeacherManage.aspx");
                }
                else if (actPage.Equals("delete"))
                {
                    targetID = Request["targetID"];
                    DoDeleteTeacher(targetID);
                    Response.Redirect("TeacherManage.aspx");
                }
        }

        protected void DoViewTeacher(string teacherID)
        {
            outBuf = new StringBuilder();

            DBManager db = new MySQLDBManager(Config.DB_SERVER, Config.DB_NAME, Config.DB_USER, Config.DB_PASSWORD);
            theTeacher = new Teacher();
            theTeacher.LoadFromDB(db, " teacher_id=" + teacherID);
            PaidGroup[] memberInGroup = theTeacher.LoadAssosicatedPaidGroup(db);
            // Query payment
           // Payment[] pays1 = Payment.LoadListFromDBbyTeacherID(db, teacherID);
           // Payment[] pays2 = Payment.LoadListFromDBbyTeacherIDInPaidGroup(db, teacherID);
            db.Close();


            // List out all paid group that the teacher is member
            StringBuilder memberGroupTxt = new StringBuilder();
            for (int i = 0; i < memberInGroup.Length; i++)
            {
                memberGroupTxt.Append("&nbsp&nbsp"+PaidGroup.GetPaidGroupID(memberInGroup[i]._paidGroupID)+ " " + memberInGroup[i]._name + "<br>");
            }

            /*
            // Only teach by himself
            int sumAllCost = 0;
            int sumMaxPayable = 0;
            int sumPaidCost = 0;
            StringBuilder pay1Txt = new StringBuilder();
            for (int i = 0; i < pays1.Length; i++)
            {
                sumAllCost += pays1[i]._sumAllCost;
                sumMaxPayable += pays1[i]._sumMaxPayable;
                sumPaidCost += pays1[i]._sumPaidCost;
                string courseTxt = "<a href=\"CourseManage.aspx?actPage=view&targetID=" + pays1[i]._courseID + "\" >" + pays1[i]._btsCourseID + " " + pays1[i]._courseName + "</a>";

                pay1Txt.AppendLine("<tr>");
                pay1Txt.AppendLine("<td align=left>&nbsp" + courseTxt + "</td>");
                pay1Txt.AppendLine("<td align=center>&nbsp" + pays1[i]._courseStartDate.ToString("dd/MM/yyyy", ci) + " - " + pays1[i]._courseEndDate.ToString("dd/MM/yyyy", ci) + "</td>");
                pay1Txt.AppendLine("<td align=center>&nbsp" + StringUtil.Int2StrComma(pays1[i]._sumAllCost) + "</td>");
                pay1Txt.AppendLine("<td align=center>&nbsp" + StringUtil.Int2StrComma(pays1[i]._sumMaxPayable) + "</td>");
                pay1Txt.AppendLine("<td align=center>&nbsp" + StringUtil.Int2StrComma(pays1[i]._sumPaidCost) + "</td>");
                pay1Txt.AppendLine("</tr>");
            }
            // Summary
            pay1Txt.AppendLine("<tr>");
            pay1Txt.AppendLine("<td align=center colspan=2 >&nbsp<b>รวม   </b></td>");
            pay1Txt.AppendLine("<td align=center><b>&nbsp" + StringUtil.Int2StrComma(sumAllCost) + "</b></td>");
            pay1Txt.AppendLine("<td align=center><b>&nbsp" + StringUtil.Int2StrComma(sumMaxPayable) + "</b></td>");
            pay1Txt.AppendLine("<td align=center><b>&nbsp" + StringUtil.Int2StrComma(sumPaidCost) + "</b></td>");
            pay1Txt.AppendLine("</tr>");

            // All in paidgroup
            int sumAllCost2 = 0;
            int sumMaxPayable2 = 0;
            int sumPaidCost2 = 0;
            StringBuilder pay2Txt = new StringBuilder();
            for (int i = 0; i < pays2.Length; i++)
            {
                sumAllCost2 += pays2[i]._sumAllCost;
                sumMaxPayable2 += pays2[i]._sumMaxPayable;
                sumPaidCost2 += pays2[i]._sumPaidCost;
                string courseTxt = "<a href=\"CourseManage.aspx?actPage=view&targetID=" + pays2[i]._courseID + "\" >" + pays2[i]._btsCourseID + " " + pays2[i]._courseName + "</a>";

                pay2Txt.AppendLine("<tr>");
                pay2Txt.AppendLine("<td align=left>&nbsp" + courseTxt + "</td>");
                pay2Txt.AppendLine("<td align=center>&nbsp" + pays2[i]._courseStartDate.ToString("dd/MM/yyyy", ci) + " - " + pays2[i]._courseEndDate.ToString("dd/MM/yyyy", ci) + "</td>");
                pay2Txt.AppendLine("<td align=center>&nbsp" + StringUtil.Int2StrComma(pays2[i]._sumAllCost) + "</td>");
                pay2Txt.AppendLine("<td align=center>&nbsp" + StringUtil.Int2StrComma(pays2[i]._sumMaxPayable) + "</td>");
                pay2Txt.AppendLine("<td align=center>&nbsp" + StringUtil.Int2StrComma(pays2[i]._sumPaidCost) + "</td>");
                pay2Txt.AppendLine("</tr>");
            }
            // Summary
            pay2Txt.AppendLine("<tr>");
            pay2Txt.AppendLine("<td align=center colspan=2 >&nbsp<b>รวม   </b></td>");
            pay2Txt.AppendLine("<td align=center><b>&nbsp" + StringUtil.Int2StrComma(sumAllCost2) + "</b></td>");
            pay2Txt.AppendLine("<td align=center><b>&nbsp" + StringUtil.Int2StrComma(sumMaxPayable2) + "</b></td>");
            pay2Txt.AppendLine("<td align=center><b>&nbsp" + StringUtil.Int2StrComma(sumPaidCost2) + "</b></td>");
            pay2Txt.AppendLine("</tr>");
            */
            /*
            StringBuilder rateTxt = new StringBuilder();
            int currentRate = theTeacher._paidGroup.GetCurrentRate(sumAllCost2);
            PaidRateInfo[] rate = theTeacher._paidGroup._rateInfo;
            for (int i = 0; i < rate.Length; i++)
            {
                string bgcolor = "";
                if (currentRate == rate[i]._percent)
                {
                    bgcolor = " bgcolor=\"#FFB148\" ";
                }

                rateTxt.AppendLine("<tr>");
                rateTxt.AppendLine("<td align=center " + bgcolor + ">&nbsp" + StringUtil.Int2StrComma(rate[i]._bound) + " </td>");
                rateTxt.AppendLine("<td align=center " + bgcolor + ">&nbsp" + rate[i]._percent + "% </td>");
                rateTxt.AppendLine("</tr>");
            }
            */

            TextReader reader = new StreamReader(Config.PATH_APP_ROOT + "\\template\\teacher_view.htm");
            String templateContent = reader.ReadToEnd();
            reader.Close();

            String htmlContent =
                String.Format(templateContent
                    , Config.URL_PIC_TEACHER + "/" + theTeacher._img
                    , Teacher.GetTeacherID(theTeacher._teacherID)
                    , theTeacher._firstname + " " + theTeacher._surname
                    , theTeacher._subject
                    , Config.URL_PIC_SYS + (theTeacher._sex.Equals("Male") ? "/boy.gif" : "/girl.gif")
                    , theTeacher._birthday.ToString("dd/MM/yyyy", ci)
                    , theTeacher._citizenID
                    , theTeacher._addr
                    , theTeacher._tel
                    , theTeacher._email
                    , memberGroupTxt.ToString()
                    , ""// pay1Txt.ToString()
                    , ""//pay2Txt.ToString()
                    );


            outBuf.Append(htmlContent);
        }

        protected void DoListTeacher(string searchStr, bool isNewSearch)
        {
            // get Page
            int pg = 1;
            if ((!isNewSearch) && (Request["pg"] != null)) pg = Int32.Parse(Request["pg"]);

            string[,] bgclass = new string[,] { { "class=\"spec\"", "class=\"td1\"" }, { "class=\"specalt\"", "class=\"alt\"" } };

            listTeacher = new List<Teacher>();
            DBManager db = new MySQLDBManager(Config.DB_SERVER, Config.DB_NAME, Config.DB_USER, Config.DB_PASSWORD);
            string qSearchSQL = Teacher.GetQSearchSQL(searchStr);
            if (qSearchSQL.Trim().Length > 0)
            {
                qSearchSQL = " WHERE " + qSearchSQL;
            }
            else // search all
            {
                qSearchSQL = " WHERE is_active=1 ";
            }

            int numRec = db.QueryCount("SELECT Count(*) FROM teacher " + qSearchSQL);
            OdbcDataReader reader = db.Query("SELECT * FROM teacher " + qSearchSQL + " order by teacher_id desc LIMIT " + Config.TBRECORD_PER_PAGE + " OFFSET " + (((pg - 1) * Config.TBRECORD_PER_PAGE)));
            int i = 0;
            while (reader.Read())
            {
                Teacher teacher = Teacher.CreateForm(reader);
                // byte[] raw = Encoding.Default.GetBytes(teacher._firstname);
                // teacher._firstname = Encoding.GetEncoding("tis-620").GetString(raw);

                string divtxt = Config.URL_PIC_TEACHER + "/" + teacher._img;

                outBuf.Append("<tr>");
                outBuf.Append("<th scope=\"row\" abbr=\"Model\" " + bgclass[i % 2, 0] + ">" + Teacher.GetTeacherID(teacher._teacherID) + "</th>");
                outBuf.Append("<td " + bgclass[i % 2, 1] + "  ><a href=\"TeacherManage.aspx?actPage=view&targetID=" + teacher._teacherID + "\" onmouseover=\"showDiv('" + divtxt + "')\" onmouseout=\"hideDiv()\" >" + teacher._firstname + " " + teacher._surname + "</a>&nbsp</td>");
                outBuf.Append("<td " + bgclass[i % 2, 1] + " align=center > <img width=20 height=24 src=\"" + Config.URL_PIC_SYS + (teacher._sex.Equals("Male")?"/boy.gif":"/girl.gif") + "\" >"  );
                /*
                for (int j = 0; j < ssList[i].skill.Count; j++)
                {
                    outBuf.Append("<LI><a href=\"SkillHandling.aspx#sdid" + ssList[i].skill[j].id + "\" onmouseover=\"showChecklist(" + ssList[i].skill[j].id + ")\" onmouseout=\"hideCheckList()\" >" + ssList[i].skill[j].name + "</a>");
                }*/
                outBuf.Append("&nbsp</td>");
                outBuf.Append("<td " + bgclass[i % 2, 1] + "  >" + teacher._addr + "&nbsp</td>");
                outBuf.Append("<td " + bgclass[i % 2, 1] + "  align=center>"
                        + "<table border=0 align=left ><tr><td align=right><b>โทรศัพท์: </b></td><td align=left>" + teacher._tel + "</td></tr>"
                        + "<tr><td align=right><b>อีเมล: </b></td><td align=left>"+teacher._email+"</td></tr></table>");
                //outBuf.Append("<td " + bgclass[i % 2, 1] + "  >" + "GETINCENTIVE" + "&nbsp</td>");
                outBuf.Append("<td " + bgclass[i % 2, 1] + "  align=center>&nbsp");
                outBuf.Append("<a href=\"javascript:setVal('actPage','edit');setVal('targetID','" + teacher._teacherID + "');doSubmit()\"><img src=\"img/sys/edit.gif\" border=0 alt=\"Edit\"></a>&nbsp");
                outBuf.Append("<a href=\"javascript:if (confirm('Delete this teacher?')) { setVal('actPage','delete');setVal('targetID','" + teacher._teacherID + "');doSubmit(); }\"><img src=\"img/sys/delete.gif\" border=0 alt=\"Delete\"></a>&nbsp");
 
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
            outBuf2.Append(String.Format("<a href=\"TeacherManage.aspx?pg={0}&qsearch={1}\">{2}</a><< ", "1", searchStr, "First"));
            for (i = pg - 5; i <= pg + 5; i++)
            {
                if ((i <= 0) || (i > maxpg))
                {
                    continue;
                }
                if (i == pg) { outBuf2.Append("<b>"+i+"</b> "); }
                else {
                    outBuf2.Append(String.Format("<a href=\"TeacherManage.aspx?pg={0}&qsearch={1}\">{0}</a> ", i.ToString(), searchStr));
                }
            }
            outBuf2.Append(String.Format(" >><a href=\"TeacherManage.aspx?pg={0}&qsearch={1}\">{2}</a> ", maxpg.ToString(), searchStr, "Last"));            
          //  <a href="#">1</a> <b>2</b> <a href="#">3</a> <a href="#">4</a>


        }

        public void DoAddTeacher()
        {
            DBManager db = new MySQLDBManager(Config.DB_SERVER, Config.DB_NAME, Config.DB_USER, Config.DB_PASSWORD);
            groupList = PaidGroup.LoadListFromDB(db, "");
            db.Close();
        }

        protected void DoAddSubmitTeacher()
        {
            Teacher t = new Teacher();
            
            // validate data
            t._firstname = Request["firstname"];
            t._surname = Request["surname"];
            t._citizenID = Request["citizen_id"];
            t._sex = Request["sex"];
            t._tel = Request["tel"];
            t._email = Request["email"];
            t._addr = Request["addr"];
            //t._subject = Request["subject"];

            if (Request["birthday"] != null)
            {
                string[] s = Request["birthday"].Split('/');
                
                t._birthday = new DateTime(Int32.Parse(s[2]) - 543, Int32.Parse(s[1]), Int32.Parse(s[0]));
            }
            else
            {
                t._birthday = new DateTime();
            }

            t._img = "noimg.jpg";
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
                        fullpath = Config.PATH_APP_ROOT + "\\" + Config.URL_PIC_TEACHER + "\\" + imgname;
                    } while (File.Exists(fullpath));

                    portrait.PostedFile.SaveAs(fullpath);
                    t._img = imgname;
                }
                catch (Exception err)
                {
                    errorText = err.Message + err.StackTrace;
                }
            }

            // Save to DB
            DBManager db = new MySQLDBManager(Config.DB_SERVER, Config.DB_NAME, Config.DB_USER, Config.DB_PASSWORD);
            db.Connect();
            t.AddToDB(db);
            db.Close();
        }

        public void DoEditTeacher(string teacherID)
        {
            DBManager db = new MySQLDBManager(Config.DB_SERVER, Config.DB_NAME, Config.DB_USER, Config.DB_PASSWORD);

            //Load GroupList
            groupList = PaidGroup.LoadListFromDB(db, "");

            theTeacher = new Teacher();
            if (!theTeacher.LoadFromDB(db, "teacher_id=" + teacherID)) theTeacher = null;

            // List Course History
            string[,] bgclass = new string[,] { { "class=\"spec\"", "class=\"td1\"" }, { "class=\"specalt\"", "class=\"alt\"" } };

            string query = "SELECT * from course ";
            query = query + "WHERE teacher_id='";
            query = query + teacherID + "' order by start_date desc";
            OdbcDataReader reader = db.Query(query);
            int i = 0;
            while (reader.Read())
            {
                Course course = Course.CreateForm(reader);
                outBuf3.Append("<tr>");
                outBuf3.Append("<th scope=\"row\" abbr=\"Model\" " + bgclass[i % 2, 0] + " align=center>" + course._startdate.ToString("dd MMM yyyy", ci) + "&nbsp</th>");
                outBuf3.Append("<td " + bgclass[i % 2, 1] + " align=center>" + course._btsCourseID + "&nbsp</td>");
                outBuf3.Append("<td " + bgclass[i % 2, 1] + " align=left>" + course._courseName + "&nbsp</td>");
                outBuf3.Append("<td " + bgclass[i % 2, 1] + "  >" + course._cost + "&nbsp</td>");
                outBuf3.Append("</tr>\n");

                i++;
            }
            db.Close();

        }

        protected void DoEditSubmitTeacher(string teacherID)
        {
            Teacher t = new Teacher();

            // validate data
            t._teacherID = Int32.Parse(teacherID);
            t._firstname = Request["firstname"];
            t._surname = Request["surname"];
            t._citizenID = Request["citizen_id"];
            t._sex = Request["sex"];
            t._tel = Request["tel"];
            t._email = Request["email"];
            t._addr = Request["addr"];
            //t._subject = Request["subject"];
            
            if (Request["birthday"] != null)
            {
                string[] s = Request["birthday"].Split('/');

                t._birthday = new DateTime(Int32.Parse(s[2]) - 543, Int32.Parse(s[1]), Int32.Parse(s[0]));
            }
            else
            {
                t._birthday = new DateTime();
            }

            // default to old value
            t._img = Request["img_old"];
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
                        fullpath = Config.PATH_APP_ROOT + "\\" + Config.URL_PIC_TEACHER + "\\" + imgname;
                    } while (File.Exists(fullpath));

                    portrait.PostedFile.SaveAs(fullpath);
                    t._img = imgname;
                }
                catch (Exception err)
                {
                    errorText = err.Message + err.StackTrace;
                }
            }

            // Save to DB
            DBManager db = new MySQLDBManager(Config.DB_SERVER, Config.DB_NAME, Config.DB_USER, Config.DB_PASSWORD);
            db.Connect();
            t.UpdateToDB(db);
            db.Close();
        }

        protected void DoDeleteTeacher(string teacherID)
        {
            Teacher t = new Teacher();
            t._teacherID = Int32.Parse(teacherID);

            DBManager db = new MySQLDBManager(Config.DB_SERVER, Config.DB_NAME, Config.DB_USER, Config.DB_PASSWORD);
            db.Connect();
            db.BeginTransaction(IsolationLevel.ReadCommitted);
            // delete paid_group_teacher_mapping first
            db.Execute("DELETE FROM paid_group_teacher_mapping WHERE teacher_id="+teacherID);
            // delete teacher
            t.DeleteToDB(db);
            db.Commit();
            db.Close();
        }
    }
}
