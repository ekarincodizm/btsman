using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using BTS.Util;

/// <summary>
/// Summary description for Config
/// </summary>
/// 


namespace BTS
{

    public class Config
    {

        // Constant



        /* ------------------
         *   Configable 
         *-------------------*/

        // WebGui
        public static string WEB_TITLE = "";
        public static string CSS_STYLE = "dogstemplate";
        public static int TBRECORD_PER_PAGE = 25;


        // Database
        public static string DB_SERVER = "localhost";
        public static string DB_NAME = "bts";
        public static string DB_USER = "root";
        public static string DB_PASSWORD = "btsman";

        // Capacity
        public static int DOWNLOADED_FILES_KEEP = 3;

        // Path
        public static string PATH_APP_ROOT = "E:\\Work\\BTS\\BTSMan";
        public static string PATH_LOG = "E:\\Work\\BTS\\BTSMan\\log\\main";
        public static string PATH_SQLLOG = "E:\\Work\\BTS\\BTSMan\\log\\db";
        // should not be changed
        public static string URL_PIC_COURSE = "img/course";
        public static string URL_PIC_TEACHER = "img/teacher";
        public static string URL_PIC_STUDENT = "img/student";
        public static string URL_PIC_ROOM = "img/room";
        public static string URL_PIC_BRANCH = "img/branch";
        public static string URL_PIC_SYS = "img/sys";

        //Predefined-data
        public static string[] COURSE_CATE = new String[] { "THA", "MTH" };

        // Env
        public static bool CONVERT_YEAR2ENG = true;

        // Logging
        // LOG_LEVEL = ERROR,WARNING,NOTICE,INFORMATION,DETAIL,DEBUG,ALL
        public static string LOG_LEVEL = "INFO";
        // SQLLOG_LEVEL = ONLY_UPDATE,ALL
        public static string SQLLOG_LEVEL = "ONLY_UPDATE";
        public static string LOG_NAME = "main.log"; 
        public static string SQLLOG_NAME = "sql.log";

        // Printing
        public static int PAGE_BREAK_MAX = 12;
        public static int PAGE_BREAK_CARD = 2;
        public static int PAGE_BREAK_RECEIPT = 5;
        // Daily registration report
        public static int PAGE_BREAK_DAILY_REG_REPORT = 10;

        // Debug
        public static bool AUTO_LOGIN = false;

        /* ------------------
         *   Internal Used
         *-------------------*/


        public static uint MAINLOG = 1;
        public static uint SQLLOG = 2;
        public static bool APPEND_LOG = true;

        // Code prefix
        public static string BRANCH_CODE_PREFIX = "B";
        public static string COURSE_CODE_PREFIX = "C";
        public static string PAID_GROUP_CODE_PREFIX = "PG";
        public static string PAYMENT_CODE_PREFIX = "PAY";
        public static string PROMOTION_CODE_PREFIX = "PRO";
        public static string REGISTRATION_CODE_PREFIX = "REG";
        public static string TRANSACTION_CODE_PREFIX = "TRS";
        public static string ROOM_CODE_PREFIX = "R";
        public static string STUDENT_CODE_PREFIX = "S";
        public static string TEACHER_CODE_PREFIX = "T";

        // Code Length
        public static int BRANCH_CODE_LENGTH = 2;
        public static int COURSE_CODE_LENGTH = 4;
        public static int PAID_GROUP_CODE_LENGTH = 4;
        public static int PAYMENT_CODE_LENGTH = 6;
        public static int PROMOTION_CODE_LENGTH = 4;
        public static int REGISTRATION_CODE_LENGTH = 8;
        public static int TRANSACTION_CODE_LENGTH = 5;
        public static int ROOM_CODE_LENGTH = 3;
        public static int STUDENT_CODE_LENGTH = 6;
        public static int TEACHER_CODE_LENGTH = 4;



        static Config() {
            Reload();
            Reconfig();
        }

