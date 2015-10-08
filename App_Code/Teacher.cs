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

    public class Teacher : CommonEntity
    {

        public int _teacherID;
        public string _firstname;
        public string _surname;
        public string _citizenID;
        public string _sex;
        public DateTime _birthday;
        public string _addr;
        public string _tel;
        public string _email;
        public string _img;
        public string _subject;
        public bool _isActive;

        public Teacher()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public static Teacher CreateForm(OdbcDataReader reader)
        {
            Teacher teacher = new Teacher();
            Teacher.CreateForm(reader, teacher);
            return teacher;
        }

        
        public static bool CreateForm(OdbcDataReader reader, Teacher teacher)
        {           
            int fCount = reader.FieldCount;
            for (int i = 0; i < fCount; i++)
            {
                string name = reader.GetName(i);

                // Map to DB field. Need to change if db changed
                switch (name) {
                    case "teacher_id": teacher._teacherID = reader.GetInt32(i);
                                      break;
                    case "sex": teacher._sex = reader.GetString(i);
                                      break;
                    case "addr": teacher._addr = reader.GetString(i);
                                      break;
                    case "tel": teacher._tel = reader.GetString(i);
                                      break;
                    case "email": teacher._email = reader.GetString(i);
                                      break;
                    case "birthday": 
                        teacher._birthday = new DateTime(reader.GetDate(i).Ticks);
                                      break;
                    case "firstname": teacher._firstname = reader.GetString(i);
                                      break;
                    case "surname": teacher._surname = reader.GetString(i);
                                      break;
                    case "citizen_id": teacher._citizenID = reader.GetString(i);
                                      break;
                    case "image": teacher._img = reader.GetString(i);
                                      break;
                    case "subject": teacher._subject = reader.GetString(i);
                                      break;
                    case "is_active": teacher._isActive = reader.GetInt32(i) > 0 ? true : false;
                                      break;

                }
            }
            return reader.HasRows;

        }


        public static string GetQSearchSQL(string qsearch)
        {
            if (qsearch == null) return "";
            return String.Format(" ( teacher_id LIKE '%{0}%' or firstname LIKE '%{0}%' or surname LIKE '%{0}%' or citizen_id LIKE '%{0}%' or subject LIKE '%{0}%' or tel LIKE '%{0}%' or email LIKE '%{0}%' or addr LIKE '%{0}%' ) AND is_active=1 ", qsearch);
        }

        public static int GetMaxRecord()
        {
            DBManager db = new MySQLDBManager(Config.DB_SERVER, Config.DB_NAME, Config.DB_USER, Config.DB_PASSWORD);
            db.Connect();
            OdbcDataReader reader = db.Query("SELECT MAX(teacher_id) FROM teacher");

            if (!reader.Read()) return 0;

            long max = reader.IsDBNull(0) ? -1 : reader.GetInt64(0);
            db.Close();
            return (int)(max+1);
        }

        public override bool AddToDB(DBManager db)
        {
            String[] key = { "firstname", "surname", "citizen_id", "tel", "email", "sex", "birthday", "addr", "image", "subject" };
            String[] val = { _firstname, _surname, _citizenID, _tel, _email, _sex, StringUtil.ConvertYearToEng(_birthday, "yyyy/MM/dd"), _addr, _img, _subject };
            return (db.Insert("teacher", key, val) > 0) ? true : false;
        }

        public override bool UpdateToDB(DBManager db)
        {
            if (_teacherID <= 0) return false;
            String[] key = { "firstname", "surname", "citizen_id", "tel", "email", "sex", "birthday", "addr", "image", "subject" };
            String[] val = { _firstname, _surname, _citizenID, _tel, _email, _sex, StringUtil.ConvertYearToEng(_birthday, "yyyy/MM/dd"), _addr, _img, _subject };
            return (db.Update("teacher", key, val, "teacher_id="+_teacherID) > 0) ? true : false;
        }

        public override bool DeleteToDB(DBManager db)
        {
            if (_teacherID <= 0) return false;
            
            // not realy delete in DB. just mark flag
            return (db.Execute("UPDATE teacher SET is_active=0 WHERE teacher_id=" + _teacherID) > 0) ? true : false;
         //   return (db.Delete("teacher", "teacher_id=" + _teacherID) > 0) ? true : false;
        }

        public override bool LoadFromDB(DBManager db, string sqlCriteria)
        {
            OdbcDataReader reader = db.Query("SELECT * FROM teacher WHERE " + sqlCriteria);
            if (!reader.Read()) return false;
            return Teacher.CreateForm(reader, this);
        }

        public static Teacher[] LoadListFromDB(DBManager db, string sqlCriteria)
        {
            OdbcDataReader reader = db.Query("SELECT * FROM teacher " + sqlCriteria);
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

        public PaidGroup[] LoadAssosicatedPaidGroup(DBManager db)
        {
            if (this._teacherID == 0) return null;

            String subQuery = "SELECT paid_group_id FROM paid_group_teacher_mapping WHERE teacher_id='" + this._teacherID + "'";
            String sql = "SELECT * FROM paid_group WHERE paid_group_id IN ( " + subQuery + " )";

            OdbcDataReader reader = db.Query(sql);
            LinkedList<PaidGroup> list = new LinkedList<PaidGroup>();
            while (reader.Read())
            {
                list.AddLast(PaidGroup.CreateForm(reader));
            }

            PaidGroup[] entities = new PaidGroup[list.Count];
            int i = 0;
            foreach (PaidGroup pg in list)
            {
                entities[i++] = pg;
            }
            return entities;
        }

        public static string GetTeacherID(int id)
        {
            return "T" + StringUtil.FillString(id.ToString(), "0", 4, true);
        }

        public static string GetTeacherID(string id)
        {
            return Config.TEACHER_CODE_PREFIX + StringUtil.FillString(id, "0", Config.TEACHER_CODE_LENGTH, true);
        }

        public override string ToString()
        {
            return String.Format("Teacher [firstname={0}, surname={1}, citizen_id={2}, sex={3}, birthday={4}, addr={5}, tel={6}, email={7}]"
                                , _firstname, _surname, _citizenID, _sex, _birthday, _addr, _tel, _email);
        }
    }
}
