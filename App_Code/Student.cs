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
using System.Reflection;

/// <summary>
/// Summary description for User
/// </summary>
/// 

namespace BTS.Entity
{

    public class Student : CommonEntity
    {
        // raw field db mapping        
        public int _studentID;
        public string _firstname;
        public string _surname;
        public string _citizenID;
        public string _nickname;
        public string _school;
        public int _level;
        public string _sex;
        public DateTime _birthday;
        public string _addr;
        public string _tel;
        public string _tel2;
        public string _email;
        public string _img;
        public string _quiz;
        public DateTime _create_date;
        public bool _isActive;

        public static Logger log = Logger.GetLogger(Config.MAINLOG);

        public Student()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public bool isQuizNoSelected(int no)
        {
            if (_quiz == null) return false;

            string[] qs = _quiz.Split(';');
            foreach (string qno in qs)
            {
                if ((qno == null) || (qno.Length == 0)) continue;
                if (Int32.Parse(qno.Substring(0,1)) == no) return true;
            }
            return false;
        }

        // 1;2;3;6:xxxxxxxx
        public string getQuizNoText(int no)
        {
            if (_quiz == null) return "";

            string[] qs = _quiz.Split(';');
            foreach (string qno in qs)
            {
                if ((qno == null) || (qno.Length == 0)) continue;

                if (Int32.Parse(qno.Substring(0, 1)) == no)
                {
                    if (qno.Length <= 2)
                    {
                        return "";
                    }
                    else
                    {
                        string[] s = qno.Split(':');
                        if (s.Length == 2) return s[1];
                        return "";
                    }
                }
            }
            return "";
        }

        public string GetTel()
        {
            string t1 = "";
            if ((this._tel != null) && (this._tel.Length > 0)) 
            {
                t1 = this._tel;
            }

            string t2 = "";
            if ((this._tel2 != null) && (this._tel2.Length > 0)) 
            {
                t2 = this._tel2;
            }

            if (t1.Length == 0) 
            {
                return t2;
            }

            return t1 + ((t2.Length>0)? ", "+t2:"");

        }

        public static string EncodeQuizText(HttpRequest request)
        {
            StringBuilder rawQuiz = new StringBuilder();

            for (int i = 1; i <= 6; i++)
            {
                if ((request["quiz" + i]) != null)
                {
                    rawQuiz.Append(";" + i.ToString());
                    rawQuiz.Append(":" + request["quiztext" + i]);
                }
            }
            if (rawQuiz.Length > 1) return rawQuiz.ToString().Substring(1);
            return "";
        }

        public static Student CreateForm(OdbcDataReader reader)
        {
            Student student = new Student();
            Student.CreateForm(reader, student);
            return student;
        }

        
        public static bool CreateForm(OdbcDataReader reader, Student student)
        {

            FieldInfo[] fields = reader.GetType().GetFields(
                                     BindingFlags.NonPublic |
                                     BindingFlags.Instance);
            PropertyInfo[] props = reader.GetType().GetProperties(
                                     BindingFlags.NonPublic |
                                     BindingFlags.Instance);

            int fCount = reader.FieldCount;
            for (int i = 0; i < fCount; i++)
            {
                string name = reader.GetName(i);
                //Console.WriteLine(name + reader.GetValue(i));

                // Map to DB field. Need to change if db changed
                switch (name) {
                    case "student_id": student._studentID = reader.GetInt32(i);
                                      break;
                    case "sex": student._sex = reader.GetString(i);
                                      break;
                    case "tel": student._tel = reader.GetString(i);
                                      break;
                    case "tel2": student._tel2 = reader.GetString(i);
                                      break;
                    case "citizen_id": student._citizenID = reader.GetString(i);
                                      break;
                    case "email": student._email = reader.GetString(i);
                                      break;
                    case "addr": student._addr = reader.GetString(i);
                                      break;
                    case "quiz": student._quiz = reader.GetString(i);
                                      break;
                    case "birthday": student._birthday = new DateTime(reader.GetDate(i).Ticks);
                                      break;
                    case "firstname":
                        /*                        
                                                string s= reader.GetDataTypeName(i);
                                                byte[] b = new byte[8];
                                                reader.GetBytes(i, 0, b, 0, b.Length);
                                                //  student._firstname = reader.GetString(i);
                                                for (int j=0;j<b.Length;j++)
                                                {
                                                    log.PutLine(Logger.INFO, b[j] + " ");                                 
                                                }
                                                student._firstname = Encoding.GetEncoding("tis-620").GetString(b);
                                                log.PutLine(Logger.INFO,"ไทยไทย" + student._firstname);
                        */
                                    student._firstname = reader.GetString(i);
                                    break;
                    case "surname": student._surname = reader.GetString(i);
                                      break;
                    case "nickname": student._nickname = reader.GetString(i);
                                      break;                    
                    case "school": student._school = reader.GetString(i);
                                      break;
                    case "level": student._level = reader.GetInt32(i);
                                      break;
                    case "image": student._img = reader.GetString(i);
                                      break;
                    case "create_date": student._create_date = new DateTime(reader.GetDate(i).Ticks);
                                      break;
                    case "is_active": student._isActive = reader.GetInt32(i) > 0 ? true : false;
                                      break;

                }
            }
            return reader.HasRows;

        }

