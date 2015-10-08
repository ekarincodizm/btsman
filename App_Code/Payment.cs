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

/// <summary>
/// Summary description for User
/// </summary>
/// 

namespace BTS.Entity
{

    public class Payment : CommonEntity
    {

        public int _courseID;
        public int _sumAllCost;
        public int _sumMaxPayable;
        public int _sumPaidCost;
        public DateTime _lastPaidDate;
        public int _paidRound;
        public int _status;

        public LinkedList<PaymentHistory> _historyList = new LinkedList<PaymentHistory>();       
        
        // helper info
        public string _btsCourseID;
        public string _courseName;
        public DateTime _courseStartDate;
        public DateTime _courseEndDate;
        public int _paidGroupID;
        public Course _course;

        // Constant
        public static int STATUS_OK = 0;
        public static int STATUS_INVALID = 1;

        protected static string ORDER_BY = "course_id";

        public Payment()
        {
        }

        public static Payment CreateForm(OdbcDataReader reader)
        {
            Payment payment = new Payment();
            Payment.CreateForm(reader, payment);
            return payment;
        }

        
        public static bool CreateForm(OdbcDataReader reader, Payment payment)
        {                      
            int fCount = reader.FieldCount;
            for (int i = 0; i < fCount; i++)
            {
                string name = reader.GetName(i);

                // Map to DB field. Need to change if db changed
                switch (name) {
                    case "course_id": payment._courseID = reader.GetInt32(i);
                                      break;
                    case "sum_all_cost": payment._sumAllCost = reader.GetInt32(i);
                                      break;
                    case "sum_max_payable": payment._sumMaxPayable = reader.GetInt32(i);
                                      break;
                    case "sum_paid_cost": payment._sumPaidCost = reader.GetInt32(i);
                                      break;
                    case "last_paid_date": payment._lastPaidDate = new DateTime(reader.GetDate(i).Ticks);
                                      break;
                    case "paid_round": payment._paidRound = reader.GetInt32(i);
                                      break;
                    case "status": payment._status = reader.GetInt32(i);
                                      break;
                    // helper info
                    case "bts_course_id": payment._btsCourseID = reader.GetString(i);
                                      break;
                    case "course_name": payment._courseName = reader.GetString(i);
                                      break;
                    case "course_start_date": payment._courseStartDate = new DateTime(reader.GetDate(i).Ticks);
                                      break;
                    case "course_end_date": payment._courseEndDate = new DateTime(reader.GetDate(i).Ticks);
                                      break;
                    case "paid_group_id": payment._paidGroupID = reader.GetInt32(i);
                                      break;


                }
            }
            return reader.HasRows;

        }

        public static LinkedList<Payment> CopyList(LinkedList<Payment> clist)
        {
            Payment[] tmpArray = new Payment[clist.Count];
            clist.CopyTo(tmpArray, 0);
            return new LinkedList<Payment>(tmpArray);
        }

        public static int UpdatePaymentByCourse(DBManager db, int courseID)
        {
            Course c = new Course();
            c.LoadFromDB(db, " course_id="+courseID);
            return UpdatePaymentByCourse(db, c);
        }


        // refresh latest payment data
        // this table data is sensitive with high update rate env
        // TODO: ensure that high transaction rate will not affect this table
        public static int UpdatePaymentByCourse(DBManager db, Course course)        
        {
            // load all courses in the same group
            Course[] coursesSameGroup = Course.LoadListFromDB(db, " WHERE paid_group_id="+course._paidGroupID + " ORDER BY " + ORDER_BY);
            
            int allIncome = 0; // in the same group
            foreach (Course c in coursesSameGroup)
            {
                c.LoadPaidGroup(db);

                int thisIncome = 0;
                Registration[] reg = Registration.LoadListFromDB(db, " WHERE course_id=" + c._courseID);
                for (int i = 0; i < reg.Length; i++)
                {
                    thisIncome += reg[i]._discountedCost;
                }

                Payment payment = new Payment();
                if (!payment.LoadFromDB(db, " course_id=" + c._courseID)) // not found, add new
                {
                    payment._courseID = c._courseID;
                    payment._sumAllCost = thisIncome;
                    payment._sumMaxPayable = c._paidGroup.GetMaxPayableByRate(allIncome, thisIncome);
                    payment._sumPaidCost = 0;
                    payment._lastPaidDate = DateTime.Now;
                    payment._paidRound = c._paidGroup._currentRound;
                    payment._status = Payment.STATUS_OK;
                    payment.AddToDB(db);
                    
                }
                else
                {
                    // collect historical data
                    payment.LoadHistory(db);

                    payment._sumAllCost = thisIncome;
                    payment._sumMaxPayable = c._paidGroup.GetMaxPayableByRate(allIncome, thisIncome);
                    payment._sumPaidCost = payment.GetHistoricalSumPaidCost();
                    payment._lastPaidDate = payment.GetLatestPaidDate();
                    payment._paidRound = c._paidGroup._currentRound;
                    payment._status = Payment.STATUS_OK;
                    payment.UpdateToDB(db);
                    
                }
                allIncome += thisIncome;
            }
            return 0;
        }

        public bool LoadCourse(DBManager db)
        {
            if (_courseID <= 0) return false;
            _course = new Course();
            _course.LoadFromDB(db, " course_id=" + _courseID);

            // Assign loaded data to helper
            this._btsCourseID = _course._btsCourseID;
            return true;
        }

        // Caution: this method implies that the history list is orderly loaded by date.
        // It won't go to query historical data in db so the best way to use this method is calling LoadHistory() first
        public DateTime GetLatestPaidDate()
        {
            if (_historyList.Count == 0) return DateTime.Now;
            PaymentHistory ph = _historyList.Last.Value;
            return ph._paidDate;
        }

