using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using BTS;
using BTS.DB;
using BTS.Entity;
using BTS.Util;

namespace BTS.Page
{

    public partial class Reload : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string actPage = Request["actPage"];

            if (actPage == null)
            {
                Response.Write("<a href=\"Reload.aspx?actPage=bts.ini\">Reload bts.ini</a><br>");
                Response.Write("<a href=\"Reload.aspx?actPage=autherizer\">Reload Authroizer</a><br>");
            }
            else if (actPage.Equals("bts.ini"))
            {
                Config.Reload();
                Config.Reconfig();
                Response.Write("Load bts.ini done.");
            }
            else if (actPage.Equals("autherizer"))
            {
                Authorizer.Reload();
                Response.Write("Reload Authorizer done.");
            }

        }
    }
    
}
