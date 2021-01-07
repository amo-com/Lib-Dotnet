using System;

namespace Amo.Lib.Exceptions
{
    public class CustomizeException : Exception
    {
        public CustomizeException()
            : base()
        {
        }

        public CustomizeException(string message, int eventType = 0, int statusCode = 0)
            : base(message)
        {
            this.EventType = eventType;
            this.StatusCode = statusCode;
        }

        public CustomizeException(string message, Exception innerException, int eventType = 0, int statusCode = 0)
            : base(message, innerException)
        {
            this.EventType = eventType;
            this.StatusCode = statusCode;
        }

        public int EventType { get; }

        public int StatusCode { get; }
    }
}
