using Amo.Lib.Enums;
using System;

namespace Amo.Lib.Attributes
{
    /// <summary>
    /// Swagger中在Header标记参数,用于控制不同controller下的method是否添加公共header
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class NeedHeaderAttribute : Attribute
    {
        private readonly NeedHeaderType type;

        public NeedHeaderAttribute(NeedHeaderType type = NeedHeaderType.All)
        {
            this.type = type;
        }

        public NeedHeaderType Level => type;
    }
}
