using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data;
using System.Data.Odbc;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using BTS.DB;
using BTS.Util;

/// <summary>
/// Summary description for User
/// </summary>
/// 

namespace BTS.Entity
{

    public class Registration : CommonEntity
    {

        public int _transactionID;
        public int _regisID;
        public string _transactionCode;
        public DateTime _regisdate;
        public int _studentID;
        public int _courseID;
        public int _promotionID;
        public int _branchID;
        public int _fullCost;
        public int _discountedCost;
        public int _paidMethod;
        public DateTime _paiddate;
        public string _seatNo;
        public string _note;
        public int _status;
        public int _paidRound;
        public string _username;
        public bool _isPaid;

        // helper info
        public string _btsCourseID;
        public string _studentFirstname;
        public string _studentSurname;
        public string _studentSchool;
        public int    _studentLevel;
        public string _courseName;
        public string _courseShortName;
        public string _courseType;
        public string _courseCategotry;
        public DateTime _courseStart;
        public DateTime _courseEnd;

        public string _branchCode;

        public Student _student;
        public Course _course;
        public Branch _branch;

        // constant
        public const int STATUS_OK = 0;
        public const int STATUS_CANCELLED = 1;


        public static string[] PAID_METHOD = { "เงินสด", "โอนเงิน", "เครดิต", "SCB", "Counter Service", "VIP" };


        public Registration()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public static string GetStatusText(int status)
        {
            switch (status) {
                case STATUS_OK: return "ปกติ";
                case STATUS_CANCELLED: return "ยกเลิก";
            }
            return "";
        }

        public static Registration CreateForm(OdbcDataReader reader)
        {
            Registration registration = new Registration();
            Registration.CreateForm(reader, registration);
            return registration;
        }

        
        public static bool CreateForm(OdbcDataReader reader, Registration reg)
        {                      
            int fCount = reader.FieldCount;
            for (int i = 0; i < fCount; i++)
            {
                string name = reader.GetName(i);               

                // Map to DB field. Need to change if db changed
                switch (name) {
                    case "regis_id": reg._regisID = reader.GetInt32(i);
                                      break;
                    case "transaction_id": reg._transactionID = reader.GetInt32(i);
                                      break;
                    case "transaction_code": reg._transactionCode = reader.GetString(i);
                                      break;
                    case "regis_date": reg._regisdate = new DateTime(reader.GetDateTime(i).Ticks);
                                      break;
                    case "student_id": reg._studentID = reader.GetInt32(i);
                                      break;
                    case "promotion_id": reg._promotionID = reader.GetInt32(i);
                                      break;
                    case "course_id": reg._courseID = reader.GetInt32(i);
                                      break;
                    case "branch_id": reg._branchID = reader.GetInt32(i);
                                      break;

                    case "full_cost": reg._fullCost = reader.GetInt32(i);
                                      break;
                    case "discounted_cost": reg._discountedCost = reader.GetInt32(i);
                                      break;
                    case "seat_no": reg._seatNo = reader.GetString(i);
                                      break;
                    case "note": reg._note = reader.GetString(i);
                                      break;
                    case "paid_method": reg._paidMethod = reader.GetInt32(i);
                                      break;
                    case "paid_round": reg._paidRound = reader.GetInt32(i);
                                      break;
                    case "paid_date": reg._paiddate = new DateTime(reader.GetDateTime(i).Ticks);
                                      break;
                    case "username": reg._username = reader.GetString(i);
                                      break;
                    case "status": reg._status = reader.GetInt32(i);
                                      break;
                    case "is_paid": reg._isPaid = reader.GetInt32(i) > 0 ? true : false;
                                      break;

                    // helper info
                    case "bts_course_id":
                                      reg._btsCourseID = reader.GetString(i);
                                      break;
                    case "student_firstname":
                                      reg._studentFirstname = reader.GetString(i);
                                      break;
                    case "student_surname":
                                      reg._studentSurname = reader.GetString(i);
                                      break;
                    case "student_school":
                                      reg._studentSchool = reader.GetString(i);
                                      break;
                    case "student_level":
                                      reg._studentLevel = reader.GetInt32(i);
                                      break;
                    case "course_name":
                                      reg._courseName = reader.GetString(i);
                                      break;
                    case "course_type":
                                      reg._courseType = reader.GetString(i);
                                      break;
                    case "course_category":
                                      reg._courseCategotry = reader.GetString(i);
                                      break;
                    case "start_date": reg._courseStart = new DateTime(reader.GetDateTime(i).Ticks);
                                      break;
                    case "end_date": reg._courseEnd = new DateTime(reader.GetDateTime(i).Ticks);
                                      break;

                    case "course_short_name":
                                      reg._courseShortName = reader.GetString(i);
                                      break;
                    case "branch_code":
                                      reg._branchCode = reader.GetString(i);
                                      break;


                }
            }
            return reader.HasRows;

        }

