using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Web;

using BTS.DB;

/// <summary>
/// Summary description for Authorizer
/// </summary>
/// 

namespace BTS.Util
{
    public class Authorizer
    {


        static HashSet<AuthKey> _authDB = new HashSet<AuthKey>();

        static Authorizer()
        {
            Reload();

            //      Add("admin", "BTSMan/TeacherManage.aspx", null);
            //      Add("admin", "BTSMan/TeacherManage.aspx", "list");
            //        Add("admin", "BTSMan/TeacherManage.aspx", "add");
            //        Add("admin", "BTSMan/TeacherManage.aspx", "*");
            //        Add("admin", "BTSMan/TeacherManage.aspx", "del");
            //        Add("admin", "BTSMan/TeacherManage.aspx", "del");
        }

        public static void Reload()
        {
            _authDB.Clear();

            DBManager db = new MySQLDBManager(Config.DB_SERVER, Config.DB_NAME, Config.DB_USER, Config.DB_PASSWORD, Config.DB_CHAR_ENC);
            OdbcDataReader reader = db.Query("SELECT r.role_id,r.name,rightname,action FROM authorization a, role r WHERE a.role_id = r.role_id");
            while (reader.Read())
            {
                int roleID = reader.GetInt32(0);
                string right = reader.GetString(2);
                string action = reader.GetString(3);
                Add(roleID, right, action);
            }
            db.Close();
        }

        public static void Add(int role, string right, string action)
        {
            if (action == null) action = "default";
            _authDB.Add(new AuthKey(role, right, action));
        }

        public static bool Verify(int role, string right, string action)
        {
            // Admin allows every right
            if (role == 1) return true;
            
            // *'* allows all actions
            if (_authDB.Contains(new AuthKey(role, right, "*"))) return true;


            if (action == null) action = "default";

            return _authDB.Contains(new AuthKey(role, right, action));
        }

        public class AuthKey
        {
            public int _role;
            public string _right;
            public string _action;

            public AuthKey(int role, string right, string action)
            {
                _role = role;
                _right = right;
                _action = action;
            }

            public override int GetHashCode()
            {
                return (_role + "|" + _right).GetHashCode();
            }

            public override bool Equals(object obj)
            {
                if (obj == null) return false;

                if (this.GetType() != obj.GetType()) return false;

                // safe because of the GetType check
                AuthKey key = (AuthKey)obj;

                // use this pattern to compare reference members
                if (!Object.Equals(_role, key._role)) return false;
                if (!Object.Equals(_right, key._right)) return false;

                // use this pattern to compare value members
                if (!_role.Equals(key._role)) return false;
                if (!_right.Equals(key._right)) return false;

                // '*' allow all action
                if (_action.Equals("*")) return true;

                if (!Object.Equals(_action, key._action)) return false;
                if (!_action.Equals(key._action)) return false;

                return true;
            }
        }
    }
}