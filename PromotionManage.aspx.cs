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

using BTS.DB;
using BTS.Entity;
using BTS.Util;

namespace BTS.Page
{


    public partial class PromotionManage : System.Web.UI.Page
    {
        public string actPage = "list";
        public StringBuilder outBuf = new StringBuilder();
        public StringBuilder outBuf2 = new StringBuilder();
        public String targetID;
        public Promotion thePromotion;
        public List<Promotion> listPromotion;
        public int fullCost = 0;
        public string filterCourseID = "all";

        public string errorText = "";
        public CultureInfo ci = new CultureInfo("en-US");
        public Logger log = Logger.GetLogger(Config.MAINLOG);
    //    protected System.Web.UI.HtmlControls.HtmlInputFile portrait;
  

        protected void Page_Load(object sender, EventArgs e)
        {
            // Authentication
            string redirect = VerifyAA.Verify(Session
                , Request
                , Response
                , "NoRight.aspx");

            // Collect paramters
            actPage = Request.Form.Get("actPage");
            if (actPage == null) actPage = Request["actPage"];
            targetID = Request.Form.Get("targetID");
            if (targetID == null) targetID = Request["targetID"];

            // log
            log.StampLine(Logger.DETAILED, "Request [" + Request["ASP.NET_SessionId"] + "][" + Request.RawUrl + "][actPage=" + actPage + "&targetID=" + targetID + "]");
            log.StampLine(Logger.DEBUG, "Param [" + Request["ASP.NET_SessionId"] + "][" + Request.Params.ToString() + "]");

                if ((actPage == null) || (actPage.Trim().Length==0) || (actPage.Equals("list")))
                {
                    string qSearch = Request.Form.Get("qsearch");
                    bool isNewSearch = false;
                    if (qSearch != null)
                    {
                        isNewSearch = true;
                    }
                    else
                    {
                        qSearch = Request["qsearch"];
                    }
                    DoListPromotion(qSearch, isNewSearch);
                }
                else if (actPage.Equals("add"))
                {
                    filterCourseID = Request.Form.Get("filter_courseid");
                    DoAddPromotion();
                }
                else if (actPage.Equals("add_submit"))
                {
                    DoAddSubmitPromotion();
                    Response.Redirect("PromotionManage.aspx");
                }
                else if (actPage.Equals("edit"))
                {
                    targetID = Request["targetID"];
                    DoEditPromotion(targetID);
                }
                else if (actPage.Equals("edit_submit"))
                {
                    targetID = Request["targetID"];
                    DoEditSubmitPromotion(targetID);
                    Response.Redirect("PromotionManage.aspx");
                }
                else if (actPage.Equals("delete"))
                {
                    targetID = Request["targetID"];
                    DoDeletePromotion(targetID);
                    Response.Redirect("PromotionManage.aspx");
                }
        }

        
        protected void DoListPromotion(string searchStr, bool isNewSearch)
        {
            // get Page
            int pg = 1;
            if ((!isNewSearch) && (Request["pg"] != null)) pg = Int32.Parse(Request["pg"]);

            string[,] bgclass = new string[,] { { "class=\"spec\"", "class=\"td1\"", "class=\"td1_grey\"" }, { "class=\"specalt\"", "class=\"alt\"", "class=\"td1_grey\"" } };
            string grey = "class=\"thspec_grey\"";

            listPromotion = new List<Promotion>();
            DBManager db = new MySQLDBManager(Config.DB_SERVER, Config.DB_NAME, Config.DB_USER, Config.DB_PASSWORD);
            
            
            string qSearchSQL = Promotion.GetQSearchSQL(searchStr);
            if (qSearchSQL.Trim().Length > 0) qSearchSQL = " WHERE " + qSearchSQL;
            int numRec = db.QueryCount("SELECT Count(*) FROM promotion p " + qSearchSQL);

            // add join condition
            //qSearchSQL = qSearchSQL + ((qSearchSQL.Trim().Length > 0)?" AND ":" WHERE ") + " c.teacher_id=t.teacher_id";

            

            Promotion[] promotions = Promotion.LoadListFromDBCustom(db, 
                "SELECT p.* FROM promotion p " + qSearchSQL + " ORDER BY is_active desc, promotion_id " + " LIMIT " + Config.TBRECORD_PER_PAGE + " OFFSET " + (((pg - 1) * Config.TBRECORD_PER_PAGE)));


            for (int i=0;i<promotions.Length;i++) {
            
                Promotion.LoadCourseList(db, promotions[i]);
                Promotion p = promotions[i];

                int n = p._isActive ? 1 : 2;

                outBuf.Append("<tr>");
                outBuf.Append("<th scope=\"row\" abbr=\"Model\" " + (p._isActive? bgclass[i % 2, 0] : grey) + " valign=top  >" + Promotion.GetPromotionID(p._promotionID) + "</th>");
                outBuf.Append("<td " + bgclass[i % 2, n] + " align=center ><img border=0 src=\""+ Config.URL_PIC_SYS + "/" + (p._isActive?"green":"red") +"light.gif\" ></td>");
                outBuf.Append("<td " + bgclass[i % 2, n] + "  ><a href=\"#\" >"
                    + "<b>" + p._promotionName + "</b></a><br>"
                    + p._promotionDesc
                    + "</td>");
                outBuf.Append("<td " + bgclass[i % 2, n] + "  ><ul>");
                int fullCost = 0;
                for (int j = 0; j < p._courses.Length; j++)
                {
                    fullCost += p._courses[j]._cost;
                    outBuf.Append("<li>" + p._courses[j]._courseName +" (<font color=red>" + p._courses[j]._cost +"</font>)</li>");
                }
                int discount = 0;
                if (fullCost > 0) 
                {
                   discount = 100 - ((p._cost*100 / fullCost));
                }

                outBuf.Append("</ul></td>");
                outBuf.Append("<td " + bgclass[i % 2, n] + " align=center ><font color=red><strike>" + fullCost + "</strike></font></td>");
                outBuf.Append("<td " + bgclass[i % 2, n] + " align=center >" + p._cost + "&nbsp<br><font color=red>(" + discount + "% off)</font></td>");
                outBuf.Append("<td " + bgclass[i % 2, n] + "  align=center>&nbsp");
                outBuf.Append("<a href=\"javascript:setVal('actPage','edit');setVal('targetID','" + p._promotionID + "');doSubmit()\"><img src=\"img/sys/edit.gif\" border=0 alt=\"Edit\"></a>&nbsp");
                outBuf.Append("<a href=\"javascript:if (confirm('Delete this promotion?')) { setVal('actPage','delete');setVal('targetID','" + p._promotionID + "');doSubmit(); }\"><img src=\"img/sys/delete.gif\" border=0 alt=\"Delete\"></a>&nbsp");
 
                outBuf.Append("</td>");
                outBuf.Append("</tr>\n");
            }
            
            db.Close();
            
            // calculate max page            
            int maxpg = numRec / Config.TBRECORD_PER_PAGE;
            if (maxpg < 1) { maxpg = 1; }
            else if (numRec % Config.TBRECORD_PER_PAGE > 0) { maxpg++; }
            // Generate Page Navi HTML
            outBuf2.Append("<b>Page</b>  ");
            for (int i = 1; i <= maxpg; i++)
            {
                if (i == pg) { outBuf2.Append("<b>"+i+"</b> "); }
                else {
                    outBuf2.Append(String.Format("<a href=\"PromotionManage.aspx?pg={0}&qsearch={1}\">{0}</a> ", i.ToString(), searchStr));
                }

            }
            
          //  <a href="#">1</a> <b>2</b> <a href="#">3</a> <a href="#">4</a>


        }

