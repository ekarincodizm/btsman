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

using BTS.Constant;
using BTS.DB;
using BTS.Entity;
using BTS.Util;

namespace BTS.Page
{


    public partial class TeacherManage : System.Web.UI.Page
    {
        public string actPage = "list";
        public string targetID = "0";

        public StringBuilder outBuf = new StringBuilder();
        public StringBuilder outBuf2 = new StringBuilder();

        public DateTime startDate;
        public DateTime endDate;
        // branch to did registration
        public string branchRegisedID;
        // branch for the course
        public string branchID;
        public string userName;

        public string branchName;
        public Registration[] reg;
        public Branch[] branchList;
        public AppUser[] userList;
        public Dictionary<string, AppUser> userAllMap;
        public string paidMethod = "-1";

        public int[] numPaidMethod = new int[Registration.PAID_METHOD.Length];
        public int[] sumCostByPaidMethod = new int[Registration.PAID_METHOD.Length];
        public int numSuccess = 0;
        public int sumAllCost = 0;

        public int[] numPaidMethodCancel = new int[Registration.PAID_METHOD.Length];
        public int[] sumCostByPaidMethodCancel = new int[Registration.PAID_METHOD.Length];
        public int numCancel = 0;
        public int sumCancelCost = 0;


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


                string sdate = Request.Form.Get("start_date");
                string edate = Request.Form.Get("end_date");
                branchRegisedID = Request.Form.Get("branch_regis_id");
                branchID = Request.Form.Get("branch_id");
                userName = Request.Form.Get("username");
                paidMethod = Request.Form.Get("paid_method");

                // Verify branch
                if (branchRegisedID == null)
                {
                    AppUser user = (AppUser)Session[SessionVar.USER];
                    if (!user.IsAdmin()) branchRegisedID = user._branchID.ToString(); else branchRegisedID = "0";
                }
                if (branchID == null)
                {
                    branchID = "0";
                }

                // Verify user
                if (userName == null)
                {
                    userName = "0";
                }          
            
                if (paidMethod == null)
                {
                    paidMethod = "-1";
                }


            
                if ((actPage == null) || (actPage.Trim().Length==0) || (actPage.Equals("report")))
                {
                    LoadData(sdate, edate, paidMethod, branchRegisedID, branchID, userName);
                    // Presentation
                    DoInitPrintDailyRegistration();
                    Session[SessionVar.PRINT_INFO] = new StringBuilder(outBuf.ToString());
                    DoReportDailyRegistration();
                }
                else if (actPage.Equals("save_as_excel"))
                {
                    LoadData(sdate, edate, paidMethod, branchRegisedID, branchID, userName);

                    String urlPath = DoSaveDailyAsExcel();

                    Response.Redirect(urlPath);
                }

        }

