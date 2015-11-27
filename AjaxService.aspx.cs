using System;
using System.Data;
using System.Threading;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Globalization;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using BTS;
using BTS.Constant;
using BTS.DB;
using BTS.Entity;
using BTS.Util;

namespace BTS.Page
{

    public partial class AjaxService : System.Web.UI.Page
    {
        public string svc;
        public StringBuilder outBuf = new StringBuilder();

        public CultureInfo ci = new CultureInfo("en-US");

        protected void Page_Load(object sender, EventArgs e)
        {
            svc = Request.Params["svc"];

            if (svc.ToLower().Equals("on_request"))
            {
                outBuf = new StringBuilder();
                outBuf.Append("");
            }
            else if (svc.ToLower().Equals(AjaxSvc.WIZ_Q_COURSES))
            {
                string searchStr = Request.Params["search"];
                ProcessWizQueryCourses(searchStr);
            }
            else if (svc.ToLower().Equals(AjaxSvc.WIZ_Q_COURSE_DETAIL))
            {
                string course_id = Request.Params["course_id"];
                ProcessWizQueryCourseDetail(course_id);
            }
            else if (svc.ToLower().Equals(AjaxSvc.WIZ_LIST_SELECTED_COURSE))
            {
                string enableAction = Request.Params["enable_action"];
                ProcessWizListSelectedCourse(enableAction, false);
            }
            else if (svc.ToLower().Equals(AjaxSvc.WIZ_ADD_SELECTED_COURSE))
            {
                string course_id = Request.Params["course_id"];
                ProcessWizAddSelectedCourse(course_id);
                ProcessWizListSelectedCourse("true", true);
            }
            else if (svc.ToLower().Equals(AjaxSvc.WIZ_REM_SELECTED_COURSE))
            {
                string course_id = Request.Params["course_id"];
                ProcessWizRemoveSelectedCourse(course_id);
                ProcessWizListSelectedCourse("true", true);
            }
            else if (svc.ToLower().Equals(AjaxSvc.WIZ_Q_STUDENTS))
            {
                string searchStr = Request.Params["search"];
                ProcessWizQueryStudents(searchStr);
            }
            else if (svc.ToLower().Equals(AjaxSvc.WIZ_Q_STUDENT_DETAIL))
            {
                string student_id = Request.Params["student_id"];
                ProcessWizQueryStudentDetail(student_id);
            }
            else if (svc.ToLower().Equals(AjaxSvc.WIZ_LIST_SELECTED_STUDENT))
            {
                string enableAction = Request.Params["enable_action"];
                ProcessWizListSelectedStudent();
            }
            else if (svc.ToLower().Equals(AjaxSvc.WIZ_ADD_SELECTED_STUDENT))
            {
                string course_id = Request.Params["student_id"];
                ProcessWizAddSelectedStudent(course_id);
                ProcessWizListSelectedStudent();
            }
            else if (svc.ToLower().Equals(AjaxSvc.WIZ_REM_SELECTED_STUDENT))
            {
                string course_id = Request.Params["student_id"];
                ProcessWizRemoveSelectedStudent(course_id);
                ProcessWizListSelectedStudent();
            }
            else if (svc.ToLower().Equals(AjaxSvc.WIZ_DISCOUNT_PROMOTION))
            {
                string promotion_id = Request.Params["promotion_id"];
                string cost = Request.Params["cost"];
                DiscountPromotion(promotion_id, cost);
                ProcessWizListSelectedCourse("false", false);
            }
            else if (svc.ToLower().Equals(AjaxSvc.WIZ_DISCOUNT_COURSE))
            {
                string course_id = Request.Params["course_id"];
                string cost = Request.Params["cost"];
                DiscountCourse(course_id, cost);
                ProcessWizListSelectedCourse("false", false);
            }
            /*
            else if (svc.ToLower().Equals("qchklist"))
            {
                string skilldefId = Request.Params["sdid"];
                ProcessQuerySkillObjList(skilldefId);
            }
            else if (svc.ToLower().Equals("qskobjcriteria"))
            {
                string skilldefId = Request.Params["soid"];
                ProcessQuerySkillObjCriteria(skilldefId);
            }
            else if (svc.ToLower().Equals("qcomparechart"))
            {
                string skill1 = Request.Params["skill1"];
                string skill2 = Request.Params["skill2"];
                string dataFile = Request.Params["datafile"];
                string u1 = Request.Params["u1"];
                string u2 = Request.Params["u2"];
                ProcessPaintCompareChart(skill1, skill2, u1, u2, dataFile);
            }
            */

        }

