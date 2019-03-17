using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Data;
namespace DSGWGPages
{
    public class ResponseNpHelper
    {
        public static HttpResponseMessage Response(NormalResponse np, bool isFormat = false)
        {
            string str = "";
            if (isFormat)
            {
                str = JsonConvert.SerializeObject(np, Formatting.Indented);
            }
            else
            {
                str = JsonConvert.SerializeObject(np);
            }
            HttpResponseMessage result = new HttpResponseMessage { Content = new StringContent(str, Encoding.GetEncoding("UTF-8"), "application/json") };
            return result;
        }
        public static HttpResponseMessage Response(bool rtu, string msg, object data = null, bool isFormat = false)
        {
            NormalResponse np = new NormalResponse(rtu, msg, data);
            string str = "";
            if (isFormat)
            {
                str = JsonConvert.SerializeObject(np, Formatting.Indented);
            }
            else
            {
                str = JsonConvert.SerializeObject(np);
            }
            HttpResponseMessage result = new HttpResponseMessage { Content = new StringContent(str, Encoding.GetEncoding("UTF-8"), "application/json") };
            return result;
        }
        public static HttpResponseMessage Response(DataTable dt, string TAG = "", bool isFormat = false)
        {
            bool rtu = dt != null;
            string msg = "";
            if (dt == null)
            {
                msg = TAG + " dt is null";
            }
            else if (dt.Rows.Count == 0)
            {
                msg = TAG + " dt.Rows.Count=0";
            }
            else
            {
                msg = "success";
            }
            NormalResponse np = new NormalResponse(rtu, msg, dt);
            string str = "";
            if (isFormat)
            {
                str = JsonConvert.SerializeObject(np, Formatting.Indented);
            }
            else
            {
                str = JsonConvert.SerializeObject(np);
            }
            HttpResponseMessage result = new HttpResponseMessage { Content = new StringContent(str, Encoding.GetEncoding("UTF-8"), "application/json") };
            return result;
        }
    }
}
