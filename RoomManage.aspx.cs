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
    public partial class RoomManage : System.Web.UI.Page
    {
        public string actPage = "list";
        public StringBuilder outBuf = new StringBuilder();
        public StringBuilder outBuf2 = new StringBuilder();
        public String targetID;
        public Room theRoom;
        public List<Room> listRoom;
        public Branch[] branchList;

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

            errorText = Request["errorText"];
            
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
                    DoListRoom(qSearch, isNewSearch);
                }
                else if (actPage.Equals("add"))
                {
                    DoAddRoom();
                }
                else if (actPage.Equals("add_submit"))
                {
                    DoAddSubmitRoom();
                    Response.Redirect("RoomManage.aspx");
                }
                else if (actPage.Equals("edit"))
                {
                    DoEditRoom(targetID);
                }
                else if (actPage.Equals("edit_submit"))
                {
                    DoEditSubmitRoom(targetID);
                    Response.Redirect("RoomManage.aspx");
                }
                else if (actPage.Equals("delete"))
                {
                    DoDeleteRoom(targetID);
                    Response.Redirect("RoomManage.aspx");
                }
        }

        
        protected void DoListRoom(string searchStr, bool isNewSearch)
        {
            // get Page
            int pg = 1;
            if ((!isNewSearch) && (Request["pg"] != null)) pg = Int32.Parse(Request["pg"]);

            string[,] bgclass = new string[,] { { "class=\"spec\"", "class=\"td1\"" }, { "class=\"specalt\"", "class=\"alt\"" } };

            listRoom = new List<Room>();
            DBManager db = new MySQLDBManager(Config.DB_SERVER, Config.DB_NAME, Config.DB_USER, Config.DB_PASSWORD);
            string whereSQL = Room.GetQSearchSQL(searchStr);
            if (whereSQL.Length > 0) whereSQL = " WHERE " + whereSQL;

            int numRec = db.QueryCount("SELECT Count(*) FROM room " + whereSQL);


            if (whereSQL.Length > 0) whereSQL = whereSQL + " AND r.branch_id=b.branch_id ";
            else whereSQL = " WHERE r.branch_id=b.branch_id ";
            

            OdbcDataReader reader = db.Query("SELECT r.*,b.branch_name as branch_name FROM room r, branch b " + whereSQL +"ORDER BY room_id LIMIT " + Config.TBRECORD_PER_PAGE + " OFFSET " + (((pg-1)*Config.TBRECORD_PER_PAGE)) );
            int i = 0;
            while (reader.Read())
            {
                Room room = Room.CreateForm(reader);

                string divtxt = Config.URL_PIC_TEACHER + "/" + room._img;

                outBuf.Append("<tr>");
                outBuf.Append("<th scope=\"row\" abbr=\"Model\" " + bgclass[i % 2, 0] + " align=center valign=top width=100px>" +Room.GetRoomID(room._roomID) + " &nbsp</th>");
                outBuf.Append("<td " + bgclass[i % 2, 1] + "  align=left><p><b>ห้อง: </b>" + room._name + "</p>");
                outBuf.Append("<p><b>สาขา: </b>" + room._branchName + "&nbsp</p>");
                outBuf.Append("<p><b>จำนวนที่นั่ง: </b>" + room._seatNo + "&nbsp</p>");
                outBuf.Append("<p><b>รายละเอียด: </b></p><p>" + room._description.Replace("\r\n","<br>") + "&nbsp</p>");                
                outBuf.Append("&nbsp</td>");
                outBuf.Append("<td " + bgclass[i % 2, 1] + "  align=center><a href=\"" + Config.URL_PIC_ROOM + "/" + room._img + "\" ><img border=0 width=200px height=150px src=\"" + Config.URL_PIC_ROOM + "/" + room._img + "\" ></a></td>");

                outBuf.Append("<td " + bgclass[i % 2, 1] + "  align=center>&nbsp");
                outBuf.Append("<a href=\"javascript:setVal('actPage','edit');setVal('targetID','" + room._roomID + "');doSubmit()\"><img src=\"img/sys/edit.gif\" border=0 alt=\"Edit\"></a>&nbsp");
                outBuf.Append("<a href=\"javascript:if (confirm('Delete this room?')) { setVal('actPage','delete');setVal('targetID','" + room._roomID + "');doSubmit(); }\"><img src=\"img/sys/delete.gif\" border=0 alt=\"Delete\"></a>&nbsp");
 
                outBuf.Append("</td>");
                outBuf.Append("</tr>\n");

                i++;
            }
            db.Close();
            
            // calculate max page            
            int maxpg = numRec / Config.TBRECORD_PER_PAGE;
            if (maxpg < 1) { maxpg = 1; }
            else if (numRec % Config.TBRECORD_PER_PAGE > 0) { maxpg++; }
            // Generate Page Navi HTML
            outBuf2.Append("<b>Page</b>  ");
            for (i = 1; i <= maxpg; i++)
            {
                if (i == pg) { outBuf2.Append("<b>"+i+"</b> "); }
                else {
                    outBuf2.Append(String.Format("<a href=\"TeacherManage.aspx?pg={0}&qsearch={1}\">{0}</a> ", i.ToString(), searchStr));
                }

            }
        }

        public void DoAddRoom()
        {
            DBManager db = new MySQLDBManager(Config.DB_SERVER, Config.DB_NAME, Config.DB_USER, Config.DB_PASSWORD);
            branchList = Branch.LoadListFromDB(db, "");
            db.Close();
        }

        protected void DoAddSubmitRoom()
        {
            try
            {
                Room r = new Room();

                // validate data
                r._name = Request["roomname"];
                r._branchID = Int32.Parse(Request["branch_id"]);
                r._seatNo = Int32.Parse(Request["seat_no"]);
                r._description = Request["description"];

                r._img = "noimg.jpg";
                if (portrait.PostedFile.FileName != "")
                {

                    string serverFileExt = Path.GetExtension(portrait.PostedFile.FileName);
                    Random rand = new Random((int)DateTime.Now.Ticks);
                    string fullpath = "";
                    string imgname = "";
                    do
                    {
                        string randomFName = rand.Next(Int32.MaxValue).ToString();
                        imgname = randomFName + serverFileExt;
                        fullpath = Config.PATH_APP_ROOT + "\\" + Config.URL_PIC_ROOM + "\\" + imgname;
                    } while (File.Exists(fullpath));

                    portrait.PostedFile.SaveAs(fullpath);
                    r._img = imgname;

                }

                // Save to DB
                DBManager db = new MySQLDBManager(Config.DB_SERVER, Config.DB_NAME, Config.DB_USER, Config.DB_PASSWORD);
                db.Connect();
                r.AddToDB(db);
                db.Close();
            }
            catch (Exception err)
            {
                errorText = err.Message + err.StackTrace;
                Response.Redirect("RoomManage.aspx?actPage=add&errorText=ข้อมูลไม่ถูกต้อง โปรดตรวจสอบอีกครั้ง");
            }



        }

        public void DoEditRoom(string roomID)
        {
            DBManager db = new MySQLDBManager(Config.DB_SERVER, Config.DB_NAME, Config.DB_USER, Config.DB_PASSWORD);
            theRoom = new Room();
            if (!theRoom.LoadFromDB(db, "room_id=" + roomID)) theRoom = null;

            branchList = Branch.LoadListFromDB(db, "");

            db.Close();
        }

        protected void DoEditSubmitRoom(string roomID)
        {
            try
            {
                Room r = new Room();

                // validate data
                r._roomID = Int32.Parse(roomID);
                r._name = Request["roomname"];
                r._branchID = Int32.Parse(Request["branch_id"]);
                r._seatNo = Int32.Parse(Request["seat_no"]);
                r._description = Request["description"];

                // default to old value
                r._img = Request["img_old"];
                if (portrait.PostedFile.FileName != "")
                {
                    string serverFileExt = Path.GetExtension(portrait.PostedFile.FileName);
                    Random rand = new Random((int)DateTime.Now.Ticks);
                    string fullpath = "";
                    string imgname = "";
                    do
                    {
                        string randomFName = rand.Next(Int32.MaxValue).ToString();
                        imgname = randomFName + serverFileExt;
                        fullpath = Config.PATH_APP_ROOT + "\\" + Config.URL_PIC_ROOM + "\\" + imgname;
                    } while (File.Exists(fullpath));

                    portrait.PostedFile.SaveAs(fullpath);
                    r._img = imgname;
                }

                // Save to DB
                DBManager db = new MySQLDBManager(Config.DB_SERVER, Config.DB_NAME, Config.DB_USER, Config.DB_PASSWORD);
                db.Connect();
                r.UpdateToDB(db);
                db.Close();
            }
            catch (Exception err)
            {
                errorText = err.Message + err.StackTrace;
                Response.Redirect("RoomManage.aspx?actPage=edit&targetID="+targetID+"&errorText=ข้อมูลไม่ถูกต้อง โปรดตรวจสอบอีกครั้ง");
            }
        }

        protected void DoDeleteRoom(string roomID)
        {
            Room t = new Room();
            t._roomID = Int32.Parse(roomID);

            DBManager db = new MySQLDBManager(Config.DB_SERVER, Config.DB_NAME, Config.DB_USER, Config.DB_PASSWORD);
            db.Connect();
            t.DeleteToDB(db);
            db.Close();
        }
    }
}
