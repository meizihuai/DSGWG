using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DSGWGPages
{
    public class Config
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public string Type { get; set; }
        public string DefaultValue { get; set; }
        public Config()
        {

        }
        public Config(string key,string value,string type="string",string defaultValue="")
        {
            Key = key;
            Value = value;
            Type = type;
            if (defaultValue == "")
            {
                defaultValue = value;
            }
            DefaultValue = defaultValue;
        }
    }
}