        protected void DiscountPromotion(string promotion_id, string cost)
        {
            // get data from session
            RegisTransaction reg = (RegisTransaction)Session[SessionVar.CURRENT_REGIS];
            if (reg == null) { reg = new RegisTransaction(); }

            int pid = Int32.Parse(promotion_id);
            foreach (Promotion p in reg._modPromotions)
            {
                if (p._promotionID == pid)
                {
                    p._discountedCost = Int32.Parse(cost);
                }
            }
        }

        protected void DiscountCourse(string course_id, string cost)
        {
            // get data from session
            RegisTransaction reg = (RegisTransaction)Session[SessionVar.CURRENT_REGIS];
            if (reg == null) { reg = new RegisTransaction(); }

            int cid = Int32.Parse(course_id);
            foreach (Course c in reg._modCourses)
            {
                if (c._courseID == cid)
                {
                    c._discountedCost = Int32.Parse(cost);
                }
            }
        }

        protected void ProcessWizQueryCourses(string searchStr)
        {
            DBManager db = new MySQLDBManager(Config.DB_SERVER, Config.DB_NAME, Config.DB_USER, Config.DB_PASSWORD, Config.DB_CHAR_ENC);

            String subQuery = " (SELECT count(*) FROM registration r WHERE r.status=0 AND r.course_id=c.course_id) ";
            String sql = "SELECT c.*," + subQuery + " AS num_registered from course c WHERE " + Course.GetQSearchSQL(searchStr);

            Course[] courses = Course.LoadListFromDBCustom(db, sql);
            db.Close();

            outBuf.Append("<table>");            
            foreach (Course c in courses)
            {
                if (c._numRegistered < c._seatLimit) {
                    string jsShowDiv = " onmouseover=\"queryCourseDetail(" + c._courseID + ");showDivAt('divdetail')\" onmouseout=\"hideDiv('divdetail')\" ";
                    string icon = "<img style=\"cursor:pointer\" border=0 src=\"" + Config.URL_PIC_SYS + "/info.gif\" " + jsShowDiv + ">";
                    outBuf.Append("<tr valign=middle><td>&nbsp&nbsp" + icon + "</td><td><a href=\"javascript:addCourse('" + c._courseID + "')\">" + c._btsCourseID + "&nbsp" + c._courseName + "[" + c._numRegistered +"/" + c._seatLimit + "]</a></td></tr>");
                } else {
                    // full 
                    string jsShowDiv = " onmouseover=\"queryCourseDetail(" + c._courseID + ");showDivAt('divdetail')\" onmouseout=\"hideDiv('divdetail')\" ";
                    string icon = "<img style=\"cursor:pointer\" border=0 src=\"" + Config.URL_PIC_SYS + "/info.gif\" " + jsShowDiv + ">";
                    outBuf.Append("<tr valign=middle><td>&nbsp&nbsp" + icon + "</td><td> <font color=red>เต็ม </font>" +c._btsCourseID + "&nbsp" + c._courseName  + "[" + c._numRegistered +"/" + c._seatLimit + "]</td></tr>");
                }
            }
            outBuf.Append("</table>");
        }