        public bool LoadCourse(DBManager db)
        {
            if (_courseID <= 0) return false;
            _course = new Course();
            _course.LoadFromDB(db, " course_id=" + _courseID);

            // Assign loaded data to helper
            this._btsCourseID = _course._btsCourseID;

            this._courseType = _course._courseType;
            this._courseCategotry = _course._category;
            this._courseStart = _course._startdate;
            this._courseEnd = _course._enddate;

            return true;
        }

        public bool LoadStudent(DBManager db)
        {
            if (_studentID <= 0) return false;
            _student = new Student();
            _student.LoadFromDB(db, " student_id=" + _studentID);
            return true;
        }

        // here is branch that make registration not branch of course
        public bool LoadBranch(DBManager db)
        {
            if (_branchID <= 0) return false;

            _branch = new Branch();
            bool result = _branch.LoadFromDB(db, " branch_id=" + _branchID);
            _branchCode = _branch._branchCode;
            return result;
        }

        public static string GetQSearchSQL(string qsearch)
        {
            if (qsearch == null) return "";
            // remove regis_date out of quick search
            //return String.Format(" (regis_id LIKE '%{0}%' or transaction_id LIKE '%{0}%' or regis_date LIKE '%{0}%' or st.student_id LIKE '%{0}%' or st.firstname LIKE '%{0}%' or st.surname LIKE '%{0}%' or c.course_name LIKE '%{0}%' or c.bts_course_id LIKE '%{0}%' or promotion_id LIKE '%{0}%' or rg.course_id LIKE '%{0}%' or branch_id LIKE '%{0}%' or seat_no LIKE '%{0}%' or note LIKE '%{0}%' or paid_method LIKE '%{0}%' or full_cost LIKE '%{0}%' or discounted_cost LIKE '%{0}%' or username LIKE '%{0}%') ", qsearch);
            return String.Format(" (regis_id LIKE '%{0}%' or transaction_id LIKE '%{0}%' or st.student_id LIKE '%{0}%' or st.firstname LIKE '%{0}%' or st.surname LIKE '%{0}%' or c.course_name LIKE '%{0}%' or c.bts_course_id LIKE '%{0}%' or promotion_id LIKE '%{0}%' or rg.course_id LIKE '%{0}%' or branch_id LIKE '%{0}%' or seat_no LIKE '%{0}%' or note LIKE '%{0}%' or paid_method LIKE '%{0}%' or full_cost LIKE '%{0}%' or discounted_cost LIKE '%{0}%' or username LIKE '%{0}%') ", qsearch);
        }


        public override bool AddToDB(DBManager db)
        {
            String[] key = { "transaction_id", "transaction_code", "regis_date", "student_id", "promotion_id", "course_id", "branch_id", "full_cost", "discounted_cost", "seat_no", "note", "paid_method", "paid_round", "paid_date", "username", "status", "is_paid" };
            String[] val = { _transactionID.ToString(), _transactionCode, StringUtil.ConvertYearToEng(_regisdate, "yyyy/MM/dd HH:mm:ss ")
                             , _studentID.ToString(), _promotionID.ToString(), _courseID.ToString(), _branchID.ToString(), _fullCost.ToString(), _discountedCost.ToString()                    
                             , _seatNo, _note, _paidMethod.ToString(), _paidRound.ToString(), StringUtil.ConvertYearToEng(_paiddate, "yyyy/MM/dd HH:mm:ss "), _username, _status.ToString(), _isPaid?"0":"1" };
            return (db.Insert("registration", key, val) > 0) ? true : false;
          
        }

        public override bool UpdateToDB(DBManager db)
        {
            // update only the paid method
            // so just update the first char of transcode
            UpdateTransactionCode();

            if (_regisID <= 0) return false;
            String[] key = { "transaction_id", "transaction_code", "regis_date", "student_id", "promotion_id", "course_id", "branch_id", "full_cost", "discounted_cost", "seat_no", "note", "paid_method", "paid_round", "paid_date", "username", "status", "is_paid" };
            String[] val = { _transactionID.ToString(),  _transactionCode, StringUtil.ConvertYearToEng(_regisdate, "yyyy/MM/dd HH:mm:ss ")
                             , _studentID.ToString(), _promotionID.ToString(), _courseID.ToString(), _branchID.ToString(), _fullCost.ToString(), _discountedCost.ToString()                    
                             , _seatNo, _note, _paidMethod.ToString(), _paidRound.ToString(), StringUtil.ConvertYearToEng(_paiddate, "yyyy/MM/dd HH:mm:ss "), _username, _status.ToString(), _isPaid?"0":"1" };
            return (db.Update("registration", key, val, "regis_id=" + _regisID) > 0) ? true : false;
        }

