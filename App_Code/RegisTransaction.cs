using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data;
using System.Data.Odbc;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using BTS.DB;
using BTS.Util;
using System.Text;

/// <summary>
/// Summary description for User
/// </summary>
/// 

namespace BTS.Entity
{

    public class RegisTransaction : CommonEntity
    {
        static readonly object _lock = new object();

        public int _transactionID;
        public string _transactionCode;
        public int _studentID;
        public int _branchID;
        public int _status;
        public int _paidMethod;
        public int _paidRound;
        public DateTime _paiddate;
        public bool _isPaid;


        public static string[] PAID_METHOD_TRANCODE = { "C", "D", "K", "T", "S", "V" };
        public static string[] USER_TRANCODE = { "A", "M", "F" };

        // raw course list
        public LinkedList<Course> _courses = new LinkedList<Course>();
        // modified promotion & cost
        public LinkedList<Promotion> _modPromotions = new LinkedList<Promotion>();
        public LinkedList<Course> _modCourses = new LinkedList<Course>();

        // helper info
        public Student _student;
        public Branch _branch;
        public Hashtable _seatNoMap = new Hashtable(); // key=courseID(string) val=seatNo(string)
        public Hashtable _noteMap = new Hashtable(); // key=courseID(string) val=note(string)
        public string _username;
        public LinkedList<Registration> _regList; // this will be available after commit to db

        // optional
        // adjust promotion cost to course cost without fragmentation
        public bool _isAdjustCost = true;

        public RegisTransaction()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public void AddCourse(Course course)
        {
            foreach (Course c in _courses)
            {
                if (c._courseID == course._courseID) return;
            }
            _courses.AddLast(course);
        }

        protected int GetTransationCountThisMonth(DBManager db, int paidMethod)
        {
            String thisMonth = DateTime.Today.ToString("yyyy-MM", new System.Globalization.CultureInfo("en-US")) + "-01";

            String sql = "select count(*) as cnt from ( SELECT distinct transaction_id FROM registration WHERE branch_id='" + this._branchID + "' and paid_method="+ paidMethod +"  and regis_date >= '" + thisMonth + "') as t1";
            OdbcDataReader reader = db.Query(sql);
            if (reader.Read())
            {
                return reader.GetInt32(0);
            }
            return 0;
        }

        public void CreateTransactionCode(DBManager db, DateTime regisdate)
        {
            // format
            // 1. operating branch code - 2 digits
            // 2. paid method C/K/D/T/V
            // 3. yyMM 1302
            // 5. number of transaction this month XXX


            Branch branch = new Branch();
            branch.LoadFromDB(db, "branch_id=" + this._branchID);

            // find the number of transaction for the user on this month
            int numRegisted = GetTransationCountThisMonth(db, _paidMethod);



            StringBuilder buf = new StringBuilder(40);
            buf.Append(branch._branchCode);
            buf.Append(PAID_METHOD_TRANCODE[_paidMethod]);
            buf.Append(regisdate.Year.ToString().Substring(2)).Append(StringUtil.FillString(regisdate.Month.ToString(), "0", 2, true));
            buf.Append(StringUtil.FillString((numRegisted + 1).ToString(), "0", 3, true));
            // set
            this._transactionCode = buf.ToString();

            //

        }

        public void CreateTransactionCode_OLD(DBManager db, DateTime regisdate)
        {
            // format
            // 1. paid method C/K/D/T
            // 2. user role A/M/F
            // 3. user id XX
            // 4. yyMM 1302
            // 5. number of transaction this month XXX

            // collect user info
            AppUser regisUser = new AppUser();
            regisUser.LoadFromDB(db, " username='" + _username + "'");
            // find the number of transaction for the user on this month

            int numRegisted = regisUser.GetRegistrationCountThisMonth(db);



            StringBuilder buf = new StringBuilder(40);
            buf.Append(PAID_METHOD_TRANCODE[_paidMethod]);
            buf.Append(USER_TRANCODE[regisUser._roleId - 1]);

            buf.Append(StringUtil.FillString(regisUser._userId.ToString(), "0", 2, true));
            buf.Append(regisdate.Year.ToString().Substring(2)).Append(StringUtil.FillString(regisdate.Month.ToString(), "0", 2, true));
            buf.Append(StringUtil.FillString((numRegisted + 1).ToString(), "0", 3, true));
            // set
            this._transactionCode = buf.ToString();

        }

