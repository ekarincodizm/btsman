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
    public class AjaxSvc
    {
        // Register course : select course
        public static string WIZ_Q_COURSES = "wiz_q_courses";
        public static string WIZ_Q_COURSE_DETAIL = "wiz_q_course_detail";
        public static string WIZ_ADD_SELECTED_COURSE = "wiz_add_selected_course";
        public static string WIZ_LIST_SELECTED_COURSE = "wiz_list_selected_course";
        public static string WIZ_REM_SELECTED_COURSE = "wiz_rem_selected_course";

        // Register course : select student
        public static string WIZ_Q_STUDENTS = "wiz_q_students";
        public static string WIZ_Q_STUDENT_DETAIL = "wiz_q_student_detail";
        public static string WIZ_ADD_SELECTED_STUDENT = "wiz_add_selected_student";
        public static string WIZ_LIST_SELECTED_STUDENT = "wiz_list_selected_student";
        public static string WIZ_REM_SELECTED_STUDENT = "wiz_rem_selected_student";

        // Register course : confirm
        public static string WIZ_DISCOUNT_PROMOTION = "wiz_discount_promotion";
        public static string WIZ_DISCOUNT_COURSE = "wiz_discount_course";

    }
}
    
