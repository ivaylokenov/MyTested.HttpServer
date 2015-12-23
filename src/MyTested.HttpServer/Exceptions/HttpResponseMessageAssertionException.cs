namespace MyTested.HttpServer.Exceptions
{
    using System;

    /// <summary>
    /// Exception for invalid HTTP response message result.
    /// </summary>
    public class HttpResponseMessageAssertionException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the HttpResponseMessageAssertionException class.
        /// </summary>
        /// <param name="message">Message for System.Exception class.</param>
        public HttpResponseMessageAssertionException(string message)
            : base(message)
        {
        }
    }
}
