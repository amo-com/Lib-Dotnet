using System;

namespace Amo.Lib
{
    /// <summary>
    /// Config文件读取方法(也可辅助于SiteSetting)
    /// ConfigurationBinder
    /// </summary>
    public interface IReadConfig
    {
        /// <summary>
        /// 获取path路径对应的值
        /// CDATA[configuration.GetSection(path).Get&lt;T&gt;()
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="path">路径</param>
        /// <returns>获取的值</returns>
        T Get<T>(string path);

        /// <summary>
        /// Setting Invoke 使用的默认方法
        /// configuration.GetSection(path).Get(type)
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="type">类型</param>
        /// <returns>value</returns>
        object Get(string path, Type type);
    }
}
