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

using BTS.Constant;
using BTS.DB;
using BTS.Entity;
using BTS.Util;

namespace BTS.Page
{


    public partial class StudentManage : System.Web.UI.Page
    {
        public string actPage = "list";
        public StringBuilder outBuf = new StringBuilder();
        public StringBuilder outBuf2 = new StringBuilder();
        public StringBuilder outBuf3 = new StringBuilder();
        public String targetID;
        public Student theStudent;
        public List<Student> listStudent;

        public string errorText = "";
        public CultureInfo ci = new CultureInfo("th-TH");
        public Logger log = Logger.GetLogger(Config.MAINLOG);     

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
                    DoListStudent(qSearch, isNewSearch);
                }
                else if (actPage.Equals("view"))
                {
                    targetID = Request["targetID"];
                    DoViewStudent(targetID);
                }
                else if (actPage.Equals("add_submit"))
                {
                    bool result = DoAddSubmitStudent();
                    if (result)
                    {
                        Response.Redirect("StudentManage.aspx");
                    }
                    else
                    {
                        Response.Redirect("StudentManage.aspx?actPage=add&errorText="+errorText);
                    }
                }
                else if (actPage.Equals("edit"))
                {
                    targetID = Request["targetID"];
                    DoEditStudent(targetID);
                }
                else if (actPage.Equals("edit_submit"))
                {
                    targetID = Request["targetID"];
                    DoEditSubmitStudent(targetID);
                    Response.Redirect("StudentManage.aspx");
                }
                else if (actPage.Equals("save_as_excel"))
                {
                    String urlPath = DoSaveStudentListAsExcel();
                    //DoListStudent("", true);
                    Response.Redirect(urlPath);
                }
                else if (actPage.Equals("delete"))
                {
                    targetID = Request["targetID"];
                    DoDeleteStudent(targetID);
                    Response.Redirect("StudentManage.aspx");
                }
        }

        protected void DoViewStudent(string studentID)
        {
            outBuf = new StringBuilder();

            DBManager db = new MySQLDBManager(Config.DB_SERVER, Config.DB_NAME, Config.DB_USER, Config.DB_PASSWORD, Config.DB_CHAR_ENC);
            theStudent = new Student();
            theStudent.LoadFromDB(db, " student_id=" + studentID);


            // quiz
            StringBuilder quizInfo = new StringBuilder();

            if (theStudent.isQuizNoSelected(1)) { quizInfo.AppendLine("&nbsp&nbsp  - โบรชัวร์ของทางโรงเรียน" + "<br>"); }
            if (theStudent.isQuizNoSelected(2)) { quizInfo.AppendLine("&nbsp&nbsp  - พี่/น้อง/เพื่อน/ครู บอกต่อกันมา" + "<br>"); }
            if (theStudent.isQuizNoSelected(3)) { quizInfo.AppendLine("&nbsp&nbsp  - เดินผ่านมาเจอ" + "<br>"); }
            if (theStudent.isQuizNoSelected(4)) { quizInfo.AppendLine("&nbsp&nbsp  - เว็บไซต์โรงเรียน (www.bts.ac.th)" + "<br>"); }
            if (theStudent.isQuizNoSelected(5)) { quizInfo.AppendLine("&nbsp&nbsp  - โครงการติวต่าง ๆ (" + theStudent.getQuizNoText(5) + ")<br>"); }
            if (theStudent.isQuizNoSelected(6)) { quizInfo.AppendLine("&nbsp&nbsp  - อื่น (" + theStudent.getQuizNoText(6) + ")<br>"); }

            // Query registration history
            Registration[] reghis = Registration.LoadListFromDBCustom(db, "SELECT rg.*,c.bts_course_id as bts_course_id, c.course_name as course_name FROM registration rg, course c WHERE "
                        + " student_id=" + theStudent._studentID + " AND rg.course_id=c.course_id ORDER BY rg.regis_id ");
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


            TextReader reader = new StreamReader(Config.PATH_APP_ROOT + "\\template\\student_view.htm");
            String templateContent = reader.ReadToEnd();
            reader.Close();

            String htmlContent =
                String.Format(templateContent
                    , Config.URL_PIC_STUDENT + "/" + theStudent._img
                    , Student.GetStudentID(theStudent._studentID)
                    , theStudent._firstname + " " + theStudent._surname + " (" + theStudent._nickname + ")"
                    , Config.URL_PIC_SYS + (theStudent._sex.Equals("Male") ? "/boy.gif" : "/girl.gif")
                    , theStudent._citizenID
                    , StringUtil.ConvertEducateLevel(theStudent._level)
                    , theStudent._school
                    , theStudent._birthday.ToString("dd/MM/yyyy", ci)
                    , theStudent._addr
                    , theStudent.GetTel()
                    , theStudent._email
                    , theStudent._create_date.ToString("dd/MM/yyyy", ci)
                    , quizInfo.ToString()
                    , regHisTxt.ToString());


            outBuf.Append(htmlContent);
        }

        protected int DeleteOldReportFile(String prefix)
        {
            DirectoryInfo di = new DirectoryInfo(Config.PATH_APP_ROOT + "\\file\\");
            FileInfo[] rgFiles = di.GetFiles(prefix + "*.csv");

            long nowTicks = DateTime.Now.Ticks;
            long keepTicks = Config.DOWNLOADED_FILES_KEEP * 86400 * 10000000;
            int numDel = 0;
            foreach (FileInfo fi in rgFiles)
            {
                string tickStr = fi.Name.Substring(prefix.Length, 20);
                try
                {
                    long ticks = long.Parse(tickStr);
                    if ((nowTicks - ticks) > keepTicks)
                    {
                        fi.Delete();
                        numDel++;
                    }
                }
                catch (Exception e)
                {
                    continue;
                }
            }
            return numDel;
        }

        protected String DoSaveStudentListAsExcel()
        {
            DBManager db = new MySQLDBManager(Config.DB_SERVER, Config.DB_NAME, Config.DB_USER, Config.DB_PASSWORD, Config.DB_CHAR_ENC);

            String prefix = "students_";
            int numDel = DeleteOldReportFile(prefix);
            log.StampLine(Logger.INFO, "Delete old " + prefix + " " + numDel + " files");

            DateTime now = DateTime.Now;
            String dateStr = StringUtil.FillString(now.Ticks.ToString(), "0", 20, true) + "_"
                + StringUtil.FillString(Session.GetHashCode().ToString(), "0", 10, true);

            String filename = prefix + dateStr + ".csv";
            String physPath = Config.PATH_APP_ROOT + "\\file\\" + filename;
            String urlPath = "file/" + filename;

            FileStream fs = File.Open(physPath, FileMode.OpenOrCreate, FileAccess.Write);
            TextWriter writer = new StreamWriter(fs, System.Text.Encoding.UTF8);

            StringBuilder stdTxt = new StringBuilder();
            //stdTxt.Append(StringUtil.GetExcelEncodingPrefix());

            stdTxt.Append("รหัส,ชื่อ-นามสกุล, ชื่อเล่น,เพศ,วันเกิด,รหัสบัตรประชาชน,ที่อยู่,อีเมล์,โทร., โรงเรียน, ชั้น, วันที่สร้าง, สถานะ \n");

            String[,] students = LoadStudentList(db);
            for (int i = 0; i < students.Length / 14; i++)
            {
                for (int j = 0; j < 13; j++)
                {
                    stdTxt.Append(StringUtil.AddCSVDQuote(students[i, j]) + ",");

                }
                stdTxt.AppendLine("");

                if (i % 100 == 99)
                {
                    writer.Write(stdTxt);
                    stdTxt = new StringBuilder();
                }
            }
            if (stdTxt.Length > 0)
            {
                writer.Write(stdTxt);
            }

            writer.Close();

            return urlPath;
        }   


        protected String[,] LoadStudentList(DBManager db)
        {
            // Load all information       
            Course c = new Course();
            c.LoadFromDB(db);

            String sqlCount = "SELECT Count(*) FROM student";
            String sql = "SELECT s.student_id, s.firstname, s.surname, s.nickname, s.sex, s.birthday, s.citizen_id, s.addr, s.email, s.tel, s.tel2, s.school, s.level, s.create_date, s.is_active";

            String sqlWhere = " FROM student s"
                             + " ORDER BY s.student_id";

            int num = db.QueryCount(sqlCount);

            String[,] result = new String[num, 14];

            int n = 0;
            OdbcDataReader reader = db.Query(sql + sqlWhere);
            while (reader.Read())
            {
                int fCount = reader.FieldCount;
                for (int i = 0; i < fCount; i++)
                {
                    string name = reader.GetName(i);
                    switch (name)
                    {
                        case "student_id": result[n, 0] = Student.GetStudentID(reader.GetInt32(i).ToString());
                            break;
                        case "firstname": result[n, 1] = reader.GetString(i);
                            break;
                        case "surname": result[n, 1] = result[n, 1] + " " + reader.GetString(i);
                            break;
                        case "nickname": result[n, 2] = reader.GetString(i);
                            break;
                        case "sex": result[n, 3] = reader.GetString(i);
                            break;
                        case "birthday": result[n, 4] = reader.GetDate(i).ToString();
                            break;
                        case "citizen_id": result[n, 5] = reader.GetString(i);
                            break;
                        case "addr": result[n, 6] = reader.GetString(i);
                            break;
                        case "email": result[n, 7] = reader.GetString(i);
                            break;
                        case "tel": result[n, 8] = "=\"" + reader.GetString(i) + "\"";
                            break;
                        case "tel2": result[n, 9] = "=\"" + reader.GetString(i) + "\"";
                            break;
                        case "school": result[n, 10] = reader.GetString(i);
                            break;
                        case "level": result[n, 11] = StringUtil.ConvertEducateLevel(reader.GetInt32(i));
                            break;
                        case "create_date": result[n, 12] = reader.GetDate(i).ToString();
                            break;
                        case "is_active": result[n, 13] = (reader.GetInt32(i) == 1 ? "ปกติ" : "ลบ");
                            break;
                    }
                }
                n++;
            }

            return result;
        }


        protected void DoListStudent(string searchStr, bool isNewSearch)
        {
            // get Page
            int pg = 1;
            if ((!isNewSearch) && (Request["pg"] != null)) pg = Int32.Parse(Request["pg"]);

            string[,] bgclass = new string[,] { { "class=\"spec\"", "class=\"td1\"" }, { "class=\"specalt\"", "class=\"alt\"" } };

            listStudent = new List<Student>();
            DBManager db = new MySQLDBManager(Config.DB_SERVER, Config.DB_NAME, Config.DB_USER, Config.DB_PASSWORD, Config.DB_CHAR_ENC);
            string qSearchSQL = Student.GetQSearchSQL(searchStr);
            if (qSearchSQL.Trim().Length > 0)
            {
                qSearchSQL = " WHERE " + qSearchSQL;
            }
            else // search all
            {
                qSearchSQL = " WHERE is_active=1 ";
            }

            int numRec = db.QueryCount("SELECT Count(*) FROM student " + qSearchSQL);

            OdbcDataReader reader = db.Query("SELECT * FROM student " + qSearchSQL + "order by student_id desc LIMIT " + Config.TBRECORD_PER_PAGE + " OFFSET " + (((pg - 1) * Config.TBRECORD_PER_PAGE)));
            int i = 0;
            while (reader.Read())
            {
                Student student = Student.CreateForm(reader);
                // byte[] raw = Encoding.Default.GetBytes(student._firstname);
                // student._firstname = Encoding.GetEncoding("tis-620").GetString(raw);

                int age = ((DateTime.Now.Year * 12 + DateTime.Now.Month) - (student._birthday.Year * 12 + student._birthday.Month)) / 12;
                string divtxt = Config.URL_PIC_STUDENT + "/" + student._img;

                string studentTxt = "<a href=\"StudentManage.aspx?actPage=view&targetID="+student._studentID+"\" onmouseover=\"showDiv('"+divtxt+"')\" onmouseout=\"hideDiv()\" >" + student._firstname + " " + student._surname + "("+ student._nickname +")" + "</a>";

                outBuf.Append("<tr>");
                outBuf.Append("<th scope=\"row\" abbr=\"Model\" " + bgclass[i % 2, 0] + ">" + Student.GetStudentID(student._studentID) + "</th>");
                outBuf.Append("<td " + bgclass[i % 2, 1] + "  >"+studentTxt+"&nbsp</td>");
                outBuf.Append("<td " + bgclass[i % 2, 1] + " align=center > <img width=20 height=24 src=\"" + Config.URL_PIC_SYS + (student._sex.Equals("Male")?"/boy.gif":"/girl.gif") + "\" >"  );
                /*
                for (int j = 0; j < ssList[i].skill.Count; j++)
                {
                    outBuf.Append("<LI><a href=\"SkillHandling.aspx#sdid" + ssList[i].skill[j].id + "\" onmouseover=\"showChecklist(" + ssList[i].skill[j].id + ")\" onmouseout=\"hideCheckList()\" >" + ssList[i].skill[j].name + "</a>");
                }*/
                outBuf.Append("&nbsp</td>");
                outBuf.Append("<td " + bgclass[i % 2, 1] + "  align=center>" + age + "&nbsp</td>");
                outBuf.Append("<td " + bgclass[i % 2, 1] + "  >" + student._school + "&nbsp</td>");
                outBuf.Append("<td " + bgclass[i % 2, 1] + "  align=center>" + StringUtil.ConvertEducateLevel(student._level) + "&nbsp</td>");
                outBuf.Append("<td " + bgclass[i % 2, 1] + "  >" + student.GetTel() + "&nbsp</td>");
                outBuf.Append("<td " + bgclass[i % 2, 1] + "  >" + student._email + "&nbsp</td>");
                outBuf.Append("<td " + bgclass[i % 2, 1] + "  align=center>&nbsp");
                outBuf.Append("<a href=\"javascript:setVal('actPage','edit');setVal('targetID','" + student._studentID + "');doSubmit()\"><img src=\"img/sys/edit.gif\" border=0 alt=\"Edit\"></a>&nbsp");
                outBuf.Append("<a href=\"javascript:if (confirm('Delete this student?')) { setVal('actPage','delete');setVal('targetID','" + student._studentID + "');doSubmit(); }\"><img src=\"img/sys/delete.gif\" border=0 alt=\"Delete\"></a>&nbsp");
 
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
            outBuf2.Append(String.Format("<a href=\"StudentManage.aspx?pg={0}&qsearch={1}\">{2}</a><< ", "1", searchStr, "First"));
            for (i = pg-5; i <= pg+5; i++)
            {
                if ((i <= 0) || (i>maxpg)) {
                    continue;
                }
                
                if (i == pg) { outBuf2.Append("<b>"+i+"</b> "); }
                else {
                    outBuf2.Append(String.Format("<a href=\"StudentManage.aspx?pg={0}&qsearch={1}\">{0}</a> ", i.ToString(), searchStr));
                }

            }
            outBuf2.Append(String.Format(" >><a href=\"StudentManage.aspx?pg={0}&qsearch={1}\">{2}</a> ", maxpg.ToString(), searchStr, "Last"));
            
          //  <a href="#">1</a> <b>2</b> <a href="#">3</a> <a href="#">4</a>


        }

        protected bool DoAddSubmitStudent()
        {
            Student t = new Student();
            
            // validate data
            t._firstname = Request["firstname"];
            t._surname = Request["surname"];
            t._nickname = Request["nickname"];
            t._citizenID = Request["citizen_id"];
            t._sex = Request["sex"];
            t._tel = Request["tel1"] + Request["tel2"] + Request["tel3"];
            t._tel2 = Request["tel21"] + Request["tel22"] + Request["tel23"];
            t._email = Request["email"];
            t._addr = Request["addr"];
            t._school = Request["school"];
            t._level = Int32.Parse(Request["level"]);
            t._quiz = Student.EncodeQuizText(Page.Request);

            if (Request["birthday"] != null)
            {
                string[] s = Request["birthday"].Split('/');

                t._birthday = new DateTime(Int32.Parse(s[2]) - 543, Int32.Parse(s[1]), Int32.Parse(s[0]));
            }
            else
            {
                t._birthday = new DateTime();
            }

            t._create_date = DateTime.Today;

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
                        fullpath = Config.PATH_APP_ROOT + "\\" + Config.URL_PIC_STUDENT + "\\" + imgname;
                    } while (File.Exists(fullpath));

                    portrait.PostedFile.SaveAs(fullpath);
                    t._img = imgname;
                }
                catch (Exception err)
                {
                    errorText = err.Message + err.StackTrace;
                }
            }

            DBManager db = null;
            try
            {
                db = new MySQLDBManager(Config.DB_SERVER, Config.DB_NAME, Config.DB_USER, Config.DB_PASSWORD, Config.DB_CHAR_ENC);
                db.Connect();

                // validate
                // duplicate citizen id
                if (t._citizenID.Length > 0)
                {
                    int count = db.QueryCount("SELECT COUNT(*) FROM student WHERE citizen_id='" + t._citizenID + "'");
                    if (count > 0)
                    {
                        errorText = "รหัสบัตรประชาชน " + t._citizenID + " มีอยู่ในระบบแล้ว";
                        return false;
                    }
                }
                // Save to DB
                t.AddToDB(db);
            }
            catch (Exception e)
            {
                errorText = "พบปัญหาบางประการ ข้อมูลไม่ถูกบันทึก";
                return false;
            }
            finally
            {
                db.Close();
            }

            return true;
        }

        public void DoEditStudent(string studentID)
        {
            DBManager db = new MySQLDBManager(Config.DB_SERVER, Config.DB_NAME, Config.DB_USER, Config.DB_PASSWORD, Config.DB_CHAR_ENC);
            theStudent = new Student();
            if (!theStudent.LoadFromDB(db, "student_id=" + studentID)) theStudent = null;

            // List Course History
            string[,] bgclass = new string[,] { { "class=\"spec\"", "class=\"td1\"" }, { "class=\"specalt\"", "class=\"alt\"" } };

            string query = "SELECT * from course ";
            query = query + "WHERE course.course_id in (select registration.course_id from registration where registration.student_id='";
            query = query + studentID + "') order by start_date desc";
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

        protected void DoEditSubmitStudent(string studentID)
        {
            Student t = new Student();

            // validate data
            t._studentID = Int32.Parse(studentID);
            t._firstname = Request["firstname"];
            t._surname = Request["surname"];
            t._nickname = Request["nickname"];
            t._citizenID = Request["citizen_id"];
            t._sex = Request["sex"];
            t._tel = Request["tel1"] + Request["tel2"] + Request["tel3"];
            t._tel2 = Request["tel21"] + Request["tel22"] + Request["tel23"];
            t._email = Request["email"];
            t._addr = Request["addr"];
            t._school = Request["school"];
            t._level = Int32.Parse(Request["level"]);
            t._quiz = Student.EncodeQuizText(Page.Request);
            
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
                        fullpath = Config.PATH_APP_ROOT + "\\" + Config.URL_PIC_STUDENT + "\\" + imgname;
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
            DBManager db = new MySQLDBManager(Config.DB_SERVER, Config.DB_NAME, Config.DB_USER, Config.DB_PASSWORD, Config.DB_CHAR_ENC);
            db.Connect();
            t.UpdateToDB(db);
            db.Close();
        }



        protected void DoDeleteStudent(string studentID)
        {
            Student t = new Student();
            t._studentID = Int32.Parse(studentID);

            DBManager db = new MySQLDBManager(Config.DB_SERVER, Config.DB_NAME, Config.DB_USER, Config.DB_PASSWORD, Config.DB_CHAR_ENC);
            db.Connect();
            t.DeleteToDB(db);
            db.Close();
        }
    }
}
