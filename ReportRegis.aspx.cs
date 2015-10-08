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

using BTS.DB;
using BTS.Entity;
using BTS.Util;

namespace BTS.Report
{


    public partial class ReportRegis : System.Web.UI.Page
    {
        public string actPage = "list";
        public StringBuilder outBuf = new StringBuilder();
        public StringBuilder outBuf2 = new StringBuilder();
        public DateTime start_dt;
        public DateTime end_dt;
        public String branchID;
//        public List<Registration> listRegistration;

        public string errorText = "";
        public CultureInfo ci = new CultureInfo("en-US");
  

        protected void Page_Load(object sender, EventArgs e)
        {

            // Collect parameters
            //HARDCODE Branch=1
            branchID = Request.Form.Get("branch_id");
            if (branchID == null) branchID = "1";

            actPage = Request.Form.Get("actPage");
            if (actPage == null) actPage = Request["actPage"];

            if (Request["start_dt"] != null)
            {
                string[] s = Request["start_dt"].Split('/');

                start_dt = new DateTime(Int32.Parse(s[2]), Int32.Parse(s[1]), Int32.Parse(s[0]));
            }
            else
            {
                start_dt = DateTime.Today;
            }

            if (Request["end_dt"] != null)
            {
                string[] s = Request["end_dt"].Split('/');

                end_dt = new DateTime(Int32.Parse(s[2]), Int32.Parse(s[1]), Int32.Parse(s[0]));
            }
            else
            {
                end_dt = DateTime.Today;
            }
            string searchStr = " regis_date between '" + start_dt.ToString("yyyy/MM/dd", ci) + "' and '" + end_dt.ToString("yyyy/MM/dd", ci) + "'";
            // string searchstr = searchstr + " and branch_id ='" + branchID + "'";

            if ((actPage == null) || (actPage.Trim().Length==0) || (actPage.Equals("list")))
            {                    
                DoListRegister(searchStr);
            }
 
        }


        protected void DoListRegister(string searchStr)
        {
            // get Page
            int pg = 1;
            if (Request["pg"]!=null) pg = Int32.Parse(Request["pg"]);

            string[,] bgclass = new string[,] { { "class=\"spec\"", "class=\"td1\"" }, { "class=\"specalt\"", "class=\"alt\"" } };

 //           listRegistration = new List<Registration>();
            DBManager db = new MySQLDBManager(Config.DB_SERVER, Config.DB_NAME, Config.DB_USER, Config.DB_PASSWORD);
            if (searchStr.Trim().Length > 0) searchStr = " WHERE " + searchStr;
            int numRec = db.QueryCount("SELECT Count(*) FROM registration " + searchStr);

            OdbcDataReader reader = db.Query("SELECT * FROM registration " + searchStr + " LIMIT " + Config.TBRECORD_PER_PAGE + " OFFSET " + (((pg - 1) * Config.TBRECORD_PER_PAGE)));
            int i = 0;
            while (reader.Read())
            {
                Registration regis = Registration.CreateForm(reader);

                outBuf.Append("<tr>");
                outBuf.Append("<th scope=\"row\" abbr=\"Model\" " + bgclass[i % 2, 0] + ">" + regis._regisID + "</th>");
                outBuf.Append("<td " + bgclass[i % 2, 1] + "  align=center>" + regis._transactionID + "&nbsp</td>");
                outBuf.Append("<td " + bgclass[i % 2, 1] + "  align=center>" + regis._regisdate.ToString("dd/MM/yyyy",ci) + "&nbsp</td>");
                outBuf.Append("<td " + bgclass[i % 2, 1] + "  align=center>" + regis._studentID + "&nbsp</td>");
                outBuf.Append("<td " + bgclass[i % 2, 1] + "  align=center>" + regis._courseID + "&nbsp</td>");
                outBuf.Append("<td " + bgclass[i % 2, 1] + "  align=center>" + regis._fullCost + "&nbsp</td>");
                outBuf.Append("<td " + bgclass[i % 2, 1] + "  align=center>" + regis._promotionID + "&nbsp</td>");
                outBuf.Append("<td " + bgclass[i % 2, 1] + "  align=center>" + regis._discountedCost + "&nbsp</td>");
                outBuf.Append("<td " + bgclass[i % 2, 1] + "  align=center>" + regis._isPaid.ToString() + "&nbsp</td>");
                outBuf.Append("</tr>\n");

                i++;
            }
            db.Close();
            
            // calculate max page            
            int maxpg = numRec / Config.TBRECORD_PER_PAGE;
            if (maxpg < 1) { maxpg = 1; }
            else if (maxpg % Config.TBRECORD_PER_PAGE > 0) { maxpg++; }
            // Generate Page Navi HTML
            outBuf2.Append("<b>Page</b>  ");
            for (i = 1; i <= maxpg; i++)
            {
                if (i == pg) { outBuf2.Append("<b>"+i+"</b> "); }
                else {
                    outBuf2.Append(String.Format("<a href=\"ReportRegis.aspx?pg={0}\">{1}</a> ", i.ToString(), i.ToString()));
                }

            }
            
        }
    }
}
