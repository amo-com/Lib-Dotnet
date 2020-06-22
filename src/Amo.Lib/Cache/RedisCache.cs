using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Amo.Lib.Cache
{
    public class RedisCache : ICache
    {
        private readonly JsonSerializerSettings jsonConfig = new JsonSerializerSettings() { ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore, NullValueHandling = NullValueHandling.Ignore };
        private IDatabase database;
        private ConnectionMultiplexer connectionMultiplexer;

        public RedisCache()
        {
        }

        public RedisCache(string address, int dbID)
        {
            ConnectRedisServer(address, dbID);
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

        public async Task ListPushAsync<T>(string key, T data, int mode = 1)
        {
            var jsonData = JsonConvert.SerializeObject(data);
            if (mode == 1)
            {
                await database.ListLeftPushAsync(key, jsonData);
            }
            else
            {
                await database.ListRightPushAsync(key, jsonData);
            }
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
            if (redisValue.IsNullOrEmpty)
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
            if (redisValue.IsNullOrEmpty)
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
                    keyValues.Add(keys[i], value.IsNullOrEmpty ? null : JsonConvert.DeserializeObject<string>(value, jsonConfig));
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

        protected void ConnectRedisServer(string address, int dbID)
        {
            if (string.IsNullOrEmpty(address))
            {
                return;
            }

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

            database = connectionMultiplexer.GetDatabase(dbID);
        }

        private T Deserialize<T>(RedisValue cacheValue)
        {
            var value = default(T);
            if (!cacheValue.IsNullOrEmpty)
            {
                value = JsonConvert.DeserializeObject<T>(cacheValue, jsonConfig);
            }

            return value;
        }
    }
}