        public override bool DeleteToDB(DBManager db)
        {
            if (_regisID <= 0) return false;
            return (db.Delete("registration", "regis_id=" + _regisID) > 0) ? true : false;
        }

        public override bool LoadFromDB(DBManager db, string sqlCriteria)
        {
            OdbcDataReader reader = db.Query("SELECT * FROM registration WHERE " + sqlCriteria);
            if (!reader.Read()) return false;
            return Registration.CreateForm(reader, this);
        }

        public static Registration[] LoadListFromDB(DBManager db, string sqlCriteria)
        {
            return LoadListFromDBCustom(db,"SELECT * FROM registration " + sqlCriteria);
        }

        public static Registration[] LoadListFromDBIncludeCourseHelper(DBManager db, string sqlCriteria)
        {
            String sqlMain = "SELECT r.*, c.bts_course_id as bts_course_id,c.course_name as course_name,c.short_name as course_short_name, b.branch_code as branch_code "
             +" FROM registration r, course c, branch b " 
             +" WHERE r.course_id=c.course_id AND r.branch_id=b.branch_id AND ";

            return LoadListFromDBCustom(db, sqlMain + sqlCriteria);
        }

        public static Registration[] LoadListFromDBCustom(DBManager db, string sqlAll)
        {
            OdbcDataReader reader = db.Query(sqlAll);
            LinkedList<Registration> list = new LinkedList<Registration>();
            while (reader.Read())
            {
                list.AddLast(Registration.CreateForm(reader));
            }

            Registration[] entities = new Registration[list.Count];
            int i = 0;
            foreach (Registration t in list)
            {
                entities[i++] = t;
            }
            return entities;
        }
        
        
 

        public void UpdateTransactionCode()
        {
            // change only paid method
            if ((_transactionCode == null) || (_transactionCode.Length == 0))
            {
                //throw new InvalidDataException("Transaction code never set");
                return;
            }
            // set
            StringBuilder buf = new StringBuilder(this._transactionCode);
            buf[2] = RegisTransaction.PAID_METHOD_TRANCODE[_paidMethod].ToCharArray()[0];
            this._transactionCode = buf.ToString();
        }

        public static StringBuilder PrintCard(DBManager db, int regisID)
        {
            StringBuilder outBuf = new StringBuilder();

            Registration theReg = new Registration();
            theReg.LoadFromDB(db, " regis_id=" + regisID);
            theReg.LoadCourse(db);
            theReg.LoadStudent(db);
            Branch branch = new Branch();
            branch.LoadFromDB(db, " branch_id=" + theReg._branchID);
            AppUser authorizer = new AppUser();
            authorizer.LoadFromDB(db, " username='" + theReg._username + "'");

            // Load all registration in the same transaction
            Registration[] reg = Registration.LoadListFromDBIncludeCourseHelper(db, " r.transaction_id="+theReg._transactionID + " AND r.branch_id="+theReg._branchID);            
           
            // Generate HTML content
            TextReader reader = new StreamReader(Config.PATH_APP_ROOT + "\\template\\registration_print_card.htm");
            String templateContent = reader.ReadToEnd();
            reader.Close();

            int[] rowH = { 22, 20, 20, 20, 20, 20, 20 };

            StringBuilder courseCalendar = new StringBuilder();
            for (int i = 0; i < reg.Length; i++)
            {
                reg[i].LoadCourse(db);
                Branch b = reg[i]._course.LoadBranchInfo(db);

                courseCalendar.Append("<tr height=\"24px\">");
                courseCalendar.Append("<td width=\"38px\" align=left><font style=\"font: 10px 'Trebuchet MS', Verdana, Arial, Helvetica, sans-serif;\">&nbsp&nbsp&nbsp" + reg[i]._btsCourseID + "</font></td>");
                courseCalendar.Append("<td width=\"100px\" align=left><font style=\"font: 10px 'Trebuchet MS', Verdana, Arial, Helvetica, sans-serif;\">&nbsp" + reg[i]._courseShortName + "</font></td>");
                courseCalendar.Append("<td width=\"17px\" align=left><font style=\"font: 10px 'Trebuchet MS', Verdana, Arial, Helvetica, sans-serif;\">" + b._branchCode + "</font></td>");
                courseCalendar.Append("<td width=\"48px\"><font style=\"font: 10px 'Trebuchet MS', Verdana, Arial, Helvetica, sans-serif;\">" + StringUtil.ConvertYearToEng(reg[i]._course._startdate, "dd/MM/yy") + "</font></td>");
                courseCalendar.Append("<td width=\"25px\"><font style=\"font: 10px 'Trebuchet MS', Verdana, Arial, Helvetica, sans-serif;\">" + reg[i]._course._dayOfWeek+"</font></td>");
                courseCalendar.Append("<td width=\"70px\"><font style=\"font: 10px 'Trebuchet MS', Verdana, Arial, Helvetica, sans-serif;\">" + reg[i]._course._opentime + "</font></td>");

                courseCalendar.Append("</tr>");
            }
            
    
/*
                <tr height="10px"><td colspan=2></td></tr>
    <tr><td width="10px" align="right">&nbsp</td><td><font size=2>คอร์ส: {4}</font></td></tr>
    <tr><td align="right">&nbsp</td><td><font size=2>ชื่อคอร์ส: {5} </font></td></tr>
    <tr><td align="right">&nbsp</td><td><font size=2>วันที่เริ่ม: {6}</font></td></tr>
    <tr><td align="right">&nbsp</td><td><font size=2>เวลา: {7}</font></td></tr>
    <tr><td align="right">&nbsp</td><td><font size=2>หนังสือ: </font></td></tr>
            */

            String htmlContent =
                String.Format(templateContent
                    , theReg._student._firstname + " " + theReg._student._surname
                    , Student.GetStudentID(theReg._student._studentID)
                    , StringUtil.ConvertYearToEng(theReg._regisdate, "dd/MM/yyyy")
                    , authorizer._firstname + " " + authorizer._surname
                    , reg[0].GetRegisTransactionID()
                    , courseCalendar.ToString()
                    );

            outBuf.Append(htmlContent);
            return outBuf;
        }

