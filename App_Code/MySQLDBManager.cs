using System;
using System.Data;
using System.Data.Odbc;
using System.Collections.Generic;
using System.Collections;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// Summary description for MySQLDBManager
/// </summary>
/// 
namespace BTS.DB
{
    public class MySQLDBManager : DBManager
    {
        protected string ODBC_MYSQL_DRIVER351 = "MySQL ODBC 3.51 Driver";
        protected string ODBC_MYSQL_DRIVER52w = "MySQL ODBC 5.1 Driver";


        public MySQLDBManager(string server, string dbName,string user,string passwd  ) : base()
        {

            _conString = "DRIVER={" + ODBC_MYSQL_DRIVER351 + "};" +
                         "SERVER="+server+";" +
                         "DATABASE="+dbName+";" +
                         "UID="+user+";" +
                         "PASSWORD="+passwd+";" +
                         //"CHARSET=utf8;"+
                         "CHARSET=tis620;" +
                         "OPTION=3";
            /*
            System.Configuration.Configuration rootWebConfig =
            System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("/BTSManDev");
            System.Configuration.ConnectionStringSettings connString = null;
            if (0 < rootWebConfig.ConnectionStrings.ConnectionStrings.Count)
            {
                connString =
                    rootWebConfig.ConnectionStrings.ConnectionStrings["LocalMySqlServer"];
                if (null != connString)
                    Console.WriteLine("Northwind connection string = \"{0}\"",
                        connString.ConnectionString);
                else
                    Console.WriteLine("No Northwind connection string");
            }

            _conString = connString.ConnectionString;
            */
            try
            {
                _con = new OdbcConnection(_conString);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }

        public override string GetInsertSQL(string table, string[] key, string[] value)
        {
            string keyPart = "";
            foreach (string k in key)
            {
                keyPart = keyPart + "," + k;
            }
            keyPart = keyPart.Substring(1);

            string valuePart = "";
            foreach (string v in value)
            {
                string val = FormatValue(v);
                valuePart = valuePart + ",'" + val + "' ";
            }
            valuePart = valuePart.Substring(1);

            string sql = String.Format("INSERT INTO {0}({1}) VALUES ({2})", table, keyPart, valuePart);
            return sql;

        }

        public override string GetUpdateSQL(string table, string[] key, string[] value, string whereSQL)
        {
            string kvSQL = "";            
            for (int i=0;i<key.Length;i++)
            {
                string val = FormatValue(value[i]);
                kvSQL = kvSQL + "," + key[i] + "='" + val +"' ";
            }
            kvSQL = kvSQL.Substring(1);

            string sql = String.Format("UPDATE {0} SET {1} WHERE {2}", table, kvSQL, whereSQL);
            return sql;

        }

        public string FormatValue(string s)
        {
            if (s==null) return "";
            s = s.Replace("'", "''");
            s = s.Replace("\\", "\\\\");
            return s;
        }

        public override string GetDeleteSQL(string table, string whereSQL)
        {
            string sql = String.Format("DELETE FROM {0} WHERE {1}", table, whereSQL);
            return sql;
        }


    }
}
