using System;

namespace Amo.Lib.Model
{
    public class LogEntity<T>
    {
        public string Site { get; set; }
        public int EventType { get; set; }
        public T Data { get; set; }
        public Exception Exception { get; set; }
    }
}
