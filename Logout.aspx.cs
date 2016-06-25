using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using BTS.Constant;
using BTS.Entity;
using BTS.Util;

namespace BTS.Page
{

    public partial class Logout : System.Web.UI.Page
    {
        public Logger log = Logger.GetLogger(Config.MAINLOG);
 
        protected void Page_Load(object sender, EventArgs e)
        {
            // log
            AppUser user = (AppUser)Session[SessionVar.USER];            
            log.StampLine(Logger.INFO, "Logout [user="+(user!=null?user._username:"null")+"]");
            
            Session[SessionVar.USER] = null;
            Session.Clear();
        }
    }
}
