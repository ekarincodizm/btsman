using System;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// Summary description for StringUtil
/// </summary>
/// 
namespace BTS.Util
{
    public class StringUtil
    {

        public static string Convert2MySQLDate(string date)
        {
            string[] d = date.Split('/');
            return d[2] + "-" + d[1] + "-" + d[0];
        }

        public static string ConvertYearToEng(DateTime d, string format)
        {
            if (Config.CONVERT_YEAR2ENG)
            {
                try
                {
                    d = d.AddYears(-543);
                }
                catch (Exception e) { 
                    // ignore convert 
                }
            }
            return d.ToString(format);
        }

        public static string ConvertYearToEng(DateTime d, string format, CultureInfo ci)
        {
            if (Config.CONVERT_YEAR2ENG)
            {
                try
                {
                    d = d.AddYears(-543);
                }
                catch (Exception e)
                {
                    // ignore convert 
                }
            }
            return d.ToString(format, ci);
        }

        public static string FillString(string str, string filler, int finalLength)
        {
            return FillString(str, filler, finalLength, false);
        }

        public static string FillString(string str, string filler, int finalLength, bool FillOnFront)
        {
            if ((str == null) || (filler == null) || (filler.Length <= 0)) return str;

            while (str.Length < finalLength)
            {
                str = (FillOnFront) ? filler + str : str + filler;
            }
            return str;
        }

        public static string AddCSVDQuote(String text)
        {
            if (text == null) return "";

            text = text.Replace(Environment.NewLine, " ");
            if (text.Contains(","))
            {
                return "\"" + text + "\"";
            }
            return text;
        }

        public static string Date2Filename(DateTime date)
        {
            return FillString(date.Year.ToString(), "0", 4, true)
                     + FillString(date.Month.ToString(), "0", 2, true)
                     + FillString(date.Day.ToString(), "0", 2, true);

        }

        public static string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }

        public static DateTime getDate(string dt)
        {
            if (dt != null)
            {
                string[] s = dt.Split('/');

                return new DateTime(Int32.Parse(s[2]), Int32.Parse(s[1]), Int32.Parse(s[0]));
            }
            else
            {
                return DateTime.Now;
            }
        }

        public static DateTime getTime(string hh, string mm)
        {
            if ((hh != null) && (mm != null))
            {
                return new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Int32.Parse(hh), Int32.Parse(mm), 0, 0);
            }
            else
            {
                return DateTime.Now;
            }
        }

        public static string ConvertEducateLevel(int level)
        {
            switch (level)
            {
                case 0: return "ไม่ระบุ";
                case 1: return "ป.1";
                case 2: return "ป.2";
                case 3: return "ป.3";
                case 4: return "ป.4";
                case 5: return "ป.5";
                case 6: return "ป.6";
                case 7: return "ม.1";
                case 8: return "ม.2";
                case 9: return "ม.3";
                case 10: return "ม.4";
                case 11: return "ม.5";
                case 12: return "ม.6";
                case 13: return "ปี 1";
                case 14: return "ปี 2";
                case 15: return "ปี 3";
                case 16: return "ปี 4";
                case 17: return "ปี 5";
                case 18: return "ปี 6";
                default: return "ไม่ระบุ";
            }
        }


        public static string Int2StrComma(int n)
        {
            string num = n.ToString();
            string sign = "";
            if (num.StartsWith("-"))
            {
                num = num.Substring(1);
                sign = "-";
            }
            StringBuilder s = new StringBuilder();
            int count = 0;
            for (int i = num.Length; i > 0; i--)
            {
                char ch = num[i-1];
                s.Insert(0,ch);
                if (((++count) == 3) && (i>1))
                {
                    s.Insert(0, ",");
                    count = 0;
                }
            }
            return sign + s.ToString();
        }

        public static string GetExcelEncodingPrefix()
        {
            //byte[] ba = new byte[] { 239, 187, 191 };
            byte[] ba = new byte[] { 255, 254};
            char[] bac = new char[Encoding.UTF8.GetCharCount(ba, 0, ba.Length)];
            Encoding.UTF8.GetChars(ba, 0, ba.Length, bac, 0);
            string baasc = new string(bac);
            return baasc;
        }

    }
}

