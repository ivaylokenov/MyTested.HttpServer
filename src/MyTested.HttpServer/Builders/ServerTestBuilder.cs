﻿namespace MyTested.HttpServer.Builders
{
    using Common;
    using Contracts;
    using Servers;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Net.Http;
    using System.Threading;

    /// <summary>
    /// Provides options to set the HTTP request and test the HTTP response.
    /// </summary>
    public class ServerTestBuilder : IServerBuilder, IServerTestBuilder
    {
        private readonly HttpClient client;
        private readonly bool disposeClient;

        private HttpRequestMessage httpRequestMessage;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerTestBuilder" /> class.
        /// </summary>
        /// <param name="client">HTTP message invoker to send the request.</param>
        /// <param name="transformRequest">Indicates whether to transform relative to fake absolute request URI.</param>
        /// <param name="disposeClient">Indicates whether to dispose the server and the client after the test completes.</param>
        /// <param name="server">IDisposable server to use for the request.</param>
        public ServerTestBuilder(
            HttpClient client,
            bool disposeClient = false)
        {
            this.client = client;
            this.disposeClient = disposeClient;
        }

        /// <summary>
        /// Adds default header to every request tested on the server.
        /// </summary>
        /// <param name="name">Name of the header.</param>
        /// <param name="value">Value of the header.</param>
        /// <returns>The same server builder.</returns>
        public IServerBuilder WithDefaultRequestHeader(string name, string value)
        {
            this.client.DefaultRequestHeaders.Add(name, value);
            return this;
        }

        /// <summary>
        /// Adds default header to every request tested on the server.
        /// </summary>
        /// <param name="name">Name of the header.</param>
        /// <param name="values">Collection of values for the header.</param>
        /// <returns>The same server builder.</returns>
        public IServerBuilder WithDefaultRequestHeader(string name, IEnumerable<string> values)
        {
            this.client.DefaultRequestHeaders.Add(name, values);
            return this;
        }

        /// <summary>
        /// Adds default collection of headers to every request tested on the server.
        /// </summary>
        /// <param name="headers">Dictionary of headers to add.</param>
        /// <returns>The same server builder.</returns>
        public IServerBuilder WithDefaultRequestHeaders(IDictionary<string, IEnumerable<string>> headers)
        {
            headers.ForEach(h => this.WithDefaultRequestHeader(h.Key, h.Value));
            return this;
        }

        /// <summary>
        /// Adds HTTP request message to the tested server.
        /// </summary>
        /// <param name="requestMessage">Instance of HttpRequestMessage.</param>
        /// <returns>Server test builder to test the returned HTTP response.</returns>
        public IServerTestBuilder WithHttpRequestMessage(HttpRequestMessage requestMessage)
        {
            this.httpRequestMessage = requestMessage;
            return this;
        }

        /// <summary>
        /// Adds HTTP request message to the tested server.
        /// </summary>
        /// <param name="httpRequestMessageBuilder">Builder for HTTP request message.</param>
        /// <returns>Server test builder to test the returned HTTP response.</returns>
        public IServerTestBuilder WithHttpRequestMessage(Action<IHttpRequestMessageBuilder> httpRequestMessageBuilder)
        {
            var httpBuilder = new HttpRequestMessageBuilder();
            httpRequestMessageBuilder(httpBuilder);
            return this.WithHttpRequestMessage(httpBuilder.GetHttpRequestMessage());
        }

        /// <summary>
        /// Tests for a particular HTTP response message.
        /// </summary>
        /// <returns>HTTP response message test builder.</returns>
        public IHttpResponseMessageTestBuilder ShouldReturnHttpResponseMessage()
        {
            var serverHandler = new ServerHttpMessageHandler(this.client, this.disposeClient);
            using (var invoker = new HttpMessageInvoker(serverHandler, true))
            {
                var stopwatch = Stopwatch.StartNew();

                var httpResponseMessage = invoker.SendAsync(this.httpRequestMessage, CancellationToken.None).Result;

                stopwatch.Stop();

                if (this.disposeClient)
                {
                    this.client.Dispose();
                }
                
                return new HttpResponseMessageTestBuilder(
                    serverHandler,
                    httpResponseMessage,
                    this.httpRequestMessage.RequestUri,
                    stopwatch.Elapsed);
            }
        }

        /// <summary>
        /// Gets the HTTP request message used in the testing.
        /// </summary>
        /// <returns>Instance of HttpRequestMessage.</returns>
        public HttpRequestMessage AndProvideTheHttpRequestMessage()
        {
            return this.httpRequestMessage;
        }

        /// <summary>
        /// Gets the HTTP client used in the testing.
        /// </summary>
        /// <returns>Instance of HttpClient.</returns>
        public HttpClient AndProvideTheHttpClient()
        {
            return this.client;
        }
    }
}