        protected void LoadData(string startDateString, string endDateString, string paidMethod, string branchRegisedID, String branchID, String username)
        {
            DBManager db = new MySQLDBManager(Config.DB_SERVER, Config.DB_NAME, Config.DB_USER, Config.DB_PASSWORD, Config.DB_CHAR_ENC);
            db.Connect();

            // Load branchlist
            branchList = Branch.LoadListFromDBCustom(db, "SELECT * from branch ORDER BY branch_id ");

            // Load userList all
            AppUser[] userListAll = AppUser.LoadListFromDB(db, "");
            userAllMap = new Dictionary<string, AppUser>();
            foreach (AppUser aUser in userListAll) {
                userAllMap.Add(aUser._username, aUser);
            }
            
            // Load userList for this login
            AppUser loginUser = (AppUser)Session[SessionVar.USER];
            String userQueryClause = "";
            if (loginUser._roleId == Role.ROLE_MANAGEMENT)
            {
                userQueryClause = " WHERE role_id >= " + Role.ROLE_MANAGEMENT;
            }
            else if (loginUser._roleId == Role.ROLE_FRONTSTAFF)
            {
                userQueryClause = " WHERE user_id = " + loginUser._userId;
            }
            userList = AppUser.LoadListFromDB(db, userQueryClause + " order by firstname");

            try 
            {
                string[] s = startDateString.Split('/');

                startDate = new DateTime(Int32.Parse(s[2]), Int32.Parse(s[1]), Int32.Parse(s[0]));
            }
            catch (Exception e)
            {
                startDate = DateTime.Today;
            }

            try
            {
                string[] s = endDateString.Split('/');
                endDate = new DateTime(Int32.Parse(s[2]), Int32.Parse(s[1]), Int32.Parse(s[0]));
                endDate = endDate.AddHours(23).AddMinutes(59).AddSeconds(59);
            }
            catch (Exception e)
            {
                endDate = DateTime.Today.AddHours(23).AddMinutes(59).AddSeconds(59);
            }

            // Get branch name
            if (branchRegisedID.Equals("0"))
            {
                branchName = "ทุกสาขา";
            }
            else
            {
                Branch b = new Branch();
                b.LoadFromDB(db, " branch_id=" + branchRegisedID);
                branchName = b._branchName;
            }

            // Filter user
            if (loginUser._roleId > Role.ROLE_MANAGEMENT)
            {
                if (username.Equals("all"))
                {
                    username = loginUser._username;
                }
            }

            // construct room list for the branch
            string roomList = "";
            if (!branchID.Equals("0"))
            {
                string roomSQL = "SELECT room_id FROM room where branch_id=" + branchID;
                Room[] rooms = Room.LoadListFromDBCustom(db, roomSQL);
                if (rooms.Length > 0)
                {
                    foreach (Room r in rooms)
                    {
                        roomList = roomList + "," + r._roomID;
                    }
                    roomList = "( " + roomList.Substring(1) + ")";
                }
            }


            string selectSQl = "SELECT rg.*, b.branch_code, s.firstname as student_firstname, s.surname as student_surname, s.school as student_school, s.level as student_level, c.bts_course_id as bts_course_id, c.course_name as course_name "
                               + " FROM registration rg, student s, course c, branch b ";
            string whereSQL = " WHERE rg.student_id=s.student_id AND rg.course_id=c.course_id "
                                + " AND rg.regis_date between '" + startDate.ToString("yyyy/MM/dd HH:mm:ss", ci) + "' and '" + endDate.ToString("yyyy/MM/dd HH:mm:ss", ci) + "' "
                                + ((!paidMethod.Equals("-1")) ? " AND rg.paid_method=" + paidMethod : "")
                                + ((!branchRegisedID.Equals("0")) ? " AND rg.branch_id=" + branchRegisedID : "")
                                + ((!username.Equals("all")) ? " AND rg.username='" + username + "'" : "")
                                + ((roomList.Length > 0) ? " AND c.room_id in " + roomList : "")
                                + " AND rg.branch_id = b.branch_id "
                                + " ORDER BY rg.regis_id ";
            reg = Registration.LoadListFromDBCustom(db, selectSQl + whereSQL);
            db.Close();


            for (int i = 0; i < reg.Length; i++)
            {
                if (reg[i]._status == 0) // normal 
                {
                    numPaidMethod[reg[i]._paidMethod]++;
                    sumCostByPaidMethod[reg[i]._paidMethod] += reg[i]._discountedCost;
                    numSuccess++;
                    sumAllCost += reg[i]._discountedCost;
                }
                else if (reg[i]._status == 1) // cancel
                {
                    numPaidMethodCancel[reg[i]._paidMethod]++;
                    sumCostByPaidMethodCancel[reg[i]._paidMethod] += reg[i]._discountedCost;
                    numCancel++;
                    sumCancelCost += reg[i]._discountedCost;
                }
            }
        }

        protected int DeleteOldReportFile(String prefix)
        {
            DirectoryInfo di = new DirectoryInfo(Config.PATH_APP_ROOT + "\\file\\");
            FileInfo[] rgFiles = di.GetFiles(prefix + "*.csv");

            long nowTicks = DateTime.Now.Ticks;
            long keepTicks = Config.DOWNLOADED_FILES_KEEP * 86400 * 10000000;
            int numDel = 0;
            foreach (FileInfo fi in rgFiles)
            {
                string tickStr = fi.Name.Substring(prefix.Length, 20);
                try
                {
                    long ticks = long.Parse(tickStr);
                    if ((nowTicks - ticks) > keepTicks) 
                    {
                        fi.Delete();
                        numDel++;
                    }
                }
                catch (Exception e)
                {
                    continue;
                }
            }
            return numDel;
        }

