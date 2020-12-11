using System;
using System.Threading;
using System.Threading.Tasks;

namespace Amo.Lib.RestClient.Attributes
{
    /// <summary>
    /// 降级(FallBack)
    /// </summary>
    [Obsolete("特性不能使用Action/Func,这不是有效的额类型,本方法待完善")]
    public class PollyFallBackAttribute : PolicyAttribute
    {
        public PollyFallBackAttribute()
        {
        }

        public PollyFallBackAttribute(Action action = null, Func<CancellationToken, Task> func = null)
        {
            this.Action = action;
            this.Func = func;
        }

        /// <summary>
        /// fallbackAction
        /// </summary>
        public Action Action { get; set; }

        /// <summary>
        /// 异步Task的fallbackAction
        /// </summary>
        public Func<CancellationToken, Task> Func { get; set; }
        public override IPolicyConfig CreatePolicy()
        {
            return new PolicyConfig.FallBackPolicy()
            {
                Action = Action,
                Func = Func,
            };
        }
    }
}
