using System;
using System.Data;
using System.Data.Odbc;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using BTS;
using BTS.DB;
using BTS.Entity;

public partial class Test : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
//        HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();
        Encoding enc = Response.ContentEncoding;
        Response.Write("<br>" + enc.HeaderName + " " + enc.BodyName + " ");
        Response.Write("<br>" + Encoding.Default.HeaderName + " " + Encoding.Default.BodyName + " ");

        DBManager db = new MySQLDBManager(Config.DB_SERVER, Config.DB_NAME, Config.DB_USER, Config.DB_PASSWORD, Config.DB_CHAR_ENC);
        //db.Execute("SET NAMES 'tis620'");
        Student theStudent = new Student();
        theStudent.LoadFromDB(db, " student_id=15318");

        Response.Write("นักเรียนชื่อ  " + theStudent._firstname + "นามสกุล "+ theStudent._surname);

        EncodingInfo[] ecs = Encoding.GetEncodings();
        for (int j = 0; j < ecs.Length; j++)
        {
            Encoding ec = ecs[j].GetEncoding();
            byte[] b = ec.GetBytes(theStudent._firstname);
            Response.Write("<br>"+ec.HeaderName + " " + ec.BodyName + " ");
            for (int i = 0; i < b.Length; i++)
            {
                Response.Write(b[i] + " ");
            }
        }

        byte[] bytes = Encoding.Unicode.GetBytes(theStudent._firstname);


        string myString = Encoding.UTF8.GetString(bytes);
        Response.Write(myString);

        bytes = Encoding.GetEncoding("tis-620").GetBytes(theStudent._firstname);           
        myString = Encoding.UTF8.GetString(bytes);
        Response.Write(myString);


        bytes = Encoding.GetEncoding("windows-874").GetBytes(theStudent._firstname);
        myString = Encoding.UTF8.GetString(bytes);
        Response.Write(myString);

        CultureInfo ci = CultureInfo.InstalledUICulture;

        Response.Write("Default Language Info:");
        Response.Write("* Name: "+ ci.Name);
        Response.Write("* Display Name: "+ ci.DisplayName);
        Response.Write("* English Name: "+ ci.EnglishName);
        Response.Write("* 2-letter ISO Name: "+ ci.TwoLetterISOLanguageName);
        Response.Write("* 3-letter ISO Name: "+ ci.ThreeLetterISOLanguageName);
        Response.Write("* 3-letter Win32 API Name: "+ ci.ThreeLetterWindowsLanguageName);


    }
}
