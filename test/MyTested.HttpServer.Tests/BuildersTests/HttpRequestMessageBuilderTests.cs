namespace MyTested.HttpServer.Tests.BuildersTests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using Exceptions;
    using Xunit;
    
    public class HttpRequestMessageBuilderTests
    {
        private const string StringContent = "TestContent";
        private const string TestHeader = "TestHeader";
        private const string TestHeaderValue = "TestHeaderValue";
        private const string AnotherTestHeaderValue = "AnotherTestHeaderValue";
        private const string RequestUri = "http://sometest.com/sometest?test=1";
        private const string BaseAddress = "http://google.com";
        private const string Version = "1.1";

        private readonly byte[] buffer = { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 };

        [Fact]
        public void WithContentShouldPopulateCorrectContent()
        {
            var httpRequestMessage = MyHttpServer
                .WorkingRemotely(BaseAddress)
                .WithHttpRequestMessage(request => request.WithContent(new StringContent(StringContent)))
                .AndProvideTheHttpRequestMessage();

            Assert.IsAssignableFrom<StringContent>(httpRequestMessage.Content);
            Assert.Equal(StringContent, httpRequestMessage.Content.ReadAsStringAsync().Result);
        }

        [Fact]
        public void WithStreamContentShouldPopulateCorrectContent()
        {
            var httpRequestMessage = MyHttpServer
                .WorkingRemotely(BaseAddress)
                .WithHttpRequestMessage(request => request.WithStreamContent(new MemoryStream(this.buffer)))
                .AndProvideTheHttpRequestMessage();

            Assert.IsAssignableFrom<StreamContent>(httpRequestMessage.Content);
            Assert.Equal(this.buffer.Length, httpRequestMessage.Content.ReadAsByteArrayAsync().Result.Length);
        }

        [Fact]
        public void WithStreamContentAndBufferSizeShouldPopulateCorrectContent()
        {
            var httpRequestMessage = MyHttpServer
                .WorkingRemotely(BaseAddress)
                .WithHttpRequestMessage(request => request.WithStreamContent(new MemoryStream(this.buffer), 8))
                .AndProvideTheHttpRequestMessage();

            Assert.IsAssignableFrom<StreamContent>(httpRequestMessage.Content);
            Assert.Equal(this.buffer.Length, httpRequestMessage.Content.ReadAsByteArrayAsync().Result.Length);
        }

        [Fact]
        public void WithByteArrayContentShouldPopulateCorrectContent()
        {
            var httpRequestMessage = MyHttpServer
                .WorkingRemotely(BaseAddress)
                .WithHttpRequestMessage(request => request.WithByteArrayContent(this.buffer))
                .AndProvideTheHttpRequestMessage();

            Assert.IsAssignableFrom<ByteArrayContent>(httpRequestMessage.Content);
            Assert.Equal(this.buffer.Length, httpRequestMessage.Content.ReadAsByteArrayAsync().Result.Length);
        }

        [Fact]
        public void WithByteArrayContentAndBufferSizeShouldPopulateCorrectContent()
        {
            var httpRequestMessage = MyHttpServer
                .WorkingRemotely(BaseAddress)
                .WithHttpRequestMessage(request => request.WithByteArrayContent(this.buffer, 2, 4))
                .AndProvideTheHttpRequestMessage();

            Assert.IsAssignableFrom<ByteArrayContent>(httpRequestMessage.Content);
            Assert.Equal(4, httpRequestMessage.Content.ReadAsByteArrayAsync().Result.Length);
        }

        [Fact]
        public void WithFormUrlEncodedContentShouldPopulateCorrectContent()
        {
            var httpRequestMessage = MyHttpServer
                .WorkingRemotely(BaseAddress)
                .WithHttpRequestMessage(request => request.WithFormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("First", "FirstValue"), new KeyValuePair<string, string>("Second", "SecondValue")
                }))
                .AndProvideTheHttpRequestMessage();

            Assert.IsAssignableFrom<FormUrlEncodedContent>(httpRequestMessage.Content);
            Assert.Equal("First=FirstValue&Second=SecondValue", httpRequestMessage.Content.ReadAsStringAsync().Result);
            Assert.Equal(MediaType.FormUrlEncoded, httpRequestMessage.Content.Headers.ContentType.MediaType);
        }

        [Fact]
        public void WithFormUrlEncodedContentShouldPopulateCorrectContentWithDirectString()
        {
            var httpRequestMessage = MyHttpServer
                .WorkingRemotely(BaseAddress)
                .WithHttpRequestMessage(request => request.WithFormUrlEncodedContent("First=FirstValue&Second=SecondValue"))
                .AndProvideTheHttpRequestMessage();

            Assert.Equal("First=FirstValue&Second=SecondValue", httpRequestMessage.Content.ReadAsStringAsync().Result);
            Assert.Equal(MediaType.FormUrlEncoded, httpRequestMessage.Content.Headers.ContentType.MediaType);
        }

        [Fact]
        public void WithJsonContentShouldPopulateCorrectContent()
        {
            var httpRequestMessage = MyHttpServer
                .WorkingRemotely(BaseAddress)
                .WithHttpRequestMessage(request => request.WithJsonContent(@"{""Age"":5}"))
                .AndProvideTheHttpRequestMessage();

            Assert.Equal(@"{""Age"":5}", httpRequestMessage.Content.ReadAsStringAsync().Result);
            Assert.Equal(MediaType.ApplicationJson, httpRequestMessage.Content.Headers.ContentType.MediaType);
        }

        [Fact]
        public void WithStringContentShouldPopulateCorrectContent()
        {
            var httpRequestMessage = MyHttpServer
                .WorkingRemotely(BaseAddress)
                .WithHttpRequestMessage(request => request.WithStringContent(StringContent))
                .AndProvideTheHttpRequestMessage();

            Assert.IsAssignableFrom<StringContent>(httpRequestMessage.Content);
            Assert.Equal(StringContent, httpRequestMessage.Content.ReadAsStringAsync().Result);
        }

        [Fact]
        public void WithStringContentAndMediaTypeShouldPopulateCorrectContent()
        {
            var httpRequestMessage = MyHttpServer
                .WorkingRemotely(BaseAddress)
                .WithHttpRequestMessage(request => request.WithStringContent(StringContent, MediaType.ApplicationXml))
                .AndProvideTheHttpRequestMessage();

            Assert.IsAssignableFrom<StringContent>(httpRequestMessage.Content);
            Assert.Equal(StringContent, httpRequestMessage.Content.ReadAsStringAsync().Result);
            Assert.Equal(MediaType.ApplicationXml, httpRequestMessage.Content.Headers.ContentType.MediaType);
        }

        [Fact]
        public void WithStringContentAndEncodingShouldPopulateCorrectContent()
        {
            var httpRequestMessage = MyHttpServer
                .WorkingRemotely(BaseAddress)
                .WithHttpRequestMessage(request => request.WithStringContent(StringContent, Encoding.UTF8))
                .AndProvideTheHttpRequestMessage();

            Assert.IsAssignableFrom<StringContent>(httpRequestMessage.Content);
            Assert.Equal(StringContent, httpRequestMessage.Content.ReadAsStringAsync().Result);
        }

        [Fact]
        public void WithStringContentEncodingAndMediaTypeShouldPopulateCorrectContent()
        {
            var httpRequestMessage = MyHttpServer
                .WorkingRemotely(BaseAddress)
                .WithHttpRequestMessage(request => request.WithStringContent(StringContent, Encoding.UTF8, MediaType.ApplicationJson))
                .AndProvideTheHttpRequestMessage();

            Assert.IsAssignableFrom<StringContent>(httpRequestMessage.Content);
            Assert.Equal(StringContent, httpRequestMessage.Content.ReadAsStringAsync().Result);
        }

        [Fact]
        public void WithHeaderShouldPopulateCorrectHeader()
        {
            var httpRequestMessage = MyHttpServer
                .WorkingRemotely(BaseAddress)
                .WithHttpRequestMessage(request => request.WithHeader(TestHeader, TestHeaderValue))
                .AndProvideTheHttpRequestMessage();

            Assert.True(httpRequestMessage.Headers.Contains(TestHeader));
            Assert.True(httpRequestMessage.Headers.First(h => h.Key == TestHeader).Value.Contains(TestHeaderValue));
        }

        [Fact]
        public void WithHeaderAndMultipleValuesShouldPopulateCorrectHeader()
        {
            var httpRequestMessage = MyHttpServer
                .WorkingRemotely(BaseAddress)
                .WithHttpRequestMessage(request => request.WithHeader(TestHeader, new[] { TestHeaderValue, AnotherTestHeaderValue }))
                .AndProvideTheHttpRequestMessage();

            Assert.True(httpRequestMessage.Headers.Contains(TestHeader));
            Assert.True(httpRequestMessage.Headers.First(h => h.Key == TestHeader).Value.Contains(TestHeaderValue));
            Assert.True(httpRequestMessage.Headers.First(h => h.Key == TestHeader).Value.Contains(AnotherTestHeaderValue));
        }

        [Fact]
        public void WithHeadersDictionaryShouldPopulateCorrectHeaders()
        {
            var httpRequestMessage = MyHttpServer
                .WorkingRemotely(BaseAddress)
                .WithHttpRequestMessage(request => request
                    .WithHeaders(new Dictionary<string, IEnumerable<string>>
                    {
                        { TestHeader, new[] { TestHeaderValue, AnotherTestHeaderValue } },
                    }))
                .AndProvideTheHttpRequestMessage();

            Assert.True(httpRequestMessage.Headers.Contains(TestHeader));
            Assert.True(httpRequestMessage.Headers.First(h => h.Key == TestHeader).Value.Contains(TestHeaderValue));
            Assert.True(httpRequestMessage.Headers.First(h => h.Key == TestHeader).Value.Contains(AnotherTestHeaderValue));
        }

        [Fact]
        public void WithContentHeaderShouldPopulateCorrectHeader()
        {
            var httpRequestMessage = MyHttpServer
                .WorkingRemotely(BaseAddress)
                .WithHttpRequestMessage(request => request
                    .WithStringContent(StringContent)
                    .WithContentHeader(TestHeader, TestHeaderValue))
                .AndProvideTheHttpRequestMessage();

            Assert.True(httpRequestMessage.Content.Headers.Contains(TestHeader));
            Assert.True(httpRequestMessage.Content.Headers.First(h => h.Key == TestHeader).Value.Contains(TestHeaderValue));
        }

        [Fact]
        public void WithContentHeaderAndMultipleValuesShouldPopulateCorrectHeader()
        {
            var headers = new[]
            {
                TestHeaderValue, AnotherTestHeaderValue
            };

            var httpRequestMessage = MyHttpServer
                .WorkingRemotely(BaseAddress)
                .WithHttpRequestMessage(request => request
                    .WithStringContent(StringContent)
                    .WithContentHeader(TestHeader, headers))
                .AndProvideTheHttpRequestMessage();

            Assert.True(httpRequestMessage.Content.Headers.Contains(TestHeader));
            Assert.True(httpRequestMessage.Content.Headers.First(h => h.Key == TestHeader).Value.Contains(TestHeaderValue));
            Assert.True(httpRequestMessage.Content.Headers.First(h => h.Key == TestHeader).Value.Contains(AnotherTestHeaderValue));
        }

        [Fact]
        public void WithContentHeadersDictionaryShouldPopulateCorrectHeaders()
        {
            var httpRequestMessage = MyHttpServer
                .WorkingRemotely(BaseAddress)
                .WithHttpRequestMessage(request => request
                    .WithStringContent(StringContent)
                    .WithContentHeaders(new Dictionary<string, IEnumerable<string>>
                    {
                        { TestHeader, new[] { TestHeaderValue, AnotherTestHeaderValue } },
                    }))
                .AndProvideTheHttpRequestMessage();

            Assert.True(httpRequestMessage.Content.Headers.Contains(TestHeader));
            Assert.True(httpRequestMessage.Content.Headers.First(h => h.Key == TestHeader).Value.Contains(TestHeaderValue));
            Assert.True(httpRequestMessage.Content.Headers.First(h => h.Key == TestHeader).Value.Contains(AnotherTestHeaderValue));
        }

        [Fact]
        public void WithContentHeadersShouldThrowExpcetionIfNoContentIsPresent()
        {
            var exception = Assert.Throws<InvalidHttpRequestMessageException>(() =>
            {
                MyHttpServer
                    .WorkingRemotely(BaseAddress)
                    .WithHttpRequestMessage(request => request
                        .WithContentHeaders(new Dictionary<string, IEnumerable<string>>
                        {
                            { TestHeader, new[] { TestHeaderValue, AnotherTestHeaderValue } },
                        }));
            });

            Assert.Equal("When building HttpRequestMessage expected content to be initialized and set in order to add content headers.", exception.Message);
        }

        [Fact]
        public void WithMethodStringShouldPopulateCorrectMethod()
        {
            var httpRequestMessage = MyHttpServer
                .WorkingRemotely(BaseAddress)
                .WithHttpRequestMessage(request => request.WithMethod("GET"))
                .AndProvideTheHttpRequestMessage();

            Assert.Equal(HttpMethod.Get, httpRequestMessage.Method);
        }

        [Fact]
        public void WithMethodShouldPopulateCorrectMethod()
        {
            var httpRequestMessage = MyHttpServer
                .WorkingRemotely(BaseAddress)
                .WithHttpRequestMessage(request => request.WithMethod(HttpMethod.Get))
                .AndProvideTheHttpRequestMessage();

            Assert.Equal(HttpMethod.Get, httpRequestMessage.Method);
        }

        [Fact]
        public void WithRequestUriStringShouldPopulateCorrectUri()
        {
            var httpRequestMessage = MyHttpServer
                .WorkingRemotely(BaseAddress)
                .WithHttpRequestMessage(request => request.WithRequestUri(RequestUri))
                .AndProvideTheHttpRequestMessage();

            Assert.Equal(new Uri(RequestUri), httpRequestMessage.RequestUri);
        }

        [Fact]
        public void WithRequestUriShouldPopulateCorrectUri()
        {
            var httpRequestMessage = MyHttpServer
                .WorkingRemotely(BaseAddress)
                .WithHttpRequestMessage(request => request.WithRequestUri(new Uri(RequestUri)))
                .AndProvideTheHttpRequestMessage();

            Assert.Equal(new Uri(RequestUri), httpRequestMessage.RequestUri);
        }

        [Fact]
        public void WithRequestUriBuilderShouldPopulateCorrectUri()
        {
            var httpRequestMessage = MyHttpServer
                .WorkingRemotely(BaseAddress)
                .WithHttpRequestMessage(request
                    => request
                        .WithRequestUri(uri
                            => uri
                                .WithScheme("http")
                                .WithHost("sometest.com")))
                .AndProvideTheHttpRequestMessage();

            Assert.Equal(new Uri(RequestUri).Host, httpRequestMessage.RequestUri.Host);
        }

        [Fact]
        public void WithVersionStringShouldPopulateCorrectVersion()
        {
            var httpRequestMessage = MyHttpServer
                .WorkingRemotely(BaseAddress)
                .WithHttpRequestMessage(request => request.WithVersion(Version))
                .AndProvideTheHttpRequestMessage();

            Assert.Equal(Version, httpRequestMessage.Version.ToString());
        }

        [Fact]
        public void WithVersionIntegersShouldPopulateCorrectVersion()
        {
            var httpRequestMessage = MyHttpServer
                .WorkingRemotely(BaseAddress)
                .WithHttpRequestMessage(request => request.WithVersion(1, 1))
                .AndProvideTheHttpRequestMessage();

            Assert.Equal(Version, httpRequestMessage.Version.ToString());
        }

        [Fact]
        public void WithVersionShouldPopulateCorrectVersion()
        {
            var httpRequestMessage = MyHttpServer
                .WorkingRemotely(BaseAddress)
                .WithHttpRequestMessage(request => request.WithVersion(new Version(1, 1)))
                .AndProvideTheHttpRequestMessage();

            Assert.Equal(Version, httpRequestMessage.Version.ToString());
        }

        [Fact]
        public void WithInvalidVersionShouldThrowInvalidHttpRequestMessageException()
        {
            var exception = Assert.Throws<InvalidHttpRequestMessageException>(() =>
            {
                var httpRequestMessage = MyHttpServer
                    .WorkingRemotely(BaseAddress)
                    .WithHttpRequestMessage(request => request.WithVersion("Invalid"))
                    .AndProvideTheHttpRequestMessage();
            });

            Assert.Equal("When building HttpRequestMessage expected version to be valid version string, but instead received invalid one.", exception.Message);
        }

        [Fact]
        public void AndAlsoShouldBuildCorrectHttpRequestMessage()
        {
            var httpRequestMessage = MyHttpServer
                .WorkingRemotely(BaseAddress)
                .WithHttpRequestMessage(request
                    => request
                        .WithMethod("GET")
                        .AndAlso()
                        .WithRequestUri(RequestUri)
                        .AndAlso()
                        .WithVersion("1.1")
                        .AndAlso()
                        .WithStringContent(StringContent))
                .AndProvideTheHttpRequestMessage();

            Assert.Equal(HttpMethod.Get, httpRequestMessage.Method);
            Assert.Equal(new Uri(RequestUri), httpRequestMessage.RequestUri);
            Assert.Equal(new Version(1, 1), httpRequestMessage.Version);
            Assert.IsAssignableFrom<StringContent>(httpRequestMessage.Content);
            Assert.Equal(StringContent, httpRequestMessage.Content.ReadAsStringAsync().Result);
        }
    }
}
