namespace MyTested.HttpServer.Tests.BuildersTests
{
    using System;
    using System.Net;
    using System.Net.Http;
    using Exceptions;
    using Xunit;

#if DNX451
    using Microsoft.Owin.Hosting;
    using Setups;
#endif

    public class HttpResponseMessageTestBuilderTests : IDisposable
    {
        private const string BaseAddress = "https://mytestedasp.net/";

#if DNX451
        private const string BaseLocalAddress = "http://localhost:9876";

        private IDisposable localServer;

        public HttpResponseMessageTestBuilderTests()
        {
            localServer = WebApp.Start<CustomStartup>(new StartOptions { Port = 9876 });
        }
#endif

        public void Dispose()
        {
#if DNX451
            localServer.Dispose();
#endif
        }
        
        [Fact]
        public void ContainingHeaderShouldNotThrowExceptionWithCorrectHeaderName()
        {
            MyHttpServer
                .WorkingRemotely(BaseAddress)
                .WithHttpRequestMessage(req => req.WithMethod(HttpMethod.Get))
                .ShouldReturnHttpResponseMessage()
                .ContainingHeader("Server");
        }

        [Fact]
        public void ContainingHeaderShouldThrowExceptionWithIncorrectHeaderName()
        {
            var exception = Assert.Throws<HttpResponseMessageAssertionException>(() =>
            {
                MyHttpServer
                    .WorkingRemotely(BaseAddress)
                    .WithHttpRequestMessage(req => req.WithMethod(HttpMethod.Get))
                    .ShouldReturnHttpResponseMessage()
                    .ContainingHeader("AnotherHeader");
            });

            Assert.Equal("When testing 'https://mytestedasp.net/' expected HTTP response message result headers to contain AnotherHeader, but none was found.", exception.Message);
        }

        [Fact]
        public void ContainingHeaderShouldNotThrowExceptionWithCorrectHeaderNameAndValue()
        {
            MyHttpServer
                .WorkingRemotely(BaseAddress)
                .WithHttpRequestMessage(req => req.WithMethod(HttpMethod.Get))
                .ShouldReturnHttpResponseMessage()
                .ContainingHeader("Server", "Microsoft-IIS/8.0");
        }

        [Fact]
        public void ContainingHeaderShouldThrowExceptionWithCorrectHeaderNameAndIncorrectValue()
        {
            var exception = Assert.Throws<HttpResponseMessageAssertionException>(() =>
            {
                MyHttpServer
                    .WorkingRemotely(BaseAddress)
                    .WithHttpRequestMessage(req => req.WithMethod(HttpMethod.Get))
                    .ShouldReturnHttpResponseMessage()
                    .ContainingHeader("Server", "AnotherHeaderValue");
            });

            Assert.Equal("When testing 'https://mytestedasp.net/' expected HTTP response message result headers to contain Server with AnotherHeaderValue value, but none was found.", exception.Message);
        }

        [Fact]
        public void ContainingHeaderShouldNotThrowExceptionWithCorrectHeaderNameAndValues()
        {
            MyHttpServer
                .WorkingRemotely(BaseAddress)
                .WithHttpRequestMessage(req => req.WithMethod(HttpMethod.Get))
                .ShouldReturnHttpResponseMessage()
                .ContainingHeader("Server", new[] { "Microsoft-IIS/8.0" });
        }

        [Fact]
        public void ContainingHeaderShouldThrowExceptionWithCorrectHeaderNameAndIncorrectValues()
        {
            var exception = Assert.Throws<HttpResponseMessageAssertionException>(() =>
            {
                MyHttpServer
                    .WorkingRemotely(BaseAddress)
                    .WithHttpRequestMessage(req => req.WithMethod(HttpMethod.Get))
                    .ShouldReturnHttpResponseMessage()
                    .ContainingHeader("Server", new[] { "AnotherHeaderValue" });
            });

            Assert.Equal("When testing 'https://mytestedasp.net/' expected HTTP response message result headers to have Server with AnotherHeaderValue value, but none was found.", exception.Message);
        }

