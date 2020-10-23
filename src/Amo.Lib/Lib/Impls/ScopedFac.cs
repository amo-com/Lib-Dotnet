namespace Amo.Lib.Impls
{
    /// <summary>
    /// IScoped的默认实现(作用域接口)
    /// </summary>
    public class ScopedFac : IScoped
    {
        private readonly string scoped;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScopedFac"/> class.
        /// 默认构造函数
        /// </summary>
        /// <param name="scoped">作用域</param>
        public ScopedFac(string scoped)
        {
            this.scoped = scoped;
        }

        /// <summary>
        /// 获取当前作用域的名字(Key)
        /// </summary>
        /// <returns>作用域</returns>
        public string GetScoped()
        {
            return scoped;
        }
    }
}
