using System;

namespace Quartz.AttributesExtension
{
    public sealed class InvalidQuartzConfigurationException : Exception
    {
        public InvalidQuartzConfigurationException(string message)
            : base(message)
        {
        }

        public InvalidQuartzConfigurationException() : base()
        {
        }

        public InvalidQuartzConfigurationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
