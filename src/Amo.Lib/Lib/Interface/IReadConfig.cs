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
        /// <param name="type">类型,ReadConfig需要读取多分文件时,如SiteData,需要按Site区分,每个Site缓存一个</param>
        /// <returns>获取的值</returns>
        T GetValue<T>(string path, string type = null);

        /// <summary>
        /// 获取path路径对应的值
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="type">类型,ReadConfig需要读取多分文件时,如SiteData,需要按Site区分,每个Site缓存一个</param>
        /// <returns>获取的值</returns>
        string GetValue(string path, string type = null);
    }
}
