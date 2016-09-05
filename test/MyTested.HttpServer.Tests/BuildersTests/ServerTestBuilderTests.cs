namespace MyTested.HttpServer.Tests.BuildersTests
{
    using HttpServer;
    using System;
    using System.Net;
    using System.Net.Http;
    using Xunit;
    using Servers;

    public class ServerTestBuilderTests
    {
        [Fact]
        public void RemoteServerShouldWorkCorrectlyWithGlobalConfiguration()
        {
            MyHttpServer.IsLocatedAt("http://google.com", clientHandler =>
            {
                clientHandler.AllowAutoRedirect = true;
            });

            MyHttpServer
                .WorkingRemotely()
                .WithHttpRequestMessage(req => req.WithMethod(HttpMethod.Get))
                .ShouldReturnHttpResponseMessage()
                .WithResponseTime(time => time.TotalMilliseconds > 0)
                .WithStatusCode(HttpStatusCode.OK)
                .ContainingContentHeader(HttpContentHeader.ContentType);

            MyHttpServer
                .WorkingRemotely()
                .WithHttpRequestMessage(req => req.WithMethod(HttpMethod.Delete))
                .ShouldReturnHttpResponseMessage()
                .WithResponseTime(time => time.TotalMilliseconds > 0)
                .WithStatusCode(HttpStatusCode.MethodNotAllowed);

            MyHttpServer
                .WorkingRemotely()
                .WithHttpRequestMessage(req => req.WithRequestUri("/notfound"))
                .ShouldReturnHttpResponseMessage()
                .WithResponseTime(time => time.TotalMilliseconds > 0)
                .WithStatusCode(HttpStatusCode.NotFound);

            RemoteServer.DisposeGlobal();
        }

        [Fact]
        public void RemoteServerShouldWorkCorrectlyWithSpecificBaseAddress()
        {
            Assert.False(RemoteServer.GlobalIsConfigured);

            MyHttpServer
                .WorkingRemotely("http://google.com")
                .WithHttpRequestMessage(req => req.WithRequestUri("/users/ivaylokenov/repos"))
                .ShouldReturnHttpResponseMessage()
                .WithResponseTime(time => time.TotalMilliseconds > 0)
                .WithStatusCode(HttpStatusCode.NotFound)
                .ContainingContentHeader(HttpContentHeader.ContentType, "text/html; charset=UTF-8");

            Assert.False(RemoteServer.GlobalIsConfigured);

            MyHttpServer
                .WorkingRemotely("https://api.github.com")
                .WithHttpRequestMessage(req => req
                    .WithRequestUri("/users/ivaylokenov/repos")
                    .WithHeader(HttpHeader.UserAgent, "MyTested.WebApi"))
                .ShouldReturnHttpResponseMessage()
                .WithResponseTime(time => time.TotalMilliseconds > 0)
                .WithStatusCode(HttpStatusCode.OK)
                .ContainingContentHeader(HttpContentHeader.ContentType, "application/json; charset=utf-8");

            Assert.False(RemoteServer.GlobalIsConfigured);
        }

        [Fact]
        public void WorkingRemotelyWithoutAnyBaseAddressShouldThrowException()
        {
            RemoteServer.DisposeGlobal();

            var exception = Assert.Throws<InvalidOperationException>(() =>
            {
                MyHttpServer
                    .WorkingRemotely()
                    .WithHttpRequestMessage(req => req.WithRequestUri("/users/ivaylokenov/repos"))
                    .ShouldReturnHttpResponseMessage()
                    .WithResponseTime(time => time.TotalMilliseconds > 0)
                    .WithStatusCode(HttpStatusCode.NotFound)
                    .ContainingContentHeader(HttpContentHeader.ContentType, "text/html; charset=UTF-8");
            });

            Assert.Equal("No remote server is configured for this particular test case. Either call MyHttpServer.IsLocatedAt() to configure a new remote server or provide test specific base address.", exception.Message);
        }
    }
}
