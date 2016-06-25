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

using BTS.Util;

namespace BTS.Page
{

    public partial class Home : System.Web.UI.Page
    {
        public string actPage = "list";
        public String targetID;
        public StringBuilder outBuf = new StringBuilder();
        public string errorText = "";
        public CultureInfo ci = new CultureInfo("en-US");


        protected void Page_Load(object sender, EventArgs e)
        {
            // Authentication
            string redirect = VerifyAA.Verify(Session
                , Request
                , Response
                , "NoRight.aspx");
        }
    }
}
