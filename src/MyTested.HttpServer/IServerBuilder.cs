namespace MyTested.HttpServer
{
    using Contracts;
    using System;
    using System.Collections.Generic;
    using System.Net.Http;

    /// <summary>
    /// Provides options to set the HTTP request to test.
    /// </summary>
    public interface IServerBuilder
    {
        /// <summary>
        /// Adds default header to every request tested on the server.
        /// </summary>
        /// <param name="name">Name of the header.</param>
        /// <param name="value">Value of the header.</param>
        /// <returns>The same server builder.</returns>
        IServerBuilder WithDefaultRequestHeader(string name, string value);

        /// <summary>
        /// Adds default header to every request tested on the server.
        /// </summary>
        /// <param name="name">Name of the header.</param>
        /// <param name="values">Collection of values for the header.</param>
        /// <returns>The same server builder.</returns>
        IServerBuilder WithDefaultRequestHeader(string name, IEnumerable<string> values);

        /// <summary>
        /// Adds default collection of headers to every request tested on the server.
        /// </summary>
        /// <param name="headers">Dictionary of headers to add.</param>
        /// <returns>The same server builder.</returns>
        IServerBuilder WithDefaultRequestHeaders(IDictionary<string, IEnumerable<string>> headers);

        /// <summary>
        /// Adds HTTP request message to the tested server.
        /// </summary>
        /// <param name="requestMessage">Instance of HttpRequestMessage.</param>
        /// <returns>Server test builder to test the returned HTTP response.</returns>
        IServerTestBuilder WithHttpRequestMessage(HttpRequestMessage requestMessage);

        /// <summary>
        /// Adds HTTP request message to the tested server.
        /// </summary>
        /// <param name="httpRequestMessageBuilder">Builder for HTTP request message.</param>
        /// <returns>Server test builder to test the returned HTTP response.</returns>
        IServerTestBuilder WithHttpRequestMessage(Action<IHttpRequestMessageBuilder> httpRequestMessageBuilder);
    }
}