        public void DoAddPromotion()
        {
            
            DBManager db = new MySQLDBManager(Config.DB_SERVER, Config.DB_NAME, Config.DB_USER, Config.DB_PASSWORD);
            PrintCourseOption(db);
            db.Close();
             
        }

        protected void DoAddSubmitPromotion()
        {
            Promotion p = new Promotion();
            
            // validate data
            p._promotionName = Request["promotion_name"];
            p._promotionDesc = Request["promotion_desc"];
            p._cost =Int32.Parse(Request["cost"]);
            p._isActive = Request["is_active"] != null ? true : false;

            // read selected course
            ArrayList idList = new ArrayList();
            for (int i = 0; i < Request.Form.AllKeys.Length; i++)
            {
                if (Request.Form.AllKeys[i].StartsWith("course"))
                {
                    string s = Request.Form.AllKeys[i].Substring(6);
                    idList.Add(s);
                }
            }
            p._courses = new Course[idList.Count];
            int n = 0;
            foreach (string id in idList)
            {
                try
                {
                    Course c = new Course();
                    c._courseID = Int32.Parse(id);
                    p._courses[n++] = c;
                }
                catch (Exception e) { Console.WriteLine(e.StackTrace); }
            }

            // Save to DB
            DBManager db = new MySQLDBManager(Config.DB_SERVER, Config.DB_NAME, Config.DB_USER, Config.DB_PASSWORD);
            db.Connect();
            p.AddToDB(db);
            db.Close();
        }

