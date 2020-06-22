namespace Amo.Lib
{
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
        /// type为配置类型,示例:多个文件时,type对应文件,path对应数据路径
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="type">类型,文件</param>
        /// <param name="path">路径</param>
        /// <returns>获取的值</returns>
        T GetValue<T>(string type, string path);

        /// <summary>
        /// 获取path路径对应的值
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns>获取的值</returns>
        string GetValue(string path);

        /// <summary>
        /// 获取path路径对应的值
        /// type为配置类型,示例:多个文件时,type对应文件,path对应数据路径
        /// </summary>
        /// <param name="type">类型,文件</param>
        /// <param name="path">路径</param>
        /// <returns>获取的值</returns>
        string GetValue(string type, string path);
    }
}
