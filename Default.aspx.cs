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

using BTS.DB;
using BTS.Constant;
using BTS.Entity;
using BTS.Util;

public partial class _Default : System.Web.UI.Page 
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // Authentication
        string redirect = VerifyAA.Verify(Session
            , Request
            , Response
            , "NoRight.aspx");

        AppUser user = (AppUser)Session[SessionVar.USER];
  /*      if (user == null)
        {
            Response.Write("Not login");
            return;
        }
        */
        /*        
        DBManager db = new MySQLDBManager("localhost", "BTS", "root", "btsman");
        ArrayList result = db.QueryAll("SELECT * FROM User");
        for (int i = 0; i < result.Count; i++)
        {
            ArrayList aRow = (ArrayList)result[i];
            for (int j = 0; j < aRow.Count; j++)
            {
                string val = db.AsString(aRow[j]);
                Response.Write(val + "&nbsp");
            }
            Response.Write("<br>");
        }
        OdbcDataReader reader = db.Query("SELECT * FROM User");
        while (reader.Read())
        {
            AppUser user = AppUser.CreateForm(reader);
            Response.Write(user + "<br>");
        }
        db.Close();
         * */
        Response.Write("<br>" + user);


    }
    protected void CreateUserWizard1_CreatedUser(object sender, EventArgs e)
    {

    }
}
