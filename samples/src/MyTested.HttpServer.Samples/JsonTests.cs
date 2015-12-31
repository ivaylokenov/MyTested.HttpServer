namespace MyTested.HttpServer.Samples
{
    using MyTested.HttpServer;
    using Newtonsoft.Json.Linq;
    using System.Linq;
    using System.Net.Http;
    using Xunit;

    public class JsonTests
    {
        private const string BaseAddress = "https://api.github.com";

        IServerBuilder httpServer;

        public JsonTests()
        {
            httpServer = MyHttpServer.WorkingRemotely(BaseAddress);
        }

        [Fact]
        public void JsonShouldContainCorrectRepository()
        {
            httpServer
                .WithHttpRequestMessage(req => req
                    .WithMethod(HttpMethod.Get)
                    .WithRequestUri("/users/ivaylokenov/repos")
                    .WithHeader(HttpHeader.UserAgent, "MyTested.HttpServer"))
                .ShouldReturnHttpResponseMessage()
                .ContainingContentHeader(HttpContentHeader.ContentType, "application/json; charset=utf-8")
                .WithContent(content =>
                {
                    var repository = JArray.Parse(content)
                        .Children()
                        .FirstOrDefault(c => (string)c["name"] == "MyTested.HttpServer");

                    Assert.NotNull(repository);
                });
        }
    }
}
