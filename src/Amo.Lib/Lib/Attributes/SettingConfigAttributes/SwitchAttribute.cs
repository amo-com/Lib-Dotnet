using System;
using System.Collections.Generic;
using System.Text;

namespace Amo.Lib.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SwitchAttribute : Attribute
    {
        public SwitchAttribute(string path)
        {
            this.Path = path;
        }

        public string Path { get; private set; }
        public string SplitChar { get; set; } = ",";
    }
}
