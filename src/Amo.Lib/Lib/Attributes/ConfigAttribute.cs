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
        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigAttribute"/> class.
        /// </summary>
        /// <param name="path">Path: root:seo:titles:ford</param>
        public ConfigAttribute(string path)
        {
            this.Path = path;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigAttribute"/> class.
        /// </summary>
        /// <param name="path">Path: root:seo:titles:ford</param>
        /// <param name="isClass">是否为自定义实体</param>
        public ConfigAttribute(string path, bool isClass)
        {
            this.Path = path;
            this.IsClass = isClass;
        }

        /// <summary>
        /// Path: root:seo:titles:ford
        /// </summary>
        public string Path { get; private set; }

        public bool IsClass { get; private set; }
    }
}
