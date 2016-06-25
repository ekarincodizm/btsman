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


    public partial class PaymentManage : System.Web.UI.Page
    {
        public string actPage = "list";
        public StringBuilder outBuf = new StringBuilder();
        public StringBuilder outBuf2 = new StringBuilder();
        public String targetID;
        public Payment thePayment;
        public List<Payment> listPayment;
        public Teacher[] listTeacher;
        public Teacher theTeacher;

        public string filterPayment = "0";

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
                    DoListPayment(qSearch, isNewSearch);
                }
                else if (actPage.Equals("view"))
                {
                    DoViewPayment(targetID);
                }
                else if (actPage.Equals("list_by_teacher"))
                {
                 //   DoListByTeacher();
                }
                else if (actPage.Equals("preview_multi"))
                {
                    targetID = Request["teacher_id"];
                    //DoPreviewMultiPayment(targetID);
                }
                else if (actPage.Equals("paid_submit"))
                {
                    targetID = Request["targetID"];
                    int paymentID = DoPaidSubmitPayment(targetID);
                    Response.Redirect("PaymentManage.aspx?actPage=init_print&targetId=" + paymentID);

                }
                else if (actPage.Equals("init_print"))
                {
                    targetID = Request["targetID"];
                    DoInitPrintReceiptPaymentData(targetID);
                    Session[SessionVar.PRINT_INFO] = new StringBuilder(outBuf.ToString());
                }/*
                else if (actPage.Equals("delete"))
                {
                    targetID = Request["targetID"];
                    DoDeletePayment(targetID);
                    Response.Redirect("PaymentManage.aspx");
                }*/
        }

        protected void DoInitPrintReceiptPaymentData(string paymentID)
        {
            DBManager db = new MySQLDBManager(Config.DB_SERVER, Config.DB_NAME, Config.DB_USER, Config.DB_PASSWORD, Config.DB_CHAR_ENC);
            db.Connect();

            PaymentHistory pm = new PaymentHistory();
            pm.LoadFromDB(db, " payment_id="+paymentID);
            pm.LoadCourse(db);
            pm._course.LoadTeacher(db);
            pm._course.LoadPaidGroup(db);
            // load teacher in this group
            Teacher[] listTeacher = pm._course._paidGroup.LoadMemberTeachers(db);
            pm.LoadReceiver(db);
            // preload all branches
            Dictionary<int, Branch> branches = Branch.LoadListFromDBAsMap(db, "");

            // Load PaymentHistory BEFORE this
            PaymentHistory[] pmList =  PaymentHistory.LoadListFromDB(db, " WHERE course_id='" + pm._courseID + "' and payment_id<='" + pm._paymentID + "' ORDER BY payment_id");


            // Construct Teacher List
            StringBuilder teachTxt = new StringBuilder();
            for (int i = 0; i < listTeacher.Length; i++)
            {
                string link = "TeacherManage.aspx?actPage=edit&targetID=" + listTeacher[i]._teacherID;
                teachTxt.Append(listTeacher[i]._firstname + " " + listTeacher[i]._surname + "<br>");
            }

            // Construct Paid history
            StringBuilder phTxt = new StringBuilder();
            for (int i=0;i<pmList.Length;i++)
            {
                PaymentHistory ph = pmList[i];
                ph.LoadReceiver(db);
                ph.LoadUser(db);
                Branch b = branches[ph._branchID];
                string link = "TeacherManage.aspx?actPage=edit&targetID=" + ph._receiverTeacherID;
                phTxt.AppendLine("<tr><td align=center>" + PaymentHistory.GetPaymentHistoryID(ph._paymentID) + "</td>");
                phTxt.AppendLine("<td align=center>" + StringUtil.ConvertYearToEng(ph._paidDate, "yyyy/MM/dd HH:mm:ss ") + "</td>");
                phTxt.AppendLine("<td align=center>" + StringUtil.Int2StrComma(ph._paidCost) + "</td>");
                phTxt.AppendLine("<td align=center>" + ph._receiverTeacher._firstname + " " + ph._receiverTeacher._surname + "</td>");                
                phTxt.AppendLine("<td align=center>" + ph._user._firstname + " "+ ph._user._surname+ "</td>");
                phTxt.AppendLine("<td align=center>" + b._branchName + "</td>");
            }

            // User
            AppUser user = new AppUser();
            user.LoadFromDB(db, " username='"+ pm._username +"'");

            // Generate HTML content
            TextReader reader = new StreamReader(Config.PATH_APP_ROOT + "\\template\\payment_print.htm");
            String templateContent = reader.ReadToEnd();
            reader.Close();

            String htmlContent =
                String.Format(templateContent
                    , StringUtil.ConvertYearToEng(pm._paidDate, "yyyy/MM/dd HH:mm:ss")
                    , pm._receiverTeacher._firstname + " " + pm._receiverTeacher._surname
                    , pm._course._btsCourseID + " \"" + pm._course._courseName + "\""
                    , StringUtil.Int2StrComma(pm._paidCost)
                    , pm._course._teacher._firstname + " " + pm._course._teacher._surname
                    , PaidGroup.GetPaidGroupID(pm._course._paidGroup._currentRound)
                    , teachTxt.ToString()
                    , StringUtil.Int2StrComma(pm._sumMaxPayable)
                    , StringUtil.Int2StrComma(pm._sumPaidCost + pm._paidCost)
                    , phTxt.ToString()
                    , user._firstname + " " + user._surname
                    );

            outBuf.Append(htmlContent);

            db.Close();
        }


        protected int DoPaidSubmitPayment(string courseID)
        {             
            string paidCost = Request["paid_cost"];
            string receiverTeacherID = Request["receiver_teacher_id"];
            AppUser user = (AppUser)Session[SessionVar.USER];

            DBManager db = new MySQLDBManager(Config.DB_SERVER, Config.DB_NAME, Config.DB_USER, Config.DB_PASSWORD, Config.DB_CHAR_ENC);
            db.Connect();
            db.BeginTransaction(IsolationLevel.ReadCommitted);

            Payment pay = new Payment();
            pay.LoadFromDB(db, " course_id="+courseID);
            pay.LoadCourse(db);

            PaidGroup pg = new PaidGroup();
            pg.LoadFromDB(db, " paid_group_id="+pay._course._paidGroupID);

            // Add history
            PaymentHistory ph = new PaymentHistory(pay, pg, Int32.Parse(paidCost), Int32.Parse(receiverTeacherID), user);
            ph.AddToDB(db);

            // refresh Payment record
            Payment.UpdatePaymentByCourse(db, Int32.Parse(courseID));
            db.Commit();

            // find latest payment
            pay.LoadHistory(db);
            int latestPaymentID = pay._historyList.Last.Value._paymentID;

            db.Close();

            return latestPaymentID;

        }

        protected void DoViewPayment(string courseID)
        {
            DBManager db = new MySQLDBManager(Config.DB_SERVER, Config.DB_NAME, Config.DB_USER, Config.DB_PASSWORD, Config.DB_CHAR_ENC);
            db.Connect();

            thePayment = new Payment();
            thePayment.LoadFromDB(db, " course_id=" + courseID);
            thePayment.LoadHistory(db);
            thePayment.LoadCourse(db);
            Course c = thePayment._course;
            c.LoadTeacher(db);
            c.LoadPaidGroup(db);
            // preload all branches
            Dictionary<int, Branch> branches = Branch.LoadListFromDBAsMap(db, "");


            PaidGroup pg = new PaidGroup();
            pg.LoadFromDB(db, " paid_group_id="+c._paidGroupID);
            // Load teachers in this group
            listTeacher = pg.LoadMemberTeachers(db);
            
            // Generate HTML content
            TextReader reader = new StreamReader(Config.PATH_APP_ROOT + "\\template\\payment_view.htm");
            String templateContent = reader.ReadToEnd();
            reader.Close();

            // Construct Teacher List
            StringBuilder teachTxt = new StringBuilder();
            for (int i=0;i<listTeacher.Length;i++){
                string link = "TeacherManage.aspx?actPage=edit&targetID=" + listTeacher[i]._teacherID;
                teachTxt.Append("<a href=\"" + link + "\" >" + listTeacher[i]._firstname + " " + listTeacher[i]._surname + "</a><br>");
            }

            // Construct RaitInfo List
            StringBuilder rinfoTxt = new StringBuilder();
            for (int i = 0; i < pg._rateInfo.Length; i++)
            {
                rinfoTxt.Append("มากกว่า " + StringUtil.Int2StrComma(pg._rateInfo[i]._bound) + " บาท ได้รับ " + pg._rateInfo[i]._percent + " %<br>");
            }

            // Construct Paid history
            StringBuilder phTxt = new StringBuilder();
            foreach (PaymentHistory ph in thePayment._historyList)
            {
                ph.LoadReceiver(db);
                ph.LoadUser(db);
                Branch b = branches[ph._branchID];
                string link = "TeacherManage.aspx?actPage=edit&targetID=" + ph._receiverTeacherID;
                phTxt.AppendLine("<tr><td align=center>" + PaymentHistory.GetPaymentHistoryID(ph._paymentID) + "</td>");
                phTxt.AppendLine("<td align=center>" + StringUtil.ConvertYearToEng(ph._paidDate, "yyyy/MM/dd HH:mm:ss") + "</td>");
                phTxt.AppendLine("<td align=center>" + StringUtil.Int2StrComma(ph._paidCost) + "</td>");
                phTxt.AppendLine("<td align=center><a href=\""+link+"\" >" + ph._receiverTeacher._firstname + " " + ph._receiverTeacher._surname + "</a></td>");
                phTxt.AppendLine("<td align=center>" + ph._user._firstname + " " + ph._user._surname + "</td>");
                phTxt.AppendLine("<td align=center>" + b._branchName + "</td>");
                phTxt.AppendLine("<td align=center><a href=\"javascript:doInitPrint(" + ph._paymentID + ")\"><img src=\"" + Config.URL_PIC_SYS + "/view.gif\" border=0></a> </td>");
            }

            String htmlContent =
                String.Format(templateContent
                    , c._courseName
                    , "<a href=\"TeacherManage.aspx?actPage=edit&targetID="+c._teacherID + "\" >" + c._teacher._firstname + " " + c._teacher._surname + "</a>"
                    , PaidGroup.GetPaidGroupID(c._paidGroupID) + " " + c._paidGroup._name
                    , teachTxt.ToString()
                    , rinfoTxt.ToString()
                    , StringUtil.Int2StrComma(thePayment._sumAllCost)
                    , StringUtil.Int2StrComma(thePayment._sumMaxPayable)
                    , StringUtil.Int2StrComma(thePayment._sumPaidCost)
                    , StringUtil.Int2StrComma(thePayment._sumMaxPayable - thePayment._sumPaidCost)
                    , phTxt.ToString()
                    );

            outBuf.Append(htmlContent);

            db.Close();
        }


        protected void DoListPayment(string searchStr, bool isNewSearch)
        {
            // get Page
            int pg = 1;
            if ((!isNewSearch) && (Request["pg"] != null)) pg = Int32.Parse(Request["pg"]);

            string[,] bgclass = new string[,] { { "class=\"spec\"", "class=\"td1\"", "class=\"td1_grey\"", "class=\"td1_red\"" }, { "class=\"specalt\"", "class=\"alt\"", "class=\"td1_grey\"", "class=\"td1_red\"" } };
            string grey = "class=\"thspec_grey\"";
            string red = "class=\"thspec_red\"";

            listPayment = new List<Payment>();
            DBManager db = new MySQLDBManager(Config.DB_SERVER, Config.DB_NAME, Config.DB_USER, Config.DB_PASSWORD, Config.DB_CHAR_ENC);
            db.Connect();

            AppUser user = (AppUser)Session[SessionVar.USER];
            filterPayment = Request["filter_payment"];
            if (filterPayment == null) filterPayment = "0"; // by default


            string whereSQL = " pm.course_id=c.course_id AND c.paid_group_id=pg.paid_group_id AND c.room_id = r.room_id ";
            // only admin can view all payment
            if (!user.IsAdmin())
            {
                whereSQL = whereSQL + " AND r.branch_id=" + user._branchID;
            }
            // filter only payable courses
            if (filterPayment.Equals("0"))
            {
                whereSQL = whereSQL + " AND sum_max_payable<>sum_paid_cost ";
            }            

            int numRec = db.QueryCount("SELECT Count(*) FROM payment pm, course c, paid_group pg, room r WHERE " + whereSQL);

            OdbcDataReader reader = db.Query("SELECT pm.course_id as course_id ,c.bts_course_id as bts_course_id ,c.course_name as course_name, c.paid_group_id as paid_group_id"
                + ", pm.sum_all_cost as sum_all_cost, pm.sum_max_payable as sum_max_payable, pm.sum_paid_cost as sum_paid_cost "
                + "FROM payment pm, course c, paid_group pg, room r WHERE "
                + whereSQL 
                + " ORDER BY c.paid_group_id, c.course_id "
                + " LIMIT " + Config.TBRECORD_PER_PAGE + " OFFSET " + (((pg - 1) * Config.TBRECORD_PER_PAGE)));
            int i = 0;
            PaidGroup currentPaidGroup = new PaidGroup();
            while (reader.Read())
            {
                Payment payment = Payment.CreateForm(reader);

                string chbname = "course" + payment._courseID;

                int sumPayable = payment._sumMaxPayable - payment._sumPaidCost;
                int n = 1;
                if (sumPayable == 0) n = 2;
                else if (sumPayable < 0) n = 3;

                string bgcolor = (sumPayable ==0) ? grey : red;


                // print paidgroup info
                if (payment._paidGroupID != currentPaidGroup._paidGroupID)
                {
                    currentPaidGroup._paidGroupID = payment._paidGroupID;
                    currentPaidGroup.LoadFromDB(db);

                    outBuf.Append("<tr><td colspan=6 " + bgclass[1, n] + "><b>");
                    outBuf.Append(PaidGroup.GetPaidGroupID(currentPaidGroup._paidGroupID)+" "+currentPaidGroup._name);
                    outBuf.Append("</b></td></tr>");
                }

                outBuf.Append("<tr>");
                //outBuf.Append("<th scope=\"row\" abbr=\"Model\" " + (sumPayable > 0 ? bgclass[i % 2, 0] : bgcolor) + ">" + payment._btsCourseID + "</th>");
                //outBuf.Append("<td " + bgclass[0, n] + " align=center ><input type=\"checkbox\" name=\"" + chbname + "\" id=\"" + chbname + "\" />" + "</td>");
                outBuf.Append("<td " + bgclass[0, n] + "  ><a href=\"CourseManage.aspx?actPage=view&targetID=" + payment._courseID + "\" >" + payment._btsCourseID + " " + payment._courseName + "</a></td>");
                outBuf.Append("<td " + bgclass[0, n] + " align=center ><b>" + StringUtil.Int2StrComma(payment._sumAllCost) + "</b></td>");
                outBuf.Append("<td " + bgclass[0, n] + " align=center ><b><font color=blue>" + StringUtil.Int2StrComma(payment._sumMaxPayable) + "</font></b></td>");
                outBuf.Append("<td " + bgclass[0, n] + " align=center ><b><font color=red>" + StringUtil.Int2StrComma(payment._sumPaidCost) + "</font></b></td>");
                outBuf.Append("<td " + bgclass[0, n] + " align=center ><b><font color=green>" + StringUtil.Int2StrComma(sumPayable) + "</font></b></td>");
/*                outBuf.Append("<td " + bgclass[i % 2, n] + "  align=center>" 
                    + payment._lastPaidDate.ToString("dd/MM/yyyy HH:mm", ci) +  "&nbsp</td>");
                */
                outBuf.Append("<td " + bgclass[0, n] + "  align=center>&nbsp");
                outBuf.Append("<a href=\"javascript:setVal('actPage','view');setVal('targetID','" + payment._courseID + "');doSubmit()\"><img src=\"img/sys/view.gif\" border=0 alt=\"View detail\"></a>&nbsp");
//                outBuf.Append("<a href=\"javascript:if (confirm('Delete this payment?')) { setVal('actPage','delete');setVal('targetID','" + payment._paymentID + "');doSubmit(); }\"><img src=\"img/sys/delete.gif\" border=0 alt=\"Delete\"></a>&nbsp");
 
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
                if (i == pg) { outBuf2.Append("<b>"+i+"</b> "); }
                else {
                    outBuf2.Append(String.Format("<a href=\"PaymentManage.aspx?pg={0}&filter_payment={1}&qsearch={1}\">{0}</a> ", i.ToString(), filterPayment, searchStr));
                }

            }
            
          //  <a href="#">1</a> <b>2</b> <a href="#">3</a> <a href="#">4</a>


        }



        protected void DoListPaymentByTeacher(string teacherID, string searchStr, bool isNewSearch)
        {

            DBManager db;
            db = new MySQLDBManager(Config.DB_SERVER, Config.DB_NAME, Config.DB_USER, Config.DB_PASSWORD, Config.DB_CHAR_ENC);
            db.Connect();

            listTeacher = Teacher.LoadListFromDB(db, " WHERE is_active=1 ORDER BY firstname ");

            if (teacherID == null) return;


            // get Page
            int pg = 1;
            if ((!isNewSearch) && (Request["pg"] != null)) pg = Int32.Parse(Request["pg"]);

            string[,] bgclass = new string[,] { { "class=\"spec\"", "class=\"td1\"", "class=\"td1_grey\"", "class=\"td1_red\"" }, { "class=\"specalt\"", "class=\"alt\"", "class=\"td1_grey\"", "class=\"td1_red\"" } };
            string grey = "class=\"thspec_grey\"";
            string red = "class=\"thspec_red\"";

            listPayment = new List<Payment>();

            AppUser user = (AppUser)Session[SessionVar.USER];
            filterPayment = Request["filter_payment"];
            if (filterPayment == null) filterPayment = "0"; // by default


            string whereSQL = " pm.course_id=c.course_id AND c.teacher_id=t.teacher_id AND c.room_id = r.room_id ";
            // only admin can view all payment
            if (!user.IsAdmin())
            {
                whereSQL = whereSQL + " AND r.branch_id=" + user._branchID;
            }
            // filter only payable courses
            if (filterPayment.Equals("0"))
            {
                whereSQL = whereSQL + " AND sum_max_payable<>sum_paid_cost ";
            }

            int numRec = db.QueryCount("SELECT Count(*) FROM payment pm, course c, teacher t, room r WHERE " + whereSQL);

            OdbcDataReader reader = db.Query("SELECT pm.course_id as course_id ,c.bts_course_id as bts_course_id ,c.course_name as course_name, t.paid_group_id as paid_group_id"
                + ", pm.sum_all_cost as sum_all_cost, pm.sum_max_payable as sum_max_payable, pm.sum_paid_cost as sum_paid_cost "
                + "FROM payment pm, course c, teacher t, room r WHERE "
                + whereSQL + " LIMIT " + Config.TBRECORD_PER_PAGE + " OFFSET " + (((pg - 1) * Config.TBRECORD_PER_PAGE)));
            int i = 0;
            while (reader.Read())
            {
                Payment payment = Payment.CreateForm(reader);

                string chbname = "course" + payment._courseID;

                int sumPayable = payment._sumMaxPayable - payment._sumPaidCost;
                int n = 1;
                if (sumPayable == 0) n = 2;
                else if (sumPayable < 0) n = 3;

                string bgcolor = (sumPayable == 0) ? grey : red;

                outBuf.Append("<tr>");
                outBuf.Append("<th scope=\"row\" abbr=\"Model\" " + (sumPayable > 0 ? bgclass[i % 2, 0] : bgcolor) + ">" + payment._btsCourseID + "</th>");
                outBuf.Append("<td " + bgclass[i % 2, n] + " align=center ><input type=\"checkbox\" name=\"" + chbname + "\" id=\"" + chbname + "\" />" + "</td>");
                outBuf.Append("<td " + bgclass[i % 2, n] + "  ><a href=\"CourseManage.aspx?actPage=view&targetID=" + payment._courseID + "\" >" + payment._courseName + "</a></td>");
                outBuf.Append("<td " + bgclass[i % 2, n] + " align=center >" + PaidGroup.GetPaidGroupID(payment._paidGroupID) + "</td>");
                outBuf.Append("<td " + bgclass[i % 2, n] + " align=center ><b>" + StringUtil.Int2StrComma(payment._sumAllCost) + "</b></td>");
                outBuf.Append("<td " + bgclass[i % 2, n] + " align=center ><b><font color=blue>" + StringUtil.Int2StrComma(payment._sumMaxPayable) + "</font></b></td>");
                outBuf.Append("<td " + bgclass[i % 2, n] + " align=center ><b><font color=red>" + StringUtil.Int2StrComma(payment._sumPaidCost) + "</font></b></td>");
                outBuf.Append("<td " + bgclass[i % 2, n] + " align=center ><b><font color=green>" + StringUtil.Int2StrComma(sumPayable) + "</font></b></td>");
                /*                outBuf.Append("<td " + bgclass[i % 2, n] + "  align=center>" 
                                    + payment._lastPaidDate.ToString("dd/MM/yyyy HH:mm", ci) +  "&nbsp</td>");
                                */
                outBuf.Append("<td " + bgclass[i % 2, n] + "  align=center>&nbsp");
                outBuf.Append("<a href=\"javascript:setVal('actPage','view');setVal('targetID','" + payment._courseID + "');doSubmit()\"><img src=\"img/sys/view.gif\" border=0 alt=\"View detail\"></a>&nbsp");
                //                outBuf.Append("<a href=\"javascript:if (confirm('Delete this payment?')) { setVal('actPage','delete');setVal('targetID','" + payment._paymentID + "');doSubmit(); }\"><img src=\"img/sys/delete.gif\" border=0 alt=\"Delete\"></a>&nbsp");

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
                    outBuf2.Append(String.Format("<a href=\"PaymentManage.aspx?pg={0}&filter_payment={1}&qsearch={1}\">{0}</a> ", i.ToString(), filterPayment, searchStr));
                }

            }

            //  <a href="#">1</a> <b>2</b> <a href="#">3</a> <a href="#">4</a>


        }
        /*
        public void DoAddPayment()
        {
            DBManager db = new MySQLDBManager(Config.DB_SERVER, Config.DB_NAME, Config.DB_USER, Config.DB_PASSWORD, Config.DB_CHAR_ENC);
            roomList = Room.LoadListFromDBCustom(db, "SELECT r.room_id, r.name, b.branch_name as branch_name FROM room r, branch b WHERE r.branch_id=b.branch_id");
            teacherList = Teacher.LoadListFromDB(db, " ORDER BY firstname");
            db.Close();
        }
        
        protected void DoAddSubmitPayment()
        {
            Payment c = new Payment();
            
            // validate data
            c._paymentName = Request["payment_name"];
            c._paymentDesc = Request["payment_desc"];
            c._roomID = Int32.Parse(Request["room_id"]);
            c._teacherID = Int32.Parse(Request["teacher_id"]);            
            c._category = Request["category"];

            c._startdate = StringUtil.getDate(Request["startdate"]);
            c._enddate = StringUtil.getDate(Request["enddate"]);
            c._dayOfWeek = Request["day_of_week"];
            c._starttime = StringUtil.getTime(Request["starttime_h"], Request["starttime_m"]);
            c._endtime = StringUtil.getTime(Request["endtime_h"], Request["endtime_m"]);

            c._cost =Int32.Parse(Request["cost"]);
            c._seatLimit = Int32.Parse(Request["seat_limit"]);
            c._bankRegisLimit = Int32.Parse(Request["bank_regis_limit"]);



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

            // Save to DB
            DBManager db = new MySQLDBManager(Config.DB_SERVER, Config.DB_NAME, Config.DB_USER, Config.DB_PASSWORD, Config.DB_CHAR_ENC);
            db.Connect();
            c.AddToDB(db);
            db.Close();
        }

        public void DoEditPayment(string paymentID)
        {
            DBManager db = new MySQLDBManager(Config.DB_SERVER, Config.DB_NAME, Config.DB_USER, Config.DB_PASSWORD, Config.DB_CHAR_ENC);
            thePayment = new Payment();
            if (!thePayment.LoadFromDB(db, "payment_id=" + paymentID)) thePayment = null;

            roomList = Room.LoadListFromDBCustom(db, "SELECT r.room_id, r.name, b.branch_name as branch_name FROM room r, branch b WHERE r.branch_id=b.branch_id");
            teacherList = Teacher.LoadListFromDB(db, " ORDER BY firstname");

            db.Close();

        }

        protected void DoEditSubmitPayment(string paymentID)
        {
            Payment c = new Payment();

            // validate data
            c._paymentID = Int32.Parse(paymentID);
            c._paymentName = Request["payment_name"];
            c._paymentDesc = Request["payment_desc"];
            c._roomID = Int32.Parse(Request["room_id"]);
            c._teacherID = Int32.Parse(Request["teacher_id"]);
            c._category = Request["category"];

            c._startdate = StringUtil.getDate(Request["startdate"]);
            c._enddate = StringUtil.getDate(Request["enddate"]);
            c._dayOfWeek = Request["day_of_week"];
            c._starttime = StringUtil.getTime(Request["starttime_h"], Request["starttime_m"]);
            c._endtime = StringUtil.getTime(Request["endtime_h"], Request["endtime_m"]);

            c._cost = Int32.Parse(Request["cost"]);
            c._seatLimit = Int32.Parse(Request["seat_limit"]);
            c._bankRegisLimit = Int32.Parse(Request["bank_regis_limit"]);
            
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

            // Save to DB
            DBManager db = new MySQLDBManager(Config.DB_SERVER, Config.DB_NAME, Config.DB_USER, Config.DB_PASSWORD, Config.DB_CHAR_ENC);
            db.Connect();
            c.UpdateToDB(db);
            db.Close();
        }

        protected void DoDeletePayment(string paymentID)
        {
            Payment t = new Payment();
            t._paymentID = Int32.Parse(paymentID);

            DBManager db = new MySQLDBManager(Config.DB_SERVER, Config.DB_NAME, Config.DB_USER, Config.DB_PASSWORD, Config.DB_CHAR_ENC);
            db.Connect();
            t.DeleteToDB(db);
            db.Close();
        }
         */
    }
}
