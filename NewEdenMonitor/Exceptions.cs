using System;

namespace NewEdenMonitor
{
    [Serializable]
    public class InvalidResourceException : Exception
    {
        public string Resource { get; private set; }

        public InvalidResourceException()
            : base()
        {
        }

        public InvalidResourceException(string message)
            : base(message)
        {
        }

        public InvalidResourceException(string resource, string message)
            : base(message)
        {
            Resource = resource;
        }

        public InvalidResourceException(string message, Exception inner)
            : base(message, inner)
        {
        }

        public InvalidResourceException(string resource, string message, Exception inner)
            : base(message, inner)
        {
            Resource = resource;
        }
    }
}
