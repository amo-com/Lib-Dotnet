using Amo.Lib.Enums;
using System;

namespace Amo.Lib.Exceptions
{
    public class CustomizeException : Exception
    {
        private readonly EventType eventType;
        public CustomizeException(string message)
            : base(message)
        {
        }

        public CustomizeException(EventType eventTypecode, string message)
            : base(message)
        {
            this.eventType = eventTypecode;
        }

        public EventType EventType => this.eventType;
    }
}