        // Extract to multiple Registration
        // TODO: Check duplicated student-course
        public override bool AddToDB(DBManager db)
        {

            if ((_modCourses == null) && (_modPromotions == null)) return false;

            LinkedList<Registration> regList = new LinkedList<Registration>();
            DateTime now = DateTime.Now;
            CreateTransactionCode(db, now);

            // Promotion
            if (_modPromotions != null)
            {
                foreach (Promotion p in _modPromotions)
                {
                    if (!Promotion.LoadCourseList(db, p)) continue;

                    int accumDiscountedCost = 0;
                    int accumFullCost = 0;
                    for (int i = 0; i < p._courses.Length; i++)
                    {
                        Course c = p._courses[i];
                        int fullCostByRatio = (int)(((float)c._cost / (float)p._fullCost) * p._cost);
                        int discountedCostByRatio = (int)(((float)c._cost / (float)p._fullCost) * p._discountedCost);
                        accumFullCost += fullCostByRatio;
                        accumDiscountedCost += discountedCostByRatio;
                        // asjust to last course
                        if ((_isAdjustCost) && (i+1==p._courses.Length)) 
                        {
                            int fragmentFullCost = p._cost - accumFullCost;
                            int fragmentDiscountedCost = p._discountedCost - accumDiscountedCost;
                            // adjust
                            fullCostByRatio += fragmentFullCost;
                            discountedCostByRatio += fragmentDiscountedCost;
                        }

                        Registration reg = new Registration();
                        //reg._transactionID = this._transactionID;
                        // reg._regisID;  auto_increament
                        reg._regisdate = now;
                        reg._studentID = this._studentID;
                        reg._courseID = c._courseID;
                        reg._promotionID = p._promotionID;
                        reg._branchID = this._branchID;
                        reg._fullCost = fullCostByRatio;
                        reg._discountedCost = discountedCostByRatio;
                        reg._seatNo = (string)_seatNoMap[reg._courseID.ToString()];
                        reg._note = (string)_noteMap[reg._courseID.ToString()];
                        reg._username = this._username;
                        reg._status = 0;
                        reg._paidMethod = this._paidMethod;
                        reg._paidRound = 0;
                        reg._paiddate = this._paiddate;
                        reg._isPaid = false;

                        regList.AddLast(reg);
                    }
                }
            }
            // Course
            if (_modCourses != null)
            {
                foreach (Course c in _modCourses)
                {
                        Registration reg = new Registration();
                        //reg._transactionID = this._transactionID;
                        // reg._regisID;  auto_increament
                        reg._regisdate = now;
                        reg._studentID = this._studentID;
                        reg._courseID = c._courseID;
                        reg._promotionID = 0;
                        reg._branchID = this._branchID;
                        reg._fullCost = c._cost;
                        reg._discountedCost = c._discountedCost;
                        reg._seatNo = (string)_seatNoMap[reg._courseID.ToString()];
                        reg._note = (string)_noteMap[reg._courseID.ToString()];
                        reg._username = this._username;
                        reg._status = 0;
                        reg._paidMethod = this._paidMethod;
                        reg._paidRound = 0;
                        reg._paiddate = this._paiddate;
                        reg._isPaid = false;

                        regList.AddLast(reg);
                }
            }

            db.BeginTransaction(IsolationLevel.ReadCommitted);

            SynchronizedAddToDB(db, this, regList);

            db.Commit();

            return true;
        }

