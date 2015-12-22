namespace MyTested.HttpServer.Builders
{
    using MyTested.HttpServer.Contracts;
    using System.Net.Http;

    /// <summary>
    /// Base class for handler test builders.
    /// </summary>
    public abstract class BaseHandlerTestBuilder : IBaseHandlerTestBuilder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseHandlerTestBuilder" /> class.
        /// </summary>
        /// <param name="handler">Instance of the HttpMessageHandler to be tested.</param>
        protected BaseHandlerTestBuilder(HttpMessageHandler handler)
        {
            this.Handler = handler;
        }

        /// <summary>
        /// Gets the HTTP message handler used in the testing.
        /// </summary>
        /// <value>Instance of HttpMessageHandler.</value>
        protected HttpMessageHandler Handler { get; private set; }

        /// <summary>
        /// Gets the HTTP message handler used in the testing.
        /// </summary>
        /// <returns>Instance of HttpMessageHandler.</returns>
        public HttpMessageHandler AndProvideTheHandler()
        {
            return this.Handler;
        }
    }
}
