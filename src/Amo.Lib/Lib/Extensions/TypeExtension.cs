using Amo.Lib.Enums;
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

        public static string ToJson<T>(this T self)
        {
            Newtonsoft.Json.JsonSerializerSettings jsetting = new Newtonsoft.Json.JsonSerializerSettings
            {
                NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore,
                ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver { IgnoreSerializableInterface = true },
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            };
            return Newtonsoft.Json.JsonConvert.SerializeObject(self, jsetting);
        }

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

        public static TSource FindSingle<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
        {
            if (source == null)
            {
                return default;
            }

            return source.FirstOrDefault(predicate);
        }

        public static List<TSource> FindAll<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
        {
            if (source == null)
            {
                return default;
            }

            return source.Where(predicate).ToList();
        }

        /// <summary>
        /// 计算错误类型
        /// <seealso cref="Enums.EventType"/>
        /// </summary>
        /// <param name="item">EventType</param>
        /// <returns>Level</returns>
        public static LogLevel GetLevel(this EventType item)
        {
            int num = (int)item;
            if (num < 100000)
            {
                return LogLevel.Info;
            }

            int key = (num / 1000) % 10;

            LogLevel level;
            switch (key)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                    level = (LogLevel)key;
                    break;
                default:
                    level = LogLevel.Info;
                    break;
            }

            return level;
        }
    }
}