        public void PrintCourseOption(DBManager db)
        {
            // validate course filter
            int a;
            if ((filterCourseID==null) || (filterCourseID.Length==0) || (!Int32.TryParse(filterCourseID,out a))) {
                filterCourseID = "all";                
            }


            // load all courses
            String whereClause = filterCourseID.Equals("all") ? "" : " AND paid_group_id="+filterCourseID+" ";
            PaidGroup[] pgroups = PaidGroup.LoadListFromDB(db, " ORDER BY name");
            Course[] courses = Course.LoadListFromDB(db, " WHERE is_active=1 " + whereClause + " ORDER BY bts_course_id");

            // paint course list
            fullCost = 0;
            int numCoursesPerRow = 5;
            
            outBuf.Append("<table border=0>");
            // print filter courses
            if (actPage.Equals("add"))
            {
                outBuf.Append("<tr><td colspan=\"" + numCoursesPerRow + "\">");
                outBuf.Append("แสดงรายการคอร์ส <select name=\"filter_courseid\" id=\"filter_courseid\" onchange=\"doChangeFilterCourse()\">");
                outBuf.Append("<option value=\"all\">ทั้งหมด</option>");
                foreach (PaidGroup pg in pgroups)
                {
                    string selected = (filterCourseID.Equals(pg._paidGroupID.ToString())) ? "selected" : "";
                    outBuf.Append("<option value=\"" + pg._paidGroupID + "\" " + selected + " >" + pg._name + "</option>");
                }
                outBuf.Append("</select>");
                outBuf.Append("</td></tr>");
            }

            for (int i = 0; i < courses.Length; i++)
            {
                string name = "course" + courses[i]._courseID;
                string isChecked = "";
                string className = " class=\"td2\" ";
                if ((thePromotion != null) && (thePromotion._courses != null))
                {
                    for (int j = 0; j < thePromotion._courses.Length; j++)
                    {
                        if (thePromotion._courses[j]._courseID == courses[i]._courseID)
                        {
                            fullCost += courses[i]._cost;
                            isChecked = " checked ";
                            className = " class=\"td2_2\" ";
                            break;
                        }
                    }
                }

                if (i % numCoursesPerRow == 0)
                {
                    outBuf.Append("<tr>\n");
                    //  outBuf.Append("<th scope=row abbr=Model class=spec>xxx</th>");
                }
                string t = "document.getElementById('" + name + "').checked";
                string t2 = t + "=!" + t + ";";
                t2 = t2 + " if (document.getElementById('" + name + "').checked) {this.className='td2_2';}";
                t2 = t2 + " sumFullCost(); ";

                string t1 = " if (!" + t + ") {this.className='td2'; }; hideDiv('divdetail');";
                outBuf.Append("<td " + className + " onMouseover=\"this.className='td2_2';queryCourseDetail('" + courses[i]._courseID + "')\" onMouseout=\"" + t1 + "\" onclick=\"" + t2 + "\"; return;\" >");
                outBuf.Append("<input type=checkbox id=" + name + " name=" + name + " " + isChecked + " onclick=\"" + t + "=!" + t + ";" + "\" ><font class=font01>&nbsp" + courses[i]._btsCourseID + "</font></td>");
                if (i % numCoursesPerRow == numCoursesPerRow-1 ) { outBuf.Append("</tr>\n"); }
                outBuf.Append("<input type=hidden id=cost_" + name + " name=cost_" + name + " value=\"" + courses[i]._cost + "\" >");
            }
            outBuf.Append("</table>");

        }

        public void DoEditPromotion(string promotionID)
        {
            DBManager db = new MySQLDBManager(Config.DB_SERVER, Config.DB_NAME, Config.DB_USER, Config.DB_PASSWORD);
            thePromotion = new Promotion();
            if (!thePromotion.LoadFromDB(db, "promotion_id=" + promotionID)) thePromotion = null;

            PrintCourseOption(db);
            db.Close();
        }    

        protected void DoEditSubmitPromotion(string promotionID)
        {
            Promotion p = new Promotion();

            // validate data
            p._promotionID = Int32.Parse(promotionID);
            p._promotionName = Request["promotion_name"];
            p._promotionDesc = Request["promotion_desc"];
            p._cost = Int32.Parse(Request["cost"]);
            p._isActive = Request["is_active"] != null ? true : false;
            
            // read selected course
            ArrayList idList = new ArrayList();
            for (int i = 0; i < Request.Form.AllKeys.Length; i++)
            {
                if (Request.Form.AllKeys[i].StartsWith("course"))
                {
                    string s = Request.Form.AllKeys[i].Substring(6);
                    idList.Add(s);
                }
            }
            p._courses = new Course[idList.Count];
            int n=0;
            foreach (string id in idList)
            {
                try
                {
                    Course c = new Course();
                    c._courseID = Int32.Parse(id);
                    p._courses[n++] = c;
                }
                catch (Exception e) { Console.WriteLine(e.StackTrace);  }
            }
                    
            
            // Save to DB
            DBManager db = new MySQLDBManager(Config.DB_SERVER, Config.DB_NAME, Config.DB_USER, Config.DB_PASSWORD);
            db.Connect();
            p.UpdateToDB(db);
            db.Close();
        }

        protected void DoDeletePromotion(string promotionID)
        {
            Promotion t = new Promotion();
            t._promotionID = Int32.Parse(promotionID);

            DBManager db = new MySQLDBManager(Config.DB_SERVER, Config.DB_NAME, Config.DB_USER, Config.DB_PASSWORD);
            db.Connect();
            t.DeleteToDB(db);
            db.Close();
        }
    }
}
