namespace MyTested.HttpServer.Contracts
{
    /// <summary>
    /// Provides options to test the HTTP response from a server.
    /// </summary>
    public interface IServerTestBuilder
    {
        /// <summary>
        /// Tests for a particular HTTP response message.
        /// </summary>
        /// <returns>HTTP response message test builder.</returns>
        IHttpHandlerResponseMessageTestBuilder ShouldReturnHttpResponseMessage();
    }
}