        protected String DoSaveDailyAsExcel()
        {
            DBManager db = new MySQLDBManager(Config.DB_SERVER, Config.DB_NAME, Config.DB_USER, Config.DB_PASSWORD, Config.DB_CHAR_ENC);

            String prefix = "registration_report_";
            int numDel = DeleteOldReportFile(prefix);
            log.StampLine(Logger.INFO, "Delete old " + prefix + " " + numDel + " files");
            
            DateTime now = DateTime.Now;
            String dateStr = StringUtil.FillString(now.Ticks.ToString(), "0", 20, true) + "_"
                + StringUtil.FillString(Session.GetHashCode().ToString(), "0", 10, true);

            String filename = prefix + dateStr + ".csv";
            String physPath = Config.PATH_APP_ROOT + "\\file\\" + filename;
            String urlPath = "file/" + filename;

            FileStream fs = File.Open(physPath, FileMode.OpenOrCreate, FileAccess.Write);
            TextWriter writer = new StreamWriter(fs, System.Text.Encoding.UTF8);

            StringBuilder stdTxt = new StringBuilder();
            //stdTxt.Append(StringUtil.GetExcelEncodingPrefix());
             //ลำดับ วันที่ เลขที่ใบเสร็จ นักเรียน โรงเรียน ระดับชั้น รหัสคอร์ส ชื่อคอร์ส ราคา วิธีการชำระ สถานะ หมายเหตุ ชื่อผู้สมัคร
            stdTxt.Append("รายงานสรุปยอดการลงทะเบียนประจำวันของสาขา " + branchName + "ตั้งแต่วันที่ " + StringUtil.ConvertYearToEng(startDate, "dd/MM/yyyy") + " ถึง " + StringUtil.ConvertYearToEng(startDate, "dd/MM/yyyy") +"\n");
            stdTxt.Append("ลำดับ, วัน-เวลา, รหัสใบเสร็จ, รหัสนักเรียน, ชื่อ-นามสกุล, โรงเรียน, ระดับชั้น,รหัสคอร์ส, ชื่อคอร์ส, ราคาจ่าย, วิธีชำระเงิน, สถานะ, ผู้รับสมัคร  \n");

            // content
            for (int i = 0; i < reg.Length; i++)
            {
                //numPaidMethod[reg[i]._paidMethod]++;
                //sumCostByPaidMethod[reg[i]._paidMethod] += reg[i]._discountedCost;
                //sumAllCost += reg[i]._discountedCost;

                AppUser theUser = userAllMap[reg[i]._username];

                stdTxt.Append(i + 1);
                stdTxt.Append(", " + reg[i]._regisdate.ToString("dd/MM/yyyy HH:mm", ci));
                stdTxt.Append(", " + reg[i].GetRegisTransactionID());
                stdTxt.Append(", " + Student.GetStudentID(reg[i]._studentID));
                stdTxt.Append(", " + StringUtil.AddCSVDQuote(reg[i]._studentFirstname + " " + reg[i]._studentSurname));
                stdTxt.Append(", " + StringUtil.AddCSVDQuote(reg[i]._studentSchool));
                stdTxt.Append(", " + StringUtil.AddCSVDQuote(StringUtil.ConvertEducateLevel(reg[i]._studentLevel)));
                stdTxt.Append(", " + reg[i]._btsCourseID );
                stdTxt.Append(", " + StringUtil.AddCSVDQuote(reg[i]._courseName));
                stdTxt.Append(", " + reg[i]._discountedCost);
                stdTxt.Append(", " + Registration.GetPaidMethodText(reg[i]._paidMethod.ToString()));
                stdTxt.Append(", " + Registration.GetStatusText(reg[i]._status));
                stdTxt.Append(", " + StringUtil.AddCSVDQuote(theUser._firstname + " " + theUser._surname));
                stdTxt.Append("\n");

                if (i % 100 == 99) 
                {
                    writer.Write(stdTxt);
                    stdTxt = new StringBuilder();
                }
            }
            if (stdTxt.Length > 0)
            {
                writer.Write(stdTxt);
            }

            writer.Close();

            return urlPath;
        }