        public static void Reload()
        {
            string configFile = System.Configuration.ConfigurationSettings.AppSettings["BTSConfig"];
            INIFileParser parser = new INIFileParser(configFile);
            
            // Database
            DB_SERVER = parser.GetSetting("Database","DB_SERVER");
            DB_NAME = parser.GetSetting("Database", "DB_NAME");
            DB_USER = parser.GetSetting("Database", "DB_USER");
            DB_PASSWORD = parser.GetSetting("Database", "DB_PASSWORD");

            // Capacity
            DOWNLOADED_FILES_KEEP = Int32.Parse(parser.GetSetting("Capacity", "DOWNLOADED_FILES_KEEP"));

            // WebGUI
            CSS_STYLE = parser.GetSetting("WebGUI", "CSS_STYLE");
            WEB_TITLE = parser.GetSetting("WebGUI", "WEB_TITLE");
            TBRECORD_PER_PAGE = Int32.Parse(parser.GetSetting("WebGUI", "TBRECORD_PER_PAGE"));

            // Path
            PATH_APP_ROOT = parser.GetSetting("Path", "PATH_APP_ROOT");
            PATH_LOG = parser.GetSetting("Path", "PATH_LOG");
            PATH_SQLLOG = parser.GetSetting("Path", "PATH_SQLLOG");
            URL_PIC_COURSE = parser.GetSetting("Path", "URL_PIC_COURSE");
            URL_PIC_TEACHER = parser.GetSetting("Path", "URL_PIC_TEACHER");
            URL_PIC_STUDENT = parser.GetSetting("Path", "URL_PIC_STUDENT");
            URL_PIC_ROOM = parser.GetSetting("Path", "URL_PIC_ROOM");
            URL_PIC_BRANCH = parser.GetSetting("Path", "URL_PIC_BRANCH");
            URL_PIC_SYS = parser.GetSetting("Path", "URL_PIC_SYS");

            // Predefined-data
            string s = parser.GetSetting("Predefined-data", "COURSE_CATE");
            COURSE_CATE = s.Split(',');

            // Logging
            LOG_LEVEL = parser.GetSetting("Logging", "LOG_LEVEL");
            SQLLOG_LEVEL = parser.GetSetting("Logging", "SQLLOG_LEVEL");
            LOG_NAME = parser.GetSetting("Logging", "LOG_NAME");
            SQLLOG_NAME = parser.GetSetting("Logging", "SQLLOG_NAME");

            // Printing
            PAGE_BREAK_MAX = Int32.Parse(parser.GetSetting("Printing", "PAGE_BREAK_MAX"));
            PAGE_BREAK_CARD = Int32.Parse(parser.GetSetting("Printing", "PAGE_BREAK_CARD"));
            PAGE_BREAK_RECEIPT = Int32.Parse(parser.GetSetting("Printing", "PAGE_BREAK_RECEIPT"));
            PAGE_BREAK_DAILY_REG_REPORT = Int32.Parse(parser.GetSetting("Printing", "PAGE_BREAK_DAILY_REG_REPORT"));

            // Env
            CONVERT_YEAR2ENG = Boolean.Parse(parser.GetSetting("Env", "CONVERT_YEAR2ENG"));

            // Debug
            AUTO_LOGIN = Boolean.Parse(parser.GetSetting("Debug", "AUTO_LOGIN"));
            

        }

        public static void Reconfig()
        {
            // Create shared system log
            Logger mainlog = Logger.CreateLogger(MAINLOG, PATH_LOG, LOG_NAME, Logger.GetSeverityByString(LOG_LEVEL));
            Logger sqllog = SQLLogger.CreateLogger(SQLLOG, PATH_SQLLOG, SQLLOG_NAME, SQLLogger.GetSeverityByString(SQLLOG_LEVEL));
        }
/*
        public static void InitConfig(string ConfigFilePath)
        {

            if ((ConfigFilePath == null) || (!File.Exists(ConfigFilePath))) return;

            Console.WriteLine("Reading config -> " + ConfigFilePath);
            StreamReader sr = new StreamReader(ConfigFilePath);
            String line;
            while ((line = sr.ReadLine()) != null)
            {
                try
                {
                    if ((line.Length <= 0) || (line.StartsWith("#"))) continue;

                    string[] config = line.Split('=');
                    if (config.Length != 2) continue;

                    config[0] = config[0].Trim();
                    config[1] = config[1].Trim();

                    SetConfig(config[0], config[1]);

                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: Reading config -> " + line + " trace -> " + e.StackTrace);
                }

            }
            sr.Close();


        }
*/

        public static bool ParseBoolean(string val)
        {
            val = val.ToUpper();
            if ((val.Equals("TRUE")) || (val.Equals("T")) || (val.StartsWith("1")))
                return true;
            else return false;
        }

        public static string ConvertPath(string path)
        {

            return path.Replace("[TODAY]", StringUtil.Date2Filename(DateTime.Today));

        }

    }
}