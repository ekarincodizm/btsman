using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Odbc;
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

    public class PaidGroup : CommonEntity
    {
        public int _paidGroupID;
        public string _name;
        public int _currentRound;
        public PaidRateInfo[] _rateInfo;
        public string _rawRateInfo;
        

        // helper info
        public LinkedList<Teacher> _teacherList;

        public PaidGroup()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public static PaidGroup CreateForm(OdbcDataReader reader)
        {
            PaidGroup paidGroup = new PaidGroup();
            PaidGroup.CreateForm(reader, paidGroup);
            return paidGroup;
        }

        public int GetMaxPayableByRate(int accumIncome, int thisIncome)
        {


            int pay = 0;
            int uncomputeIncome = thisIncome;

            for (int i=0;i<_rateInfo.Length;i++) 
            {
                if ((i < _rateInfo.Length - 1) && (accumIncome >= _rateInfo[i + 1]._bound))
                {
                    continue;            
                }

                int toComputed = 0;

                if (i < _rateInfo.Length - 1)
                { // not last bound

                    if (accumIncome + uncomputeIncome > _rateInfo[i + 1]._bound)
                    {
                        toComputed = _rateInfo[i + 1]._bound - accumIncome;
                    } else {
                        toComputed = uncomputeIncome;
                    }

                } else { // last bound
                    toComputed = uncomputeIncome; // all remaining income
                }
                //System.out.println(" + "+((toComputed * rate[i]) / 100)+" "+toComputed+"@"+rate[i]+"%");

                pay += (toComputed * _rateInfo[i]._percent) / 100;
                accumIncome+=toComputed;
                uncomputeIncome -= toComputed;

                if (uncomputeIncome <=0) return pay; 

            }

            return pay;

       }




        /*
        public int GetMaxPayableByRate(int totalIncome,int computedIncome, int thisIncome)
        {
            int percentLast = 0;
            int payment = 0;
            int currentPercent = 0;

            for (int i=0; i<_rateInfo.Length; i++)
            {
                if (totalIncome >= _rateInfo[i]._bound)
                {
                    if (computedIncome >= _rateInfo[i]._bound)
                    {
                        currentPercent = _rateInfo[i]._percent;
                    }
                    else
                    {
                        int n = _rateInfo[i]._bound - computedIncome;
                        // cut only income < bound
                        int n2 = thisIncome > n ? n : thisIncome;

                        payment += ((n2 * _rateInfo[i]._percent) / 100);
                    }
                }


                if (thisIncome > _rateInfo[i]._bound)
                    payment += ((thisIncome - _rateInfo[i]._bound) * (_rateInfo[i]._percent - percentLast) / 100);
                percentLast = _rateInfo[i]._percent;
            }
            return (Int32)payment;
        }*/

        public int GetCurrentRate(int sumIncome)
        {
            int retRate = 0;

            for (int i = 0; i < _rateInfo.Length; i++)
            {
                if (sumIncome >= _rateInfo[i]._bound)
                    retRate = _rateInfo[i]._percent;
                else
                    break;
            }
            return retRate;
        }

        
        public static bool CreateForm(OdbcDataReader reader, PaidGroup paidGroup)
        {           
            
            int fCount = reader.FieldCount;
            for (int i = 0; i < fCount; i++)
            {
                string name = reader.GetName(i);
                // Map to DB field. Need to change if db changed
                switch (name) {
                    case "paid_group_id": paidGroup._paidGroupID = reader.GetInt32(i);
                                      break;
                    case "name": paidGroup._name = reader.GetString(i);
                                      break;
                    case "current_round": paidGroup._currentRound = reader.GetInt32(i);
                                      break;
                    case "rate_info": paidGroup._rawRateInfo = reader.GetString(i);
                                      paidGroup._rateInfo = PaidRateInfo.Parse(paidGroup._rawRateInfo);
                                      break;

                    // helper info

                }
            }
            return reader.HasRows;

        }

        public static string GetQSearchSQL(string qsearch)
        {
            return "";
        }


        public override bool AddToDB(DBManager db)
        {
            String[] key = { "paid_group_id", "name", "current_round", "rate_info" };
            String[] val = { _paidGroupID.ToString(), _name, _currentRound.ToString(), PaidRateInfo.ToString(_rateInfo) };
            return (db.Insert("paid_group", key, val) > 0) ? true : false;
        }

        public override bool UpdateToDB(DBManager db)
        {
            if (_paidGroupID <= 0) return false;
            String[] key = { "paid_group_id", "name", "current_round", "rate_info" };
            String[] val = { _paidGroupID.ToString(), _name, _currentRound.ToString(), PaidRateInfo.ToString(_rateInfo) };
            return (db.Update("paid_group", key, val, "paid_group_id=" + _paidGroupID) > 0) ? true : false;
        }

        public override bool DeleteToDB(DBManager db)
        {
            if (_paidGroupID <= 0) return false;
            return (db.Delete("paid_group", "paid_group_id=" + _paidGroupID) > 0) ? true : false;
        }

        public bool AddTeacherToDB(DBManager db, String teacherID)
        {
            String[] key = { "paid_group_id", "teacher_id" };
            String[] val = { _paidGroupID.ToString(), teacherID };
            return (db.Insert("paid_group_teacher_mapping", key, val) > 0) ? true : false;
        }

        public bool RemoveTeacherToDB(DBManager db, String teacherID)
        {
            String[] key = { "paid_group_id", "teacher_id" };
            String[] val = { _paidGroupID.ToString(), teacherID };
            return (db.Delete("paid_group_teacher_mapping", "paid_group_id=" + _paidGroupID + " AND teacher_id=" + teacherID) > 0) ? true : false;
        }



        public override bool LoadFromDB(DBManager db, string sqlCriteria)
        {
            OdbcDataReader reader = db.Query("SELECT * FROM paid_group WHERE " + sqlCriteria);
            if (!reader.Read()) return false;
            return PaidGroup.CreateForm(reader, this);
        }

        public bool LoadFromDB(DBManager db)
        {
            if (this._paidGroupID == 0) return false;

            OdbcDataReader reader = db.Query("SELECT * FROM paid_group WHERE paid_group_id=" + this._paidGroupID);
            if (!reader.Read()) return false;
            return PaidGroup.CreateForm(reader, this);
        }

        public static PaidGroup[] LoadListFromDB(DBManager db, string sqlCriteria)
        {
            return LoadListFromDBCustom(db, "SELECT * FROM paid_group " + sqlCriteria);
        }

        public static PaidGroup[] LoadListFromDBCustom(DBManager db, string sqlAll)
        {
            OdbcDataReader reader = db.Query(sqlAll);
            LinkedList<PaidGroup> list = new LinkedList<PaidGroup>();
            while (reader.Read())
            {
                list.AddLast(PaidGroup.CreateForm(reader));
            }

            PaidGroup[] entities = new PaidGroup[list.Count];
            int i = 0;
            foreach (PaidGroup r in list)
            {
                entities[i++] = r;
            }
            return entities;
        }

        // load all teacher in this paidgroup
        // paidGroupID must be set before
        public Teacher[] LoadMemberTeachers(DBManager db)
        {
            if (this._paidGroupID == 0) return null;

            String subQuery = "SELECT teacher_id FROM paid_group_teacher_mapping WHERE paid_group_id='" + this._paidGroupID + "'";
            String sql = "SELECT * FROM teacher WHERE teacher_id IN ( "+subQuery+" )";

            OdbcDataReader reader = db.Query(sql);
            LinkedList<Teacher> list = new LinkedList<Teacher>();
            while (reader.Read())
            {
                list.AddLast(Teacher.CreateForm(reader));
            }

            Teacher[] entities = new Teacher[list.Count];
            int i = 0;
            foreach (Teacher t in list)
            {
                entities[i++] = t;
            }
            return entities;

        }

        // load all teacher NOT in this paidgroup
        // paidGroupID must be set before
        public Teacher[] LoadNonMemberTeachers(DBManager db)
        {
            if (this._paidGroupID == 0) return null;

            String subQuery = "SELECT teacher_id FROM paid_group_teacher_mapping WHERE paid_group_id='" + this._paidGroupID + "'";
            String sql = "SELECT * FROM teacher WHERE is_active=1 AND teacher_id NOT IN ( " + subQuery + " ) ORDER BY teacher_id";

            OdbcDataReader reader = db.Query(sql);
            LinkedList<Teacher> list = new LinkedList<Teacher>();
            while (reader.Read())
            {
                list.AddLast(Teacher.CreateForm(reader));
            }

            Teacher[] entities = new Teacher[list.Count];
            int i = 0;
            foreach (Teacher t in list)
            {
                entities[i++] = t;
            }
            return entities;

        }

        public static string GetPaidGroupID(int id)
        {
            return GetPaidGroupID(id.ToString());
        }

        public static string GetPaidGroupID(string id)
        {
            return Config.PAID_GROUP_CODE_PREFIX + StringUtil.FillString(id, "0", Config.PAID_GROUP_CODE_LENGTH, true);
        }



        public override string ToString()
        {
            return String.Format("PaidGroup [paidGroup_id={0}, current_round={1}, name={2} ]"
                                , _paidGroupID, _currentRound, _name);
        }
        public string GetPaidinfo(int pos, int mode)    // mode1 = bound , mode2 = percent
        {
            if (this._rateInfo.Length >= pos && pos > 0)
                return mode == 1 ? this._rateInfo[pos - 1]._bound.ToString() : this._rateInfo[pos - 1]._percent.ToString();
            return "";
        }
        public string BuildRateInfoString(string input)
        {
            StringBuilder sb = new StringBuilder();
            string[] aRate = input.Split(';');
            PaidRateInfo[] pr = new PaidRateInfo[aRate.Length];
            for (int i = 0; i < aRate.Length; i++)
            {
                string[] pair = aRate[i].Split(':');
                if (!String.IsNullOrEmpty(pair[0]) && !String.IsNullOrEmpty(pair[1]))
                {
                    sb.Append(aRate[i]);
                    sb.Append(";");
                }
            }
            string ret = sb.ToString();
            if (ret.EndsWith(";") && ret.Length > 1)
                ret = ret.Remove(ret.Length-1);
            return ret;
        }
    }

    public class PaidRateInfo
    {
        public int _bound;
        public int _percent;

        public PaidRateInfo(int bound, int percent)
        {
            _bound = bound;
            _percent = percent;
        }

        public static PaidRateInfo[] Parse(string rateInfo)
        {
            string[] aRate = rateInfo.Split(';');
            PaidRateInfo[] pr = new PaidRateInfo[aRate.Length];
            for (int i = 0; i < aRate.Length; i++)
            {
                string[] pair = aRate[i].Split(':');
                PaidRateInfo rinfo = new PaidRateInfo(Int32.Parse(pair[0]), Int32.Parse(pair[1]));
                pr[i] = rinfo;
            }
            return pr;
        }

        public override string ToString()
        {
            return _bound.ToString() + ":" + _percent.ToString();
        }

        public static string ToString(PaidRateInfo[] rateInfo)
        {
            StringBuilder info = new StringBuilder();
            for (int i = 0; i < rateInfo.Length; i++)
            {
                info.Append(rateInfo[i].ToString());
                if (i < rateInfo.Length-1) { info.Append(";"); }
            }
            return info.ToString();
        }
    }
}
