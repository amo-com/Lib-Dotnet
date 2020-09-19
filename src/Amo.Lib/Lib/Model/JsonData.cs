using Amo.Lib.Enums;

namespace Amo.Lib.Model
{
    public class JsonData
    {
        public static T Create<T, T1>()
            where T : JsonData<T1>, new()
        {
            return new T();
        }

        /// <summary>
        /// 无返回结果的成功请求
        /// </summary>
        /// <typeparam name="T">返回体JsonData泛型</typeparam>
        /// <typeparam name="T1">JsonData的子类型</typeparam>
        /// <returns>返回体</returns>
        public static T Success<T, T1>()
            where T : JsonData<T1>, new()
        {
            T self = new T();
            self.Code = (int)EventType.Success;
            return self;
        }

        /// <summary>
        /// 返回指定类型的成功请求
        /// </summary>
        /// <typeparam name="T">返回体JsonData泛型</typeparam>
        /// <typeparam name="T1">JsonData的子类型</typeparam>
        /// <param name="data">数据内容</param>
        /// <returns>返回体</returns>
        public static T Success<T, T1>(T1 data)
            where T : JsonData<T1>, new()
        {
            T self = new T();
            self.Code = (int)EventType.Success;
            self.Data = data;
            return self;
        }

        /// <summary>
        /// 返回指定类型和自定义Code的成功请求
        /// </summary>
        /// <typeparam name="T">返回体JsonData泛型</typeparam>
        /// <typeparam name="T1">JsonData的子类型</typeparam>
        /// <param name="data">数据内容</param>
        /// <param name="code">自定义code</param>
        /// <returns>返回体</returns>
        public static T Success<T, T1>(T1 data, int code)
            where T : JsonData<T1>, new()
        {
            T self = new T();
            self.Code = code;
            self.Data = data;
            return self;
        }

        /// <summary>
        /// 返回指定类型,自定义Code,和标记message的成功请求
        /// </summary>
        /// <typeparam name="T">返回体JsonData泛型</typeparam>
        /// <typeparam name="T1">JsonData的子类型</typeparam>
        /// <param name="data">数据内容</param>
        /// <param name="code">自定义code</param>
        /// <param name="message">Message</param>
        /// <returns>返回体</returns>
        public static T Success<T, T1>(T1 data, int code, string message)
            where T : JsonData<T1>, new()
        {
            T self = new T();
            self.Code = code;
            self.Data = data;
            self.Message = message;
            return self;
        }

        /// <summary>
        /// 返回失败请求
        /// </summary>
        /// <typeparam name="T">返回体JsonData泛型</typeparam>
        /// <typeparam name="T1">JsonData的子类型</typeparam>
        /// <returns>返回体</returns>
        public static T Faild<T, T1>()
            where T : JsonData<T1>, new()
        {
            T self = new T();
            self.Code = (int)EventType.ApiError;
            return self;
        }

        /// <summary>
        /// 返回自定义code的失败请求
        /// </summary>
        /// <typeparam name="T">返回体JsonData泛型</typeparam>
        /// <typeparam name="T1">JsonData的子类型</typeparam>
        /// <param name="code">自定义code</param>
        /// <returns>返回体</returns>
        public static T Faild<T, T1>(int code)
            where T : JsonData<T1>, new()
        {
            T self = new T();
            self.Code = code;
            return self;
        }

        /// <summary>
        /// 返回带message的失败请求
        /// </summary>
        /// <typeparam name="T">返回体JsonData泛型</typeparam>
        /// <typeparam name="T1">JsonData的子类型</typeparam>
        /// <param name="message">message说明</param>
        /// <returns>返回体</returns>
        public static T Faild<T, T1>(string message)
            where T : JsonData<T1>, new()
        {
            T self = new T();
            self.Code = (int)EventType.ApiError;
            self.Message = message;
            return self;
        }

        /// <summary>
        /// 返回带自定义code和message的失败请求
        /// </summary>
        /// <typeparam name="T">返回体JsonData泛型</typeparam>
        /// <typeparam name="T1">JsonData的子类型</typeparam>
        /// <param name="code">StatusCode</param>
        /// <param name="message">Message</param>
        /// <returns>返回体</returns>
        public static T Faild<T, T1>(int code, string message)
            where T : JsonData<T1>, new()
        {
            T self = new T();
            self.Code = code;
            self.Message = message;
            return self;
        }
    }
}
