﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Amo.Lib.Attributes
{
    /// <summary>
    /// 引用Controller上定义的ApiParameter
    /// 默认不继承Controller的
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class ApiParameterNamesAttribute : Attribute
    {
        public ApiParameterNamesAttribute(params string[] names)
        {
            this.Names = names;
        }

        public string[] Names { get; private set; }
    }
}
