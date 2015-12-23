namespace MyTested.HttpServer.Contracts
{
    /// <summary>
    /// Used for adding AndAlso() method to the the HTTP response message tests.
    /// </summary>
    public interface IAndHttpHandlerResponseMessageTestBuilder : IHttpHandlerResponseMessageTestBuilder
    {
        /// <summary>
        /// AndAlso method for better readability when chaining HTTP response message tests.
        /// </summary>
        /// <returns>The same HTTP response message test builder.</returns>
        IHttpHandlerResponseMessageTestBuilder AndAlso();
    }
}