        // Caution: this method implies that the history list is loaded.
        // It won't go to query historical data in db so the best way to use this method is calling LoadHistory() first
        public int GetHistoricalSumPaidCost()
        {
            if (_historyList.Count == 0) return 0;
            int sumPaidCost = 0;
            foreach (PaymentHistory ph in _historyList)
            {
                sumPaidCost += ph._paidCost;
            }
            return sumPaidCost;
        }

        public static string GetQSearchSQL(string qsearch)
        {
            return "";
            //if (qsearch == null) return "";
            //return String.Format(" (payment_id LIKE '%{0}%' or payment_name LIKE '%{0}%' or payment_desc LIKE '%{0}%' or room_id LIKE '%{0}%' or c.teacher_id LIKE '%{0}%' or category LIKE '%{0}%' or cost LIKE '%{0}%' or seat_limit LIKE '%{0}%' or bank_regis_limit LIKE '%{0}%') ", qsearch);
        }


        public override bool AddToDB(DBManager db)
        {
            String[] key = { "course_id", "sum_all_cost", "sum_max_payable", "sum_paid_cost", "last_paid_date", "paid_round", "status" };
            String[] val = { _courseID.ToString(), _sumAllCost.ToString(), _sumMaxPayable.ToString(), _sumPaidCost.ToString()
                             ,StringUtil.ConvertYearToEng( _lastPaidDate, "yyyy/MM/dd HH:mm:ss"), _paidRound.ToString(), _status.ToString() };
            return (db.Insert("payment", key, val) > 0) ? true : false;
        }

        public override bool UpdateToDB(DBManager db)
        {
            if (_courseID <= 0) return false;
            String[] key = { "course_id", "sum_all_cost", "sum_max_payable", "sum_paid_cost", "last_paid_date", "paid_round", "status" };
            String[] val = { _courseID.ToString(), _sumAllCost.ToString(), _sumMaxPayable.ToString(), _sumPaidCost.ToString()
                             ,StringUtil.ConvertYearToEng( _lastPaidDate, "yyyy/MM/dd HH:mm:ss"), _paidRound.ToString(), _status.ToString() };
            return (db.Update("payment", key, val, "course_id=" + _courseID) > 0) ? true : false;
        }

        public override bool DeleteToDB(DBManager db)
        {
            if (_courseID <= 0) return false;
            return (db.Delete("payment", "course_id=" + _courseID) > 0) ? true : false;
        }

        public bool LoadHistory(DBManager db)
        {
            if (_courseID <=0 ) return false;
            
            _historyList.Clear();
            PaymentHistory[] ph = PaymentHistory.LoadListFromDB(db, " WHERE course_id=" + this._courseID + " ORDER BY paid_date");
            for (int i = 0; i < ph.Length; i++) _historyList.AddLast(ph[i]);
            return true;
        }

        // Load data from db into itself
        public override bool LoadFromDB(DBManager db, string sqlCriteria)
        {
            OdbcDataReader reader = db.Query("SELECT * FROM payment WHERE " + sqlCriteria);
            if (!reader.Read()) return false;
            return Payment.CreateForm(reader, this);
        }

        public static Payment[] LoadListFromDB(DBManager db, string sqlCriteria)
        {
            return LoadListFromDBCustom(db,"SELECT * FROM payment " + sqlCriteria);
        }

        public static Payment[] LoadListFromDBbyTeacherID(DBManager db, string teacherID)
        {
            string sql = "SELECT p.*, c.bts_course_id as bts_course_id, c.course_name as course_name, c.start_date as course_start_date, c.end_date as course_end_date "
                        +" FROM payment p, course c, teacher t WHERE "
                        +" t.teacher_id=" + teacherID
                        +" AND p.course_id=c.course_id "
                        +" AND c.teacher_id=t.teacher_id";
            return LoadListFromDBCustom(db, sql);
        }

        public static Payment[] LoadListFromDBbyTeacherIDInPaidGroup(DBManager db, string teacherID)
        {
            string sql = "SELECT p.*, c.bts_course_id as bts_course_id, c.course_name as course_name, c.start_date as course_start_date, c.end_date as course_end_date "
                         + " FROM payment p, course c, teacher t, paid_group pg WHERE "
                         + " pg.paid_group_id = (select paid_group_id from teacher where teacher_id="+teacherID+")"
                         + " AND p.course_id=c.course_id "
                         + " AND c.teacher_id=t.teacher_id "
                         + " AND t.paid_group_id=pg.paid_group_id ";
                        
            return LoadListFromDBCustom(db, sql);
        }


        public static Payment[] LoadListFromDBCustom(DBManager db, string sqlAll)
        {
            OdbcDataReader reader = db.Query(sqlAll);
            LinkedList<Payment> list = new LinkedList<Payment>();
            while (reader.Read())
            {
                list.AddLast(Payment.CreateForm(reader));
            }
            
            Payment[] entities = new Payment[list.Count];
            int i = 0;
            foreach (Payment t in list)
            {
                entities[i++] = t;
            }
            return entities;
        }

        public override string ToString()
        {
            return String.Format("Payment [course_id={0}, sum_all_cost={1}, sum_max_payable={2}, sum_paid_cost={3}, last_paid_date={4}, paid_round={5}, status={6}]"
                               , _courseID.ToString(), _sumAllCost.ToString(), _sumMaxPayable.ToString(), _sumPaidCost.ToString(), StringUtil.ConvertYearToEng(_lastPaidDate, "yyyy/MM/dd")
                               , _paidRound.ToString(), _status.ToString());
        }
    }
}
