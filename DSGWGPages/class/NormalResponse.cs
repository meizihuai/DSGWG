using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DSGWGPages
{
    public class NormalResponse
    {
        public bool Result { get; set; }
        public string Msg { get; set; }
        public object Data { get; set; }
        public NormalResponse(bool result, string msg)
        {
            Result = result;
            Msg = msg;
        }
        public NormalResponse(bool result, string msg, object data)
        {
            Result = result;
            Msg = msg;
            Data = data;
        }
    }
}
