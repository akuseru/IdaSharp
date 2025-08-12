using System;

namespace IdaPro;

/// <summary>
    /// Exception thrown when IDA operation fails
    /// </summary>
    public class IdaException : Exception
    {
        public IdaException() : base() { }
        public IdaException(string message) : base(message) { }
        public IdaException(string message, Exception innerException) : base(message, innerException) { }
    }