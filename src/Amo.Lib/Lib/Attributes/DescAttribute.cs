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
        private readonly string desc;
        public DescAttribute(string desc)
        {
            this.desc = desc;
        }

        public string Desc => desc;
    }
}
