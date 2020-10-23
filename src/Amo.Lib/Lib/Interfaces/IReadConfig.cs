using System;

namespace Amo.Lib
{
    /// <summary>
    /// Config文件读取方法(也可辅助于SiteSetting)
    /// 两种格式: 1:config,2:ConcurrentDictionary&lt;type, config&gt;
    /// </summary>
    public interface IReadConfig
    {
        /// <summary>
        /// 获取path路径对应的值
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="path">路径</param>
        /// <returns>获取的值</returns>
        T GetValue<T>(string path);

        /*
        /// <summary>
        /// 获取path路径对应的值
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="path">路径</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>获取的值</returns>
        T GetValue<T>(string path, T defaultValue);
        */

        /// <summary>
        /// 获取path路径对应的值
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns>获取的值</returns>
        string GetValue(string path);

        /// <summary>
        /// 获取path路径对应的值
        /// 默认的Read方法
        /// </summary>
        /// <param name="path">The key of the configuration section's value to convert.</param>
        /// <param name="type">The type to convert the value to.</param>
        /// <returns>获取的值</returns>
        object GetValue(string path, Type type);

        /*
        /// <summary>
        /// 获取path路径对应的值
        /// </summary>
        /// <param name="path">The key of the configuration section's value to convert.</param>
        /// <param name="type">The type to convert the value to.</param>
        /// <param name="defaultValue">The default value to use if no value is found.</param>
        /// <returns>获取的值</returns>
        object GetValue(string path, Type type, object defaultValue);
        */

        /// <summary>
        /// 实体信息绑定
        /// 默认的Bind方法
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="instance">实体引用</param>
        void Bind(string path, object instance);
    }
}
