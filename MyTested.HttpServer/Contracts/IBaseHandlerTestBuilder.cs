namespace MyTested.HttpServer.Contracts
{
    using System.Net.Http;

    /// <summary>
    /// Base class for handler test builders.
    /// </summary>
    public interface IBaseHandlerTestBuilder
    {
        /// <summary>
        /// Gets the HTTP message handler used in the testing.
        /// </summary>
        /// <returns>Instance of HttpMessageHandler.</returns>
        HttpMessageHandler AndProvideTheHandler();
    }
}
