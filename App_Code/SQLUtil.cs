using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// Summary description for SQLUtil
/// </summary>

namespace BTS.Util
{

    public class SQLUtil
    {

        public static string ConstructClauseFromList(string fieldName, ArrayList list, string op)
        {
            StringBuilder clause = new StringBuilder("( ");
            for (int i=0;i<list.Count;i++) {
                if (i > 0) {
                    clause.Append(" "+op+" ");
                }
                clause.Append(fieldName).Append("='").Append((String)list[i]).Append("' ");
            }
            clause.Append(" ) ");
            return clause.ToString();
        }

    }
}
