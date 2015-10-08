using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using BTS.Entity;
using BTS.Constant;


namespace BTS.Page
{

    public partial class NoRight : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AppUser user = (AppUser)Session[SessionVar.USER];

            Response.Write("<br><font color=red size=3>คุณไม่มีสิทธิ์เข้าถึงการทำงานส่วนนี้  โปรดติดต่อผู้ดูแลระบบ</font>");
            Response.Write("<br>" + user);
            Response.Write("<br><a href=\"javascript:history.back()\">กลับไปหน้าที่แล้ว</a>");
            //Response.Write("<br><a href=\"" + Request["backPage"] + "\">กลับไปหน้าที่แล้ว</a>");

        }
    }
}

