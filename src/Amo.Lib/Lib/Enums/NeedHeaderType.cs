namespace Amo.Lib.Enums
{
    /// <summary>
    /// 标记Swagger自定义Parameter
    /// </summary>
    public enum NeedHeaderType
    {
        /// <summary>
        /// 全部的 = <see cref="Necessary"/> + <see cref="Optional"/>
        /// </summary>
        All,

        /// <summary>
        /// 必须的
        /// </summary>
        Necessary,

        /// <summary>
        /// 可选的
        /// </summary>
        Optional,

        /// <summary>
        /// 都没有
        /// </summary>
        None,
    }
}
