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

        /// <summary>
        /// 获取path路径对应的值
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns>获取的值</returns>
        string GetValue(string path);
    }
}
