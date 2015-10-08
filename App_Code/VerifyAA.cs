using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.SessionState;
using BTS.Entity;
using BTS.Constant;
using BTS.Util;

/// <summary>
/// Summary description for VerifyAA
/// </summary>
/// 

namespace BTS.Util
{
    public class VerifyAA
    {
        public string redirectPage = "";

        //static string Verify(int idxAppName, string right, string actPage, string redirectPage)
        public static string Verify(HttpSessionState Session, HttpRequest Request, HttpResponse Response, string redirectPage)
        {
            string checkRight = "true";

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
                }
            }
            else
            {
                String loginPage = "AppLogin.aspx";
                if (Session[SessionVar.USER] == null)
                {
                    //return "loginPage + \"?message=คุณยังไม่ได้ทำการล็อกอินเข้าระบบ\"";
                    Response.Redirect(loginPage + "?message=คุณยังไม่ได้ทำการล็อกอินเข้าระบบ");
                }
            }


            String noRightPage = redirectPage + "?backPage=" + Request.UrlReferrer;

            AppUser user = (AppUser)Session[SessionVar.USER];
            if (user == null)
            {
                //return noRightPage;
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

                if (!Authorizer.Verify(user._roleId, right, Request.Form["actPage"]))
                {
                    //return noRightPage;
                    Response.Redirect(noRightPage);
                }
            }
            return "";
        }
    }
}