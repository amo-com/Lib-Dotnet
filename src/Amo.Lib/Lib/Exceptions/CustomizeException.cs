using Amo.Lib.Enums;
using System;

namespace Amo.Lib.Exceptions
{
    public class CustomizeException : Exception
    {
        private readonly int eventType;
        public CustomizeException(string message)
            : base(message)
        {
        }

        public CustomizeException(int eventTypecode, string message)
            : base(message)
        {
            this.eventType = eventTypecode;
        }

        public int EventType => this.eventType;
    }
}