        public static string GetQSearchSQL(string qsearch)
        {
            if (qsearch == null) return "";
            // check date format

            String[] subs = qsearch.Split(' ');

            StringBuilder allSQL = new StringBuilder();
            foreach (String sub in subs) {
                String subSQL = GetSubQSearchSQL(sub);
                if (allSQL.Length == 0) 
                {
                    allSQL.Append(subSQL);
                } else {
                    allSQL.Append( " AND "+subSQL);                
                }
            }

            allSQL.Append(" AND is_active=1 ");
            return allSQL.ToString();
        }

        protected static string GetSubQSearchSQL(string qsearch)
        {
            // date search
            String[] dmy = qsearch.Split('/');
            string dateSQL = "";
            if (dmy.Length == 3)
            {
                string d = StringUtil.FillString(dmy[0], "0", 2);
                string m = StringUtil.FillString(dmy[1], "0", 2);
                string y = dmy[2];
                Int32 n;

                if ((y.Length == 4) && (Int32.TryParse(d, out n)) && (Int32.TryParse(m, out n)) && (Int32.TryParse(y, out n)))
                {
                    int ly = n - 543;
                    int gy = n + 543;

                    dateSQL = " ( birthday = '" + y + "-" + m + "-" + d + "' ";
                    dateSQL += " or birthday = '" + ly + "-" + m + "-" + d + "' ";
                    dateSQL += " or birthday = '" + gy + "-" + m + "-" + d + "' ) ";
                }
                return dateSQL;
            }
            // class search
            try
            {
                if (qsearch.StartsWith("ป."))
                {
                    string rem = qsearch.Substring(2);
                    int num = Int32.Parse(rem);
                    return " ( level=" + num + " ) ";
                }
                else if (qsearch.StartsWith("ม."))
                {
                    string rem = qsearch.Substring(2);
                    int num = Int32.Parse(rem) + 6;
                    return " ( level=" + num + " ) ";
                }
            }
            catch (FormatException e)
            {
                // just try normal search
            }

            // else
            return String.Format(" ( student_id LIKE '%{0}%' or firstname LIKE '%{0}%' or surname LIKE '%{0}%' or citizen_id LIKE '%{0}%' or nickname LIKE '%{0}%' or school LIKE '%{0}%' or tel LIKE '%{0}%' or email LIKE '%{0}%' or addr LIKE '%{0}%' ) ", qsearch);

        }



        public override bool AddToDB(DBManager db)
        {
            String[] key = { "firstname", "surname", "nickname", "citizen_id", "tel", "tel2", "email", "sex", "birthday", "addr", "image", "school", "level", "quiz", "create_date" };
            String[] val = { _firstname, _surname, _nickname, _citizenID, _tel, _tel2, _email, _sex, StringUtil.ConvertYearToEng(_birthday, "yyyy/MM/dd"), _addr, _img, _school, _level.ToString(), _quiz, StringUtil.ConvertYearToEng(_create_date, "yyyy/MM/dd") };
            return (db.Insert("student", key, val) > 0) ? true : false;
        }

        public override bool UpdateToDB(DBManager db)
        {
            if (_studentID <= 0) return false;
            String[] key = { "firstname", "surname", "nickname", "citizen_id", "tel", "tel2", "email", "sex", "birthday", "addr", "image", "school", "level", "quiz" };
            String[] val = { _firstname, _surname, _nickname, _citizenID, _tel, _tel2, _email, _sex, StringUtil.ConvertYearToEng(_birthday, "yyyy/MM/dd"), _addr, _img, _school, _level.ToString(), _quiz };
            return (db.Update("student", key, val, "student_id=" + _studentID) > 0) ? true : false;
        }

        public override bool DeleteToDB(DBManager db)
        {
            if (_studentID <= 0) return false;

            // not realy delete in DB. just mark flag
            return (db.Execute("UPDATE student SET is_active=0 WHERE student_id=" + _studentID)> 0) ? true : false;
            // return (db.Delete("student", "student_id=" + _studentID) > 0) ? true : false;
        }

        public override bool LoadFromDB(DBManager db, string sqlCriteria)
        {            
            return LoadFromDBCustom(db, "SELECT * FROM student WHERE " + sqlCriteria);
        }

        public bool LoadFromDBCustom(DBManager db, string sqlAll)
        {

            OdbcDataReader reader = db.Query(sqlAll);
            if (!reader.Read()) return false;
            return Student.CreateForm(reader, this);
        }

        public static Student[] LoadListFromDB(DBManager db, string sqlCriteria)
        {
            return LoadListFromDB(db, "SELECT * FROM student " + sqlCriteria);
        }
        
        public static Student[] LoadListFromDBCustom(DBManager db, string sqlAll)
        {
            OdbcDataReader reader = db.Query(sqlAll);
            LinkedList<Student> list = new LinkedList<Student>();
            while (reader.Read())
            {
                list.AddLast(Student.CreateForm(reader));
            }

            Student[] entities = new Student[list.Count];
            int i = 0;
            foreach (Student s in list)
            {
                entities[i++] = s;
            }
            return entities;
        }
        
        public static string GetStudentID(int id)
        {
            return GetStudentID(id.ToString());
        }

        public static string GetStudentID(string id)
        {
            return Config.STUDENT_CODE_PREFIX + StringUtil.FillString(id, "0", Config.STUDENT_CODE_LENGTH, true);
        }
    
        public override string ToString()
        {
            return String.Format("Student [student_id={0}, firstname={1}, surname={2} ,nickname={3}, citizen_id={4}, sex={5}, birthday={6}, addr={7}, tel={8}, tel2={9}, email={10}, school={11}, level={12}]"
                                ,_studentID, _firstname, _surname, _nickname, _citizenID, _sex, _birthday, _addr, _tel, _tel2, _email, _school, _level);
        }
    }
}
