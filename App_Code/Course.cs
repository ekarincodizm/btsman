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

    public class Course : CommonEntity
    {

        public int _courseID;
        public string _btsCourseID;
        public string _courseName;
        public string _shortName;
        public string _courseDesc;
        public int _roomID;
        public int _teacherID;
        public int _paidGroupID;
        public string _category;
        public DateTime _startdate;
        public DateTime _enddate;
        public string _dayOfWeek;
        public string _opentime;
        public int _cost;
        public int _seatLimit;
        public int _bankRegisLimit;
        public string _img;
        public bool _isActive;

        // helper info
        public Teacher _teacher;
        public PaidGroup _paidGroup;
        public int _discountedCost;
        public int _numRegistered;
        public string _seatNo;


        public Course()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public override int GetHashCode()
        {
            return _courseID.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            if (this.GetType() != obj.GetType()) return false;

            // safe because of the GetType check
            Course c = (Course)obj;

            // use this pattern to compare reference members
            if (!Object.Equals(_courseID, c._courseID)) return false;

            // use this pattern to compare value members
            if (!_courseID.Equals(c._courseID)) return false;

            return true;
        }

        public static Course CreateForm(OdbcDataReader reader)
        {
            Course course = new Course();
            Course.CreateForm(reader, course);
            return course;
        }

        
        public static bool CreateForm(OdbcDataReader reader, Course course)
        {           
            if (course._teacher == null) { course._teacher = new Teacher(); }
            if (course._paidGroup == null) { course._paidGroup = new PaidGroup(); }
            
            int fCount = reader.FieldCount;
            for (int i = 0; i < fCount; i++)
            {
                string name = reader.GetName(i);

                // Map to DB field. Need to change if db changed
                switch (name) {
                    case "course_id": course._courseID = reader.GetInt32(i);
                                      break;
                    case "bts_course_id": course._btsCourseID = reader.GetString(i);
                                      break;
                    case "course_name": course._courseName = reader.GetString(i);
                                      break;
                    case "short_name": course._shortName = reader.GetString(i);
                                      break;
                    case "course_desc": course._courseDesc = reader.GetString(i);
                                      break;
                    case "room_id": course._roomID = reader.GetInt32(i);
                                      break;
                    case "teacher_id": course._teacherID = reader.GetInt32(i);
                                      course._teacher._teacherID = course._teacherID;
                                      break;
                    case "paid_group_id": course._paidGroupID = reader.GetInt32(i);
                                      course._paidGroup._paidGroupID = course._paidGroupID;
                                      break;
                    case "category": course._category = reader.GetString(i);
                                      break;
                    case "start_date": course._startdate = new DateTime(reader.GetDate(i).Ticks);
                                      break;
                    case "end_date": course._enddate = new DateTime(reader.GetDate(i).Ticks);
                                      break;
                    case "day_of_week": course._dayOfWeek = reader.GetString(i);
                                      break;
                    case "open_time": course._opentime = reader.GetString(i);
                                      break;
                    case "cost":      course._cost = reader.GetInt32(i);
                                      course._discountedCost = course._cost;
                                      break;
                    case "seat_limit": course._seatLimit = reader.GetInt32(i);
                                      break;
                    case "bank_regis_limit": course._bankRegisLimit = reader.GetInt32(i);
                                      break;
                    case "image": course._img = reader.GetString(i);
                                      break;    
                    case "is_active": course._isActive = reader.GetInt32(i) > 0 ? true : false;
                                      break;
                    // helper info
                    case "teacher_firstname": 
                                      course._teacher._firstname = reader.GetString(i);
                                      break;
                    case "teacher_surname":
                                      course._teacher._surname = reader.GetString(i);
                                      break;
                    case "num_registered": 
                                      course._numRegistered = reader.GetInt32(i);
                                      break;


                }
            }
            return reader.HasRows;

        }

        public static LinkedList<Course> CopyList(LinkedList<Course> clist)
        {
            Course[] tmpArray = new Course[clist.Count];
            clist.CopyTo(tmpArray, 0);
            return new LinkedList<Course>(tmpArray);
        }


        public static string GetQSearchSQL(string qsearch)
        {
            if (qsearch == null) return "";
            return String.Format(" (course_id LIKE '%{0}%' or bts_course_id LIKE '%{0}%' or course_name LIKE '%{0}%' or short_name LIKE '%{0}%' or course_desc LIKE '%{0}%' or room_id LIKE '%{0}%' or c.teacher_id LIKE '%{0}%' or category LIKE '%{0}%' or cost LIKE '%{0}%' or seat_limit LIKE '%{0}%' or bank_regis_limit LIKE '%{0}%') ", qsearch);
        }


        public override bool AddToDB(DBManager db)
        {
            String[] key = { "bts_course_id", "course_name", "short_name", "course_desc", "room_id", "teacher_id", "paid_group_id", "category", "start_date", "end_date", "day_of_week", "open_time", "cost", "seat_limit", "bank_regis_limit", "image", "is_active" };
            String[] val = { _btsCourseID, _courseName, _shortName, _courseDesc, _roomID.ToString(), _teacherID.ToString(),  _paidGroupID.ToString(), _category
                               ,StringUtil.ConvertYearToEng(_startdate, "yyyy/MM/dd"), StringUtil.ConvertYearToEng(_enddate, "yyyy/MM/dd"), _dayOfWeek, _opentime
                               ,_cost.ToString(), _seatLimit.ToString(), _bankRegisLimit.ToString(), _img, _isActive?"0":"1" };
            return (db.Insert("course", key, val) > 0) ? true : false;
        }

        public override bool UpdateToDB(DBManager db)
        {
            if (_courseID <= 0) return false;
            String[] key = { "bts_course_id", "course_name", "short_name", "course_desc", "room_id", "teacher_id", "paid_group_id", "category", "start_date", "end_date", "day_of_week", "open_time", "cost", "seat_limit", "bank_regis_limit", "image", "is_active" };
            String[] val = { _btsCourseID, _courseName, _shortName, _courseDesc, _roomID.ToString(), _teacherID.ToString(),  _paidGroupID.ToString(), _category
                               ,StringUtil.ConvertYearToEng(_startdate, "yyyy/MM/dd"), StringUtil.ConvertYearToEng(_enddate, "yyyy/MM/dd"), _dayOfWeek, _opentime 
                               ,_cost.ToString(), _seatLimit.ToString(), _bankRegisLimit.ToString(), _img, _isActive?"0":"1" };
            return (db.Update("course", key, val, "course_id=" + _courseID) > 0) ? true : false;
        }

        public override bool DeleteToDB(DBManager db)
        {
            if (_courseID <= 0) return false;
            return (db.Delete("course", "course_id=" + _courseID) > 0) ? true : false;
        }

        public bool LoadTeacher(DBManager db)
        {
            if (_teacherID <=0 ) return false;
            _teacher = new Teacher();
            _teacher.LoadFromDB(db, " teacher_id="+_teacherID);
            return true;
        }

        public bool LoadPaidGroup(DBManager db)
        {
            if (_paidGroupID <= 0) return false;
            _paidGroup = new PaidGroup();
            _paidGroup.LoadFromDB(db, " paid_group_id=" + _paidGroupID);
            return true;
        }

        public Branch LoadBranchInfo(DBManager db)
        {
            if (_courseID <= 0) return null;
            String sql = "SELECT b.* FROM room r, branch b WHERE r.branch_id=b.branch_id AND r.room_id=" + this._roomID;
            OdbcDataReader reader = db.Query(sql);
            if (reader.Read()) {
                return Branch.CreateForm(reader);
            }
            return null;
        }

        public int GetRegistrationCount(DBManager db)
        {
            if (_courseID <= 0) return -1;
            return db.QueryCount("SELECT count(*) FROM registration WHERE course_id="+ _courseID);
        }

        // Load data from db into itself
        public override bool LoadFromDB(DBManager db, string sqlCriteria)
        {
            OdbcDataReader reader = db.Query("SELECT * FROM course WHERE " + sqlCriteria);
            if (!reader.Read()) return false;
            return Course.CreateForm(reader, this);
        }
        public bool LoadFromDB(DBManager db)
        {
            if (_courseID <= 0) return false;
            return LoadFromDB(db, " course_id="+_courseID);
        }


        public static Course[] LoadListFromDB(DBManager db, string sqlCriteria)
        {
            return LoadListFromDBCustom(db,"SELECT * FROM course " + sqlCriteria);
        }

        public static Course[] LoadListFromDBCustom(DBManager db, string sqlAll)
        {
            OdbcDataReader reader = db.Query(sqlAll);
            LinkedList<Course> list = new LinkedList<Course>();
            while (reader.Read())
            {
                list.AddLast(Course.CreateForm(reader));
            }
            
            Course[] entities = new Course[list.Count];
            int i = 0;
            foreach (Course t in list)
            {
                entities[i++] = t;
            }
            return entities;
        }

        public static int GetMaxCourseID(DBManager db)
        {
            OdbcDataReader reader = db.Query("SELECT MAX(course_id) from course");
            if (reader.Read())
            {
                return reader.GetInt32(0);
            }
            return -1;
        }
        public static string GetCourseID(int id)
        {
            return GetCourseID(id.ToString());
        }

        public static string GetCourseID(string id)
        {
            return Config.COURSE_CODE_PREFIX + StringUtil.FillString(id, "0", Config.COURSE_CODE_LENGTH, true);
        }

        public override string ToString()
        {
            return String.Format("Course [course_id={0}, bts_course_id={1}, course_name={2}, course_desc={3}, room_id={4}, teacher_id={5}, paid_group_id={6}, start_date={7}, end_date={8}, day_of_week={9}, open_time={10}, cost={11}, seat_limit={12}, bank_regis_limit={13}, img={14}, is_active={15}]"
                                , _courseID, _btsCourseID, _courseName, _courseDesc, _roomID.ToString(), _teacherID.ToString(), _paidGroupID.ToString() 
                               , StringUtil.ConvertYearToEng(_startdate, "yyyy/MM/dd"), StringUtil.ConvertYearToEng(_enddate, "yyyy/MM/dd"), _dayOfWeek, _opentime
                               , _cost.ToString(), _seatLimit.ToString(), _bankRegisLimit.ToString(), _img, _isActive ? "0" : "1");
        }
    }
}