        protected void ProcessWizQueryCourseDetail(string courseID)
        {
            DBManager db = new MySQLDBManager(Config.DB_SERVER, Config.DB_NAME, Config.DB_USER, Config.DB_PASSWORD, Config.DB_CHAR_ENC);
            Course c = new Course(); 
            c.LoadFromDB(db, " course_id="+courseID);
            c.LoadTeacher(db);
            db.Close();

            TextReader reader = new StreamReader(Config.PATH_APP_ROOT + "\\template\\" + AjaxSvc.WIZ_Q_COURSE_DETAIL + ".htm");
            String templateContent = reader.ReadToEnd();
            reader.Close();

            String htmlContent =
                String.Format(templateContent
                    , Config.URL_PIC_COURSE + "/" + c._img
                    , c._btsCourseID + " " + c._courseName
                    , c._startdate.ToString("dd/MM/yyyy", ci) + " - " + c._enddate.ToString("dd/MM/yyyy", ci) + "<br>"
                      + c._dayOfWeek + " " + c._opentime
                    , c._teacher._firstname + " " + c._teacher._surname
                    , StringUtil.Int2StrComma(c._cost)
                    , c._courseDesc);

            outBuf.Append(htmlContent);

        }

        protected int GetDiscountedCost(LinkedList<Course> courses, Course aCourse)
        {
            foreach (Course c in courses)
            {
                if (aCourse._courseID == c._courseID)
                {
                    return c._discountedCost; 
                }
            }
            return 0;
        }

