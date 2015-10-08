using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// Summary description for SessionVar
/// </summary>
/// 

namespace BTS.Constant
{
    public class SessionVar
    {
        
        public static string USER = "USER"; // AppUser        
        public static string CURRENT_REGIS = "CURRENT_REGIS"; // Registration
        public static string BRANCH_SELECTED = ""; // branch to operate
        public static string PRINT_INFO = "PRINT_INFO"; // StringBuilder
        public static string EXCEL_FILENAME = "EXCEL_FILENAME"; // String
        public static string EXCEL_INFO = "EXCEL_INFO"; // StringBuilder
    }
}
