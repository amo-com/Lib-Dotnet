using Amo.Lib.Enums;

namespace Amo.Lib.Model
{
    /// <summary>
    /// Api统一封装
    /// </summary>
    /// <typeparam name="TType">数据实体类型</typeparam>
    public class JsonData<TType>
    {
        /// <summary>
        /// 自定义返回的StatusCode
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 返回的Json数据实体
        /// </summary>
        public TType Data { get; set; }

        /// <summary>
        /// 提示信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 无返回结果的成功请求
        /// </summary>
        /// <returns>返回体</returns>
        public static JsonData<TType> Success()
        {
            JsonData<TType> returnData = new JsonData<TType>()
            {
                Code = (int)EventType.Success,
            };
            return returnData;
        }

        /// <summary>
        /// 返回指定类型的成功请求
        /// </summary>
        /// <param name="data">数据内容</param>
        /// <returns>返回体</returns>
        public static JsonData<TType> Success(TType data)
        {
            JsonData<TType> returnData = new JsonData<TType>()
            {
                Data = data,
                Code = (int)EventType.Success,
            };
            return returnData;
        }

        /// <summary>
        /// 返回指定类型和自定义Code的成功请求
        /// </summary>
        /// <param name="data">数据内容</param>
        /// <param name="code">自定义code</param>
        /// <returns>返回体</returns>
        public static JsonData<TType> Success(TType data, int code)
        {
            JsonData<TType> returnData = new JsonData<TType>()
            {
                Data = data,
                Code = code,
            };
            return returnData;
        }

        /// <summary>
        /// 返回指定类型,自定义Code,和标记message的成功请求
        /// </summary>
        /// <param name="data">数据内容</param>
        /// <param name="code">自定义code</param>
        /// <param name="message">Message</param>
        /// <returns>返回体</returns>
        public static JsonData<TType> Success(TType data, int code, string message)
        {
            JsonData<TType> returnData = new JsonData<TType>()
            {
                Data = data,
                Code = code,
                Message = message,
            };
            return returnData;
        }

        /// <summary>
        /// 返回失败请求
        /// </summary>
        /// <returns>返回体</returns>
        public static JsonData<TType> Faild()
        {
            JsonData<TType> returnData = new JsonData<TType>()
            {
                Code = (int)EventType.ApiError,
            };
            return returnData;
        }

        /// <summary>
        /// 返回自定义code的失败请求
        /// </summary>
        /// <param name="code">自定义code</param>
        /// <returns>返回体</returns>
        public static JsonData<TType> Faild(int code)
        {
            JsonData<TType> returnData = new JsonData<TType>()
            {
                Code = code,
            };
            return returnData;
        }

        /// <summary>
        /// 返回带message的失败请求
        /// </summary>
        /// <param name="message">message说明</param>
        /// <returns>返回体</returns>
        public static JsonData<TType> Faild(string message)
        {
            JsonData<TType> returnData = new JsonData<TType>()
            {
                Code = (int)EventType.ApiError,
                Message = message,
            };
            return returnData;
        }

        /// <summary>
        /// 返回带自定义code和message的失败请求
        /// </summary>
        /// <param name="code">StatusCode</param>
        /// <param name="message">Message</param>
        /// <returns>返回体</returns>
        public static JsonData<TType> Faild(int code, string message)
        {
            JsonData<TType> returnData = new JsonData<TType>()
            {
                Code = code,
                Message = message,
            };
            return returnData;
        }
    }
}
