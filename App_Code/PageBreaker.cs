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
    public class PageBreaker
    {
        protected int _max;
        protected int _counter;

        public PageBreaker(int max )
        {
            _max = max;
            _counter = 0;
        }

        public void Print(StringBuilder outBuf,StringBuilder content, int num)
        {
            _counter += num;

            if (_counter > _max) 
            { 
                outBuf.AppendLine("<P CLASS=\"pagebreakhere\">&nbsp</p>");
                _counter = num;
            }
            outBuf.AppendLine(content.ToString());
        }
    }
}

