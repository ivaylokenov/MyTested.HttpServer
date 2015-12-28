namespace MyTested.HttpServer.Contracts
{
    using System.Net.Http;

    /// <summary>
    /// Provides options to test the HTTP response from a server.
    /// </summary>
    public interface IServerTestBuilder
    {
        /// <summary>
        /// Tests for a particular HTTP response message.
        /// </summary>
        /// <returns>HTTP response message test builder.</returns>
        IHttpResponseMessageTestBuilder ShouldReturnHttpResponseMessage();

        /// <summary>
        /// Gets the HTTP request message used in the testing.
        /// </summary>
        /// <returns>Instance of HttpRequestMessage.</returns>
        HttpRequestMessage AndProvideTheHttpRequestMessage();

        /// <summary>
        /// Gets the HTTP client used in the testing.
        /// </summary>
        /// <returns>Instance of HttpClient.</returns>
        HttpClient AndProvideTheHttpClient();
    }
}
