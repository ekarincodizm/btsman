using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using BTS;
using BTS.DB;
using BTS.Entity;
using BTS.Util;

namespace BTS.Page
{

    public partial class DevTool : System.Web.UI.Page
    {
        public bool isSubmit = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (IsPostBack)
            {
                string devaction = Request["devaction"];
                if (devaction == null) return;

                if (devaction.Equals("add_mock_teacher"))
                {
                    int num = 10;
                    if (Request["param1"] != null) num = Int32.Parse(Request["param1"]);
                    
                    AddMockTeacher(num);
                    Response.Write("DONE: " + devaction + " "+ num + "<BR>");
                    isSubmit = true;
                } else if (devaction.Equals("add_mock_student"))
                {
                    int num = 10;
                    if (Request["param1"] != null) num = Int32.Parse(Request["param1"]);

                    AddMockStudent(num);
                    Response.Write("DONE: " + devaction + " " + num + "<BR>");
                    isSubmit = true;
                }
            }


        }

        protected void AddMockStudent(int num)
        {
            DBManager db = new MySQLDBManager(Config.DB_SERVER, Config.DB_NAME, Config.DB_USER, Config.DB_PASSWORD, Config.DB_CHAR_ENC);
            db.Connect();

            Random rand = new Random((int)DateTime.Now.Ticks);

            for (int i = 0; i < num; i++)
            {
                Student t = new Student();

                t._firstname = StringUtil.RandomString(8, true);
                t._surname = StringUtil.RandomString(12, true);
                t._nickname = StringUtil.RandomString(7, true);
                t._img = "student"+rand.Next(1,6)+".jpg";
                t._birthday = new DateTime(rand.Next(1980, 2005), rand.Next(1, 13), rand.Next(1, 29));
                t._school = StringUtil.RandomString(20, true);
                t._tel = "08" + rand.Next(10000000, 99999999);
                t._tel2 = "08" + rand.Next(10000000, 99999999);
                t._sex = (rand.Next(0,2)>0?"Male":"Female");
                t._addr = StringUtil.RandomString(50, true);
        
                t.AddToDB(db);
            }
        }

        protected void AddMockTeacher(int num)
        {
            DBManager db = new MySQLDBManager(Config.DB_SERVER, Config.DB_NAME, Config.DB_USER, Config.DB_PASSWORD, Config.DB_CHAR_ENC);
            db.Connect();

            Random rand = new Random((int)DateTime.Now.Ticks);

            for (int i = 0; i < num; i++)
            {
                Teacher t = new Teacher();

                t._firstname = StringUtil.RandomString(11, true);
                t._surname = StringUtil.RandomString(16, true);
                t._img = "teacher" + rand.Next(1, 6) + ".jpg";
                t._birthday = new DateTime(rand.Next(1900, 2005), rand.Next(1, 13), rand.Next(1, 29));
                t._subject = StringUtil.RandomString(30, true);
                t._tel = "08" + rand.Next(10000000, 99999999);
                t._sex = (rand.Next(0, 2) > 0 ? "Male" : "Female");
                t._addr = StringUtil.RandomString(50, true);

                t.AddToDB(db);
            }
        }

    }
}
