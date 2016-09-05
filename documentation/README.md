<h1><img src="https://raw.githubusercontent.com/ivaylokenov/MyTested.HttpServer/master/documentation/logo.png" align="left" alt="MyTested.HttpServer" width="100">&nbsp; MyTested.HttpServer - Fluent testing<br />&nbsp; for remote servers</h1>
====================================

## Full list of available features

### Table of contents

 - [Server configuration](#server-configuration)
 - [HTTP request message](#http-request-message)
 - [HTTP response message](#http-response-message)
 - [Helper classes and methods](#helper-classes-and-methods)

### Server configuration

You have the option to configure global server base address to be used across all test cases. Additionally, you may specify default headers, which will be sent on every request.

```c#
// sets a test case by providing the base address directly
MyHttpServer
	.WorkingRemotely("http://mytestedasp.net")
	.WithHttpRequestMessage(req => req.WithMethod(HttpMethod.Get))
	.ShouldReturnHttpResponseMessage()
	.WithStatusCode(HttpStatusCode.OK);
	
// setting the same address on every test case will cause a lot of repeating code
// so it is better to set it globally for all test cases
MyHttpServer.IsLocatedAt("http://mytestedasp.net");

// additionally HTTP client handler options can be specified
MyHttpServer
	.IsLocatedAt("http://mytestedasp.net", handler => handler.AllowAutoRedirect = false);

// additionally default headers can be set and they will be sent on every request
MyHttpServer
	.IsLocatedAt("http://mytestedasp.net")
	.WithDefaultRequestHeader("Authorization", "MyToken");

// then each test case will use the globally configured address and headers
MyHttpServer
	.WorkingRemotely()
	.WithHttpRequestMessage(req => req.WithMethod(HttpMethod.Get))
	.ShouldReturnHttpResponseMessage()
	.WithStatusCode(HttpStatusCode.OK);

// additionally the test builder can be saved to a variable saving the repeating calls
var httpServer = MyHttpServer
	.IsLocatedAt("http://mytestedasp.net")
	.WithDefaultRequestHeader("Authorization", "MyToken");

// and then write only the test specific details
httpServer
	.WithHttpRequestMessage(req => req.WithMethod(HttpMethod.Get))
	.ShouldReturnHttpResponseMessage()
	.WithStatusCode(HttpStatusCode.OK);
```

[To top](#table-of-contents)
  
### HTTP request message

You can mock the HttpRequestMessage class to suit your testing needs:

```c#
// adding HttpContent to the request message
httpServer
	.WithHttpRequestMessage(request => request
		.WithContent(someContent));
		
// adding StreamContent to the request message
httpServer
	.WithHttpRequestMessage(request => request
		.WithStreamContent(someStreamContent));
		
// adding ByteArrayContent to the request message
httpServer
	.WithHttpRequestMessage(request => request
		.WithByteArrayContent(someByteArrayContent));
		
// adding form URL encoded content to the request message
// by collection of key-value pairs
httpServer
	.WithHttpRequestMessage(request => request
		.WithFormUrlEncodedContent(someKeyValuePairCollection));
		
// adding form URL encoded content to the request message
// by query string
httpServer
	.WithHttpRequestMessage(request => request
		.WithFormUrlEncodedContent("First=FirstValue&Second=SecondValue"));
		
// adding JSON content to the request message
httpServer
	.WithHttpRequestMessage(request => request
		.WithJsonContent(someJsonString));
		
// adding StringContent to the request message
// with text/plain media type
httpServer
	.WithHttpRequestMessage(request => request
		.WithStringContent(someStringContent));
		
// adding StringContent to the request message
// with custom media type
httpServer
	.WithHttpRequestMessage(request => request
		.WithStringContent(someStringContent, MediaType.ApplicationXml));
		
// adding StringContent to the request message
// with custom encoding
httpServer
	.WithHttpRequestMessage(request => request
		.WithStringContent(someStringContent, Encoding.UTF8));
		
// adding StringContent to the request message
// with custom encoding and media type
httpServer
	.WithHttpRequestMessage(request => request
		.WithStringContent(someStringContent, Encoding.UTF8, MediaType.ApplicationXml));
		
// adding custom header to the request message
httpServer
	.WithHttpRequestMessage(request => request
		.WithHeader(HttpHeaderHeader.Accept, MediaType.TextHtml));
		
// adding custom header with multiple values to the request message
httpServer
	.WithHttpRequestMessage(request => request
		.WithHeader("SomeHeader", new[] { "SomeHeaderValue", "AnotherHeaderValue" }));
		
// adding custom headers provided as dictionary to the request message
httpServer
	.WithHttpRequestMessage(request => request
		.WithHeaders(someDictionaryWithHeaders));
		
// adding custom content header to the request message
// * adding content headers requires content to be initialized and set
httpServer
	.WithHttpRequestMessage(request => request
		.WithContentHeader(HttpContentHeader.ContentType, MediaType.ApplicationJson));
		
// adding custom content header with multiple values to the request message
// * adding content headers requires content to be initialized and set
httpServer
	.WithHttpRequestMessage(request => request
		.WithContentHeader("SomeContentHeader", new[] { "SomeContentHeaderValue", "AnotherContentHeaderValue" }));
		
// adding custom content headers provided as dictionary to the request message
// * adding content headers requires content to be initialized and set
httpServer
	.WithHttpRequestMessage(request => request
		.WithContentHeaders(someDictionaryWithContentHeaders));
		
// adding HTTP method as string to the request message
httpServer
	.WithHttpRequestMessage(request => request
		.WithMethod("GET"));
		
// adding strongly-typed HTTP method to the request message
httpServer
	.WithHttpRequestMessage(request => request
		.WithMethod(HttpMethod.Get));
		
// adding request URI as string to the request message
httpServer
	.WithHttpRequestMessage(request => request
		.WithRequestUri("RequestUri"));
		
// adding request URI as Uri class to the request message
httpServer
	.WithHttpRequestMessage(request => request
		.WithRequestUri(someUri));
		
// adding request URI with Uri builder to the request message
httpServer
	.WithHttpRequestMessage(request => request
		.WithRequestUri(uri => uri
			.WithHost("somehost.com")
			.AndAlso() // AndAlso is not necessary
			.WithAbsolutePath("/someuri/1")
			.AndAlso()
			.WithPort(80)
			.AndAlso()
			.WithScheme("http")
			.AndAlso()
			.WithFragment(string.Empty)
			.AndAlso()
			.WithQuery("?query=Test")));
			
// adding request version as string to the request message
httpServer
	.WithHttpRequestMessage(request => request
		.WithVersion("1.1"));
		
// adding request version using version numbers to the request message
httpServer
	.WithHttpRequestMessage(request => request
		.WithVersion(1, 1));
		
// adding request version using Version class to the request message
httpServer
	.WithHttpRequestMessage(request => request
		.WithVersion(someVersion));
		
// adding different options to the request message
httpServer
	.WithHttpRequestMessage(request => request
		.WithMethod(HttpMethod.Get)
		.AndAlso() // AndAlso is not necessary
		.WithRequestUri(someUri)
		.AndAlso()
		.WithVersion(someVersion)
		.AndAlso()
		.WithStringContent(someStringContent));
		
// adding request message by providing HttpRequestMessage instance
httpServer
	.WithHttpRequestMessage(httpRequestMessage);
```

[To top](#table-of-contents)

### HTTP response message

```c#
// tests whether the server returns HttpResponseMessage
// with content as expected string
httpServer
	.WithHttpRequestMessage(request)
	.ShouldReturnHttpResponseMessage()
	.WithContent("SomeString");

// tests whether the server returns HttpResponseMessage
// with content passing specific assertions
httpServer
	.WithHttpRequestMessage(request)
	.ShouldReturnHttpResponseMessage()
	.WithContent(content => 
	{
		// can be combined with libraries like AngleSharp for easier assertions
		Assert.Contains("<h1>Hello!</h1>", content);
	});
	
// tests whether the server returns HttpResponseMessage
// with content passing a predicate
httpServer
	.WithHttpRequestMessage(request)
	.ShouldReturnHttpResponseMessage()
	.WithContent(content => content.StartsWith("<html>"));
	
// tests whether the server returns HttpResponseMessage
// containing header with specific name
httpServer
	.WithHttpRequestMessage(request)
	.ShouldReturnHttpResponseMessage()
	.ContainingHeader("SomeHeader");
	
// tests whether the server returns HttpResponseMessage
// containing header with specific name and value
httpServer
	.WithHttpRequestMessage(request)
	.ShouldReturnHttpResponseMessage()
	.ContainingHeader("SomeHeader", "SomeValue");
	
// tests whether the server returns HttpResponseMessage
// containing header with specific name and values
httpServer
	.WithHttpRequestMessage(request)
	.ShouldReturnHttpResponseMessage()
	.ContainingHeader("SomeHeader", new[] { "SomeHeaderValue", "AnotherHeaderValue" });
		
// tests whether the server returns HttpResponseMessage
// containing content header with specific name
httpServer
	.WithHttpRequestMessage(request)
	.ShouldReturnHttpResponseMessage()
	.ContainingContentHeader("SomeContentHeader");
	
// tests whether the server returns HttpResponseMessage
// containing content header with specific name and value
httpServer
	.WithHttpRequestMessage(request)
	.ShouldReturnHttpResponseMessage()
	.ContainingContentHeader("SomeContentHeader", "SomeContentValue");
	
// tests whether the server returns HttpResponseMessage
// containing content header with specific name and values
httpServer
	.WithHttpRequestMessage(request)
	.ShouldReturnHttpResponseMessage()
	.ContainingContentHeader("SomeContentHeader", new[] { "SomeContentHeaderValue", "AnotherContentHeaderValue" });

// tests whether the server returns HttpResponseMessage
// with specific status code
httpServer
	.WithHttpRequestMessage(request)
	.ShouldReturnHttpResponseMessage()
	.WithStatusCode(HttpStatusCode.OK);
	
// tests whether the server returns HttpResponseMessage
// with success status code (from 200 to 299)
httpServer
	.WithHttpRequestMessage(request)
	.ShouldReturnHttpResponseMessage()
	.WithSuccessStatusCode();
	
// tests whether the server returns HttpResponseMessage
// with HTTP version as string
httpServer
	.WithHttpRequestMessage(request)
	.ShouldReturnHttpResponseMessage()
	.WithVersion("1.1");
	
// tests whether the server returns HttpResponseMessage
// with HTTP version as numbers
httpServer
	.WithHttpRequestMessage(request)
	.ShouldReturnHttpResponseMessage()
	.WithVersion(1, 1);
	
// tests whether the server returns HttpResponseMessage
// with HTTP version as Version class
httpServer
	.WithHttpRequestMessage(request)
	.ShouldReturnHttpResponseMessage()
	.WithVersion(someVersion);
	
// tests whether the response time passes a predicate
httpServer
	.WithHttpRequestMessage(request)
	.ShouldReturnHttpResponseMessage()
	.WithResponseTime(responseTime => responseTime.TotalMilliseconds < 100);
	
// tests whether the response time passes specific assertions
httpServer
	.WithHttpRequestMessage(request)
	.ShouldReturnHttpResponseMessage()
	.WithResponseTime(responseTime =>
	{
		// do whatever you want with the response time - log it, aggregate it
		// * touch it, bring it, pay it, watch it, turn it, leave it, stop, format it
		Assert.IsTrue(responseTime.TotalMilliseconds < 100);
	});
	
// tests whether the server returns HttpResponseMessage
// with different type of properties by using AndAlso()
httpServer
	.WithHttpRequestMessage(request)
	.ShouldReturnHttpResponseMessage()
	.WithSuccessStatusCode()
	.AndAlso() // AndAlso is not necessary
	.ContainingHeader("SomeHeader")
	.AndAlso()
	.WithContent("Hello!");
```

[To top](#table-of-contents)

### Helper classes and methods

The library gives you helper classes for common magic strings:

```c#
// MediaType class contains common media type strings
httpServer
	.WithHttpRequestMessage(request => request
		.WithHeader(HttpHeader.Accept, MediaType.ApplicationJson)); // represents "application/json"
	
// HttpHeader class contains common HTTP header names
httpServer
	.WithHttpRequestMessage(request => request
		.WithHeader(HttpHeader.Accept, MediaType.ApplicationJson)); // represents "Accept" HTTP header
	
// HttpContentHeader class contains common HTTP content header names
httpServer
	.WithHttpRequestMessage(request => request
		.WithContentHeader(HttpContentHeader.ContentType, MediaType.ApplicationJson)) // represents "Content-Type" HTTP header
```

Additionally, you can get different test specific objects used in the test case where applicable by using AndProvide... methods. Useful for additional custom test assertions:

```c#
// get the HTTP client used in the testing
var client = httpServer
	.WithHttpRequestMessage(request)
	.AndProvideTheHttpClient();

// get the HTTP request message used in the testing
var httpRequest = httpServer
	.WithHttpRequestMessage(request)
	.AndProvideTheHttpRequestMessage();
	
// get the HTTP handler used to send the request
var handler = httpServer
	.WithHttpRequestMessage(request)
	.ShouldReturnHttpResponseMessage()
	.AndProvideTheHandler();
	
// get the HTTP response message
var httpResponse = httpServer
	.WithHttpRequestMessage(request)
	.ShouldReturnHttpResponseMessage()
	.AndProvideTheHttpResponseMessage();
	
// get the response time
var responseTime = httpServer
	.WithHttpRequestMessage(request)
	.ShouldReturnHttpResponseMessage()
	.AndProvideTheResponseTime();
```

[To top](#table-of-contents)