        [Fact]
        public void ContainingHeaderShouldThrowExceptionWithCorrectHeaderNameAndOneCorrectAndOneIncorrectValues()
        {
            var exception = Assert.Throws<HttpResponseMessageAssertionException>(() =>
            {
                MyHttpServer
                    .WorkingRemotely(BaseAddress)
                    .WithHttpRequestMessage(req => req.WithMethod(HttpMethod.Get))
                    .ShouldReturnHttpResponseMessage()
                    .ContainingHeader("Server", new[] { "TestHeaderValue", "AnotherHeaderValue" });
            });

            Assert.Equal("When testing 'https://mytestedasp.net/' expected HTTP response message result headers to contain Server with 2 values, but instead found 1.", exception.Message);

        }
        
        [Fact]
        public void ContainingContentHeaderShouldNotThrowExceptionWithCorrectHeaderName()
        {
            MyHttpServer
                .WorkingRemotely(BaseAddress)
                .WithHttpRequestMessage(req => req.WithMethod(HttpMethod.Get))
                .ShouldReturnHttpResponseMessage()
                .ContainingContentHeader("Content-Type");
        }

        [Fact]
        public void ContainingContentHeaderShouldThrowExceptionWithIncorrectHeaderName()
        {
            var exception = Assert.Throws<HttpResponseMessageAssertionException>(() =>
            {
                MyHttpServer
                    .WorkingRemotely(BaseAddress)
                    .WithHttpRequestMessage(req => req.WithMethod(HttpMethod.Get))
                    .ShouldReturnHttpResponseMessage()
                    .ContainingContentHeader("AnotherHeader");
            });

            Assert.Equal("When testing 'https://mytestedasp.net/' expected HTTP response message result content headers to contain AnotherHeader, but none was found.", exception.Message);
        }

        [Fact]
        public void ContainingContentHeaderShouldNotThrowExceptionWithCorrectHeaderNameAndValue()
        {
            MyHttpServer
                .WorkingRemotely(BaseAddress)
                .WithHttpRequestMessage(req => req.WithMethod(HttpMethod.Get))
                .ShouldReturnHttpResponseMessage()
                .ContainingContentHeader("Content-Type", "text/html; charset=utf-8");
        }

        [Fact]
        public void ContainingContentHeaderShouldThrowExceptionWithCorrectHeaderNameAndIncorrectValue()
        {
            var exception = Assert.Throws<HttpResponseMessageAssertionException>(() =>
            {
                MyHttpServer
                    .WorkingRemotely(BaseAddress)
                    .WithHttpRequestMessage(req => req.WithMethod(HttpMethod.Get))
                    .ShouldReturnHttpResponseMessage()
                    .ContainingContentHeader("Content-Type", "AnotherHeaderValue");
            });

            Assert.Equal("When testing 'https://mytestedasp.net/' expected HTTP response message result content headers to contain Content-Type with AnotherHeaderValue value, but none was found.", exception.Message);
        }

        [Fact]
        public void ContainingContentHeaderShouldNotThrowExceptionWithCorrectHeaderNameAndValues()
        {
            MyHttpServer
                .WorkingRemotely(BaseAddress)
                .WithHttpRequestMessage(req => req.WithMethod(HttpMethod.Get))
                .ShouldReturnHttpResponseMessage()
                .ContainingContentHeader("Content-Type", new[] { "text/html; charset=utf-8" });
        }

        [Fact]
        public void ContainingContentHeaderShouldThrowExceptionWithCorrectHeaderNameAndIncorrectValues()
        {
            var exception = Assert.Throws<HttpResponseMessageAssertionException>(() =>
            {
                MyHttpServer
                    .WorkingRemotely(BaseAddress)
                    .WithHttpRequestMessage(req => req.WithMethod(HttpMethod.Get))
                    .ShouldReturnHttpResponseMessage()
                    .ContainingContentHeader("Content-Type", new[] { "AnotherHeaderValue" });
            });

            Assert.Equal("When testing 'https://mytestedasp.net/' expected HTTP response message result content headers to have Content-Type with AnotherHeaderValue value, but none was found.", exception.Message);
        }

