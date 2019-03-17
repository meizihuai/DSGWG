using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DSGWGPages
{
    public class ProgramConfig
    {
        public int id;
        public string name;
        public string program;
        public string filePath;
        public string fileExten;
        public string projectName;
        public string key;
        public string value;
        public string defaultValue;
        public string oldValue;
        public string mark;
        public ProgramConfig()
        {

        }
        public ProgramConfig(int id,string name,string program,string filePath,string fileExten,string projectName,string key,string value,string mark)
        {
            this.id = id;
            this.name = name;
            this.program = program;
            this.filePath = filePath;
            this.fileExten = fileExten;
            this.projectName = projectName;
            this.key = key;
            this.value = value;
            this.defaultValue = value;
            this.mark = mark;
        }
    }
}
