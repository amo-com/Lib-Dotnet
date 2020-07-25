using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Amo.Lib.CoreApi
{
    public class ParameterInfo
    {
        public ParameterInfo(string name, string type, string desc, bool required)
        {
            this.Name = name;
            this.Type = type;
            this.Desc = desc;
            this.Required = required;
        }

        public string Name { get; set; }
        public string Type { get; set; }
        public string Desc { get; set; }
        public bool Required { get; set; }
    }
}