        [Fact]
        public void ContainingContentHeaderShouldThrowExceptionWithCorrectHeaderNameAndOneCorrectAndOneIncorrectValues()
        {
            var exception = Assert.Throws<HttpResponseMessageAssertionException>(() =>
            {
                MyHttpServer
                    .WorkingRemotely(BaseAddress)
                    .WithHttpRequestMessage(req => req.WithMethod(HttpMethod.Get))
                    .ShouldReturnHttpResponseMessage()
                    .ContainingContentHeader("Content-Type", new[] { "TestHeaderValue", "AnotherHeaderValue" });
            });

            Assert.Equal("When testing 'https://mytestedasp.net/' expected HTTP response message result content headers to contain Content-Type with 2 values, but instead found 1.", exception.Message);
        }
        
        [Fact]
        public void WithStatusCodeShouldNotThrowExceptionWithValidStatusCode()
        {
            MyHttpServer
                .WorkingRemotely(BaseAddress)
                .WithHttpRequestMessage(req => req.WithMethod(HttpMethod.Get))
                .ShouldReturnHttpResponseMessage()
                .WithStatusCode(HttpStatusCode.OK);
        }

        [Fact]
        public void WithStatusCodeShouldThrowExceptionWithInvalidStatusCode()
        {
            var exception = Assert.Throws<HttpResponseMessageAssertionException>(() =>
            {

                MyHttpServer
                    .WorkingRemotely(BaseAddress)
                    .WithHttpRequestMessage(req => req.WithMethod(HttpMethod.Get))
                    .ShouldReturnHttpResponseMessage()
                    .WithStatusCode(HttpStatusCode.BadRequest);
            });

            Assert.Equal("When testing 'https://mytestedasp.net/' expected HTTP response message result status code to be 400 (BadRequest), but instead received 200 (OK).", exception.Message);
        }

        [Fact]
        public void WithVersionShouldNotThrowExceptionWithValidVersionAsString()
        {
            MyHttpServer
                .WorkingRemotely(BaseAddress)
                .WithHttpRequestMessage(req => req.WithMethod(HttpMethod.Get))
                .ShouldReturnHttpResponseMessage()
                .WithVersion("1.1");
        }

        [Fact]
        public void WithVersionShouldNotThrowExceptionWithValidVersionWithMajorAndMinor()
        {
            MyHttpServer
                .WorkingRemotely(BaseAddress)
                .WithHttpRequestMessage(req => req.WithMethod(HttpMethod.Get))
                .ShouldReturnHttpResponseMessage()
                .WithVersion(1, 1);
        }

        [Fact]
        public void WithVersionShouldNotThrowExceptionWithValidVersion()
        {
            MyHttpServer
                .WorkingRemotely(BaseAddress)
                .WithHttpRequestMessage(req => req.WithMethod(HttpMethod.Get))
                .ShouldReturnHttpResponseMessage()
                .WithVersion(new Version(1, 1));
        }

        [Fact]
        public void WithVersionShouldThrowExceptionWithInvalidVersion()
        {
            var exception = Assert.Throws<HttpResponseMessageAssertionException>(() =>
            {
                MyHttpServer
                    .WorkingRemotely(BaseAddress)
                    .WithHttpRequestMessage(req => req.WithMethod(HttpMethod.Get))
                    .ShouldReturnHttpResponseMessage()
                    .WithVersion("1.0");
            });

            Assert.Equal("When testing 'https://mytestedasp.net/' expected HTTP response message result version to be 1.0, but instead received 1.1.", exception.Message);
        }

