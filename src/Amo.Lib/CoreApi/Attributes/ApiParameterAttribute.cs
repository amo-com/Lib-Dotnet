using Amo.Lib.CoreApi.Models;
using Amo.Lib.Enums;
using System;

namespace Amo.Lib.CoreApi.Attributes
{
    /// <summary>
    /// Swagger中在Header标记参数,用于控制不同controller下的method添加header
    /// 对应Swagger的Microsoft.OpenApi.Models.OpenApiParameter
    /// Method不继承Class的,配合ApiParameterNames使用,在ApiParameterNames中引用需要继承的
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class ApiParameterAttribute : Attribute, IEquatable<ApiParameterAttribute>
    {
        public ApiParameterAttribute(string name, string type, string desc, ApiParameterLocation location, bool required, ApiParameterNeed needType)
        {
            this.Name = name;
            this.Type = type;
            this.Desc = desc;
            this.Location = location;
            this.Required = required;
            this.NeedType = needType;
        }

        public string Name { get; private set; }
        public string Type { get; private set; }
        public string Desc { get; private set; }
        public bool Required { get; private set; }
        public ApiParameterLocation Location { get; private set; }
        public ApiParameterNeed NeedType { get; private set; }

        public bool Equals(ApiParameterAttribute another)
        {
            return Name == another.Name
                && Type == another.Type
                && Desc == another.Desc
                && Required == another.Required
                && Location == another.Location
                && NeedType == another.NeedType;
        }

        public override int GetHashCode()
        {
            return 1;
        }
    }
}