        protected void ProcessWizListSelectedCourse(string enableAction, bool recalculatePromotion)
        {
            bool enableAct;
            Boolean.TryParse(enableAction,out enableAct);

            outBuf.Append("<br>");
            // get data from session
            RegisTransaction reg = (RegisTransaction)Session[SessionVar.CURRENT_REGIS];
            if (reg == null) { return; }            
            if (reg._courses.Count == 0)
            {
                outBuf.Append("&nbsp&nbspยังไม่ได้เลือกคอร์ส");
            }

            if (recalculatePromotion)
            {

                // copy to temp list
                Course[] courses = new Course[reg._courses.Count];
                reg._courses.CopyTo(courses, 0);
                LinkedList<Course> copyCourses = new LinkedList<Course>(courses);

                // find matching promotions
                DBManager db = new MySQLDBManager(Config.DB_SERVER, Config.DB_NAME, Config.DB_USER, Config.DB_PASSWORD, Config.DB_CHAR_ENC);
                
                PromotionMatcher matcher = Promotion.LoadFromDBByMatchingCourses(db, reg._courses);
                db.Close();

                // store back to reg
                reg._modCourses = (matcher._matchedCourses != null) ? matcher._matchedCourses : new LinkedList<Course>();
                reg._modPromotions = (matcher._matchedPromotions != null) ? matcher._matchedPromotions : new LinkedList<Promotion>();
                Session[SessionVar.CURRENT_REGIS] = reg;
            }

            // print
            Promotion[] proArray = new Promotion[reg._modPromotions.Count];
            reg._modPromotions.CopyTo(proArray, 0);

            int sumCost = 0;
            int sumFullCost = 0;
            outBuf.Append("<table>");
            for (int i = 0; i < proArray.Length; i++)
            {
                sumCost+=proArray[i]._discountedCost;
                outBuf.Append("<tr valign=middle><td>&nbsp&nbsp&nbsp</td><td>" + Promotion.GetPromotionID(proArray[i]._promotionID) + " " + proArray[i]._promotionName);
                string costTxt = StringUtil.Int2StrComma(proArray[i]._discountedCost);
                if (!enableAct)
                {
                    costTxt = " <a href=\"javascript:modifyPCost(" + proArray[i]._promotionID + "," + proArray[i]._discountedCost + ")\" >" + StringUtil.Int2StrComma(proArray[i]._discountedCost) + "</a>";
                }
                if (proArray[i]._cost != proArray[i]._discountedCost)
                {
                    costTxt = " <strike>" + StringUtil.Int2StrComma(proArray[i]._cost) + "</strike> &nbsp ลดพิเศษเหลือ " + costTxt;
                }

                outBuf.Append(" ( <strike>" + StringUtil.Int2StrComma(proArray[i].GetFullCost()) + "</strike> &nbsp"+costTxt+" )</td></tr>");
                for (int j = 0; j < proArray[i]._courses.Length; j++)
                {
                    Course c = proArray[i]._courses[j];
                    sumFullCost += c._cost;
                    string jsShowDiv = " onmouseover=\"queryCourseDetail(" + c._courseID + ");showDivAt('divdetail')\" onmouseout=\"hideDiv('divdetail')\" ";
                    string icon = "<img style=\"cursor:pointer\" border=0 src=\"" + Config.URL_PIC_SYS + "/info.gif\" " + jsShowDiv + ">";
                    string courseLine = c._btsCourseID + "&nbsp" + c._courseName;
                    string costLine = " ( <strike>" + StringUtil.Int2StrComma(c._cost) + "</strike> )";
                    
                    if (enableAct)
                    {
                        courseLine = "<a href=\"javascript:removeCourse('" + c._courseID + "')\">" + courseLine + "</a>";
                    }
                    outBuf.Append("<tr valign=middle><td>&nbsp&nbsp" + icon + "</td><td>&nbsp&nbsp&nbsp" + courseLine + costLine + "</td></tr>");
                    // remove from copied list
                    //copyCourses.Remove(c);
                }
            }

            // print no promotion
            foreach (Course c in reg._modCourses)
            {
                sumCost+=c._discountedCost;
                sumFullCost += c._cost;
                string jsShowDiv = " onmouseover=\"queryCourseDetail(" + c._courseID + ");showDivAt('divdetail')\" onmouseout=\"hideDiv('divdetail')\" ";
                string icon = "<img style=\"cursor:pointer\" border=0 src=\"" + Config.URL_PIC_SYS + "/info.gif\" " + jsShowDiv + ">";
                string courseLine = c._btsCourseID + "&nbsp" + c._courseName;
                if (enableAct)
                {
                    courseLine = "<a href=\"javascript:removeCourse('" + c._courseID + "')\">" + courseLine + "</a>";
                }

                string costTxt = " ( ";
                if (c._cost != c._discountedCost)
                {
                    costTxt = costTxt + " <strike>" + StringUtil.Int2StrComma(c._cost) + "</strike> &nbsp ลดพิเศษเหลือ ";
                }
                if (!enableAct)
                {
                    costTxt = costTxt + "<a href=\"javascript:modifyCCost(" + c._courseID + "," + c._discountedCost + ")\" >" + StringUtil.Int2StrComma(c._discountedCost) + "</a> )";
                }
                else
                {
                    costTxt = costTxt + StringUtil.Int2StrComma(c._discountedCost) + " )";
                }

                outBuf.Append("<tr valign=middle><td>&nbsp&nbsp" + icon + "</td><td>" + courseLine + costTxt + "</td></tr>");
            }
            outBuf.Append("</table><br><br>");
            outBuf.Append("&nbsp&nbsp<b>รวมค่าใช้จ่ายทั้งหมด <strike>"+ StringUtil.Int2StrComma(sumFullCost) +"</strike>&nbsp<font size=2 color=red>" + StringUtil.Int2StrComma(sumCost) + "</font> บาท</b>" );
        }
        

        protected void ProcessWizAddSelectedCourse(string course_id)
        {
            // get data from session
            RegisTransaction reg = (RegisTransaction)Session[SessionVar.CURRENT_REGIS];
            if (reg == null) { reg = new RegisTransaction(); }

                        
            DBManager db = new MySQLDBManager(Config.DB_SERVER, Config.DB_NAME, Config.DB_USER, Config.DB_PASSWORD, Config.DB_CHAR_ENC);
            Course course = new Course();
            course.LoadFromDB(db," course_id=" + course_id);
            db.Close();

            if (course != null)
            {
                reg.AddCourse(course);
            }

            // save back to session
            Session[SessionVar.CURRENT_REGIS] = reg;
        }