        public static void SynchronizedAddToDB(DBManager db, RegisTransaction regt, LinkedList<Registration> regList)
        {
            lock (_lock) {
                // get new transaction_id
                if (regt._transactionID == 0)
                {
                    int maxID = RegisTransaction.GetMaxTransRegisID(db, regt._branchID) + 1;
                    regt._transactionID = maxID;
                }
                foreach (Registration reg in regList)
                {
                    // Add new registration
                    reg._transactionID = regt._transactionID;
                    reg._transactionCode = regt._transactionCode;
                    reg.AddToDB(db);
                    // Update Payment
                    Payment.UpdatePaymentByCourse(db, reg._courseID);
                }
                regt._regList = regList;
            }
        }

        public override bool UpdateToDB(DBManager db)
        {
            return true;
            /*
            if (_registrationID <= 0) return false;
            String[] key = { "registration_name", "registration_desc", "room_id", "teacher_id", "category", "start_date", "end_date", "day_of_week", "start_time", "end_time", "cost", "seat_limit", "bank_regis_limit", "image", "is_active" };
            String[] val = { _registrationName, _registrationDesc, _roomID.ToString(), _teacherID.ToString(), _category
                               ,StringUtil.ConvertYearToEng(_startdate, "yyyy/MM/dd"), StringUtil.ConvertYearToEng(_enddate, "yyyy/MM/dd"), _dayOfWeek
                               ,_starttime.Hour.ToString() + ":" + _starttime.Minute.ToString(), _endtime.Hour.ToString() + ":" + _endtime.Minute.ToString()
                               ,_cost.ToString(), _seatLimit.ToString(), _bankRegisLimit.ToString(), _img, _isActive?"0":"1" };
            return (db.Update("registration", key, val, "registration_id=" + _registrationID) > 0) ? true : false;
             */
        }
        /*
        public static string GetRegisTransactionID(int id)
        {
            return GetRegisTransactionID(id.ToString());
        }
        */




        public override bool DeleteToDB(DBManager db)
        {
            return true;
            /*
            if (_regisID <= 0) return false;
            return (db.Delete("registration", "registration_id=" + _regisID) > 0) ? true : false;
             */
        }

        public static int GetMaxTransRegisID(DBManager db, int branchId)
        {
            OdbcDataReader reader = db.Query("SELECT MAX(transaction_id) from registration where branch_id="+branchId);
            if (reader.Read())
            {
                if (reader.IsDBNull(0)) return 1;
                int maxID = (int)reader.GetInt64(0);
                return maxID;
            }
            return 0;
        }

        public override bool LoadFromDB(DBManager db, string sqlCriteria)
        {
            return true;
            /*
            OdbcDataReader reader = db.Query("SELECT * FROM registration WHERE " + sqlCriteria);
            if (!reader.Read()) return false;
            return RegisTransaction.CreateForm(reader, this);
             */
        }
        /*
        public static RegisTransaction[] LoadListFromDB(DBManager db, string sqlCriteria)
        {
            return LoadListFromDBCustom(db,"SELECT * FROM registration " + sqlCriteria);
        }

        public static RegisTransaction[] LoadListFromDBCustom(DBManager db, string sqlAll)
        {
            OdbcDataReader reader = db.Query(sqlAll);
            LinkedList<RegisTransaction> list = new LinkedList<RegisTransaction>();
            while (reader.Read())
            {
                list.AddLast(RegisTransaction.CreateForm(reader));
            }

            RegisTransaction[] entities = new RegisTransaction[list.Count];
            int i = 0;
            foreach (RegisTransaction t in list)
            {
                entities[i++] = t;
            }
            return entities;
        }
        */
        public bool LoadStudent(DBManager db)
        {
            if (_studentID <= 0) return false;

            _student = new Student();
            return _student.LoadFromDB(db, " student_id=" + _studentID);
        }


        public bool LoadBranch(DBManager db)
        {
             if (_branchID <= 0) return false;

            _branch = new Branch();
            return _branch.LoadFromDB(db, " branch_id=" + _branchID);
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
