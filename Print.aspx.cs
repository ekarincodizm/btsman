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

    public partial class Print : System.Web.UI.Page
    {
        public StringBuilder outBuf;
        public CultureInfo ci = new CultureInfo("en-US");

        protected void Page_Load(object sender, EventArgs e)
        {

            outBuf = (StringBuilder)Session[SessionVar.PRINT_INFO];
            if (outBuf == null) outBuf = new StringBuilder("<b>NO DATA</b>");

        }
        
    }
}