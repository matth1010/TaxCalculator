using System;

namespace TaxCalculator.API.Shared
{
    public class BadRequestException : Exception
    {
        public BadRequestException()
        {
        }

        public BadRequestException(string message) : base(message)
        {
        }

        public BadRequestException(string userMessage, string systemMessage) : base(userMessage)
        {
            SystemMessage = systemMessage;
        }

        public BadRequestException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public string SystemMessage { get; }
    }
}
