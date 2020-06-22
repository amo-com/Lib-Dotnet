using Amo.Lib.Interface;
using Amo.Lib.Model;
using System;
using System.Threading.Tasks;

namespace Amo.Lib
{
    public class BaseProxy : IBaseProxy
    {
        private readonly ILog log;
        public BaseProxy(ILog log)
        {
            this.log = log;
        }

        public async Task<(T data, bool state)> HandleTask<T>(Task<T> task, params string[] args)
        {
            var data = default(T);
            bool state = true;
            try
            {
                data = await task;
            }
            catch (Exception ex)
            {
                state = false;
                log.Warn(new LogEntity()
                {
                    EventType = Enums.EventType.TaskError,
                    Exception = ex,
                    Body = args == null ? null : $"{string.Join("|||", args)}",
                });
            }

            return (data, state);
        }

        public async Task<bool> HandleTask(Task task, params string[] args)
        {
            bool state = true;
            try
            {
                await task;
            }
            catch (Exception ex)
            {
                state = false;
                log.Warn(new LogEntity()
                {
                    EventType = Enums.EventType.TaskError,
                    Exception = ex,
                    Body = args == null ? null : $"{string.Join("|||", args)}",
                });
            }

            return state;
        }
    }
}
