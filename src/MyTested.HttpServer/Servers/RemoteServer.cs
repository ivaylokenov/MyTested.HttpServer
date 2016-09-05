namespace MyTested.HttpServer.Servers
{
    using System;
    using System.Net.Http;

    /// <summary>
    /// Test server for remote server testing.
    /// </summary>
    public static class RemoteServer
    {
        /// <summary>
        /// Gets the global HTTP client used to send the request.
        /// </summary>
        /// <value>HttpClient instance.</value>
        public static HttpClient GlobalClient { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the remote server is configured and requests can be sent.
        /// </summary>
        /// <value>True or false.</value>
        public static bool GlobalIsConfigured
        {
            get { return GlobalClient != null; }
        }

        /// <summary>
        /// Creates new HTTP client for the remote server.
        /// </summary>
        /// <param name="baseAddress">Base address to use for the requests.</param>
        /// <returns>HttpClient instance.</returns>
        public static HttpClient CreateNewClient(string baseAddress, Action<HttpClientHandler> httpClientHandlerSetup = null)
        {
            var handler = new HttpClientHandler
            {
                UseCookies = false,
                AllowAutoRedirect = false,
            };

            httpClientHandlerSetup?.Invoke(handler);

            return new HttpClient(handler)
            {
                BaseAddress = new Uri(baseAddress),
                Timeout = TimeSpan.FromMinutes(1)
            };
        }

        /// <summary>
        /// Configures singleton global instance of the HTTP remote server client.
        /// </summary>
        /// <param name="baseAddress">Base address to use for the requests.</param>
        public static void ConfigureGlobal(string baseAddress, Action<HttpClientHandler> httpClientHandlerSetup = null)
        {
            if (GlobalIsConfigured)
            {
                DisposeGlobal();
            }

            GlobalClient = CreateNewClient(baseAddress, httpClientHandlerSetup);
        }

        /// <summary>
        /// Disposes the global HTTP remote server client.
        /// </summary>
        /// <returns>True or false, indicating whether the client was disposed successfully.</returns>
        public static bool DisposeGlobal()
        {
            if (GlobalClient == null)
            {
                return false;
            }

            GlobalClient.Dispose();
            GlobalClient = null;

            return true;
        }
    }
}
