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

    public class PaymentHistory : CommonEntity
    {

        public int _paymentID;
        public int _courseID;
        public DateTime _paidDate;
        public int _paidCost;
        public int _sumAllCost;
        public int _sumMaxPayable;
        public int _sumPaidCost;

        public int _paidRound;
        public string _costInfo;
        public int _receiverTeacherID;
        public string _username;
        public int _branchID;
               
        // helper info
        public Teacher _receiverTeacher;
        public Course _course;
        public AppUser _user;

        public PaymentHistory()
        {
        }

        public PaymentHistory(Payment p, PaidGroup pg,int paidCost, int receiverId, AppUser user)
        {
            this._courseID = p._courseID;
            this._paidDate = DateTime.Now;
            this._paidCost = paidCost;
            this._sumAllCost = p._sumAllCost;
            this._sumMaxPayable = p._sumMaxPayable;
            this._sumPaidCost = p._sumPaidCost;
            this._paidRound = pg._currentRound;
            this._costInfo = pg._rawRateInfo;
            this._receiverTeacherID = receiverId;
            this._username = user._username;
            this._branchID = user._branchID;
        }

        public static PaymentHistory CreateForm(OdbcDataReader reader)
        {
            PaymentHistory paymentHistory = new PaymentHistory();
            PaymentHistory.CreateForm(reader, paymentHistory);
            return paymentHistory;
        }

        
        public static bool CreateForm(OdbcDataReader reader, PaymentHistory paymentHistory)
        {                      
            int fCount = reader.FieldCount;
            for (int i = 0; i < fCount; i++)
            {
                string name = reader.GetName(i);

                // Map to DB field. Need to change if db changed
                switch (name)
                {
                    case "payment_id": paymentHistory._paymentID = reader.GetInt32(i);
                        break;
                    case "course_id": paymentHistory._courseID = reader.GetInt32(i);
                        break;
                    case "paid_cost": paymentHistory._paidCost = reader.GetInt32(i);
                        break;
                    case "paid_date": paymentHistory._paidDate = new DateTime(reader.GetDateTime(i).Ticks);
                        break;
                    case "sum_all_cost": paymentHistory._sumAllCost = reader.GetInt32(i);
                        break;
                    case "sum_max_payable": paymentHistory._sumMaxPayable = reader.GetInt32(i);
                        break;
                    case "sum_paid_cost": paymentHistory._sumPaidCost = reader.GetInt32(i);
                        break;
                    case "cost_info": paymentHistory._costInfo = reader.GetString(i);
                        break;
                    case "paid_round": paymentHistory._paidRound = reader.GetInt32(i);
                        break;
                    case "receiver_teacher_id": paymentHistory._receiverTeacherID = reader.GetInt32(i);
                        break;
                    case "username": paymentHistory._username = reader.GetString(i);
                        break;
                    case "branch_id": paymentHistory._branchID = reader.GetInt32(i);
                        break;

                    // helper info
                }
            }
            return reader.HasRows;

        }

        public bool LoadCourse(DBManager db)
        {
            if (_courseID <= 0) return false;
            _course = new Course();
            _course.LoadFromDB(db, " course_id=" + _courseID);
            return true;
        }

        public bool LoadReceiver(DBManager db)
        {
            if (_receiverTeacherID <= 0) return false;
            _receiverTeacher = new Teacher();
            _receiverTeacher.LoadFromDB(db, " teacher_id=" + _receiverTeacherID);
            return true;
        }

        public bool LoadUser(DBManager db)
        {
            if (_username == null) return false;
            _user = new AppUser();
            _user.LoadFromDB(db, " username='" + _username+"'");
            return true;
        }

        public static LinkedList<PaymentHistory> CopyList(LinkedList<PaymentHistory> clist)
        {
            PaymentHistory[] tmpArray = new PaymentHistory[clist.Count];
            clist.CopyTo(tmpArray, 0);
            return new LinkedList<PaymentHistory>(tmpArray);
        }


        public static string GetQSearchSQL(string qsearch)
        {
            return "";
            //if (qsearch == null) return "";
            //return String.Format(" (paymentHistory_id LIKE '%{0}%' or paymentHistory_name LIKE '%{0}%' or paymentHistory_desc LIKE '%{0}%' or room_id LIKE '%{0}%' or c.teacher_id LIKE '%{0}%' or category LIKE '%{0}%' or cost LIKE '%{0}%' or seat_limit LIKE '%{0}%' or bank_regis_limit LIKE '%{0}%') ", qsearch);
        }


        public override bool AddToDB(DBManager db)
        {
            String[] key = { "course_id", "paid_cost", "sum_all_cost", "sum_max_payable", "sum_paid_cost", "cost_info", "paid_date", "paid_round", "receiver_teacher_id", "username", "branch_id" };
            String[] val = { _courseID.ToString(), _paidCost.ToString(), _sumAllCost.ToString(), _sumMaxPayable.ToString(), _sumPaidCost.ToString(), _costInfo
                             ,StringUtil.ConvertYearToEng( _paidDate, "yyyy/MM/dd HH:mm:ss"), _paidRound.ToString(), _receiverTeacherID.ToString(), _username, _branchID.ToString() };
            return (db.Insert("payment_history", key, val) > 0) ? true : false;
        }

        public override bool UpdateToDB(DBManager db)
        {
            if (_paymentID <= 0) return false;
            String[] key = { "course_id", "paid_cost", "sum_all_cost", "sum_max_payable", "sum_paid_cost", "cost_info", "paid_date", "paid_round", "receiver_teacher_id", "username", "branch_id" };
            String[] val = { _courseID.ToString(), _paidCost.ToString(), _sumAllCost.ToString(), _sumMaxPayable.ToString(), _sumPaidCost.ToString(), _costInfo
                             ,StringUtil.ConvertYearToEng( _paidDate, "yyyy/MM/dd HH:mm:ss"), _paidRound.ToString(), _receiverTeacherID.ToString(), _username, _branchID.ToString() };
            return (db.Update("payment_history", key, val, "payment_id=" + _paymentID) > 0) ? true : false;
        }

        public override bool DeleteToDB(DBManager db)
        {
            if (_courseID <= 0) return false;
            return (db.Delete("paymentHistory", "payment_id=" + _paymentID) > 0) ? true : false;
        }

        // Load data from db into itself
        public override bool LoadFromDB(DBManager db, string sqlCriteria)
        {
            OdbcDataReader reader = db.Query("SELECT * FROM payment_history WHERE " + sqlCriteria);
            if (!reader.Read()) return false;
            return PaymentHistory.CreateForm(reader, this);
        }

        public static PaymentHistory[] LoadListFromDB(DBManager db, string sqlCriteria)
        {
            return LoadListFromDBCustom(db, "SELECT * FROM payment_history " + sqlCriteria);
        }

        public static PaymentHistory[] LoadListFromDBCustom(DBManager db, string sqlAll)
        {
            OdbcDataReader reader = db.Query(sqlAll);
            LinkedList<PaymentHistory> list = new LinkedList<PaymentHistory>();
            while (reader.Read())
            {
                list.AddLast(PaymentHistory.CreateForm(reader));
            }
            
            PaymentHistory[] entities = new PaymentHistory[list.Count];
            int i = 0;
            foreach (PaymentHistory t in list)
            {
                entities[i++] = t;
            }
            return entities;
        }


        public static string GetPaymentHistoryID(int id)
        {
            return GetPaymentHistoryID(id.ToString());
        }

        public static string GetPaymentHistoryID(string id)
        {
            return Config.PAYMENT_CODE_PREFIX + StringUtil.FillString(id, "0", Config.PAYMENT_CODE_LENGTH, true);
        }

        public override string ToString()
        {
            return String.Format("PaymentHistory [payment_id={0}, course_id={1}, paid_cost={2}, sum_all_cost={3}, sum_max_payable={4}, sum_paid_cost={5}, cost_info={6}, paid_date={7}, paid_round={8}, receiver={9}, username={10}, branch_id={11}]"
                                , _paymentID.ToString(), _courseID.ToString(), _paidCost.ToString(), _sumAllCost.ToString(), _sumMaxPayable.ToString(), _sumPaidCost.ToString(), _costInfo
                                , StringUtil.ConvertYearToEng(_paidDate, "yyyy/MM/dd HH:mm:ss")
                                , _paidRound.ToString(), _receiverTeacherID.ToString(), _username, _branchID.ToString() );
        }
    }
}
