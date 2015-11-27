using System;
using System.Data;
using System.Data.Odbc;
using System.Configuration;
using System.Collections.Generic;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using BTS.Util;

/// <summary>
/// Summary description for SQLGenerator
/// </summary>
/// 



namespace BTS.DB
{
    public abstract class DBManager
    {
        protected OdbcConnection _con = null;
        protected OdbcTransaction _trans = null;

        protected string _conString =
                         //"DRIVER={MySQL ODBC 5.2w Driver};" +
                         // "DRIVER={MySQL ODBC 3.51 Driver};" +
                         "DRIVER={MySQL ODBC 3.51 Driver};" +
                         "SERVER=localhost;" +
                         "DATABASE=bts;" +
                         "UID=root;" +
                         "PASSWORD=btsman;" +
                         "OPTION=3";

        protected Logger _log = null;
        protected Logger _mlog = null;

        public DBManager()
        {
            SetLogger();
        }


        public void Connect()
        {
            _con.Open();
        }

        public void Close()
        {
            if (_con != null) { _con.Close(); }
        }

        public void Close(OdbcDataReader reader)
        {
            if (reader != null) { reader.Close(); }
            Close();
        }

        public String GetConnectionString()
        {
            return _conString;
        }

        public OdbcConnection GetConnection()
        {
            return _con;
        }

        public void BeginTransaction(IsolationLevel level) {
            _trans = _con.BeginTransaction(level);
        }

        public void Commit()
        {
            if (_trans != null)
            {
                _trans.Commit();
            }
        }

        public void Rollback()
        {
            if (_trans != null)
            {
                _trans.Rollback();
            }
        }

        public OdbcDataReader Query(string sql)
        {
            Log(SQLLogger.ALL, sql);

            if (_con.State != ConnectionState.Open) this.Connect();

            OdbcCommand cmd = _con.CreateCommand();
            cmd.CommandText = sql;
            if (_trans != null)
            {
                cmd.Transaction = _trans;
            }
            OdbcDataReader reader = cmd.ExecuteReader();
            return reader;
        }

        public int QueryCount(string sql)
        {
            Log(SQLLogger.ALL, sql);

            if (_con.State != ConnectionState.Open) this.Connect();

            OdbcCommand cmd = _con.CreateCommand();
            cmd.CommandText = sql;
            if (_trans != null)
            {
                cmd.Transaction = _trans;
            }
            OdbcDataReader reader = cmd.ExecuteReader();
            reader.Read();
            return reader.GetInt32(0); 
        }

        public ArrayList QueryAll(string sql)
        {
            try
            {
                Log(SQLLogger.ALL, sql);

                if (_con.State != ConnectionState.Open) this.Connect();

                ArrayList array = new ArrayList();
                OdbcDataReader reader = this.Query(sql);
                while (reader.Read())
                {
                    ArrayList a = new ArrayList();
                    for (int i = 0; i < reader.FieldCount; i++)
                        a.Add(reader[i]);
                    array.Add(a);
                }
                reader.Close();
                return array;
            }
            catch (OdbcException ex)
            {
                throw ex;
            }
        }

        public int Execute(string sql)
        {
            Log(SQLLogger.ONLY_CHANGE, sql);

            if (_con.State != ConnectionState.Open) this.Connect();

            OdbcCommand cmd = _con.CreateCommand();            
            if (_trans != null)
            {
                cmd.Transaction = _trans;
            }
            cmd.CommandText = sql;

            int affectRow = cmd.ExecuteNonQuery();
            cmd.Dispose();
            return affectRow;
        }

        public string Formatted(object o)
        {
            if (o is System.DBNull)
                return "NULL";
            if (o is bool)
                return (bool)o ? "1" : "0";
            if (o is int)
                return ((int)o).ToString();
            if (o is string)
                return "'" + ((string)o) + "'";
            return o.ToString();  // will probably result in an error.
        }

        public string AsString(object o)
        {
            if (o is System.DBNull)
                return "NULL";
            if (o is bool)
                return (bool)o ? "1" : "0";
            if (o is int)
                return ((int)o).ToString();
            if (o is string)
                return (string)o;
            return o.ToString();  // will probably result in an error.
        }

        public int Insert(string table, string[] key, string[] value)
        {
            string sql = GetInsertSQL(table, key, value);
            return Execute(sql);
        }

        public int Update(string table, string[] key, string[] value, string whereSQL)
        {
            string sql = GetUpdateSQL(table, key, value, whereSQL);
            return Execute(sql);
        }

        public int Delete(string table, string whereSQL)
        {
            string sql = GetDeleteSQL(table, whereSQL);
            return Execute(sql);
        }

        public void SetLogger()
        {
            _log = Logger.GetLogger(Config.SQLLOG);
            _mlog = Logger.GetLogger(Config.MAINLOG);
        }

        protected void Log(uint serv, string sql)
        {
            if (_log == null) return;
            _log.PutLine(serv, sql);
        }

        public abstract string GetInsertSQL(string table, string[] key, string[] value);
        public abstract string GetUpdateSQL(string table, string[] key, string[] value, string whereSQL);
        public abstract string GetDeleteSQL(string table, string whereSQL);
        
    }
}
