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
/// Summary description for Role
/// </summary>
/// 

namespace BTS.Entity
{

    public class Role : CommonEntity
    {
        public int _roleId;
        public string _name;

        // predefined role
        public static int ROLE_ADMIN = 1;
        public static int ROLE_MANAGEMENT = 2;
        public static int ROLE_FRONTSTAFF = 3;

        public Role()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public static Role CreateForm(OdbcDataReader reader)
        {
            Role role = new Role();
            Role.CreateForm(reader, role);
            return role;
        }

        
        public static bool CreateForm(OdbcDataReader reader, Role role)
        {           
            int fCount = reader.FieldCount;
            for (int i = 0; i < fCount; i++)
            {
                string name = reader.GetName(i);

                // Map to DB field. Need to change if db changed
                switch (name) {
                    case "name": role._name = reader.GetString(i);
                                      break;
                    case "role_id": role._roleId = reader.GetInt32(i);
                                      break;
                }
            }
            return reader.HasRows;

        }

        public static string GetQSearchSQL(string qsearch)
        {
            if (qsearch == null) return "";
            return String.Format(" name LIKE '%{0}%' ", qsearch);
        }


        public override bool AddToDB(DBManager db)
        {
            String[] key = { "name" };
            String[] val = { _name };
            return (db.Insert("role", key, val) > 0) ? true : false;
        }

        public override bool UpdateToDB(DBManager db)
        {
            if (_roleId <= 0) return false;
            String[] key = { "name", "role_id" };
            String[] val = { _name, _roleId.ToString() };
            return (db.Update("Role", key, val, "role_id=" + _roleId) > 0) ? true : false;
        }

        public override bool DeleteToDB(DBManager db)
        {
            if (String.IsNullOrEmpty(_name)) return false;
            return (db.Delete("Role", "role_id='" + _roleId + "'") > 0) ? true : false;
        }

        public override bool LoadFromDB(DBManager db, string sqlCriteria)
        {
            OdbcDataReader reader = db.Query("SELECT * FROM role WHERE " + sqlCriteria);
            if (!reader.Read()) return false;
            return Role.CreateForm(reader, this);
        }

        public static Role[] LoadListFromDB(DBManager db, string sqlCriteria)
        {
            OdbcDataReader reader = db.Query("SELECT * FROM role " + sqlCriteria);
            LinkedList<Role> list = new LinkedList<Role>();
            while (reader.Read())
            {
                list.AddLast(Role.CreateForm(reader));
            }

            Role[] entities = new Role[list.Count];
            int i = 0;
            foreach (Role t in list)
            {
                entities[i++] = t;
            }
            return entities;
        }

        public override string ToString()
        {
            return String.Format("Role [role_id={0}, name={1}]"
                                , _roleId, _name);
        }
    }
}
