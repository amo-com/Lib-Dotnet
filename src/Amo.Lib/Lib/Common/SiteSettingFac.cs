using Amo.Lib.Attributes;
using Amo.Lib.Extensions;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Amo.Lib
{
    public class SiteSettingFac<T>
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
        /// 获取实例
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

                T setting = Invoke(site, readConfig);
                _SettingCache.GetOrAdd(cacheKey, setting);
            }

            return _SettingCache[cacheKey];
        }

        public static void SetSetting(string site, T setting)
        {
            if (string.IsNullOrEmpty(site))
            {
                return;
            }

            string cacheKey = GetCacheKey(site);

            _SettingCache[cacheKey] = setting;
        }

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

        private static string GetCacheType()
        {
            return typeof(T).FullName;
        }

        private static string GetCacheKey(string site)
        {
            return $"{GetCacheType()}:{site}";
        }

        private static T Invoke(string site, IReadConfig readConfig)
        {
            T setting = new T();
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
                        var configType = regex.Replace(configAttr.Type ?? string.Empty, site); // 替换Site
                        string value;
                        if (!string.IsNullOrEmpty(configType))
                        {
                            value = readConfig.GetValue(configType, configPath);
                        }
                        else
                        {
                            value = readConfig.GetValue(configPath);
                        }

                        var propertyValue = Utils.ChangeType(value, property.PropertyType); // 类型转换
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

                // 获取单字典属性的值
                var dictionaryAttr = property.GetAttributes<DictionaryAttribute>(false);
                if (dictionaryAttr != null && dictionaryAttr.Count() > 0)
                {
                    var value = dictionaryAttr.Where(q => q.Site == site).FirstOrDefault()?.Value;
                    var propertyValue = Utils.ChangeType(value, property.PropertyType);
                    property.SetValue(setting, propertyValue);

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
