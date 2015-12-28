namespace MyTested.HttpServer.Builders
{
    using Common;
    using Contracts;
    using Exceptions;
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using Utilities;

    /// <summary>
    /// Used for testing HTTP response message results from handlers.
    /// </summary>
    public class HttpResponseMessageTestBuilder : IAndHttpResponseMessageTestBuilder
    {
        private readonly HttpResponseMessage httpResponseMessage;
        private readonly HttpMessageHandler handler;
        private readonly Uri requestUri;
        private readonly TimeSpan responseTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpResponseMessageTestBuilder" /> class.
        /// </summary>
        /// <param name="handler">Tested HTTP message handler.</param>
        /// <param name="httpResponseMessage">HTTP response result from the tested handler.</param>
        /// <param name="requestUri">HTTP request Uri.</param>
        /// <param name="responseTime">Measured response time from the tested handler.</param>
        public HttpResponseMessageTestBuilder(
            HttpMessageHandler handler,
            HttpResponseMessage httpResponseMessage,
            Uri requestUri,
            TimeSpan responseTime)
        {
            CommonValidator.CheckForNullReference(httpResponseMessage, errorMessageName: "HttpResponseMessage");
            this.handler = handler;
            this.httpResponseMessage = httpResponseMessage;
            this.requestUri = requestUri;
            this.responseTime = responseTime;
        }

        /// <summary>
        /// Tests whether the content of the HTTP response message is of certain type.
        /// </summary>
        /// <typeparam name="TContentType">Type of expected HTTP content.</typeparam>
        /// <returns>The same HTTP response message test builder.</returns>
        public IAndHttpResponseMessageTestBuilder WithContentOfType<TContentType>()
            where TContentType : HttpContent
        {
            HttpResponseMessageValidator.WithContentOfType<TContentType>(
                this.httpResponseMessage.Content,
                this.ThrowNewHttpResponseMessageAssertionException);

            return this;
        }

        /// <summary>
        /// Tests whether the content of the HTTP response message is the provided string.
        /// </summary>
        /// <param name="content">Expected string content.</param>
        /// <returns>The same HTTP response message test builder.</returns>
        public IAndHttpResponseMessageTestBuilder WithStringContent(string content)
        {
            HttpResponseMessageValidator.WithStringContent(
                this.httpResponseMessage.Content,
                content,
                this.ThrowNewHttpResponseMessageAssertionException);

            return this;
        }
        
        /// <summary>
        /// Tests whether the HTTP response message contains response header with certain name.
        /// </summary>
        /// <param name="name">Name of expected response header.</param>
        /// <returns>The same HTTP response message test builder.</returns>
        public IAndHttpResponseMessageTestBuilder ContainingHeader(string name)
        {
            HttpResponseMessageValidator.ContainingHeader(this.httpResponseMessage.Headers, name, this.ThrowNewHttpResponseMessageAssertionException);
            return this;
        }

        /// <summary>
        /// Tests whether the HTTP response message contains response header with certain name and value.
        /// </summary>
        /// <param name="name">Name of expected response header.</param>
        /// <param name="value">Value of expected response header.</param>
        /// <returns>The same HTTP response message test builder.</returns>
        public IAndHttpResponseMessageTestBuilder ContainingHeader(string name, string value)
        {
            HttpResponseMessageValidator.ContainingHeader(this.httpResponseMessage.Headers, name, value, this.ThrowNewHttpResponseMessageAssertionException);
            return this;
        }

        /// <summary>
        /// Tests whether the HTTP response message contains response header with certain name and collection of value.
        /// </summary>
        /// <param name="name">Name of expected response header.</param>
        /// <param name="values">Collection of values in the expected response header.</param>
        /// <returns>The same HTTP response message test builder.</returns>
        public IAndHttpResponseMessageTestBuilder ContainingHeader(string name, IEnumerable<string> values)
        {
            HttpResponseMessageValidator.ContainingHeader(this.httpResponseMessage.Headers, name, values, this.ThrowNewHttpResponseMessageAssertionException);
            return this;
        }

        /// <summary>
        /// Tests whether the HTTP response message contains response headers provided by dictionary.
        /// </summary>
        /// <param name="headers">Dictionary containing response headers.</param>
        /// <returns>The same HTTP response message test builder.</returns>
        public IAndHttpResponseMessageTestBuilder ContainingHeaders(IDictionary<string, IEnumerable<string>> headers)
        {
            HttpResponseMessageValidator.ValidateHeadersCount(headers, this.httpResponseMessage.Headers, this.ThrowNewHttpResponseMessageAssertionException);
            headers.ForEach(h => this.ContainingHeader(h.Key, h.Value));
            return this;
        }

        /// <summary>
        /// Tests whether the HTTP response message contains content header with certain name.
        /// </summary>
        /// <param name="name">Name of expected content header.</param>
        /// <returns>The same HTTP response message test builder.</returns>
        public IAndHttpResponseMessageTestBuilder ContainingContentHeader(string name)
        {
            HttpResponseMessageValidator.ValidateContent(this.httpResponseMessage.Content, this.ThrowNewHttpResponseMessageAssertionException);
            HttpResponseMessageValidator.ContainingHeader(
                this.httpResponseMessage.Content.Headers,
                name,
                this.ThrowNewHttpResponseMessageAssertionException,
                isContentHeader: true);

            return this;
        }

        /// <summary>
        /// Tests whether the HTTP response message contains content header with certain name and value.
        /// </summary>
        /// <param name="name">Name of expected content header.</param>
        /// <param name="value">Value of expected content header.</param>
        /// <returns>The same HTTP response message test builder.</returns>
        public IAndHttpResponseMessageTestBuilder ContainingContentHeader(string name, string value)
        {
            HttpResponseMessageValidator.ValidateContent(this.httpResponseMessage.Content, this.ThrowNewHttpResponseMessageAssertionException);
            HttpResponseMessageValidator.ContainingHeader(
                this.httpResponseMessage.Content.Headers,
                name,
                value,
                this.ThrowNewHttpResponseMessageAssertionException,
                isContentHeader: true);

            return this;
        }

        /// <summary>
        /// Tests whether the HTTP response message contains content header with certain name and collection of value.
        /// </summary>
        /// <param name="name">Name of expected content header.</param>
        /// <param name="values">Collection of values in the expected content header.</param>
        /// <returns>The same HTTP response message test builder.</returns>
        public IAndHttpResponseMessageTestBuilder ContainingContentHeader(string name, IEnumerable<string> values)
        {
            HttpResponseMessageValidator.ValidateContent(this.httpResponseMessage.Content, this.ThrowNewHttpResponseMessageAssertionException);
            HttpResponseMessageValidator.ContainingHeader(
                this.httpResponseMessage.Content.Headers,
                name,
                values,
                this.ThrowNewHttpResponseMessageAssertionException,
                isContentHeader: true);

            return this;
        }

        /// <summary>
        /// Tests whether the HTTP response message contains content headers provided by dictionary.
        /// </summary>
        /// <param name="headers">Dictionary containing content headers.</param>
        /// <returns>The same HTTP response message test builder.</returns>
        public IAndHttpResponseMessageTestBuilder ContainingContentHeaders(
            IDictionary<string, IEnumerable<string>> headers)
        {
            HttpResponseMessageValidator.ValidateContent(this.httpResponseMessage.Content, this.ThrowNewHttpResponseMessageAssertionException);
            HttpResponseMessageValidator.ValidateHeadersCount(
                headers,
                this.httpResponseMessage.Content.Headers,
                this.ThrowNewHttpResponseMessageAssertionException,
                isContentHeaders: true);

            headers.ForEach(h => this.ContainingContentHeader(h.Key, h.Value));
            return this;
        }

        /// <summary>
        /// Tests whether HTTP response message status code is the same as the provided HttpStatusCode.
        /// </summary>
        /// <param name="statusCode">Expected status code.</param>
        /// <returns>The same HTTP response message test builder.</returns>
        public IAndHttpResponseMessageTestBuilder WithStatusCode(HttpStatusCode statusCode)
        {
            HttpResponseMessageValidator.WithStatusCode(this.httpResponseMessage, statusCode, this.ThrowNewHttpResponseMessageAssertionException);
            return this;
        }

        /// <summary>
        /// Tests whether HTTP response message version is the same as the provided version as string.
        /// </summary>
        /// <param name="version">Expected version as string.</param>
        /// <returns>The same HTTP response message test builder.</returns>
        public IAndHttpResponseMessageTestBuilder WithVersion(string version)
        {
            var parsedVersion = VersionValidator.TryParse(version, this.ThrowNewHttpResponseMessageAssertionException);
            return this.WithVersion(parsedVersion);
        }

        /// <summary>
        /// Tests whether HTTP response message version is the same as the provided version.
        /// </summary>
        /// <param name="major">Major number in the expected version.</param>
        /// <param name="minor">Minor number in the expected version.</param>
        /// <returns>The same HTTP response message test builder.</returns>
        public IAndHttpResponseMessageTestBuilder WithVersion(int major, int minor)
        {
            return this.WithVersion(new Version(major, minor));
        }

        /// <summary>
        /// Tests whether HTTP response message version is the same as the provided version.
        /// </summary>
        /// <param name="version">Expected version.</param>
        /// <returns>The same HTTP response message test builder.</returns>
        public IAndHttpResponseMessageTestBuilder WithVersion(Version version)
        {
            HttpResponseMessageValidator.WithVersion(this.httpResponseMessage, version, this.ThrowNewHttpResponseMessageAssertionException);
            return this;
        }

        /// <summary>
        /// Tests whether HTTP response message reason phrase is the same as the provided reason phrase as string.
        /// </summary>
        /// <param name="reasonPhrase">Expected reason phrase as string.</param>
        /// <returns>The same HTTP response message test builder.</returns>
        public IAndHttpResponseMessageTestBuilder WithReasonPhrase(string reasonPhrase)
        {
            HttpResponseMessageValidator.WithReasonPhrase(this.httpResponseMessage, reasonPhrase, this.ThrowNewHttpResponseMessageAssertionException);
            return this;
        }

        /// <summary>
        /// Tests whether HTTP response message returns success status code between 200 and 299.
        /// </summary>
        /// <returns>The same HTTP response message test builder.</returns>
        public IAndHttpResponseMessageTestBuilder WithSuccessStatusCode()
        {
            HttpResponseMessageValidator.WithSuccessStatusCode(this.httpResponseMessage, this.ThrowNewHttpResponseMessageAssertionException);
            return this;
        }
        
        /// <summary>
        /// Tests whether the measured response time passes given assertions.
        /// </summary>
        /// <param name="assertions">Action containing all assertions on the measured response time.</param>
        /// <returns>The same HTTP response message test builder.</returns>
        public IAndHttpResponseMessageTestBuilder WithResponseTime(Action<TimeSpan> assertions)
        {
            assertions(this.responseTime);
            return this;
        }

        /// <summary>
        /// Tests whether the measured response time passes given predicate.
        /// </summary>
        /// <param name="predicate">Predicate testing the measured response time.</param>
        /// <returns>The same HTTP response message test builder.</returns>
        public IAndHttpResponseMessageTestBuilder WithResponseTime(Func<TimeSpan, bool> predicate)
        {
            if (!predicate(this.responseTime))
            {
                this.ThrowNewHttpResponseMessageAssertionException("response time", "to pass the given condition", "it failed");
            }

            return this;
        }
        
        /// <summary>
        /// AndAlso method for better readability when chaining HTTP response message tests.
        /// </summary>
        /// <returns>The same HTTP response message test builder.</returns>
        public IHttpResponseMessageTestBuilder AndAlso()
        {
            return this;
        }

        /// <summary>
        /// Gets the HTTP message handler used in the testing.
        /// </summary>
        /// <returns>Instance of HttpMessageHandler.</returns>
        public HttpMessageHandler AndProvideTheHandler()
        {
            return this.handler;
        }

        /// <summary>
        /// Gets the HTTP response message used in the testing.
        /// </summary>
        /// <returns>Instance of HttpResponseMessage.</returns>
        public HttpResponseMessage AndProvideTheHttpResponseMessage()
        {
            return this.httpResponseMessage;
        }

        /// <summary>
        /// Gets the response time measured in the testing.
        /// </summary>
        /// <returns>Instance of TimeSpan.</returns>
        public TimeSpan AndProvideTheResponseTime()
        {
            return this.responseTime;
        }

        /// <summary>
        /// Throws HttpResponseMessageAssertionException with specific message.
        /// </summary>
        /// <param name="propertyName">Tested property name of the HTTP response message.</param>
        /// <param name="expectedValue">Expected value.</param>
        /// <param name="actualValue">Actual value.</param>
        protected void ThrowNewHttpResponseMessageAssertionException(string propertyName, string expectedValue, string actualValue)
        {
            throw new HttpResponseMessageAssertionException(string.Format(
                    "When testing '{0}' expected HTTP response message result {1} {2}, but {3}.",
                    this.requestUri.ToString(),
                    propertyName,
                    expectedValue,
                    actualValue));
        }
    }
}
