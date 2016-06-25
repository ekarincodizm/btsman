using System;
using System.Data;
using System.Threading;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Globalization;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using BTS;
using BTS.Constant;
using BTS.DB;
using BTS.Entity;
using BTS.Util;

namespace BTS.Page
{

    public partial class SaveAsExcel : System.Web.UI.Page
    {
        public StringBuilder outBuf;
        public CultureInfo ci = new CultureInfo("en-US");

        protected void Page_Load(object sender, EventArgs e)
        {

            String fname = (String)Session[SessionVar.EXCEL_FILENAME];
            if (fname == null) fname = "download.csv";
            outBuf = (StringBuilder)Session[SessionVar.EXCEL_INFO];
            
            // create a writer and open the file
            FileStream fs = File.Open("d:\\test.csv", FileMode.OpenOrCreate, FileAccess.Write); 
            TextWriter tw = new StreamWriter(fs, System.Text.Encoding.UTF8);

            // write a line of text to the file
            tw.WriteLine(outBuf.ToString());

            // close the stream
            tw.Close();
            
            

            if (outBuf == null) outBuf = new StringBuilder("NO DATA");

            //Response.AppendHeader("Content-Type", "application/vnd.ms-excel; charset=UTF-8");
            Response.AppendHeader("Content-disposition", "attachment; filename="+fname);
            Response.AppendHeader("Content-Transfer-Encoding","binary");
            Response.Charset = "UTF-8";
            Response.ContentType = "application/vnd.ms-excel";


            Response.Write(outBuf.ToString());


        }
        
    }
}