        public static StringBuilder PrintReceipt(DBManager db, int regisID, string title)
        {
            Registration theReg = new Registration();
            theReg.LoadFromDB(db, " regis_id=" + regisID);
            theReg.LoadCourse(db);
            theReg.LoadStudent(db);
            theReg.LoadBranch(db);
            return PrintReceipt(db, theReg, title);
        }

        public static StringBuilder PrintReceipt(DBManager db, Registration theReg, string title)
        {
            StringBuilder outBuf = new StringBuilder();

            Branch branch = theReg._branch;
            AppUser authorizer = new AppUser();
            authorizer.LoadFromDB(db, " username='" + theReg._username + "'");
            // Load all course registered in the same transaction
            String sql = "SELECT rg.*,c.course_name as course_name "
                        +" FROM registration rg, course c "
                        +" WHERE rg.course_id=c.course_id AND transaction_id=" + theReg._transactionID + " AND branch_id="+ theReg._branchID  +" ORDER BY regis_id ";

            Registration[] regCourses = Registration.LoadListFromDBCustom(db, sql);
            // load branch code
            regCourses[0].LoadBranch(db);

            // Generate HTML content
            TextReader reader = new StreamReader(Config.PATH_APP_ROOT + "\\template\\registration_print_receipt.htm");
            String templateContent = reader.ReadToEnd();
            reader.Close();

            StringBuilder courseTxt = new StringBuilder();
            int sumFullCost = 0;
            int sumDiscountedCost = 0;
            for (int i = 0; i < regCourses.Length; i++)
            {
                regCourses[i].LoadCourse(db);
                Branch b = regCourses[i]._course.LoadBranchInfo(db);

                sumFullCost += regCourses[i]._fullCost;
                sumDiscountedCost += regCourses[i]._discountedCost;

                String startDateInfo = "-";
                String endDateInfo = "-";
                if (regCourses[i]._courseType == "คอร์สสด")
                {
                    startDateInfo = StringUtil.ConvertYearToEng(regCourses[i]._course._startdate, "dd/MM/yyyy");
                    endDateInfo = StringUtil.ConvertYearToEng(regCourses[i]._course._enddate, "dd/MM/yyyy");
                }

                courseTxt.Append("<tr>");
                courseTxt.Append("<td align=center><font size=2>" + regCourses[i]._course._btsCourseID + "</font></td>");
                courseTxt.Append("<td><font size=1>" + regCourses[i]._courseName + "</font></td>");
                courseTxt.Append("<td align=center><font size=2>" + startDateInfo + "</font></td>");
                courseTxt.Append("<td align=center><font size=2>" + endDateInfo + "</font></td>");
                courseTxt.Append("<td align=center><font size=2>" + regCourses[i]._course._opentime + "</font></td>");
                courseTxt.Append("<td align=center><font size=2>" + StringUtil.Int2StrComma(regCourses[i]._fullCost) + "</font></td>");
                courseTxt.Append("<td align=center><font size=2>" + StringUtil.Int2StrComma(regCourses[i]._fullCost - regCourses[i]._discountedCost) + "</font></td>");
                courseTxt.Append("<td align=center><font size=2>" + b._branchCode + "</font></td>");
                courseTxt.AppendLine("</tr>");
            }

            // paid method
            StringBuilder paidMethodTxt = new StringBuilder();
            for (int i = 0; i < PAID_METHOD.Length; i++)
            {
                if (theReg._paidMethod == i)
                {
                    paidMethodTxt.Append("  [√]");
                }
                else
                {
                    paidMethodTxt.Append("  [&nbsp&nbsp]");
                }
                paidMethodTxt.Append(GetPaidMethodText(i.ToString()));
            }

            String htmlContent =
                String.Format(templateContent
                    , theReg.GetRegisTransactionID()
                    , branch._branchName
                    , StringUtil.ConvertYearToEng(theReg._regisdate, "dd/MM/yyyy HH:mm")
                    , Student.GetStudentID(theReg._student._studentID)
                    , theReg._student._firstname + " " + theReg._student._surname
                    , theReg._student._school
                    , StringUtil.ConvertEducateLevel(theReg._student._level)
                    , theReg._student.GetTel()
                    , courseTxt.ToString()
                    , paidMethodTxt.ToString()
                    , StringUtil.Int2StrComma(sumFullCost - sumDiscountedCost)
                    , StringUtil.Int2StrComma(sumDiscountedCost)
                    , authorizer._firstname + " " + authorizer._surname
                    , title
                    , StringUtil.ConvertYearToEng(theReg._paiddate, "dd/MM/yyyy")
                    );

            outBuf.Append(htmlContent);

            return outBuf;
        }

