namespace MyTested.HttpServer.Samples
{
    using MyTested.HttpServer;
    using System.Net;
    using System.Net.Http;
    using Xunit;

    public class ServerTests
    {
        private const string BaseAddress = "http://mytestedasp.net";

        IServerBuilder httpServer;

        public ServerTests()
        {
            httpServer = MyHttpServer.WorkingRemotely(BaseAddress);
        }

        [Fact]
        public void BaseAddressShouldReturnOkStatus()
        {
            httpServer
                .WithHttpRequestMessage(req => req.WithMethod(HttpMethod.Get))
                .ShouldReturnHttpResponseMessage()
                .WithStatusCode(HttpStatusCode.OK);
        }

        [Fact]
        public void BaseAddressShouldReturnProperServerHeader()
        {
            httpServer
                .WithHttpRequestMessage(req => req.WithMethod(HttpMethod.Get))
                .ShouldReturnHttpResponseMessage()
                .ContainingHeader(HttpHeader.Server, "Microsoft-IIS/8.0");
        }
    }
}
