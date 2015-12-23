namespace MyTested.HttpServer
{
    using Builders;
    using Servers;
    using System;

    /// <summary>
    /// Starting point of the testing framework, which provides a way to specify the remote server to be tested.
    /// </summary>
    public static class MyHttpServer
    {
        /// <summary>
        /// Configures global remote server.
        /// </summary>
        /// <param name="baseAddress">Base address to use for the requests.</param>
        /// <returns>Server builder.</returns>
        public static IServerBuilder IsLocatedAt(string baseAddress)
        {
            RemoteServer.ConfigureGlobal(baseAddress);
            return WorkingRemotely();
        }

        /// <summary>
        /// Processes HTTP request on globally configured remote HTTP server.
        /// </summary>
        /// <returns>Server builder to set specific HTTP requests.</returns>
        public static IServerBuilder WorkingRemotely()
        {
            if (RemoteServer.GlobalIsConfigured)
            {
                return new ServerTestBuilder(RemoteServer.GlobalClient);
            }

            throw new InvalidOperationException("No remote server is configured for this particular test case. Either call MyHttpServer.IsLocatedAt() to configure a new remote server or provide test specific base address.");
        }

        /// <summary>
        /// Processes HTTP request on the remote HTTP server located at the provided base address.
        /// </summary>
        /// <param name="baseAddress">Base address to use for the requests.</param>
        /// <returns>Server builder to set specific HTTP requests.</returns>
        public static IServerBuilder WorkingRemotely(string baseAddress)
        {
            return new ServerTestBuilder(RemoteServer.CreateNewClient(baseAddress), disposeClient: true);
        }
    }
}