        [Fact]
        public void WithReasonPhraseShouldNotThrowExceptionWithValidPhrase()
        {
            MyHttpServer
                .WorkingRemotely(BaseAddress)
                .WithHttpRequestMessage(req => req.WithMethod(HttpMethod.Get))
                .ShouldReturnHttpResponseMessage()
                .WithReasonPhrase("OK");
        }

        [Fact]
        public void WithReasonPhraseShouldThrowExceptionWithInvalidPhrase()
        {
            var exception = Assert.Throws<HttpResponseMessageAssertionException>(() =>
            {
                MyHttpServer
                    .WorkingRemotely(BaseAddress)
                    .WithHttpRequestMessage(req => req.WithMethod(HttpMethod.Get))
                    .ShouldReturnHttpResponseMessage()
                    .WithReasonPhrase("Invalid reason phrase");
            });

            Assert.Equal("When testing 'https://mytestedasp.net/' expected HTTP response message result reason phrase to be 'Invalid reason phrase', but instead received 'OK'.", exception.Message);
        }

        [Fact]
        public void WithSuccessStatusCodeShouldNotThrowExceptionWithValidStatusCode()
        {
            MyHttpServer
                .WorkingRemotely(BaseAddress)
                .WithHttpRequestMessage(req => req.WithMethod(HttpMethod.Get))
                .ShouldReturnHttpResponseMessage()
                .WithSuccessStatusCode();
        }

        [Fact]
        public void AndAlsoShouldWorkCorrectly()
        {
            MyHttpServer
                .WorkingRemotely(BaseAddress)
                .WithHttpRequestMessage(req => req.WithMethod(HttpMethod.Get))
                .ShouldReturnHttpResponseMessage()
                .WithSuccessStatusCode()
                .AndAlso()
                .WithReasonPhrase("OK");
        }

        [Fact]
        public void AndProvideTheHttpResponseMessageShouldWorkCorrectly()
        {
            var response = MyHttpServer
                .WorkingRemotely(BaseAddress)
                .WithHttpRequestMessage(req => req.WithMethod(HttpMethod.Get))
                .ShouldReturnHttpResponseMessage()
                .WithSuccessStatusCode()
                .AndAlso()
                .WithReasonPhrase("OK")
                .AndProvideTheHttpResponseMessage();

            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public void WithResponseTimePredicateShouldWorkCorrectly()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "/test");

            MyHttpServer
                .WorkingRemotely(BaseAddress)
                .WithHttpRequestMessage(req => req.WithMethod(HttpMethod.Get))
                .ShouldReturnHttpResponseMessage()
                .WithResponseTime(
                    responseTime => responseTime.TotalMilliseconds > 0 && responseTime.TotalMilliseconds < int.MaxValue);
        }

        [Fact]
        public void WithResponseTimeAssertionsShouldWorkCorrectly()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "/test");

            MyHttpServer
                .WorkingRemotely(BaseAddress)
                .WithHttpRequestMessage(req => req.WithMethod(HttpMethod.Get))
                .ShouldReturnHttpResponseMessage()
                .WithResponseTime(responseTime =>
                {
                    Assert.True(responseTime.TotalMilliseconds > 0);
                    Assert.True(responseTime.TotalMilliseconds < int.MaxValue);
                });
        }

        [Fact]
        public void AndAlsoShouldWorkCorrectlyWithResponseTime()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "/test");

            MyHttpServer
                .WorkingRemotely(BaseAddress)
                .WithHttpRequestMessage(req => req.WithMethod(HttpMethod.Get))
                .ShouldReturnHttpResponseMessage()
                .WithResponseTime(responseTime => responseTime.TotalMilliseconds > 0)
                .AndAlso()
                .WithStatusCode(HttpStatusCode.OK);
        }

        [Fact]
        public void AndProvideTheResponseTimeShouldWorkCorrectly()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "/test");

            var responseTime = MyHttpServer
                .WorkingRemotely(BaseAddress)
                .WithHttpRequestMessage(req => req.WithMethod(HttpMethod.Get))
                .ShouldReturnHttpResponseMessage()
                .AndProvideTheResponseTime();

            Assert.True(responseTime.TotalMilliseconds > 0);
        }

        [Fact]
        public void WithResponseTimeShouldThrowExceptionIfPredicateIsNotValid()
        {
            var exception = Assert.Throws<HttpResponseMessageAssertionException>(() =>
            {
                var request = new HttpRequestMessage(HttpMethod.Post, "/test");

                MyHttpServer
                    .WorkingRemotely(BaseAddress)
                    .WithHttpRequestMessage(req => req.WithMethod(HttpMethod.Get))
                    .ShouldReturnHttpResponseMessage()
                    .WithResponseTime(
                        responseTime => responseTime.TotalMilliseconds < 0);
            });

            Assert.Equal("When testing 'https://mytestedasp.net/' expected HTTP response message result response time to pass the given condition, but it failed.", exception.Message);
        }

