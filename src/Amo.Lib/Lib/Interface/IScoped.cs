namespace Amo.Lib
{
    /// <summary>
    /// Scoped的接口,用于获取对应Scoped
    /// </summary>
    public interface IScoped
    {
        /// <summary>
        /// 获取当前作用域的名字(Key)
        /// </summary>
        /// <returns>作用域</returns>
        string GetScoped();
    }
}
