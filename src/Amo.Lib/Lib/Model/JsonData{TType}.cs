namespace Amo.Lib.Model
{
    /// <summary>
    /// Api数据结构统一封装
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

        public static JsonData<TType> Create()
        {
            return JsonData.Create<JsonData<TType>, TType>();
        }

        public static JsonData<TType> Success()
        {
            return JsonData.Success<JsonData<TType>, TType>();
        }

        public static JsonData<TType> Success(TType data)
        {
            return JsonData.Success<JsonData<TType>, TType>(data);
        }

        public static JsonData<TType> Success(TType data, int code)
        {
            return JsonData.Success<JsonData<TType>, TType>(data, code);
        }

        public static JsonData<TType> Success(TType data, int code, string message)
        {
            return JsonData.Success<JsonData<TType>, TType>(data, code, message);
        }

        public static JsonData<TType> Faild()
        {
            return JsonData.Faild<JsonData<TType>, TType>();
        }

        public static JsonData<TType> Faild(int code)
        {
            return JsonData.Faild<JsonData<TType>, TType>(code);
        }

        public static JsonData<TType> Faild(string message)
        {
            return JsonData.Faild<JsonData<TType>, TType>(message);
        }

        public static JsonData<TType> Faild(int code, string message)
        {
            return JsonData.Faild<JsonData<TType>, TType>(code, message);
        }
    }
}
