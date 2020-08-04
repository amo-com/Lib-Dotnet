using System;

namespace Amo.Lib.Attributes
{
    /// <summary>
    /// Setting标记,读取config配置
    /// Config配置,JsonConfig的数据存储路径
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ConfigAttribute : Attribute
    {
        private readonly string type;
        private readonly string path;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigAttribute"/> class.
        /// </summary>
        /// <param name="path">Path: root:seo:titles:ford</param>
        /// <param name="type">Config类型,Program,Public,[Site]</param>
        public ConfigAttribute(string path, string type = null)
        {
            this.path = path;
            this.type = type;
        }

        /// <summary>
        /// Path: root:seo:titles:ford
        /// </summary>
        public string Path => path;

        /// <summary>
        /// Config类型,Program,Public,[Site]
        /// </summary>
        public string Type => type;
    }
}
