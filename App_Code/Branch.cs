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
/// Summary description for User
/// </summary>
/// 

namespace BTS.Entity
{

    public class Branch : CommonEntity
    {

        public int _branchID;
        public string _branchName;
        public string _branchCode;
        public string _address;
        public string _tel;
        public string _supervisor;        
        public string _img;


        // helper info


        public Branch()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public static Branch CreateForm(OdbcDataReader reader)
        {
            Branch branch = new Branch();
            Branch.CreateForm(reader, branch);
            return branch;
        }

        
        public static bool CreateForm(OdbcDataReader reader, Branch branch)
        {           
            int fCount = reader.FieldCount;
            for (int i = 0; i < fCount; i++)
            {
                string name = reader.GetName(i);

                // Map to DB field. Need to change if db changed
                switch (name) {
                    case "branch_id": branch._branchID = reader.GetInt32(i);
                                      break;
                    case "branch_name": branch._branchName = reader.GetString(i);
                                      break;
                    case "branch_code": branch._branchCode = reader.GetString(i);
                                      break;
                    case "address": branch._address = reader.GetString(i);
                                      break;
                    case "tel": branch._tel = reader.GetString(i);
                                      break;
                    case "img": branch._img = reader.GetString(i);
                                      break;
                    case "supervisor": branch._supervisor = reader.GetString(i);
                                      break;
                }
            }
            return reader.HasRows;

        }

        public static string GetQSearchSQL(string qsearch)
        {
            if (qsearch == null) return "";
            return String.Format(" branch_id LIKE '%{0}%' or branch_name LIKE '%{0}%' or branch_code LIKE '%{0}%' or address LIKE '%{0}%' or tel LIKE '%{0}%' or supervisor LIKE '%{0}%'", qsearch);
        }


        public override bool AddToDB(DBManager db)
        {
            String[] key = { "branch_name", "branch_code", "address", "tel", "img", "supervisor" };
            String[] val = { _branchName, _branchCode, _address, _tel, _img, _supervisor };
            return (db.Insert("branch", key, val) > 0) ? true : false;
        }

        public override bool UpdateToDB(DBManager db)
        {
            if (_branchID <= 0) return false;
            String[] key = { "branch_name", "branch_code", "address", "tel", "img", "supervisor" };
            String[] val = { _branchName, _branchCode, _address, _tel, _img, _supervisor };
            return (db.Update("branch", key, val, "branch_id=" + _branchID) > 0) ? true : false;
        }

        public override bool DeleteToDB(DBManager db)
        {
            if (_branchID <= 0) return false;
            return (db.Delete("branch", "branch_id=" + _branchID) > 0) ? true : false;
        }

        public override bool LoadFromDB(DBManager db, string sqlCriteria)
        {
            OdbcDataReader reader = db.Query("SELECT * FROM branch WHERE " + sqlCriteria);
            if (!reader.Read()) return false;
            return Branch.CreateForm(reader, this);
        }

        public static Dictionary<int, Branch> LoadListFromDBAsMap(DBManager db, string sqlWhere)
        {

            Branch[] branches = LoadListFromDBCustom(db, "SELECT * FROM branch " + sqlWhere);
            Dictionary<int, Branch> map = new  Dictionary<int, Branch>();
            foreach (Branch b in branches)
            {
                map.Add(b._branchID, b);
            }
            return map;
        }

        public static Branch[] LoadListFromDB(DBManager db, string sqlWhere)
        {
            return LoadListFromDBCustom(db, "SELECT * FROM branch "+sqlWhere);
        }

        public static Branch[] LoadListFromDBCustom(DBManager db, string sqlAll)
        {
            OdbcDataReader reader = db.Query(sqlAll);
            LinkedList<Branch> list = new LinkedList<Branch>();
            while (reader.Read())
            {
                list.AddLast(Branch.CreateForm(reader));
            }

            Branch[] entities = new Branch[list.Count];
            int i = 0;
            foreach (Branch b in list)
            {
                entities[i++] = b;
            }
            return entities;
        }

        public static string GetBranchID(int id)
        {
            return GetBranchID(id.ToString());
        }

        public static string GetBranchID(string id)
        {
            return Config.BRANCH_CODE_PREFIX + StringUtil.FillString(id, "0", Config.BRANCH_CODE_LENGTH, true);
        }



        public override string ToString()
        {
            return String.Format("Branch [branch_id={0}, branch_name={1}, branch_name={2}, address={3}, tel={4}, img={5}, supervisor={6} ]"
                                , _branchID, _branchName, _branchCode, _address, _tel, _img, _supervisor);
        }
    }
}
