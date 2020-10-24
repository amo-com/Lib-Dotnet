using System;

namespace Amo.Lib.Model
{
    public class LogEntity<T> : LogEntity
    {
        public T Data { get; set; }
    }
}
