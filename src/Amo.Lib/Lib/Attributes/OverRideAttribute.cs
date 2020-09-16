using System;

namespace Amo.Lib.Attributes
{
    /// <summary>
    /// 标记覆盖接口实现类(会覆盖所有基实现类,DI实例替换)
    /// 用于快速的拦截接口,不仅限于dao,service和handler任何一个接口都可以被覆盖,di会自动替换成新的
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class OverRideAttribute : Attribute
    {
        private readonly string[] sites;
        public OverRideAttribute(params string[] sites)
        {
            this.sites = sites;
        }

        public string[] Sites => sites;
    }
}
