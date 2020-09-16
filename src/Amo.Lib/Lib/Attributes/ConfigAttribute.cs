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
        private readonly string path;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigAttribute"/> class.
        /// </summary>
        /// <param name="path">Path: root:seo:titles:ford</param>
        public ConfigAttribute(string path)
        {
            this.path = path;
        }

        /// <summary>
        /// Path: root:seo:titles:ford
        /// </summary>
        public string Path => path;
    }
}
