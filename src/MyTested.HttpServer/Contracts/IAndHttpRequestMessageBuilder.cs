namespace MyTested.HttpServer.Contracts
{
    /// <summary>
    /// Used for adding AndAlso() method to the the HTTP request message builder.
    /// </summary>
    public interface IAndHttpRequestMessageBuilder : IHttpRequestMessageBuilder
    {
        /// <summary>
        /// AndAlso method for better readability when building HTTP request message.
        /// </summary>
        /// <returns>The same HTTP request message builder.</returns>
        IHttpRequestMessageBuilder AndAlso();
    }
}
