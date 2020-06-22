using Amo.Lib.RestClient.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Amo.Lib.RestClient.Contexts
{
    /// <summary>
    /// 表示请求Api的参数描述
    /// </summary>
    public class ApiParameterDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiParameterDescriptor"/> class.
        /// 请求Api的参数描述
        /// </summary>
        /// <param name="parameter">参数信息</param>
        public ApiParameterDescriptor(ParameterInfo parameter)
        {
            this.Value = null;

            // this.Member = parameter ?? throw new ArgumentNullException(nameof(parameter)); ;
            this.Name = parameter.Name;
            this.Index = parameter.Position;

            // this.ParameterType = parameter.ParameterType;
            this.Attributes = parameter.GetCustomAttributes(true).Select(item => item as Attribute).Where(item => item != null).ToList();
            this.ParameterAttribute = this.Attributes.Select(item => item as ParameterAttribute).Where(item => item != null).FirstOrDefault() ?? new QueryAttribute(); // 默认做为Query
        }

        public ApiParameterDescriptor(string name, string value, List<Attribute> attributes)
        {
            this.Name = name;
            this.Value = value;
            this.Attributes = attributes;
            this.ParameterAttribute = this.Attributes?.Select(item => item as ParameterAttribute).Where(item => item != null).FirstOrDefault() ?? new QueryAttribute(); // 默认做为Query
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiParameterDescriptor"/> class.
        /// 请求Api的参数描述
        /// </summary>
        protected ApiParameterDescriptor()
        {
        }

        /// <summary>
        /// 获取参数名称
        /// </summary>
        public string Name { get; protected set; }

        /*
        /// <summary>
        /// 获取关联的参数信息
        /// </summary>
        //public ParameterInfo Member { get; protected set; }

        /// <summary>
        /// 获取参数类型
        /// </summary>
        //public Type ParameterType { get; protected set; }
        */

        /// <summary>
        /// 获取参数索引
        /// </summary>
        public int Index { get; protected set; }

        /// <summary>
        /// 获取参数值
        /// </summary>
        public object Value { get; protected set; }

        /// <summary>
        /// 获取关联的参数特性
        /// </summary>
        public List<Attribute> Attributes { get; protected set; }

        /// <summary>
        /// 参数的类型,Query,Header,Body
        /// </summary>
        public ParameterAttribute ParameterAttribute { get; protected set; }

        /// <summary>
        /// 值转换为字符串
        /// </summary>
        /// <returns>字符串类型的值</returns>
        public override string ToString()
        {
            return this.Value?.ToString();
        }

        /// <summary>
        /// 克隆新设置新的值
        /// </summary>
        /// <param name="value">新的参数值</param>
        /// <returns>Clone一份新的</returns>
        public virtual ApiParameterDescriptor Clone(object value)
        {
            return new ApiParameterDescriptor
            {
                Name = this.Name,
                Index = this.Index,
                Value = value,

                // Member = this.Member,
                // ParameterType = this.ParameterType,
                Attributes = this.Attributes,
                ParameterAttribute = this.ParameterAttribute,
            };
        }
    }
}