        protected void DoInitPrintDailyRegistration()
        {
            outBuf = new StringBuilder();

            // Generate HTML content
            int maxpg = reg.Length / Config.PAGE_BREAK_DAILY_REG_REPORT;
            if (maxpg < 1) { maxpg = 1; }
            else if (reg.Length % Config.PAGE_BREAK_DAILY_REG_REPORT > 0) { maxpg++; }

            for (int j = 0; j < maxpg; j++)
            {
                int idx1 = j * Config.PAGE_BREAK_DAILY_REG_REPORT;
                int idx2 = (j+1) * Config.PAGE_BREAK_DAILY_REG_REPORT;
                if (idx2>reg.Length) idx2=reg.Length;

                // header table
                //ลำดับ วันที่ เลขที่ใบเสร็จ นักเรียน โรงเรียน ระดับชั้น รหัสคอร์ส ชื่อคอร์ส ราคา วิธีการชำระ สถานะ หมายเหตุ ชื่อผู้สมัคร
                StringBuilder contentTxt = new StringBuilder();
                contentTxt.AppendLine("<tr bgcolor=\"#CAE8EA\">");
                contentTxt.AppendLine("<th scope=\"col\" align=center width=\"30px\" NOWRAP><font size=2><b>ลำดับ</b></font></th>");
                contentTxt.AppendLine("<th scope=\"col\" align=center width=\"80px\"><font size=2><b>วัน-เวลา</b></font></th>");
                contentTxt.AppendLine("<th scope=\"col\" align=center width=\"100px\"><font size=2><b>รหัสใบเสร็จ</b></font></th>");
                contentTxt.AppendLine("<th scope=\"col\" align=center width=\"150px\"><font size=2><b>นักเรียน</b></font></th>");
                contentTxt.AppendLine("<th scope=\"col\" align=center width=\"100px\"><font size=2><b>โรงเรียน</b></font></th>");
                contentTxt.AppendLine("<th scope=\"col\" align=center width=\"100px\"><font size=2><b>ระดับชั้น</b></font></th>");
                contentTxt.AppendLine("<th scope=\"col\" align=center width=\"300px\"><font size=2><b>คอร์ส</b></font></th>");
                contentTxt.AppendLine("<th scope=\"col\" align=center width=\"70px\"><font size=2><b>ราคาจ่าย</b></font></th>");
                contentTxt.AppendLine("<th scope=\"col\" align=center width=\"70px\"><font size=2><b>วิธีชำระเงิน</b></font></th>");
                contentTxt.AppendLine("<th scope=\"col\" align=center width=\"70px\"><font size=2><b>สถานะ</b></font></th>");
                contentTxt.AppendLine("<th scope=\"col\" align=center width=\"150px\"><font size=2><b>ชื่อผู้สมัคร</b></font></th>");
                contentTxt.AppendLine("</tr>");

           
                // content
                for (int i = idx1; i < idx2; i++)
                {
                    //numPaidMethod[reg[i]._paidMethod]++;
                    //sumCostByPaidMethod[reg[i]._paidMethod] += reg[i]._discountedCost;
                    //sumAllCost += reg[i]._discountedCost;

                    AppUser theUser = userAllMap[reg[i]._username];

                    contentTxt.AppendLine("<tr>");
                    contentTxt.AppendLine("<td><font size=2>" + (i + 1) + "</font></td>");
                    contentTxt.AppendLine("<td align=center><font size=2>" + reg[i]._regisdate.ToString("dd/MM/yyyy HH:mm", ci) + "</font></td>");
                    contentTxt.AppendLine("<td align=center><font size=2>" + reg[i].GetRegisTransactionID() + "</font></td>");
                    contentTxt.AppendLine("<td><font size=2>" + reg[i]._studentFirstname + " " + reg[i]._studentSurname + "</font></td>");
                    contentTxt.AppendLine("<td><font size=2>" + reg[i]._studentSchool + "</font></td>");
                    contentTxt.AppendLine("<td><font size=2>" + StringUtil.ConvertEducateLevel(reg[i]._studentLevel) + "</font></td>");
                    contentTxt.AppendLine("<td><font size=2>" + reg[i]._btsCourseID + " " + reg[i]._courseName + "</font></td>");
                    contentTxt.AppendLine("<td align=center><font size=2>" + StringUtil.Int2StrComma(reg[i]._discountedCost) + "</font></td>");
                    contentTxt.AppendLine("<td align=center><font size=2>" + Registration.GetPaidMethodText(reg[i]._paidMethod.ToString()) + "</font></td>");
                    contentTxt.AppendLine("<td><font size=2>" + Registration.GetStatusText(reg[i]._status) + "</font></td>");
                    contentTxt.AppendLine("<td><font size=2>" + theUser._firstname + " " + theUser._surname + "</font></td>");
                    contentTxt.AppendLine("</tr>");
                }

                // summary table
                StringBuilder summaryTxt = new StringBuilder();
                if (j == (maxpg - 1))
                {
                    // success                        
                    summaryTxt.AppendLine("<br>");
                    summaryTxt.AppendLine("<table width=\"300px\" align=right border=1 cellpadding=0 cellspacing=0 bordercolor=\"#C1DAD7\" bgcolor=\"#FFFFFF\">");
                    summaryTxt.AppendLine(" <tr bgcolor=\"#CAE8EA\"><td colspan=3 align=center><b>สรุปยอดรับสมัคร</b></td></tr>");
                    summaryTxt.AppendLine(" <tr bgcolor=\"#CAE8EA\"><td width=\"100px\" align=center><b>วิธีชำระเงิน</b></td><td width=\"100px\" align=center><b>รายการ</b></td><td width=\"100px\" align=center><b>ยอดรวม(บาท)</b></td></tr>");
                      for (int k=0;k<numPaidMethod.Length;k++) 
                      {
                          summaryTxt.AppendLine("<tr bgcolor=\"#FFFFFF\">");
                          summaryTxt.AppendLine("<td align=center>"+Registration.GetPaidMethodText(k.ToString())+"</td>");
                          summaryTxt.AppendLine("<td align=center>"+numPaidMethod[k]+"</td>");
                          summaryTxt.AppendLine("<td align=right>"+StringUtil.Int2StrComma(sumCostByPaidMethod[k])+"&nbsp&nbsp</td>");
                          summaryTxt.AppendLine("</tr>");
                      }
                    summaryTxt.AppendLine("<tr bgcolor=\"#FFFFFF\">");
                    summaryTxt.AppendLine("<td align=center><b>รวมทั้งสิ้น</b></td>");
                    summaryTxt.AppendLine("<td align=center><b>"+ numSuccess +"</b></td>");
                    summaryTxt.AppendLine("<td align=right><b>"+ StringUtil.Int2StrComma(sumAllCost) +"</b>&nbsp&nbsp</td>");
                    summaryTxt.AppendLine("</tr>");
//                    summaryTxt.AppendLine("</table>");
                    // cancel
//                    summaryTxt.AppendLine("<br>");
//                    summaryTxt.AppendLine("<table width=\"300px\" align=right border=1 cellpadding=0 cellspacing=0 bordercolor=\"#C1DAD7\" bgcolor=\"#FFFFFF\">");
                    summaryTxt.AppendLine(" <tr bgcolor=\"#CAE8EA\"><td colspan=3 align=center><b>สรุปยอดยกเลิก</b></td></tr>");
                    summaryTxt.AppendLine(" <tr bgcolor=\"#CAE8EA\"><td width=\"100px\" align=center><b>วิธีชำระเงิน</b></td><td width=\"100px\" align=center><b>รายการ</b></td><td width=\"100px\" align=center><b>ยอดรวม(บาท)</b></td></tr>");
                    for (int k = 0; k < numPaidMethodCancel.Length; k++)
                    {
                        summaryTxt.AppendLine("<tr bgcolor=\"#FFFFFF\">");
                        summaryTxt.AppendLine("<td align=center>" + Registration.GetPaidMethodText(k.ToString()) + "</td>");
                        summaryTxt.AppendLine("<td align=center>" + numPaidMethodCancel[k] + "</td>");
                        summaryTxt.AppendLine("<td align=right>" + StringUtil.Int2StrComma(sumCostByPaidMethodCancel[k]) + "&nbsp&nbsp</td>");
                        summaryTxt.AppendLine("</tr>");
                    }
                    summaryTxt.AppendLine("<tr bgcolor=\"#FFFFFF\">");
                    summaryTxt.AppendLine("<td align=center><b>รวมทั้งสิ้น</b></td>");
                    summaryTxt.AppendLine("<td align=center><b>" + numCancel + "</b></td>");
                    summaryTxt.AppendLine("<td align=right><b>" + StringUtil.Int2StrComma(sumCancelCost) + "</b>&nbsp&nbsp</td>");
                    summaryTxt.AppendLine("</tr>");
                    summaryTxt.AppendLine("</table>");

                }


                TextReader reader = new StreamReader(Config.PATH_APP_ROOT + "\\template\\report_daily_registration_print.htm");
                String templateContent = reader.ReadToEnd();
                reader.Close();

                String htmlContent =
                    String.Format(templateContent
                        , branchName
                        , StringUtil.ConvertYearToEng(startDate, "dd/MM/yyyy")
                        , StringUtil.ConvertYearToEng(endDate, "dd/MM/yyyy")
                        , contentTxt.ToString()
                        , summaryTxt.ToString()
                        );

                outBuf.AppendLine(htmlContent);
                
                // Add page break for all not last page
                
                if (j < maxpg - 1)
                {
                    outBuf.AppendLine("<P CLASS=\"pagebreakhere\">&nbsp</p>");
                }
                
            }
        }



