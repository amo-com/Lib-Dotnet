using Amo.Lib.Enums;
using System;

namespace Amo.Lib.Attributes
{
    /// <summary>
    /// Swagger中在Header标记参数,用于控制不同controller下的method添加header
    /// 对应Swagger的Microsoft.OpenApi.Models.OpenApiParameter
    /// Method不继承Class的,配合ApiParameterNames使用,在ApiParameterNames中引用需要继承的
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ApiParameterAttribute : Attribute
    {
        private readonly string name;
        private readonly string type;
        private readonly string desc;
        private readonly bool required;
        private readonly ApiParameterLocation location;
        private readonly ApiParameterNeed needType;

        public ApiParameterAttribute(string name, string type, string desc, ApiParameterLocation location, bool required, ApiParameterNeed needType)
        {
            this.name = name;
            this.type = type;
            this.desc = desc;
            this.location = location;
            this.required = required;
            this.needType = needType;
        }

        public string Name => name;
        public string Type => type;
        public string Desc => desc;
        public bool Required => required;
        public ApiParameterLocation Location => location;
        public ApiParameterNeed NeedType => needType;
    }
}