#if DNX451
        [Fact]
        public void WithSuccessStatusCodeShouldThrowExceptionWithInvalidStatusCode()
        {
            var exception = Assert.Throws<HttpResponseMessageAssertionException>(() =>
            {
                MyHttpServer
                    .WorkingRemotely(BaseLocalAddress)
                    .WithHttpRequestMessage(req => req.WithMethod(HttpMethod.Get))
                    .ShouldReturnHttpResponseMessage()
                    .WithSuccessStatusCode();
            });

            Assert.Equal("When testing 'http://localhost:9876/' expected HTTP response message result status code to be between 200 and 299, but it was not.", exception.Message);
        }

        [Fact]
        public void WithStringContentShouldNotThrowExceptionWithCorrectContent()
        {
            MyHttpServer
                .WorkingRemotely(BaseLocalAddress)
                .WithHttpRequestMessage(req => req.WithMethod(HttpMethod.Get))
                .ShouldReturnHttpResponseMessage()
                .WithContent("Not found!");
        }

        [Fact]
        public void WithStringContentShouldThrowExceptionWithIncorrectContent()
        {
            var exception = Assert.Throws<HttpResponseMessageAssertionException>(() =>
            {
                MyHttpServer
                    .WorkingRemotely(BaseLocalAddress)
                    .WithHttpRequestMessage(req => req.WithMethod(HttpMethod.Get))
                    .ShouldReturnHttpResponseMessage()
                    .WithContent("Another string");
            });

            Assert.Equal("When testing 'http://localhost:9876/' expected HTTP response message result string content to be 'Another string', but was in fact 'Not found!'.", exception.Message);
        }

        [Fact]
        public void WithStringContentAssertionsShouldWorkCorrectlyWithCorrectAssertions()
        {
            MyHttpServer
                .WorkingRemotely(BaseLocalAddress)
                .WithHttpRequestMessage(req => req.WithMethod(HttpMethod.Get))
                .ShouldReturnHttpResponseMessage()
                .WithContent(content => 
                {
                    Assert.Equal("Not found!", content);
                });
        }

        [Fact]
        public void WithStringContentAssertionsShouldWorkCorrectlyWithCorrectPredicate()
        {
            MyHttpServer
                .WorkingRemotely(BaseLocalAddress)
                .WithHttpRequestMessage(req => req.WithMethod(HttpMethod.Get))
                .ShouldReturnHttpResponseMessage()
                .WithContent(content => content == "Not found!");
        }
        
        [Fact]
        public void WithStringContentAssertionsShouldThrowExceptionWithIncorrectPredicate()
        {
            var exception = Assert.Throws<HttpResponseMessageAssertionException>(() =>
            {
                MyHttpServer
                    .WorkingRemotely(BaseLocalAddress)
                    .WithHttpRequestMessage(req => req.WithMethod(HttpMethod.Get))
                    .ShouldReturnHttpResponseMessage()
                    .WithContent(content => content == "Not");
            });

            Assert.Equal("When testing 'http://localhost:9876/' expected HTTP response message result content to pass the given condition, but it failed.", exception.Message);
        }
#endif
    }
}
