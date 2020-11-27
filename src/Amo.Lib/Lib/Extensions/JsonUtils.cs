using System;
using System.Collections.Generic;
using System.Text;

namespace Amo.Lib.Extensions
{
    public class JsonUtils
    {
        /// <summary>
        /// Newtonsoft实现的反序列化封装
        /// </summary>
        /// <typeparam name="T">实体结构</typeparam>
        /// <param name="json">需要解析的数据</param>
        /// <returns>解析后的实体数据</returns>
        public static T Deserialize<T>(string json)
        {
            try
            {
                Newtonsoft.Json.JsonSerializerSettings jsetting = new Newtonsoft.Json.JsonSerializerSettings();
                jsetting.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json, jsetting);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static object Deserialize(string json, Type type)
        {
            try
            {
                Newtonsoft.Json.JsonSerializerSettings jsetting = new Newtonsoft.Json.JsonSerializerSettings();
                jsetting.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                return Newtonsoft.Json.JsonConvert.DeserializeObject(json, type, jsetting);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Newtonsoft实现的序列化封装
        /// </summary>
        /// <typeparam name="T">实体结构</typeparam>
        /// <param name="obj">需要序列化的实体</param>
        /// <returns>处理后的数据</returns>
        public static string Serialize<T>(T obj)
        {
            try
            {
                Newtonsoft.Json.JsonSerializerSettings jsetting = new Newtonsoft.Json.JsonSerializerSettings();
                jsetting.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                return Newtonsoft.Json.JsonConvert.SerializeObject(obj, jsetting);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string Serialize(object obj, Type type)
        {
            try
            {
                Newtonsoft.Json.JsonSerializerSettings jsetting = new Newtonsoft.Json.JsonSerializerSettings();
                jsetting.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                return Newtonsoft.Json.JsonConvert.SerializeObject(obj, type, jsetting);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
