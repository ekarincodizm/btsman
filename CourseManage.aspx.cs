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


    public partial class CourseManage : System.Web.UI.Page
    {
        public string actPage = "list";
        public string quickSearch = "";
        public string filterSearch = "";
        public StringBuilder outBuf = new StringBuilder();
        public StringBuilder outBuf2 = new StringBuilder();
        public String targetID;
        public Course theCourse;
        public List<Course> listCourse;

        public Room[] roomList;
        public Teacher[] teacherList;
        public PaidGroup[] paidGroupList;

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
                    string filter = Request.Form.Get("filter");                    

                    bool isNewSearch = false;
                    if (qSearch != null)
                    {
                        isNewSearch = true;
                    }
                    else
                    {
                        qSearch = Request["qsearch"];
                        filter = Request["filter"];
                    }

                    quickSearch = qSearch;
                    filterSearch = filter;
                    DoListCourse(qSearch, isNewSearch, filter);
                }
                else if (actPage.Equals("view"))
                {
                    targetID = Request["targetID"];
                    DoViewCourse(targetID);
                }
                else if (actPage.Equals("init_print"))
                {
                    targetID = Request["targetID"];
                    outBuf = new StringBuilder();
                    outBuf.Append(DoInitPrintStudentList(targetID));
                    Session[SessionVar.PRINT_INFO] = new StringBuilder(outBuf.ToString());

                }
                else if (actPage.Equals("save_as_excel"))
                {
                    targetID = Request["targetID"];
                   // outBuf = DoSaveStudentListAsExcel(targetID);
                    //Session[SessionVar.EXCEL_INFO] = new StringBuilder(outBuf.ToString());

                    //DoViewCourse(targetID);
                    //actPage = "view";
                    //Response.Redirect("CourseManage.aspx?actPage=view&targetID="+targetID);

                    String urlPath = DoSaveStudentListAsExcel(targetID);
                    //DoListStudent("", true);
                    Response.Redirect(urlPath);
                    
                    Response.Redirect("SaveAsExcel.aspx");
                }
                else if (actPage.Equals("add"))
                {
                    DoAddCourse();
                }
                else if (actPage.Equals("add_submit"))
                {
                    DoAddSubmitCourse();
                    Response.Redirect("CourseManage.aspx");
                }
                else if (actPage.Equals("edit"))
                {
                    targetID = Request["targetID"];
                    DoEditCourse(targetID);
                }
                else if (actPage.Equals("edit_submit"))
                {
                    targetID = Request["targetID"];
                    DoEditSubmitCourse(targetID);
                    Response.Redirect("CourseManage.aspx");
                }
                else if (actPage.Equals("delete"))
                {
                    targetID = Request["targetID"];
                    DoDeleteCourse(targetID);
                    Response.Redirect("CourseManage.aspx?errorText="+errorText);
                }
        }

        protected void DoViewCourse(string courseID)
        {
            DBManager db = new MySQLDBManager(Config.DB_SERVER, Config.DB_NAME, Config.DB_USER, Config.DB_PASSWORD, Config.DB_CHAR_ENC);
            theCourse = new Course();
            theCourse.LoadFromDB(db, " course_id=" + courseID);
            theCourse.LoadTeacher(db);
            theCourse.LoadPaidGroup(db);            
            db.Close();

            TextReader reader = new StreamReader(Config.PATH_APP_ROOT + "\\template\\course_view.htm");
            String templateContent = reader.ReadToEnd();
            reader.Close();

            String[,] students = LoadStudentList(db, courseID);
            StringBuilder  stdTxt = new StringBuilder();
            int numStudent = (students.Length / 9);
            int numCancel = 0;
            for (int i = 0; i < numStudent; i++)
            {
                stdTxt.Append("<tr>");
                for (int j = 0; j < 9; j++)
                {
                    // edit registration
                    if (j == 8)
                    {
                        stdTxt.Append("<td>&nbsp&nbsp<a href=\"javascript:jumpToEditRegistration(" + students[i, j] + ")\"><img src=\"img/sys/edit.gif\" border=\"0\" alt=\"Edit\"></a></td>");
                        continue;
                    }

                    
                    stdTxt.Append("<td>&nbsp&nbsp" + students[i, j]+"</td>");
                    if ((j == 6) && (students[i, j].Equals("ยกเลิก")))
                    {
                        numCancel++;
                    }


                }
                stdTxt.Append("</tr>");
            }

            String courseScheduleDetail = "-";
            if (theCourse._courseType == "คอร์สสด") {
                courseScheduleDetail =  theCourse._startdate.ToString("dd/MM/yyyy", ci) + " - " + theCourse._enddate.ToString("dd/MM/yyyy", ci) + "<br>" + theCourse._dayOfWeek + " " + theCourse._opentime;
            }

            String htmlContent =
                String.Format(templateContent
                    , Config.URL_PIC_COURSE + "/" + theCourse._img                    
                    , theCourse._btsCourseID + " " + theCourse._courseName
                    , theCourse._shortName
                    , theCourse._courseType
                    , theCourse._category
                    , courseScheduleDetail
                    , theCourse._teacher._firstname + " " + theCourse._teacher._surname
                    , PaidGroup.GetPaidGroupID(theCourse._paidGroupID)+ " " + theCourse._paidGroup._name
                    , StringUtil.Int2StrComma(theCourse._cost)
                    , theCourse._seatLimit.ToString()
                    , theCourse._bankRegisLimit.ToString()
                    , (numStudent - numCancel).ToString()
                    , theCourse._courseDesc
                    , "สมัคร " + numStudent + " คน ยกเลิก " + (numCancel) + " คน คงเหลือ " + (numStudent - numCancel) + " คน"
                    , stdTxt.ToString() );

            outBuf.Append(htmlContent);

        }

        protected String DoInitPrintStudentList(string courseID)
        {
            DBManager db = new MySQLDBManager(Config.DB_SERVER, Config.DB_NAME, Config.DB_USER, Config.DB_PASSWORD, Config.DB_CHAR_ENC);
            theCourse = new Course();
            theCourse.LoadFromDB(db);

            String[,] students = LoadStudentList(db, courseID);
            StringBuilder  stdTxt = new StringBuilder();
            for (int i=0;i<students.Length/9;i++) {
                stdTxt.Append("<tr>");
                for (int j = 0; j < 8; j++)
                {
                    stdTxt.Append("<td nowrap><font size=2>&nbsp&nbsp" + students[i, j]+"</font></td>");
                    
                }
                stdTxt.Append("</tr>");
            }

            TextReader reader = new StreamReader(Config.PATH_APP_ROOT + "\\template\\course_print_student.htm");
            String templateContent = reader.ReadToEnd();
            reader.Close();

            String htmlContent =
                String.Format(templateContent
                    , theCourse._btsCourseID + " " + theCourse._courseName
                    , stdTxt.ToString() );

            return htmlContent;
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

        protected String DoSaveStudentListAsExcel(string courseID)
        {
            DBManager db = new MySQLDBManager(Config.DB_SERVER, Config.DB_NAME, Config.DB_USER, Config.DB_PASSWORD, Config.DB_CHAR_ENC);
            theCourse = new Course();
            theCourse._courseID = Int32.Parse(courseID);
            theCourse.LoadFromDB(db);

            String prefix = "course_students_";
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

            /*

            Encoding tis620 = Encoding.GetEncoding("tis-620");
            Encoding utf8 = Encoding.UTF8;

            Encoding encFrom = Encoding.Unicode;
            Encoding encTo = Encoding.UTF8;

            
            String unicodeString = "รหัสชีวิต";
            // Convert the string into a byte[].
             byte[] unicodeBytes = encFrom.GetBytes(unicodeString);

         // Perform the conversion from one encoding to the other.
             byte[] asciiBytes = Encoding.Convert(encFrom, encTo, unicodeBytes);
            
         // Convert the new byte[] into a char[] and then into a string.
         // This is a slightly different approach to converting to illustrate
         // the use of GetCharCount/GetChars.
             char[] asciiChars = new char[encTo.GetCharCount(asciiBytes, 0, asciiBytes.Length)];
             encTo.GetChars(asciiBytes, 0, asciiBytes.Length, asciiChars, 0);

//         char[] unicodeChars = new char[encFrom.GetCharCount(unicodeBytes, 0, unicodeBytes.Length)];
//         encFrom.GetChars(unicodeBytes, 0, unicodeBytes.Length, unicodeChars, 0);

         //stdTxt.Append("ก");
            */
            /*
         stdTxt.Append(asciiChars + "\n");
         stdTxt.Append(unicodeString + "\n");
           // stdTxt.Append(unicodeChars+"\n");
            stdTxt.Append("คิดถึงจัง");*/
            
         //stdTxt.AppendLine((asciiString);
         //stdTxt.AppendLine(unicodeString);
            // รหัส,ชื่อ-นามสกุล,โรงเรียน,โทร.,อีเมล์,จำนวนเงิน,หมายเหตุ 


            //stdTxt.Append(StringUtil.GetExcelEncodingPrefix());

            stdTxt.Append("รายชิ้อนักเรียนที่ลงทะเบียนคอร์ส " + theCourse._btsCourseID + " " + theCourse._courseName + "\n");
            stdTxt.Append("วันที่สมัคร,ชื่อ-นามสกุล,โรงเรียน,โทร.,อีเมล์,จำนวนเงิน, สถานะ, หมายเหตุ \n" );

            String[,] students = LoadStudentList(db, courseID);            
            for (int i = 0; i < students.Length / 9; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    stdTxt.Append(StringUtil.AddCSVDQuote(students[i, j])+",");

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

        protected String[,] LoadStudentList(DBManager db, string courseID)
        {
            // Load all information       
            Course c = new Course();
            c.LoadFromDB(db);

            String sqlCount = "SELECT Count(*) ";
            String sql = "SELECT r.regis_date, s.firstname, s.surname, s.email, s.tel, s.school ,r.discounted_cost as discounted_cost, r.note as note, r.status as status, r.regis_id as regis_id ";
           
            String sqlWhere = " FROM student s, registration r "
                             + " WHERE s.student_id=r.student_id AND s.is_active=1 AND r.course_id=" + courseID + " ORDER BY r.status, s.firstname, s.surname";

            int num = db.QueryCount(sqlCount + sqlWhere);

            String[,] result = new String[num,9];

            int n = 0;
            OdbcDataReader reader =  db.Query(sql + sqlWhere);
            while (reader.Read())
            {
                int fCount = reader.FieldCount;
                for (int i = 0; i < fCount; i++)
                {
                    string name = reader.GetName(i);
                    switch (name)
                    {
                        case "regis_date": result[n, 0] = new DateTime(reader.GetDateTime(i).Ticks).ToString();
                                           break;
                        case "firstname":  result[n,1] = reader.GetString(i);
                                           break;
                        case "surname": result[n, 1] = result[n, 1] + " " + reader.GetString(i);
                                           break;
                        case "school": result[n, 2] = reader.GetString(i);
                                           break;
                        case "tel": result[n, 3] =  reader.GetString(i);
                                           break;
                        case "email": result[n, 4] = reader.GetString(i);
                                           break;
                        case "discounted_cost": result[n, 5] = reader.GetInt32(i).ToString();
                                           break;
                        case "status": result[n, 6] = (reader.GetInt32(i)==0?"ปกติ":"ยกเลิก");
                                           break;
                        case "note": result[n, 7] = reader.GetString(i);
                                           break;
                        case "regis_id": result[n, 8] = reader.GetInt32(i).ToString();
                                           break;
                    }
                }
                n++;
            }

            return result;
        }
        
        protected void DoListCourse(string searchStr, bool isNewSearch, string filter)
        {
            // get Page
            int pg = 1;
            if ((!isNewSearch) && (Request["pg"] != null)) pg = Int32.Parse(Request["pg"]);

            if ((filter==null) || (filter.Length==0) || ((!filter.Equals("all")) && (!filter.Equals("only_close"))))
            {
                filter = "only_open";
            }


            string[,] bgclass = new string[,] { { "class=\"spec\"", "class=\"td1\"" }, { "class=\"specalt\"", "class=\"alt\"" } };

            listCourse = new List<Course>();
            DBManager db = new MySQLDBManager(Config.DB_SERVER, Config.DB_NAME, Config.DB_USER, Config.DB_PASSWORD, Config.DB_CHAR_ENC);
            
            
            string qSearchSQL = Course.GetQSearchSQL(searchStr);

            //filter


            string curDate = "'"+DateTime.Now.ToString("yyyy-MM-dd", ci)+"'"; // "'2011-05-01'";
            string filterSQL = "";
            if (filter.Equals("only_open"))
            {
                filterSQL = "( end_date > "+ curDate + " or course_type='คอร์ส DVD' )";
            }
            else if (filter.Equals("only_close"))
            {
                filterSQL = "( end_date < " + curDate + " or course_type='คอร์ส DVD' )";
            }
            if (filterSQL.Length > 0) 
            {
                if (qSearchSQL.Trim().Length > 0)
                {
                    qSearchSQL = qSearchSQL + " AND " + filterSQL;
                }
                else
                {
                    qSearchSQL = filterSQL;
                }                
            }

            // Add WHERE
            if (qSearchSQL.Trim().Length > 0) qSearchSQL = " WHERE " + qSearchSQL;
            int numRec = db.QueryCount("SELECT Count(*) FROM course c " + qSearchSQL);

            // add join condition
            qSearchSQL = qSearchSQL + ((qSearchSQL.Trim().Length > 0)?" AND ":" WHERE ") + " c.teacher_id=t.teacher_id";

            String subQuery = " (SELECT count(*) FROM registration r WHERE r.status=0 AND r.course_id=c.course_id) ";

            OdbcDataReader reader = db.Query("SELECT c.*,t.firstname as teacher_firstname, t.surname as teacher_surname, "
                + subQuery + " AS num_registered "
                + " FROM course c, teacher t "
                + qSearchSQL + " order by bts_course_id desc LIMIT " + Config.TBRECORD_PER_PAGE + " OFFSET " + (((pg - 1) * Config.TBRECORD_PER_PAGE)));
            int i = 0;
            while (reader.Read())
            {
                Course course = Course.CreateForm(reader);
                // byte[] raw = Encoding.Default.GetBytes(course._firstname);
                // course._firstname = Encoding.GetEncoding("tis-620").GetString(raw);

                string divtxt = Config.URL_PIC_COURSE + "/" + course._img;

                outBuf.Append("<tr>");
                outBuf.Append("<th scope=\"row\" abbr=\"Model\" " + bgclass[i % 2, 0] + ">" + course._btsCourseID + (course._numRegistered >= course._seatLimit ? "<br><font color=red>[เต็ม]</font>":"") + "</th>");
                outBuf.Append("<td " + bgclass[i % 2, 1] + "  ><a href=\"CourseManage.aspx?actPage=view&targetID=" + course._courseID + "\" >"
                    + "<b>" + course._courseName + "</b></a><br>"
                    + course._courseDesc
                    + "</td>");
                outBuf.Append("<td " + bgclass[i % 2, 1] + " align=center>" + StringUtil.Int2StrComma(course._cost) + "&nbsp</td>");
                outBuf.Append("<td " + bgclass[i % 2, 1] + "  align=center>" 
                    + course._startdate.ToString("dd/MM/yyyy", ci) + " - " +  course._enddate.ToString("dd/MM/yyyy", ci) + "<br>"
                    + course._dayOfWeek + " " + course._opentime
                    +  "&nbsp</td>");
                outBuf.Append("<td " + bgclass[i % 2, 1] + "  >" + course._teacher._firstname + " " + course._teacher._surname + "&nbsp</td>");
                outBuf.Append("<td " + bgclass[i % 2, 1] + "  align=center>&nbsp");
                outBuf.Append("<a href=\"javascript:setVal('actPage','view');setVal('targetID','" + course._courseID + "');doSubmit()\"><img src=\"img/sys/view.gif\" border=0 alt=\"View detail\"></a>&nbsp");
                outBuf.Append("<a href=\"javascript:setVal('actPage','edit');setVal('targetID','" + course._courseID + "');doSubmit()\"><img src=\"img/sys/edit.gif\" border=0 alt=\"Edit\"></a>&nbsp");
                outBuf.Append("<a href=\"javascript:if (confirm('Delete this course?')) { setVal('actPage','delete');setVal('targetID','" + course._courseID + "');doSubmit(); }\"><img src=\"img/sys/delete.gif\" border=0 alt=\"Delete\"></a>&nbsp");
 
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
            outBuf2.Append(String.Format("<a href=\"CourseManage.aspx?pg={0}&qsearch={1}\">{2}</a><< ", "1", searchStr, "First"));
            for (i = pg - 5; i <= pg + 5; i++)
            {
                if ((i <= 0) || (i > maxpg))
                {
                    continue;
                }
                if (i == pg) { outBuf2.Append("<b>"+i+"</b> "); }
                else {
                    outBuf2.Append(String.Format("<a href=\"CourseManage.aspx?pg={0}&qsearch={1}&filter={2}\">{0}</a> ", i.ToString(), searchStr, filter));
                }

            }
            outBuf2.Append(String.Format(" >><a href=\"CourseManage.aspx?pg={0}&qsearch={1}\">{2}</a> ", maxpg.ToString(), searchStr, "Last"));
            
          //  <a href="#">1</a> <b>2</b> <a href="#">3</a> <a href="#">4</a>


        }


        public void DoAddCourse()
        {
            DBManager db = new MySQLDBManager(Config.DB_SERVER, Config.DB_NAME, Config.DB_USER, Config.DB_PASSWORD, Config.DB_CHAR_ENC);
            roomList = Room.LoadListFromDBCustom(db, "SELECT r.room_id, r.name, b.branch_name as branch_name FROM room r, branch b WHERE r.branch_id=b.branch_id");
            teacherList = Teacher.LoadListFromDB(db, " ORDER BY firstname");
            paidGroupList = paidGroupList = PaidGroup.LoadListFromDB(db, " ORDER BY paid_group_id");
            db.Close();
        }

        protected void DoAddSubmitCourse()
        {
            Course c = new Course();
            
            // validate data
            c._btsCourseID = Request["bts_course_id"];
            c._courseName = Request["course_name"];
            c._shortName = Request["short_name"];
            c._courseType = Request["course_type"];
            c._courseDesc = Request["course_desc"];
            c._roomID = Int32.Parse(Request["room_id"]);
            c._teacherID = Int32.Parse(Request["teacher_id"]);
            c._paidGroupID = Int32.Parse(Request["paid_group_id"]);
            c._category = Request["category"];

            c._startdate = StringUtil.getDate(Request["startdate"]);
            c._enddate = StringUtil.getDate(Request["enddate"]);
            c._dayOfWeek = Request["day_of_week"];
            c._opentime = Request["opentime"];

            c._cost =Int32.Parse(Request["cost"]);
            c._seatLimit = Int32.Parse(Request["seat_limit"]);
            c._bankRegisLimit = 0; // remove field



            c._img = "noimg.jpg";
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
                        fullpath = Config.PATH_APP_ROOT + "\\" + Config.URL_PIC_COURSE + "\\" + imgname;
                    } while (File.Exists(fullpath));

                    portrait.PostedFile.SaveAs(fullpath);
                    c._img = imgname;
                }
                catch (Exception err)
                {
                    errorText = err.Message + err.StackTrace;
                }
            }

            DBManager db = new MySQLDBManager(Config.DB_SERVER, Config.DB_NAME, Config.DB_USER, Config.DB_PASSWORD, Config.DB_CHAR_ENC);
            db.Connect();
            // Validate if bts code okay
            Course[] dupBTSCourse = Course.LoadListFromDBCustom(db, "SELECT * FROM course c, payment p WHERE bts_course_id='" + c._btsCourseID + "' AND c.course_id=p.course_id AND (p.sum_max_payable>p.sum_paid_cost OR p.sum_max_payable=0)");
            if (dupBTSCourse.Length == 0)
            {
                // no duplicate bts
                // Save to DB
                // Save to DB
                db.BeginTransaction(IsolationLevel.ReadCommitted);

                c.AddToDB(db);
                c._courseID = Course.GetMaxCourseID(db);

                // Update Payment with empty record
                Payment payment = new Payment();
                payment._courseID = c._courseID;
                payment._sumAllCost = 0;
                payment._sumPaidCost = 0;
                payment._status = 0;
                payment._lastPaidDate = DateTime.Now;
                payment.AddToDB(db);

                db.Commit();
                db.Close();
            }
        }

        public void DoEditCourse(string courseID)
        {
            DBManager db = new MySQLDBManager(Config.DB_SERVER, Config.DB_NAME, Config.DB_USER, Config.DB_PASSWORD, Config.DB_CHAR_ENC);
            theCourse = new Course();
            if (!theCourse.LoadFromDB(db, "course_id=" + courseID)) theCourse = null;

            roomList = Room.LoadListFromDBCustom(db, "SELECT r.room_id, r.name, b.branch_name as branch_name FROM room r, branch b WHERE r.branch_id=b.branch_id");
            teacherList = Teacher.LoadListFromDB(db, " ORDER BY firstname");
            paidGroupList = PaidGroup.LoadListFromDB(db, " ORDER BY paid_group_id");

            db.Close();

        }

        protected void DoEditSubmitCourse(string courseID)
        {
            Course c = new Course();

            // validate data
            c._courseID = Int32.Parse(courseID);
            c._btsCourseID = Request["bts_course_id"];
            c._courseName = Request["course_name"];
            c._shortName = Request["short_name"];
            c._courseType = Request["course_type"];
            c._courseDesc = Request["course_desc"];
            c._roomID = Int32.Parse(Request["room_id"]);
            c._teacherID = Int32.Parse(Request["teacher_id"]);
            c._paidGroupID = Int32.Parse(Request["paid_group_id"]);
            c._category = Request["category"];

            c._startdate = StringUtil.getDate(Request["startdate"]);
            c._enddate = StringUtil.getDate(Request["enddate"]);
            c._dayOfWeek = Request["day_of_week"];
            c._opentime = Request["opentime"];

            c._cost = Int32.Parse(Request["cost"]);
            c._seatLimit = Int32.Parse(Request["seat_limit"]);
            c._bankRegisLimit = 0; //remove field
            
            // default to old value
            c._img = Request["img_old"];
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
                        fullpath = Config.PATH_APP_ROOT + "\\" + Config.URL_PIC_COURSE + "\\" + imgname;
                    } while (File.Exists(fullpath));

                    portrait.PostedFile.SaveAs(fullpath);
                    c._img = imgname;
                }
                catch (Exception err)
                {
                    errorText = err.Message + err.StackTrace;
                }
            }

            
            DBManager db = new MySQLDBManager(Config.DB_SERVER, Config.DB_NAME, Config.DB_USER, Config.DB_PASSWORD, Config.DB_CHAR_ENC);
            db.Connect();
            // Validate if bts code okay
            Course[] dupBTSCourse = Course.LoadListFromDBCustom(db, "SELECT * FROM course c, payment p WHERE bts_course_id='" + c._btsCourseID + "' AND c.course_id=p.course_id AND c.course_id<>" + c._courseID + " AND (p.sum_max_payable>p.sum_paid_cost OR p.sum_max_payable=0)");
            if (dupBTSCourse.Length == 0) {
                // no duplicate bts
                // Save to DB
                c.UpdateToDB(db);
                db.Close();
            }



        }

        protected void DoDeleteCourse(string courseID)
        {
            Course t = new Course();
            t._courseID = Int32.Parse(courseID);
            DBManager db = null;
            try {
                db = new MySQLDBManager(Config.DB_SERVER, Config.DB_NAME, Config.DB_USER, Config.DB_PASSWORD, Config.DB_CHAR_ENC);
                db.Connect();
                
                // Check if payment id paid all
                // If no, unable to delete this course
                // Else
                // Update status=1 (invalid)
                Payment payment = new Payment();
                if (payment.LoadFromDB(db, " course_id=" + courseID))
                {
                    if (payment._sumPaidCost < payment._sumAllCost) // not yet paid all  
                    {
                        errorText = "ยังมีเงินที่เบิกจ่ายไม่หมด " + (payment._sumAllCost - payment._sumPaidCost);
                        return;
                    }
                    else
                    {
                        payment._status = Payment.STATUS_INVALID;
                        payment.UpdateToDB(db);
                    }
                }
                
                // Delete course
                t.DeleteToDB(db);
            } finally {
                if (db!=null) db.Close();
            }
        }
    }
}
