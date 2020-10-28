using System.Collections.Generic;
using System.Threading.Tasks;

namespace Amo.Lib
{
    /// <summary>
    /// 缓存
    /// </summary>
    [Attributes.Autowired(Enums.ScopeType.Root)]
    public interface ICache
    {
        /// <summary>
        /// 获取指定键的缓存值
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <returns>缓存值</returns>
        T Get<T>(string key);

        /// <summary>
        /// 获取指定键的缓存值
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <returns>缓存值</returns>
        Task<T> GetAsync<T>(string key);

        /// <summary>
        /// 从缓存中移除指定的缓存值
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>移除成功or失败</returns>
        bool Remove(string key);

        /// <summary>
        /// 从缓存中移除指定的缓存值
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>移除成功or失败</returns>
        Task<bool> RemoveAsync(string key);

        /// <summary>
        /// 删除所有缓存
        /// </summary>
        void Clear();

        /// <summary>
        /// 执行脚本命令
        /// </summary>
        /// <param name="command">脚本</param>
        /// <returns>Task</returns>
        Task ExecuteAsync(string command);

        /// <summary>
        /// 将指定键的对象添加到缓存中
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="data">缓存值</param>
        /// <returns>添加成功or失败</returns>
        bool Insert(string key, object data);

        /// <summary>
        /// 将指定键的对象添加到缓存中
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="data">缓存值</param>
        /// <returns>添加成功or失败</returns>
        Task<bool> InsertAsync(string key, object data);

        /// <summary>
        /// 将指定键的对象添加到缓存中
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="data">缓存值</param>
        /// <param name="days">Number of days.</param>
        /// <param name="hours">Number of hours.</param>
        /// <param name="minutes">Number of minutes.</param>
        /// <param name="seconds">Number of seconds.</param>
        /// <param name="milliseconds">Number of milliseconds.</param>
        /// <returns>添加成功or失败</returns>
        bool Insert(string key, object data, int days = 0, int hours = 0, int minutes = 0, int seconds = 0, int milliseconds = 0);

        /// <summary>
        /// 将指定键的对象添加到缓存中
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="data">缓存值</param>
        /// <param name="days">Number of days.</param>
        /// <param name="hours">Number of hours.</param>
        /// <param name="minutes">Number of minutes.</param>
        /// <param name="seconds">Number of seconds.</param>
        /// <param name="milliseconds">Number of milliseconds.</param>
        /// <returns>添加成功or失败</returns>
        Task<bool> InsertAsync(string key, object data, int days = 0, int hours = 0, int minutes = 0, int seconds = 0, int milliseconds = 0);

        /// <summary>
        /// 判断key是否存在
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>TRUE存在;FALSE不存在</returns>
        bool Exists(string key);

        /// <summary>
        /// 判断key是否存在
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>TRUE存在;FALSE不存在</returns>
        Task<bool> ExistsAsync(string key);

        /// <summary>
        /// 向List中添加数据
        /// </summary>
        /// <typeparam name="T">缓存数据类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="data">缓存值</param>
        /// <param name="mode">1:LEFT;2:RIGHT</param>
        void ListPush<T>(string key, T data, int mode = 1);

        /// <summary>
        /// 向List中添加数据
        /// </summary>
        /// <typeparam name="T">缓存数据类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="data">缓存值</param>
        /// <param name="mode">1:LEFT;2:RIGHT</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<long> ListPushAsync<T>(string key, T data, int mode = 1);

        /// <summary>
        /// 从List中取出数据并从List中移除
        /// </summary>
        /// <typeparam name="T">缓存数据类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="mode">1:RIGHT;2:LEFT</param>
        /// <returns>缓存值</returns>
        T ListPop<T>(string key, int mode = 1);

        /// <summary>
        /// 从List中取出数据并从List中移除
        /// </summary>
        /// <typeparam name="T">缓存数据类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="mode">1:RIGHT;2:LEFT</param>
        /// <returns>缓存值</returns>
        Task<T> ListPopAsync<T>(string key, int mode = 1);

        /// <summary>
        /// 获取List的长度
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>List的长度</returns>
        long ListLength(string key);

        /// <summary>
        /// 获取List的长度
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>List的长度</returns>
        Task<long> ListLengthAsync(string key);

        /// <summary>
        /// 模糊匹配获取对应key值
        /// </summary>
        /// <param name="keyPattern">模糊匹配条件</param>
        /// <returns>key集合</returns>
        List<string> GetAllKeys(string keyPattern);

        /// <summary>
        /// 模糊匹配获取对应key值
        /// </summary>
        /// <param name="keyPattern">模糊匹配条件</param>
        /// <returns>key集合</returns>
        Task<List<string>> GetAllKeysAsync(string keyPattern);

        /// <summary>
        /// 批量新增集合数据
        /// </summary>
        /// <param name="keyValues">数据集合</param>
        /// <param name="days">days</param>
        /// <param name="hours">hours</param>
        /// <param name="minutes">minutes</param>
        /// <param name="seconds">seconds</param>
        /// <param name="milliseconds">milliseconds</param>
        void BatchSet(Dictionary<string, string> keyValues, int days = 0, int hours = 0, int minutes = 0, int seconds = 0, int milliseconds = 0);

        /// <summary>
        /// 批量获取集合数据(key/value形式)
        /// </summary>
        /// <param name="keys">keys集合</param>
        /// <returns>数据集合</returns>
        Task<Dictionary<string, string>> BatchGetAsync(List<string> keys);

        /// <summary>
        /// 读取set类型的数据
        /// </summary>
        /// <typeparam name="T">要获取数据类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <returns>缓存值</returns>
        T GetSet<T>(string key);

        /// <summary>
        /// 读取set类型的数据
        /// </summary>
        /// <typeparam name="T">要获取数据类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <returns>缓存值</returns>
        Task<T> GetSetAsync<T>(string key);

        /// <summary>
        /// 根据实际配置主节点数判断当前集群是否可用
        /// </summary>
        /// <param name="nodeCount">主节点数</param>
        /// <returns>TRUE可用;FALSE不可用</returns>
        bool ClusterCurrentState(int nodeCount);

        /// <summary>
        /// 重置Redis链接
        /// </summary>
        /// <returns>重置成功or失败</returns>
        bool RetryConnect();
    }
}
