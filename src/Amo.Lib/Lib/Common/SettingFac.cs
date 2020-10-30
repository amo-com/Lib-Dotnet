using Amo.Lib.Attributes;
using Amo.Lib.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Amo.Lib
{
    public class SettingFac<T>
        where T : class, new()
    {
        /// <summary>
        /// Setting字典
        /// </summary>
        protected static readonly ConcurrentDictionary<string, T> _SettingCache = new ConcurrentDictionary<string, T>();

        /// <summary>
        /// 继承类的类型对应的SettingBase,Invoke时需要用的基础配置,每种类型的都要存储,替换原先使用的类属性,如ReadConfig
        /// </summary>
        protected static readonly ConcurrentDictionary<string, IReadConfig> _ReadConfigCache = new ConcurrentDictionary<string, IReadConfig>();

        /// <summary>
        /// 获取实例(如果不存在,则调用T默认的Invoke初始化)
        /// <typeparamref name="T"/>
        /// 需要先对_ReadConfig赋值
        /// </summary>
        /// <param name="site">站点Site</param>
        /// <returns>获取setting实例</returns>
        public static T GetSetting(string site)
        {
            if (string.IsNullOrEmpty(site))
            {
                site = SiteConst.NNN;
            }

            string cacheType = GetCacheType();
            string cacheKey = GetCacheKey(site);
            if (!_SettingCache.ContainsKey(cacheKey))
            {
                IReadConfig readConfig = null;
                if (_ReadConfigCache.ContainsKey(cacheType))
                {
                    readConfig = _ReadConfigCache[cacheType];
                }

                T setting = new T();
                Invoke(site, setting, readConfig);
                _SettingCache.GetOrAdd(cacheKey, setting);
            }

            return _SettingCache[cacheKey];
        }

        public static T GetOrUpdateSetting(string site)
        {
            if (string.IsNullOrEmpty(site))
            {
                site = SiteConst.NNN;
            }

            string cacheType = GetCacheType();
            string cacheKey = GetCacheKey(site);

            bool isExist = _SettingCache.TryGetValue(cacheKey, out T setting);
            if (!isExist)
            {
                setting = new T();
                _SettingCache.GetOrAdd(cacheKey, setting);
            }

            IReadConfig readConfig = null;
            if (_ReadConfigCache.ContainsKey(cacheType))
            {
                readConfig = _ReadConfigCache[cacheType];
            }

            Invoke(site, setting, readConfig);

            return _SettingCache[cacheKey];
        }

        /// <summary>
        /// 直接缓存处理好的Setting数据
        /// </summary>
        /// <param name="site">Key</param>
        /// <param name="setting">Value</param>
        public static void SetSetting(string site, T setting)
        {
            if (string.IsNullOrEmpty(site))
            {
                return;
            }

            string cacheKey = GetCacheKey(site);

            _SettingCache[cacheKey] = setting;
        }

        /// <summary>
        /// 获取Setting的所有Key值
        /// </summary>
        /// <returns>Key列表</returns>
        public static List<string> GetSettingKeys()
        {
            return _SettingCache.Keys.ToList();
        }

        /// <summary>
        /// 新增或更新(T)ReadConfig缓存
        /// </summary>
        /// <param name="readConfig">ReadConfig实例</param>
        /// <returns>结果</returns>
        public static bool UpdateOrAddReadConfig(IReadConfig readConfig)
        {
            string cacheType = GetCacheType();
            _ReadConfigCache[cacheType] = readConfig;
            return true;
        }

        public static IReadConfig GetReadConfig()
        {
            string cacheType = GetCacheType();
            if (_ReadConfigCache.ContainsKey(cacheType))
            {
                return _ReadConfigCache[cacheType];
            }

            return null;
        }

        public static List<string> GetReadConfigKeys()
        {
            return _ReadConfigCache.Keys.ToList();
        }

        private static string GetCacheType()
        {
            return typeof(T).FullName;
        }

        private static string GetCacheKey(string site)
        {
            return $"{GetCacheType()}:{site}";
        }

        private static T Invoke(string site, T setting, IReadConfig readConfig)
        {
            PropertyInfo[] properties = setting.GetType().GetProperties();
            var regex = new Regex(DataConst.SiteParam, RegexOptions.IgnoreCase);
            foreach (PropertyInfo property in properties)
            {
                // Sites标记开关列表,bool类型
                var sitesAttr = property.GetAttribute<SitesAttribute>(false);
                if (sitesAttr != null)
                {
                    if (property.PropertyType == typeof(bool))
                    {
                        var propertyValue = sitesAttr.Sites?.Contains(site);
                        property.SetValue(setting, propertyValue);
                    }

                    continue;
                }

                // 读取外部config配置中的值
                var configAttr = property.GetAttribute<ConfigAttribute>(false);
                if (configAttr != null)
                {
                    if (readConfig != null)
                    {
                        var configPath = regex.Replace(configAttr.Path ?? string.Empty, site); // 替换Site

                        /*
                         * Old Code
                          string value = readConfig.GetValue(configPath);
                          var propertyValue = Utils.ChangeType(value, property.PropertyType); // 类型转换
                        if (isClass)
                        {
                            // var typeVar = property.PropertyType.MakeGenericType();
                            propertyValue = Activator.CreateInstance(property.PropertyType);
                            readConfig.Bind(configPath, propertyValue);
                        }
                        else
                        {
                            propertyValue = readConfig.Get(configPath, property.PropertyType);
                        }
                            */

                        var propertyValue = readConfig.Get(configPath, property.PropertyType);
                        property.SetValue(setting, propertyValue);
                    }

                    continue;
                }

                var descAttr = property.GetAttribute<DescAttribute>(false);
                if (descAttr != null)
                {
                    if (property.PropertyType == typeof(string))
                    {
                        var propertyValue = regex.Replace(descAttr.Desc ?? string.Empty, site); // 替换Site
                        property.SetValue(setting, propertyValue);
                    }

                    continue;
                }

                // 都没有的执行默认的私有Set方法
                if (property.SetMethod != null)
                {
                    int parametersNumber = property.SetMethod.GetParameters().Count();
                    object[] args = new object[parametersNumber];
                    for (int i = 0; i < parametersNumber; i++)
                    {
                        args[i] = null;
                    }

                    property.SetMethod.Invoke(setting, args);
                }
            }

            return setting;
        }
    }
}