        protected void DoReportDailyRegistration()
        {
            outBuf = new StringBuilder();

            // Presentaion
            string[,] bgclass = new string[,] { { "class=\"spec\"", "class=\"td1\"" }, { "class=\"specalt\"", "class=\"alt\"" } };
            for (int i = 0; i < reg.Length; i++)
            {
                //numPaidMethod[reg[i]._paidMethod]++;
                //sumCostByPaidMethod[reg[i]._paidMethod] += reg[i]._discountedCost;
                AppUser theUser = userAllMap[reg[i]._username];
                //sumAllCost += reg[i]._discountedCost;

                string studentTxt = "<a href=\"StudentManage.aspx?actPage=view&targetID=" + reg[i]._studentID + "\">" + reg[i]._studentFirstname + " " + reg[i]._studentSurname + "</a>";
                string courseTxt = "<a href=\"CourseManage.aspx?actPage=view&targetID="+reg[i]._courseID+"\">" + reg[i]._btsCourseID + " " + reg[i]._courseName + "</a>";
                outBuf.Append("<tr>");
                outBuf.Append("<th scope=\"row\" abbr=\"Model\" " + bgclass[i % 2, 0] + ">" + (i + 1) + "</th>");
                outBuf.Append("<td " + bgclass[i % 2, 1] + "  >" + reg[i]._regisdate.ToString("dd/MM/yyyy HH:mm", ci) + "&nbsp</td>");
                outBuf.Append("<td " + bgclass[i % 2, 1] + " align=center  >" + reg[i].GetRegisTransactionID() + "&nbsp</td>");
                outBuf.Append("<td " + bgclass[i % 2, 1] + "  >" + studentTxt + "&nbsp</td>");
                outBuf.Append("<td " + bgclass[i % 2, 1] + "  >" + reg[i]._studentSchool + "&nbsp</td>");
                outBuf.Append("<td " + bgclass[i % 2, 1] + "  >" + StringUtil.ConvertEducateLevel( reg[i]._studentLevel) + "&nbsp</td>");
                outBuf.Append("<td " + bgclass[i % 2, 1] + "  >" + courseTxt + "&nbsp</td>");
                outBuf.Append("<td " + bgclass[i % 2, 1] + " align=right >" + StringUtil.Int2StrComma(reg[i]._discountedCost) + "&nbsp</td>");
                outBuf.Append("<td " + bgclass[i % 2, 1] + "  >" + Registration.GetPaidMethodText(reg[i]._paidMethod.ToString()) + "&nbsp</td>");
                outBuf.Append("<td " + bgclass[i % 2, 1] + "  >" + Registration.GetStatusText(reg[i]._status) + "&nbsp</td>");
                outBuf.Append("<td " + bgclass[i % 2, 1] + "  >" + theUser._firstname + " " + theUser._surname + "&nbsp</td>");
                outBuf.Append("</tr>\n");
            }

        }


    }
}
