using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using BTS.DB;

/// <summary>
/// Summary description for CommonEntity
/// </summary>
/// 
namespace BTS.Entity
{
    public abstract class CommonEntity
    {
        public CommonEntity()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public abstract bool LoadFromDB(DBManager db, string sqlCriteria);
        public abstract bool AddToDB(DBManager db);
        public abstract bool UpdateToDB(DBManager db);
        public abstract bool DeleteToDB(DBManager db);

    }
}
