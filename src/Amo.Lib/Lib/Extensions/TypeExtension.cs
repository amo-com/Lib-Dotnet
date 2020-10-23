using Amo.Lib.Enums;
using Amo.Lib.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Amo.Lib.Extensions
{
    public static class TypeExtension
    {
        /// <summary>
        /// 自动执行属性的私有Set,不需要参数的,属性的Set为方法,且不需要参数,
        /// 解决SiteSetting基于Site生成各种开关和配置的问题
        /// 如果存在继承问题,Set方法用protected标记
        /// </summary>
        /// <typeparam name="TType">实例类型</typeparam>
        /// <param name="type">type实例</param>
        public static void AutoInvokePropertySet<TType>(this TType type)
        {
            Type currentType = typeof(TType);

            // 只处理类型本身的,基类的由基类自己处理,属性有私有Set方法的,全部自动执行一遍
            List<PropertyInfo> properties = type.GetType().GetProperties()
                .Where(p => p.DeclaringType == currentType && p.SetMethod != null && !p.SetMethod.IsPublic).ToList();
            foreach (PropertyInfo property in properties)
            {
                int parametersNumber = property.SetMethod.GetParameters().Count();
                object[] args = new object[parametersNumber];
                for (int i = 0; i < parametersNumber; i++)
                {
                    args[i] = null;
                }

                property.SetMethod.Invoke(type, args);
            }
        }

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

        /// <summary>
        /// Newtonsoft实现的序列化封装
        /// </summary>
        /// <typeparam name="T">数据结构</typeparam>
        /// <param name="self">数据实体</param>
        /// <returns>序列化后的数据</returns>
        public static string ToJson<T>(this T self)
        {
            Newtonsoft.Json.JsonSerializerSettings jsetting = new Newtonsoft.Json.JsonSerializerSettings
            {
                NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore,
                ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver { IgnoreSerializableInterface = true },
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore,
                Error = (serializer, err) => err.ErrorContext.Handled = true
            };
            return Newtonsoft.Json.JsonConvert.SerializeObject(self, jsetting);
        }

        public static T GetData<T>(this JsonData<T> self)
        {
            if (self != null && self.Code == 200)
            {
                return self.Data;
            }

            return default;
        }

        /// <summary>
        /// 判断List不是空(not null, count > 0)
        /// </summary>
        /// <typeparam name="T">数据结构</typeparam>
        /// <param name="list">需要判断的列表</param>
        /// <returns>是否满足非空</returns>
        public static bool IsNotEmpty<T>(this List<T> list)
        {
            return !list.IsNullOrEmpty();
        }

        public static bool IsNullOrEmpty<T>(this List<T> list)
        {
            if (list == null || list.Count == 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 单项和List的包含判定
        /// </summary>
        /// <typeparam name="T">数据结构</typeparam>
        /// <param name="item">单项</param>
        /// <param name="list">List列表</param>
        /// <returns>是否被包含</returns>
        public static bool IsBelong<T>(this T item, params T[] list)
        {
            if (list.Contains(item))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 获取Enum的Description信息
        /// </summary>
        /// <param name="value">枚举值</param>
        /// <param name="isTop">??</param>
        /// <returns>Description信息</returns>
        public static string EnumDescription(this Enum value, bool isTop = false)
        {
            Type enumType = value.GetType();
            DescriptionAttribute attr = null;
            if (isTop)
            {
                attr = (DescriptionAttribute)Attribute.GetCustomAttribute(enumType, typeof(DescriptionAttribute));
            }
            else
            {
                // 获取枚举常数名称。
                string name = Enum.GetName(enumType, value);
                if (name != null)
                {
                    // 获取枚举字段。
                    FieldInfo fieldInfo = enumType.GetField(name);
                    if (fieldInfo != null)
                    {
                        // 获取描述的属性。
                        attr = Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute), false) as DescriptionAttribute;
                    }
                }
            }

            if (attr != null && !string.IsNullOrEmpty(attr.Description))
            {
                return attr.Description;
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// IQueryable扩展Find方法
        /// </summary>
        /// <typeparam name="TSource">源数据类型</typeparam>
        /// <param name="source">源数据</param>
        /// <param name="predicate">查询算式</param>
        /// <returns>匹配的结果</returns>
        public static TSource FindSingle<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
        {
            if (source == null)
            {
                return default;
            }

            return source.FirstOrDefault(predicate);
        }

        /// <summary>
        /// IQueryable扩展FindAll方法
        /// </summary>
        /// <typeparam name="TSource">源数据类型</typeparam>
        /// <param name="source">源数据</param>
        /// <param name="predicate">查询算式</param>
        /// <returns>匹配的结果集</returns>
        public static List<TSource> FindAll<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
        {
            if (source == null)
            {
                return default;
            }

            return source.Where(predicate).ToList();
        }
    }
}
