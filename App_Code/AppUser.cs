using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
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
/// Summary description for AppUser
/// </summary>
/// 

namespace BTS.Entity
{

    public class AppUser : CommonEntity
    {

        public string _username;
        public int _userId;
        public string _passwd;
        public string _firstname;
        public string _surname;
        public int _roleId;
        public int _branchID;
        public bool _isValid; 
        public string _encodedPassword;

        // helper info
        public string _branchName;

        public AppUser()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public static AppUser CreateForm(OdbcDataReader reader)
        {
            AppUser user = new AppUser();
            AppUser.CreateForm(reader, user);
            return user;
        }
        
        public static bool CreateForm(OdbcDataReader reader, AppUser user)
        {            
            int fCount = reader.FieldCount;
            for (int i = 0; i < fCount; i++)
            {
                string name = reader.GetName(i);
                // Map to DB field. Need to change if db changed
                switch (name)
                {
                    case "username": user._username = reader.GetString(i);
                        break;
                    case "user_id": user._userId = reader.GetInt32(i);
                        break;
                    case "passwd": user._encodedPassword = reader.GetString(i);
                        break;
                    case "firstname": user._firstname = reader.GetString(i);
                        break;
                    case "surname": user._surname = reader.GetString(i);
                        break;
                    case "role_id": user._roleId = reader.GetInt32(i);
                        break;
                    case "branch_id": user._branchID = reader.GetInt32(i);
                        break;
                    case "is_valid": user._isValid = reader.GetInt32(i) > 0 ? true : false;
                        break;

                    // helper info
                    case "branch_name": user._branchName = reader.GetString(i);
                        break;


                }
            }
            return reader.HasRows;

        }

        public bool IsAdmin()
        {
            return _roleId == 1 ? true : false;
        }

        public static string GetQSearchSQL(string qsearch)
        {
            if (qsearch == null) return "";
            return String.Format(" username LIKE '%{0}%' or firstname LIKE '%{0}%' or surname LIKE '%{0}%' ", qsearch);
        }


        public override bool AddToDB(DBManager db)
        {
            //username at param????
            String[] key = { "username", "passwd", "firstname", "surname", "role_id", "branch_id" };
            String[] val = { _username, _passwd, _firstname, _surname, _roleId.ToString(), _branchID.ToString() };
            return (db.Insert("user", key, val) > 0) ? true : false;
        }

        public override bool UpdateToDB(DBManager db)
        {
            if (String.IsNullOrEmpty(_username)) return false;
            String[] key = { "passwd", "firstname", "surname", "role_id", "branch_id" };
            String[] val = { _passwd, _firstname, _surname, _roleId.ToString(), _branchID.ToString() };
            return (db.Update("user", key, val, "username='" + _username + "'") > 0) ? true : false;
        }

        public override bool DeleteToDB(DBManager db)
        {
            if (String.IsNullOrEmpty(_username)) return false;
            return (db.Delete("user", "username='" + _username + "'") > 0) ? true : false;
        }

        public override bool LoadFromDB(DBManager db, string sqlCriteria)
        {
            OdbcDataReader reader = db.Query("SELECT * FROM user WHERE " + sqlCriteria);
            if (!reader.Read()) return false;
            return AppUser.CreateForm(reader, this);
        }

        public static AppUser[] LoadListFromDB(DBManager db, string sqlCriteria)
        {
            OdbcDataReader reader = db.Query("SELECT * FROM user " + sqlCriteria);
            LinkedList<AppUser> list = new LinkedList<AppUser>();
            while (reader.Read())
            {
                list.AddLast(AppUser.CreateForm(reader));
            }

            AppUser[] entities = new AppUser[list.Count];
            int i = 0;
            foreach (AppUser t in list)
            {
                entities[i++] = t;
            }
            return entities;
        }


        public int GetRegistrationCountThisMonth(DBManager db)
        {

            String thisMonth = DateTime.Today.ToString("yyyy-MM", new System.Globalization.CultureInfo("en-US")) + "-01";

            String sql = "select count(*) as cnt from ( SELECT distinct transaction_id FROM registration WHERE username='"+_username+"'  and regis_date >= '" + thisMonth + "') as t1";
            OdbcDataReader reader = db.Query(sql);
            if (reader.Read())
            {
               return reader.GetInt32(0);
            }
            return 0;
        }

        public string GetMD5Password()
        {
            return GetMD5Encoded(this._passwd);
        }

        public static string GetMD5Encoded(string data)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] bs = System.Text.Encoding.UTF8.GetBytes(data);
            bs = x.ComputeHash(bs);
            // This is to make the same cipher as PHP
            System.Text.StringBuilder s = new System.Text.StringBuilder();
            foreach (byte b in bs)
            {
                s.Append(b.ToString("x2").ToLower());
            }
            string password = s.ToString();
            return password;

        }
        /*
        public static string GetTeacherID(int id)
        {
            return "T" + StringUtil.FillString(id.ToString(), "0", 4, true);
        }

        public static string GetTeacherID(string id)
        {
            return "T" + StringUtil.FillString(id, "0", 4, true);
        }
        */
        public override string ToString()
        {
            return String.Format("AppUser [username={0}, userid={1}, firstname={2}, surname={3}, role_id={4}, branch_id={5}]"
                                , _username, _userId, _firstname, _surname, _roleId, _branchID);
        }
    }
}
