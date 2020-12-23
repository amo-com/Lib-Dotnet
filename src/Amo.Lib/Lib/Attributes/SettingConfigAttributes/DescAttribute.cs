using System;

namespace Amo.Lib.Attributes
{
    /// <summary>
    /// Setting的Desc标记,不需要读取外部数据,直接使用
    /// invoke时拼接出带site的描述
    /// ..[Desc(DataConst.SiteParam)]
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class DescAttribute : Attribute
    {
        public DescAttribute(string desc)
        {
            this.Desc = desc;
        }

        public string Desc { get; private set; }
    }
}
