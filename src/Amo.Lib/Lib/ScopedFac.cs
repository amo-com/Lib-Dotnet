namespace Amo.Lib
{
    /// <summary>
    /// ISite的默认实现(作用域接口)
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

        public string GetScoped()
        {
            return scoped;
        }
    }
}