        protected void ProcessWizRemoveSelectedCourse(string course_id)
        {
            // get data from session
            RegisTransaction reg = (RegisTransaction)Session[SessionVar.CURRENT_REGIS];
            if (reg == null) { return; }

            foreach (Course c in reg._courses)
            {
                if (c._courseID.ToString().Equals(course_id))
                {
                    reg._courses.Remove(c);
                    break;
                }
            }
            
        }

        protected void ProcessWizQueryStudents(string searchStr)
        {
            DBManager db = new MySQLDBManager(Config.DB_SERVER, Config.DB_NAME, Config.DB_USER, Config.DB_PASSWORD, Config.DB_CHAR_ENC);
            Student[] students = Student.LoadListFromDBCustom(db, "SELECT * from student s WHERE " + Student.GetQSearchSQL(searchStr));
            db.Close();

            outBuf.Append("<table>");
            foreach (Student s in students)
            {
                string jsShowDiv = " onmouseover=\"queryStudentDetail(" + s._studentID + ");showDivAt('divdetail')\" onmouseout=\"hideDiv('divdetail')\" ";
                string icon = "<img style=\"cursor:pointer\" border=0 src=\"" + Config.URL_PIC_SYS + "/info.gif\" " + jsShowDiv + ">";
                outBuf.Append("<tr valign=middle><td>&nbsp&nbsp" + icon + "</td><td><a href=\"javascript:addStudent('" + s._studentID + "')\">"
                    + Student.GetStudentID(s._studentID) + "&nbsp" + s._firstname + "&nbsp" + s._surname + "(" + s._nickname + ")</a></td></tr>");
            }
            outBuf.Append("</table>");
        }

        protected void ProcessWizQueryStudentDetail(string studentID)
        {
            DBManager db = new MySQLDBManager(Config.DB_SERVER, Config.DB_NAME, Config.DB_USER, Config.DB_PASSWORD, Config.DB_CHAR_ENC);
            Student s = new Student();
            s.LoadFromDB(db, " student_id=" + studentID);
            db.Close();

            TextReader reader = new StreamReader(Config.PATH_APP_ROOT + "\\template\\" + AjaxSvc.WIZ_Q_STUDENT_DETAIL + ".htm");
            String templateContent = reader.ReadToEnd();
            reader.Close();

            String htmlContent =
                String.Format(templateContent
                    , Config.URL_PIC_STUDENT + "/" + s._img
                    , Student.GetStudentID(s._studentID)
                    , s._firstname + " " + s._surname + " (" + s._nickname + ")"
                    , s._citizenID
                    , Config.URL_PIC_SYS + (s._sex.Equals("Male") ? "/boy.gif" : "/girl.gif")
                    , s._school
                    , s._birthday.ToString("dd/MM/yyyy", ci)
                    , s._addr
                    , s.GetTel()
                    , s._email
                    );

            outBuf.Append(htmlContent);

        }