        public static int GetMaxRegisID(DBManager db)
        {
            OdbcDataReader reader = db.Query("SELECT MAX(regis_id) from registration");
            if (reader.Read())
            {
                if (reader.IsDBNull(0)) return 1;
                int maxID = (int)reader.GetInt64(0);
                return maxID;
            }
            return 0;
        }


        public static string GetRegistrationID(int id)
        {
            return GetRegistrationID(id.ToString());
        }

        public static string GetRegistrationID(string id)
        {
            return Config.REGISTRATION_CODE_PREFIX + StringUtil.FillString(id, "0", Config.REGISTRATION_CODE_LENGTH, true);
        }

        
        public string GetRegisTransactionID()
        {
            // old format
            // [SL/SM]><Year ปีคศ. 2 หลัก><Running no. 5 หลัก> เช่น SL1000001
            if ((_transactionCode == null) || (_transactionCode.Length == 0))
            {
                int yearDigit = this._regisdate.Year % 100;
                return this._branchCode + yearDigit + StringUtil.FillString(this._transactionID.ToString(), "0", Config.TRANSACTION_CODE_LENGTH, true);
            }
            // new format
            else
            {
                return _transactionCode;
            }
        }

        public static string GetPaidMethodText(string paidMethod) {
            try {
                int pm = Int32.Parse(paidMethod);
                return GetPaidMethodText(pm);
            } catch (Exception e) {
                return GetPaidMethodText(-1);
            }
            
            
        }
        public static string GetPaidMethodText(int paidMethod) 
        {
            if ((paidMethod <0) || (paidMethod>=PAID_METHOD.Length))
            { 
                return "";
            }
            return PAID_METHOD[paidMethod];
        }

        public override string ToString()
        {
            return "";
            /*
            return String.Format("Registration [registration_id={0}, registration_name={1}, registration_desc={2}, room_id={3}, teacher_id={4}, start_date={5}, end_date={6}, day_of_week={7}, start_time={8}, end_time={9}, cost={10}, seat_limit={11}, bank_regis_limit={12}, img={13}, is_active={14}]"
                                , _registrationName, _registrationDesc, _roomID.ToString(), _teacherID.ToString(), _category
                               , StringUtil.ConvertYearToEng(_startdate, "yyyy/MM/dd"), StringUtil.ConvertYearToEng(_enddate, "yyyy/MM/dd"), _dayOfWeek
                               , StringUtil.ConvertYearToEng(_starttime, "yyyy/MM/dd"), StringUtil.ConvertYearToEng(_endtime, "yyyy/MM/dd")
                               , _cost.ToString(), _seatLimit.ToString(), _bankRegisLimit.ToString(), _img, _isActive ? "0" : "1");
             */
        }
    }
}
