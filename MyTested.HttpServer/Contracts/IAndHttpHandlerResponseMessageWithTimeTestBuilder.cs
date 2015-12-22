namespace MyTested.HttpServer.Contracts
{
    /// <summary>
    /// Used for adding AndAlso() method to the the HTTP response message with response time tests.
    /// </summary>
    public interface IAndHttpHandlerResponseMessageWithTimeTestBuilder
        : IHttpHandlerResponseMessageWithTimeTestBuilder
    {
        /// <summary>
        /// AndAlso method for better readability when chaining HTTP response message with response time tests.
        /// </summary>
        /// <returns>The same HTTP response message test builder.</returns>
        IHttpHandlerResponseMessageWithTimeTestBuilder AndAlso();
    }
}
