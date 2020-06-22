using System;
using System.Collections.Generic;
using System.Text;

namespace Amo.Lib.Enums
{
    /// <summary>
    /// 实例作用域
    /// </summary>
    public enum ScopeType
    {
        /// <summary>
        /// 全局的单实例
        /// </summary>
        Root = 1,

        /// <summary>
        /// Site作用域的实例,每个Site一个作用域,
        /// </summary>
        Site = 2,
    }
}
