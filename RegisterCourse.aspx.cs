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


    public partial class PromotionManage : System.Web.UI.Page
    {
        public string actPage = "list";
        public StringBuilder outBuf = new StringBuilder();
        public StringBuilder outBuf2 = new StringBuilder();
        public String targetID;
        public Promotion thePromotion;
        public List<Promotion> listPromotion;
        public int fullCost = 0;

        public string errorText = "";
        public CultureInfo ci = new CultureInfo("th-TH");
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


            if ((actPage == null) || (actPage.Trim().Length == 0) || (actPage.Equals("new")) || (actPage.Equals("select_course")))
            {
                if ((actPage == null) || (actPage.Trim().Length == 0) || (actPage.Equals("new")))
                {
                    // clear session data
                    Session[SessionVar.CURRENT_REGIS] = null;
                }
                
                // DoListPromotion(Request["qsearch"]);
            }
            else if (actPage.Equals("select_student"))
            {
                RegisTransaction reg = (RegisTransaction)Session[SessionVar.CURRENT_REGIS];
                if ((reg == null) || (reg._courses == null) || (reg._courses.Count == 0))
                {
                    actPage = "select_course";
                    return;
                }
            }
            else if (actPage.Equals("confirm_registration"))
            {
                RegisTransaction reg = (RegisTransaction)Session[SessionVar.CURRENT_REGIS];
                if ((reg == null) || (reg._student == null))
                {
                    actPage = "select_student";
                    return;
                }

                ProcessPrintAdditionalInfoForm(reg);
            }
            else if (actPage.Equals("add_new_student_submit"))
            {
                Student t =ProcessWizAddNewStudent();

                if (t == null)
                {

                    Response.Redirect("RegisterCourse.aspx?actPage=select_student&errorText="+errorText);
                    return;
                }

                // save to session
                RegisTransaction reg = (RegisTransaction)Session[SessionVar.CURRENT_REGIS];
                reg._student = t;
                reg._studentID = t._studentID;
                Session[SessionVar.CURRENT_REGIS] = reg;
                // list
                Response.Redirect("RegisterCourse.aspx?actPage=select_student");
            }
            else if (actPage.Equals("submit_registration"))
            {
                RegisTransaction reg = (RegisTransaction)Session[SessionVar.CURRENT_REGIS];
                if ((reg == null) || (reg._courses == null) || (reg._courses.Count == 0))
                {
                    errorText = "ข้อมูลการลงทะเบียนไม่สมบูรณ์ โปรดลองใหม่อีกครั้ง";
                    return;
                }

                ProcessRegistration(reg);
                // clear session data
                Session[SessionVar.CURRENT_REGIS] = null;
                Response.Redirect("RegisterCourse.aspx?actPage=registration_complete&targetId="+reg._transactionID);
            }
            else if (actPage.Equals("registration_complete"))
            {
                DoInitPrintAll(targetID);
            }
            else if (actPage.Equals("cancel_registration"))
            {
                // clear session data
                Session[SessionVar.CURRENT_REGIS] = null;
                Response.Redirect("Home.aspx");
            }

            
        }

        protected void DoInitPrintAll(string regisTransactionID)
        {
            DBManager db = new MySQLDBManager(Config.DB_SERVER, Config.DB_NAME, Config.DB_USER, Config.DB_PASSWORD, Config.DB_CHAR_ENC);
            db.Connect();

            outBuf = new StringBuilder();
            // find one of registration from list by transaction id
            AppUser user = (AppUser)Session[SessionVar.USER];
            Registration[] reg = Registration.LoadListFromDB(db, " WHERE transaction_id="+regisTransactionID+" AND branch_id="+ user._branchID +" ORDER BY regis_id ");

            // print
            PageBreaker pb = new PageBreaker(Config.PAGE_BREAK_MAX);
            StringBuilder tmpBuf = Registration.PrintCard(db, reg[0]._regisID);
            pb.Print(outBuf, tmpBuf, Config.PAGE_BREAK_CARD);
            outBuf.AppendLine("<br>");

            pb.Print(outBuf, Registration.PrintReceipt(db, reg[0]._regisID, "สำหรับนักเรียน"), Config.PAGE_BREAK_RECEIPT);
            outBuf.AppendLine("<br>");
            pb.Print(outBuf, Registration.PrintReceipt(db, reg[0]._regisID, "สำหรับโรงเรียน"), Config.PAGE_BREAK_RECEIPT);

            db.Close();
            // Save to session
            Session[SessionVar.PRINT_INFO] = new StringBuilder(outBuf.ToString());
        }

        protected void ProcessPrintAdditionalInfoForm(RegisTransaction reg)
        {            
            outBuf = new StringBuilder();
            foreach (Course c in reg._courses)
            {
                string seatName = "seat_no" + c._courseID;
                string noteName = "note" + c._courseID;
                outBuf.AppendLine("<tr><td align=left>"+ c._btsCourseID +"</td>");
                outBuf.AppendLine("<td align=center><input type=text class=\"txtbox1\" style=\"width:20px\" maxlength=\"10\" name=\"" + seatName + "\"></td>");
                outBuf.AppendLine("<td align=center><input type=text class=\"txtbox1\" style=\"width:150px\" maxlength=\"200\" name=\"" + noteName + "\"></td></tr>");
            }
        }

        // TODO: Check duplicated registration
        protected void ProcessRegistration(RegisTransaction reg)
        {
            AppUser user = (AppUser)Session[SessionVar.USER];

            // Collect additional variable
            // * User information comes from session NOT DB
            reg._username = user._username;
            reg._paidMethod = Int32.Parse(Request["paid_method"]);
            reg._branchID = Int32.Parse((String)Session[SessionVar.BRANCH_SELECTED]);
            reg._paiddate = StringUtil.getDate(Request["paid_date"]);

            // seat
            for (int i = 0; i < Request.Form.AllKeys.Length; i++)
            {
                if (Request.Form.AllKeys[i].StartsWith("seat_no"))
                {
                    string courseID = Request.Form.AllKeys[i].Substring(7);
                    string seatNo = Request[Request.Form.AllKeys[i]];
                    try
                    {
                        reg._seatNoMap.Add(courseID, seatNo);
                    }
                    catch (Exception ex) { }
                    continue;
                }
                if (Request.Form.AllKeys[i].StartsWith("note"))
                {
                    string courseID = Request.Form.AllKeys[i].Substring(4);
                    string note = Request[Request.Form.AllKeys[i]];
                    try
                    {
                        reg._noteMap.Add(courseID, note);
                    }
                    catch (Exception ex) { }
                    continue;
                }
            }

            DBManager db = new MySQLDBManager(Config.DB_SERVER, Config.DB_NAME, Config.DB_USER, Config.DB_PASSWORD, Config.DB_CHAR_ENC);
            db.Connect();
            reg.AddToDB(db);           
            db.Close();
        }

        protected Student ProcessWizAddNewStudent()
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
                    return null;
                }
            }


            // Do validation
            // Save to DB and read to get student id
            // Need to use transaction
            DBManager db = null;
            try
            {
                db = new MySQLDBManager(Config.DB_SERVER, Config.DB_NAME, Config.DB_USER, Config.DB_PASSWORD, Config.DB_CHAR_ENC);
                db.Connect();
                db.BeginTransaction(IsolationLevel.ReadCommitted);

                // validate
                // duplicate citizen id
                if (t._citizenID.Length > 0)
                {
                    int count = db.QueryCount("SELECT COUNT(*) FROM student WHERE citizen_id='" + t._citizenID + "'");
                    if (count > 0)
                    {
                        errorText = "รหัสบัตรประชาชน " + t._citizenID + " มีอยู่ในระบบแล้ว";
                        return null;
                    }
                }
                // Save to DB
                t.AddToDB(db);

                // Get just saved student
                Student savedStudent = new Student();
                savedStudent.LoadFromDBCustom(db, "SELECT * FROM student ORDER BY student_id DESC LIMIT 1");
                db.Commit();
                return savedStudent;
            }
            catch (Exception e)
            {
                errorText = "พบปัญหาบางประการ ข้อมูลไม่ถูกบันทึก";
                return null;
            }
            finally
            {
                db.Close();
            }


        }

    }
}
