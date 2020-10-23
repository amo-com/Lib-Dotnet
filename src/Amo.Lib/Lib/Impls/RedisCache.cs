using Amo.Lib.Enums;
using Amo.Lib.Model;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Amo.Lib.Impls
{
    public class RedisCache : ICache
    {
        protected static readonly JsonSerializerSettings _jsonConfig = new JsonSerializerSettings() { ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore, NullValueHandling = NullValueHandling.Ignore };
        protected static IDatabase database;
        protected static ConnectionMultiplexer connectionMultiplexer;
        protected static string address;
        protected static int dbId;
        protected readonly ILog _log;
        protected readonly string _scoped;

        private static object locker = new object();
        public RedisCache(IScoped scopedFac, ILog log)
        {
            this._scoped = scopedFac.GetScoped();
            this._log = log;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="address">Redis Server Address</param>
        /// <param name="dbID">Redis DB</param>
        /// <returns>初始化结果</returns>
        public bool Init(string address, int dbID)
        {
            try
            {
                ConnectRedisServer(address, dbID);
            }
            catch (Exception ex)
            {
                _log?.Error(new LogEntity<string>() { Site = _scoped, EventType = (int)EventType.RedisUnavailable, Exception = ex });
                return false;
            }

            return true;
        }

        public virtual bool RetryConnect()
        {
            return false;
        }

        public T Get<T>(string key)
        {
            var cacheValue = database.StringGet(key);
            var value = default(T);
            if (!cacheValue.IsNullOrEmpty)
            {
                value = Deserialize<T>(cacheValue);
            }

            return value;
        }

        public async Task<T> GetAsync<T>(string key)
        {
            var cacheValue = await database.StringGetAsync(key);
            var value = default(T);
            if (!cacheValue.IsNullOrEmpty)
            {
                value = Deserialize<T>(cacheValue);
            }

            return value;
        }

        public bool Remove(string key)
        {
            return database.KeyDelete(key);
        }

        public async Task<bool> RemoveAsync(string key)
        {
            return await database.KeyDeleteAsync(key);
        }

        public void Clear()
        {
            database.Execute("FLUSHALL");
        }

        public bool Insert(string key, object data)
        {
            var jsonData = JsonConvert.SerializeObject(data);
            return database.StringSet(key, jsonData);
        }

        public async Task<bool> InsertAsync(string key, object data)
        {
            var jsonData = JsonConvert.SerializeObject(data);
            return await database.StringSetAsync(key, jsonData);
        }

        public bool Insert(string key, object data, int days, int hours, int minutes, int seconds, int milliseconds)
        {
            var jsonData = JsonConvert.SerializeObject(data);
            var timeSpan = new TimeSpan(days, hours, minutes, seconds, milliseconds);
            return database.StringSet(key, jsonData, timeSpan);
        }

        public async Task<bool> InsertAsync(string key, object data, int days, int hours, int minutes, int seconds, int milliseconds)
        {
            var jsonData = JsonConvert.SerializeObject(data);
            var timeSpan = new TimeSpan(days, hours, minutes, seconds, milliseconds);
            return await database.StringSetAsync(key, jsonData, timeSpan);
        }

        public bool Exists(string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                return database.KeyExists(key);
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> ExistsAsync(string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                return await database.KeyExistsAsync(key);
            }
            else
            {
                return false;
            }
        }

        public void ListPush<T>(string key, T data, int mode = 1)
        {
            var jsonData = JsonConvert.SerializeObject(data);
            if (mode == 1)
            {
                database.ListLeftPush(key, jsonData);
            }
            else
            {
                database.ListRightPush(key, jsonData);
            }
        }

        public async Task<long> ListPushAsync<T>(string key, T data, int mode = 1)
        {
            var jsonData = JsonConvert.SerializeObject(data);
            long len;
            if (mode == 1)
            {
                len = await database.ListLeftPushAsync(key, jsonData);
            }
            else
            {
                len = await database.ListRightPushAsync(key, jsonData);
            }

            return len;
        }

        public T ListPop<T>(string key, int mode = 1)
        {
            RedisValue redisValue = RedisValue.Null;
            if (mode == 1)
            {
                redisValue = database.ListRightPop(key);
            }
            else
            {
                redisValue = database.ListLeftPop(key);
            }

            var value = default(T);
            if (!redisValue.IsNullOrEmpty)
            {
                value = Deserialize<T>(redisValue);
            }

            return value;
        }

        public async Task<T> ListPopAsync<T>(string key, int mode = 1)
        {
            RedisValue redisValue = RedisValue.Null;
            if (mode == 1)
            {
                redisValue = await database.ListRightPopAsync(key);
            }
            else
            {
                redisValue = await database.ListLeftPopAsync(key);
            }

            var value = default(T);
            if (!redisValue.IsNullOrEmpty)
            {
                value = Deserialize<T>(redisValue);
            }

            return value;
        }

        public long ListLength(string key)
        {
            return database.ListLength(key);
        }

        public async Task<long> ListLengthAsync(string key)
        {
            return await database.ListLengthAsync(key);
        }

        public List<string> GetAllKeys(string keyPattern)
        {
            string[] keys = (string[])database.ScriptEvaluate(LuaScript.Prepare("return redis.call('KEYS',@keypattern)"), new { keypattern = keyPattern });
            return keys.ToList();
        }

        public async Task<List<string>> GetAllKeysAsync(string keyPattern)
        {
            string[] keys = (string[])(await database.ScriptEvaluateAsync(LuaScript.Prepare("return redis.call('KEYS',@keypattern)"), new { keypattern = keyPattern }));
            return keys.ToList();
        }

        public void BatchSet(Dictionary<string, string> keyValues, int days, int hours, int minutes, int seconds, int milliseconds)
        {
            if (keyValues != null && keyValues.Count > 0)
            {
                var timeSpan = new TimeSpan(days, hours, minutes, seconds, milliseconds);
                var batch = database.CreateBatch();
                foreach (var item in keyValues)
                {
                    batch.StringSetAsync(item.Key, JsonConvert.SerializeObject(item.Value), timeSpan);
                }

                batch.Execute();
            }
        }

        public async Task<Dictionary<string, string>> BatchGetAsync(List<string> keys)
        {
            Dictionary<string, string> keyValues = new Dictionary<string, string>();
            if (keys != null && keys.Count > 0)
            {
                keys = keys.Distinct().ToList();
                var tasks = new List<Task<RedisValue>>();
                var batch = database.CreateBatch();
                foreach (var key in keys)
                {
                    tasks.Add(batch.StringGetAsync(key));
                }

                batch.Execute();

                int len = keys.Count > tasks.Count ? tasks.Count : keys.Count;
                for (int i = 0; i < len; i++)
                {
                    var value = await tasks[i];
                    keyValues.Add(keys[i], value.IsNullOrEmpty ? null : JsonConvert.DeserializeObject<string>(value, _jsonConfig));
                }
            }

            return keyValues;
        }

        public T GetSet<T>(string key)
        {
            var cacheValue = database.SetMembers(key).FirstOrDefault();
            var value = default(T);
            if (!cacheValue.IsNullOrEmpty)
            {
                value = Deserialize<T>(cacheValue);
            }

            return value;
        }

        public async Task<T> GetSetAsync<T>(string key)
        {
            var cacheValue = (await database.SetMembersAsync(key)).FirstOrDefault();
            var value = default(T);
            if (!cacheValue.IsNullOrEmpty)
            {
                value = Deserialize<T>(cacheValue);
            }

            return value;
        }

        public bool ClusterCurrentState(int nodeCount)
        {
            var multiplexer = database.Multiplexer;
            var endpoints = multiplexer.GetEndPoints(true);
            if (endpoints == null || endpoints.Count() == 0)
            {
                return false;
            }

            List<IServer> servers = endpoints.Select(q => multiplexer.GetServer(q)).ToList();
            return servers.Where(q => q.IsConnected == true && q.IsSlave == false).Count() >= nodeCount;
        }

        /// <summary>
        /// 创建Connect
        /// </summary>
        /// <param name="address">Redis Server Address</param>
        /// <param name="dbId">Redis DB</param>
        protected void ConnectRedisServer(string address, int dbId)
        {
            lock (locker)
            {
                if (database == null || !connectionMultiplexer.IsConnected || address != RedisCache.address || dbId != RedisCache.dbId)
                {
                    if (string.IsNullOrEmpty(address))
                    {
                        return;
                    }

                    try
                    {
                        if (address.Contains(";"))
                        {
                            if (address.Contains(","))
                            {
                                // 支持多终端节点连接,配置格式为dbAddress = server01;server02,password=******
                                string strPwd = address.Split(',')[1];
                                string[] strEndPoints = address.Split(',')[0].Split(';');
                                ConfigurationOptions options = new ConfigurationOptions();
                                foreach (string endPoint in strEndPoints)
                                {
                                    options.EndPoints.Add(endPoint);
                                }

                                options.Password = strPwd.Split('=')[1];
                                options.AllowAdmin = true;
                                connectionMultiplexer = ConnectionMultiplexer.Connect(options);
                            }
                            else
                            {
                                string[] strEndPoints = address.Split(';');
                                ConfigurationOptions options = new ConfigurationOptions();
                                foreach (string endPoint in strEndPoints)
                                {
                                    options.EndPoints.Add(endPoint);
                                }

                                options.AllowAdmin = true;
                                connectionMultiplexer = ConnectionMultiplexer.Connect(options);
                            }
                        }
                        else
                        {
                            connectionMultiplexer = ConnectionMultiplexer.Connect(address);
                        }

                        // System.Threading.Thread.Sleep(20);
                        database = connectionMultiplexer.GetDatabase(dbId);
                        RedisCache.address = address;
                        RedisCache.dbId = dbId;
                        _log?.Info(new LogEntity<string>() { Site = _scoped, EventType = (int)EventType.RedisConnect });
                    }
                    catch (Exception ex)
                    {
                        _log?.Error(new LogEntity<string>() { Site = _scoped, EventType = (int)EventType.RedisUnavailable, Exception = ex });
                    }
                }
            }

            return;
        }

        private T Deserialize<T>(RedisValue cacheValue)
        {
            var value = default(T);
            if (!cacheValue.IsNullOrEmpty)
            {
                value = JsonConvert.DeserializeObject<T>(cacheValue, _jsonConfig);
            }

            return value;
        }
    }
}