        protected void ProcessWizListSelectedStudent()
        {
            outBuf.Append("<br>");
            // get data from session
            RegisTransaction reg = (RegisTransaction)Session[SessionVar.CURRENT_REGIS];
            if (reg == null) { return; }
            if (reg._student == null)
            {
                outBuf.Append("&nbsp&nbspยังไม่ได้เลือกนักเรียน");
                return;
            }

            // Query registration history
            DBManager db = new MySQLDBManager(Config.DB_SERVER, Config.DB_NAME, Config.DB_USER, Config.DB_PASSWORD, Config.DB_CHAR_ENC);
            Registration[] reghis = Registration.LoadListFromDBCustom(db, "SELECT rg.*, c.bts_course_id as bts_course_id, c.course_name as course_name FROM registration rg, course c WHERE "
                        + " student_id="+reg._student._studentID+" AND rg.course_id=c.course_id ORDER BY rg.regis_id ");
            db.Close();
            StringBuilder regHisTxt = new StringBuilder();
            if (reghis.Length > 0)
            {
                for (int i = 0; i < reghis.Length; i++)
                {
                    string statusTxt = Registration.GetStatusText(reghis[i]._status);
                    if (reghis[i]._status == Registration.STATUS_CANCELLED)
                    {
                        statusTxt = "<font color=red>" + statusTxt + "</font>";
                    }

                    regHisTxt.AppendLine("<tr>");
                    regHisTxt.AppendLine("<td>&nbsp" + reghis[i]._regisdate.ToString("dd/MM/yyyy HH:mm", ci) + "</td>");
                    regHisTxt.AppendLine("<td>&nbsp" + "<a href=\"CourseManage.aspx?actPage=view&targetID=" + reghis[i]._courseID + "\" >" + reghis[i]._btsCourseID + " " + reghis[i]._courseName + "</a>" + "</td>");                    
                    regHisTxt.AppendLine("<td align=center>" + StringUtil.Int2StrComma(reghis[i]._fullCost) + "</td>");
                    regHisTxt.AppendLine("<td align=center>" + StringUtil.Int2StrComma(reghis[i]._discountedCost) + "</td>");
                    regHisTxt.AppendLine("<td align=center>" + statusTxt + "</td>");
                    regHisTxt.AppendLine("<td align=center>" + reghis[i]._note + "</td>");

                    regHisTxt.AppendLine("</tr>");
                }
            }
            else
            {
                regHisTxt.AppendLine("<tr><td colspan=3 align=center>ไม่พบข้อมูลการลงทะเบียน</td></tr>");
            }


            TextReader reader = new StreamReader(Config.PATH_APP_ROOT + "\\template\\" + AjaxSvc.WIZ_LIST_SELECTED_STUDENT + ".htm");
            String templateContent = reader.ReadToEnd();
            reader.Close();

            Student s = reg._student;
            String htmlContent =
                String.Format(templateContent
                    , Config.URL_PIC_STUDENT + "/" + s._img
                    , Student.GetStudentID(s._studentID)
                    , s._firstname + " " + s._surname + " (" + s._nickname +")"
                    , s._citizenID
                    , Config.URL_PIC_SYS + (s._sex.Equals("Male") ? "/boy.gif" : "/girl.gif")
                    , s._school
                    , s._birthday.ToString("dd/MM/yyyy", ci)
                    , s._addr
                    , s.GetTel()
                    , s._email
                    , regHisTxt.ToString());


            outBuf.Append(htmlContent);
        }


        protected void ProcessWizAddSelectedStudent(string student_id)
        {
            // get data from session
            RegisTransaction reg = (RegisTransaction)Session[SessionVar.CURRENT_REGIS];
            if (reg == null) { reg = new RegisTransaction(); }


            DBManager db = new MySQLDBManager(Config.DB_SERVER, Config.DB_NAME, Config.DB_USER, Config.DB_PASSWORD, Config.DB_CHAR_ENC);
            Student student = new Student();
            student.LoadFromDB(db, " student_id=" + student_id);
            db.Close();

            if (student != null)
            {
                reg._student = student;
                reg._studentID = student._studentID;
            }

            // save back to session
            Session[SessionVar.CURRENT_REGIS] = reg;
        }

        protected void ProcessWizRemoveSelectedStudent(string course_id)
        {
            // get data from session
            RegisTransaction reg = (RegisTransaction)Session[SessionVar.CURRENT_REGIS];
            if (reg == null) { return; }

            foreach (Course c in reg._courses)
            {
                if (c._courseID.ToString().Equals(course_id))
                {
                    reg._courses.Remove(c);
                    break;
                }
            }

        }


        
    }
}