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


    public partial class RegistrationManage : System.Web.UI.Page
    {
        public string actPage = "list";
        public StringBuilder outBuf = new StringBuilder();
        public StringBuilder outBuf2 = new StringBuilder();
        public String targetID;
        public Registration theReg; 
        public List<Registration> listRegistration;
        public Teacher[] listTeacher;

        public string filterRegistration = "0";

        public string errorText = "";
        public string msgText = "";
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
                    DoListRegistration(qSearch, isNewSearch);
                }            
                else if (actPage.Equals("edit"))
                {
                    DoEditRegistration(targetID);
                }                
                else if (actPage.Equals("edit_submit"))
                {
                    targetID = Request["targetID"];
                    DoEditSubmitRegistration(targetID);
                    Response.Redirect("RegistrationManage.aspx?actPage=edit&targetID=" + targetID + "&msgText=" + msgText);
                    //Response.Redirect("RegistrationManage.aspx?actPage=init_print&targetId=" + registrationID);
                   
                }
                else if (actPage.Equals("refund"))
                {
                    targetID = Request["targetID"];
                    DoRefund(targetID);
                    Response.Redirect("RegistrationManage.aspx?actPage=edit&targetID=" + targetID + "&msgText=" + msgText);
                    //Response.Redirect("RegistrationManage.aspx?actPage=init_print&targetId=" + registrationID);

                }
                else if (actPage.Equals("init_print_card"))
                {
                    targetID = Request["targetID"];
                    outBuf = new StringBuilder();
                    outBuf.Append(DoInitPrinRegistrationCard(targetID));
                    Session[SessionVar.PRINT_INFO] = new StringBuilder(outBuf.ToString());
                }
                else if (actPage.Equals("init_print_receipt"))
                {
                    targetID = Request["targetID"];
                    outBuf = new StringBuilder();
                    outBuf.Append(DoInitPrinRegistrationReceipt(targetID, "สำหรับนักเรียน"));
                    outBuf.Append(DoInitPrinRegistrationReceipt(targetID, "สำหรับโรงเรียน"));
                    Session[SessionVar.PRINT_INFO] = new StringBuilder(outBuf.ToString());
                }
                else if (actPage.Equals("init_print_all"))
                {
                    targetID = Request["targetID"];
                    outBuf = new StringBuilder();

                    PageBreaker pb = new PageBreaker(Config.PAGE_BREAK_MAX);

                    StringBuilder tmpBuf = DoInitPrinRegistrationCard(targetID);
                    pb.Print(outBuf, tmpBuf, Config.PAGE_BREAK_CARD);
                    outBuf.AppendLine("<br>");
                    
                    tmpBuf = DoInitPrinRegistrationReceipt(targetID, "ต้นฉบับ");
                    pb.Print(outBuf, tmpBuf, Config.PAGE_BREAK_RECEIPT);
                    outBuf.AppendLine("<br>");
                    tmpBuf = DoInitPrinRegistrationReceipt(targetID, "สำเนา");
                    pb.Print(outBuf, tmpBuf, Config.PAGE_BREAK_RECEIPT);

                    Session[SessionVar.PRINT_INFO] = new StringBuilder(outBuf.ToString());
                }


        }
        protected void DoEditSubmitRegistration(string regisID)
        {           

            string paidMethod = Request.Form.Get("paid_method");
            string note = Request.Form.Get("note");

            DBManager db = new MySQLDBManager(Config.DB_SERVER, Config.DB_NAME, Config.DB_USER, Config.DB_PASSWORD, Config.DB_CHAR_ENC);
            db.Connect();
            db.BeginTransaction(IsolationLevel.ReadCommitted);

            theReg = new Registration();
            theReg.LoadFromDB(db, " regis_id=" + regisID);


            // Save to DB
            theReg.UpdateToDB(db);

            db.Commit();
            db.Close();

            msgText = "แก้ไขข้อมูลเรียบร้อย";
        }
    

        protected void DoRefund(string regisID)
        {
            int status = Int32.Parse(Request.Form.Get("status"));
            int refundCost = Int32.Parse(Request.Form.Get("refund_cost"));
            string paidMethod = Request.Form.Get("paid_method");
            string note = Request.Form.Get("note");


            DBManager db = new MySQLDBManager(Config.DB_SERVER, Config.DB_NAME, Config.DB_USER, Config.DB_PASSWORD, Config.DB_CHAR_ENC);
            db.Connect();
            db.BeginTransaction(IsolationLevel.ReadCommitted);

            theReg = new Registration();
            theReg._status = status;
            theReg.LoadFromDB(db, " regis_id=" + regisID);
            theReg.LoadCourse(db);

            // TODO: Check if the fund is paid to teacher?
            if (refundCost <= theReg._discountedCost)
            {
                theReg._discountedCost -= refundCost;                
            }
            theReg._status = status;
            theReg._paidMethod = Int32.Parse(paidMethod);
            theReg._note = note;

            // Save to DB
            theReg.UpdateToDB(db);

            // Update payment
            Payment.UpdatePaymentByCourse(db, theReg._course);

            db.Commit();
            db.Close();

            if (refundCost > 0)
            {
                msgText = "คืนเงิน " + refundCost + " บาท เรียบร้อยแล้ว คงเหลือเงิน " + theReg._discountedCost + " บาท";
            } else {
                msgText = "แก้ไขข้อมูลเรียบร้อย";
            }
        }

        protected void DoEditRegistration(string regisID)
        {
            DBManager db = new MySQLDBManager(Config.DB_SERVER, Config.DB_NAME, Config.DB_USER, Config.DB_PASSWORD, Config.DB_CHAR_ENC);
            db.Connect();

            theReg = new Registration();
            theReg.LoadFromDB(db, " regis_id=" + regisID);
            theReg.LoadCourse(db);
            theReg.LoadStudent(db);
            theReg.LoadBranch(db);
//            Branch branch = new Branch();
//            branch.LoadFromDB(db, " branch_id=" + theReg._branchID);
            
            // Generate HTML content
            TextReader reader = new StreamReader(Config.PATH_APP_ROOT + "\\template\\registration_edit.htm");
            String templateContent = reader.ReadToEnd();
            reader.Close();

            string promotionTxt = theReg._promotionID > 0 ? 
                "<a href=\"PromotionManage.aspx?actPage=view&targetID="+ theReg._promotionID + "\" >" + Promotion.GetPromotionID(theReg._promotionID) + " </a>" : "-";

            String htmlContent =
                String.Format(templateContent
                    , theReg.GetRegisTransactionID()
                    , Registration.GetRegistrationID(theReg._regisID)
                    , "<a href=\"CourseManage.aspx?actPage=view&targetID=" + theReg._course._courseID + "\" >" + theReg._course._btsCourseID + " " + theReg._course._courseName + "</a>"
                    , promotionTxt
                    , "<a href=\"StudentManage.aspx?actPage=view&targetID=" + theReg._studentID + "\" >" + Student.GetStudentID(theReg._student._studentID) + " " + theReg._student._firstname + " " + theReg._student._surname + "</a>"
                    , StringUtil.ConvertYearToEng(theReg._regisdate, "yyyy/MM/dd HH:mm:ss")
                    , StringUtil.ConvertYearToEng(theReg._paiddate, "yyyy/MM/dd")
                    , theReg._branch._branchName
                    , StringUtil.Int2StrComma(theReg._fullCost)
                    , StringUtil.Int2StrComma(theReg._discountedCost)
                    , Registration.GetPaidMethodText(theReg._paidMethod.ToString())
                    , theReg._seatNo
                    , theReg._username
                    , Registration.GetStatusText(theReg._status)
                    );

            outBuf.Append(htmlContent);
            
            db.Close();
        }

        protected StringBuilder DoInitPrinRegistrationCard(string regisID)
        {
            DBManager db = new MySQLDBManager(Config.DB_SERVER, Config.DB_NAME, Config.DB_USER, Config.DB_PASSWORD, Config.DB_CHAR_ENC);
            db.Connect();
            StringBuilder tmpBuf = Registration.PrintCard(db, Int32.Parse(regisID));
            db.Close();
            return tmpBuf;
        }

        protected StringBuilder DoInitPrinRegistrationReceipt(string regisID, string title)
        {
            DBManager db = new MySQLDBManager(Config.DB_SERVER, Config.DB_NAME, Config.DB_USER, Config.DB_PASSWORD, Config.DB_CHAR_ENC);
            db.Connect();
            StringBuilder tmpBuf = Registration.PrintReceipt(db, Int32.Parse(regisID), title); 
            db.Close();
            return tmpBuf;
        }

        protected void DoListRegistration(string searchStr, bool isNewSearch)
        {
            // get Page
            int pg = 1;
            if ((!isNewSearch) && (Request["pg"]!=null)) pg = Int32.Parse(Request["pg"]);

            string[,] bgclass = new string[,] { { "class=\"spec\"", "class=\"td1\"", "class=\"td1_grey\"" }, { "class=\"specalt\"", "class=\"alt\"", "class=\"td1_grey\"" } };
            string grey = "class=\"thspec_grey\"";

            listRegistration = new List<Registration>();
            DBManager db = new MySQLDBManager(Config.DB_SERVER, Config.DB_NAME, Config.DB_USER, Config.DB_PASSWORD, Config.DB_CHAR_ENC);
            db.Connect();

            string qSearchSQL = Registration.GetQSearchSQL(searchStr);
            if (qSearchSQL.Trim().Length > 0) qSearchSQL = " WHERE " + qSearchSQL;

            // add join condition
            qSearchSQL = qSearchSQL + ((qSearchSQL.Trim().Length > 0) ? " AND " : " WHERE ")
                    + " rg.student_id=st.student_id AND rg.course_id=c.course_id ";



            int numRec = db.QueryCount("SELECT Count(*) FROM registration rg, student st, course c " + qSearchSQL);

            OdbcDataReader reader = db.Query("SELECT rg.*, st.firstname as student_firstname, st.surname as student_surname, c.bts_course_id as bts_course_id, c.course_name FROM registration rg, student st, course c "
                + qSearchSQL + " ORDER BY regis_id desc " + " LIMIT " + Config.TBRECORD_PER_PAGE + " OFFSET " + (((pg - 1) * Config.TBRECORD_PER_PAGE)));
            
            int i = 0;
            int j = 0;
            int currentRegisTransactionID = 0;
            while (reader.Read())   
            {
                Registration reg = Registration.CreateForm(reader);


                if (reg._transactionID != currentRegisTransactionID)
                {
                    currentRegisTransactionID = reg._transactionID;
                    j++;

                    reg.LoadBranch(db);
                    outBuf.Append("<tr>");
                    outBuf.Append("<th colspan=9 scope=\"row\" abbr=\"Model\" " + bgclass[j % 2, 0] + "><b>" + reg.GetRegisTransactionID() + "</b></th>");
                    outBuf.Append("</tr>\n");
                    
                }

                string studentInfo = "<a href=\"StudentManage.aspx?actPage=view&targetID="+reg._studentID+"\" >" + Student.GetStudentID(reg._studentID) +" "+ reg._studentFirstname + " " + reg._studentSurname + "</a>";
                string courseInfo = "<a href=\"CourseManage.aspx?actPage=view&targetID=" + reg._courseID + "\" >" + reg._btsCourseID + " " + reg._courseName + "</a>";
                string costInfo = StringUtil.Int2StrComma(reg._discountedCost);
                string statusInfo = Registration.GetStatusText(reg._status);
                if (reg._status > 0) {
                    costInfo = "<font color=red>" + costInfo + "</font>"; 
                    statusInfo = "<font color=red>" + statusInfo + "</font>"; 
                }

                outBuf.Append("<tr>");
                outBuf.Append("<th scope=\"row\" abbr=\"Model\" " + bgclass[j % 2, 0] + ">" +  Registration.GetRegistrationID(reg._regisID) + "</th>");
                outBuf.Append("<td " + bgclass[j % 2, 1] + "  align=center>" + reg._regisdate.ToString("dd/MM/yyyy HH:mm", ci) + "&nbsp</td>");
                outBuf.Append("<td " + bgclass[j % 2, 1] + "  align=center>" + reg._paiddate.ToString("dd/MM/yyyy", ci) + "&nbsp</td>");
                outBuf.Append("<td " + bgclass[j % 2, 1] + "  align=left>" + courseInfo + "</td>");                
                outBuf.Append("<td " + bgclass[j % 2, 1] + "  align=left>" + studentInfo + "</td>");
                outBuf.Append("<td " + bgclass[j % 2, 1] + "  align=right>" + costInfo + "&nbsp</td>");
                outBuf.Append("<td " + bgclass[j % 2, 1] + "  align=center>" + statusInfo + "</td>");
                outBuf.Append("<td " + bgclass[j % 2, 1] + "  align=left>" + reg._note + "</td>");

                outBuf.Append("<td " + bgclass[j % 2, 1] + "  align=center>");
                outBuf.Append("<a href=\"javascript:setVal('actPage','edit');setVal('targetID','" + reg._regisID + "');doSubmit()\"><img src=\"img/sys/edit.gif\" border=0 alt=\"Edit\"></a>&nbsp");
                outBuf.Append("<a href=\"javascript:setVal('actPage','init_print_all');setVal('targetID','" + reg._regisID + "');doSubmit()\"><img src=\"img/sys/print.gif\" border=0 alt=\"พิมพ์ทุกอย่าง\"></a>&nbsp");

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
            outBuf2.Append(String.Format("<a href=\"RegistrationManage.aspx?pg={0}&qsearch={1}\">{2}</a><< ", "1", searchStr, "First"));
            for (i = pg - 10; i <= pg + 10; i++)
            {
                if ((i <= 0) || (i > maxpg))
                {
                    continue;
                }
                if (i == pg) { outBuf2.Append("<b>"+i+"</b> "); }
                else {
                    outBuf2.Append(String.Format("<a href=\"RegistrationManage.aspx?pg={0}&qsearch={1}\">{0}</a> ", i.ToString(), searchStr));
                }

            }
            outBuf2.Append(String.Format(" >><a href=\"RegistrationManage.aspx?pg={0}&qsearch={1}\">{2}</a> ", maxpg.ToString(), searchStr, "Last"));
        }
   
    }
}
