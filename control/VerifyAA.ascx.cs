using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using BTS.Entity;
using BTS.Constant;
using BTS.Util;

namespace BTS.Control
{

    public partial class control_VerifyAA : System.Web.UI.UserControl
    {
        public string redirectPage = "";
        public string checkRight = "true";

        protected void Page_Load(object sender, EventArgs e)
        {
            // DEBUG
            if (Config.AUTO_LOGIN)
            {
                if (Session[SessionVar.USER] == null)
                {
                    AppUser auser = new AppUser();
                    auser._username = "netta";
                    auser._firstname = "Weerawat";
                    auser._surname = "Seetalalai";
                    auser._roleId = 1;
                    auser._branchID = 1;
                    auser._branchName = "BTS สีลม";
                    Session[SessionVar.USER] = auser;

                    // preload all branches into Session
                    Branch[] b = new Branch[2];
                    b[0] = new Branch();
                    b[0]._branchID = 1;
                    b[0]._branchName = "BTS สีลม";
                    b[1] = new Branch();
                    b[1]._branchID = 2;
                    b[1]._branchName = "BTS สยาม";
                    Session["BRANCHES"] = b;

                }
            }
            else
            {
                String loginPage = "AppLogin.aspx";
                if (Session[SessionVar.USER] == null)
                {
                    Response.Write("<br><font color=red size=3>คุณยังไม่ได้ทำการล็อกอินเข้าระบบ </font>");
                    Response.Write("<br><a href=\"" + "AppLogin.aspx" + "\">ไปหน้าล็อกอิน</a>");
                    Response.Redirect(loginPage + "?message=คุณยังไม่ได้ทำการล็อกอินเข้าระบบ");
                }
            }


            String noRightPage = redirectPage + "?backPage=" + Request.UrlReferrer;

            AppUser user = (AppUser)Session[SessionVar.USER];
            if (user == null)
            {
                Response.Redirect(noRightPage);
                /*
                string attName = "redirectPage";
                if (Context.Items.Contains(attName))
                {
                    if (Context.Items[attName] != null)
                    {
                        redirectPage = (string)Context.Items[attName];
                        Response.Redirect(redirectPage);
                    }
                }
                */
            }

            if (checkRight.ToUpper().Equals("TRUE"))
            {
                int idxAppName = Request.Path.Substring(1).IndexOf("/");
                string right = Request.Path.Substring(idxAppName + 2);

                if (!Authorizer.Verify(user._roleId, right, Request["actPage"]))
                {

                    Response.Redirect(noRightPage);
                }
            }

        }
    }
}
