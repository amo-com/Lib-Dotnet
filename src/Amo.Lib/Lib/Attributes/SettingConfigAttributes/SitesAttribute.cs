using System;

namespace Amo.Lib.Attributes
{
    /// <summary>
    /// Site列表,标记Class基于接口实现时对应的Site,替换目录和伪目录的工厂模式,每个Site有自己的Class去实现接口,无法自动注解
    /// 可以用于DI标记实现对应的网站,当一个接口有多个实现类时需要使用,达到每个作用域的实现只有一个,否则会随机取一个实现
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    public class SitesAttribute : Attribute
    {
        public SitesAttribute(params string[] sites)
        {
            this.Sites = sites;
        }

        public string[] Sites { get; private set; }
    }
}
