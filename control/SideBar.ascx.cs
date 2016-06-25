using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using BTS.Entity;
using BTS.Constant;

namespace BTS.Control
{

    public partial class SideBar : System.Web.UI.UserControl
    {
        //Variable
        public string userID;

        protected void Page_Load(object sender, EventArgs e)
        {
            AppUser user = (AppUser)Session[SessionVar.USER];

            if (user != null)
                userID = user._username;
            else
                userID = string.Empty;
        }
    }
}
