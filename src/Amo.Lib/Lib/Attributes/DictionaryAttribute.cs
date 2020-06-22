using System;

namespace Amo.Lib.Attributes
{
    /// <summary>
    /// Setting标记,以字典的形式配置各Site配置
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class DictionaryAttribute : Attribute
    {
        private readonly string site;
        private readonly object value;
        public DictionaryAttribute(string site, object value)
        {
            this.site = site;
            this.value = value;
        }

        public string Site => site;
        public object Value => value;
    }
}
