<h1><img src="https://raw.githubusercontent.com/ivaylokenov/MyTested.HttpServer/master/documentation/logo.png" align="left" alt="MyTested.HttpServer" width="100">&nbsp; MyTested.HttpServer - Fluent testing<br />&nbsp; for remote servers</h1>
====================================

MyTested.HttpServer is unit testing library providing easy fluent interface to test remote HTTP servers. It is testing framework agnostic, so you can combine it with the testing library of your choice (e.g. NUnit, xUnit, etc.). MyTested.HttpServer can be easily used with frameworks like [AngleSharp](https://github.com/AngleSharp/AngleSharp) or [Json.NET](https://github.com/JamesNK/Newtonsoft.Json) in order to create HTML or JSON test assertions over a remote (or localhost) web server.

## Documentation

Please see the [documentation](https://github.com/ivaylokenov/MyTested.HttpServer/tree/master/documentation) for full list of available features. Everything listed there is fully covered by unit tests.

## Installation

MyTested.HttpServer is designed to work with both .NET 4.5+ and .NET Core. You can install it library using NuGet into your Test class project.

    Install-Package MyTested.HttpServer

After the downloading is complete, just add `using MyTested.HttpServer;` and you are ready to test in the most elegant and developer friendly way.
	
    using MyTested.HttpServer;
	
For other interesting packages check out:

 - [MyTested.AspNetCore.Mvc](https://github.com/ivaylokenov/MyTested.AspNetCore.Mvc) - fluent testing framework for ASP.NET Core MVC
 - [MyTested.WebApi](https://github.com/ivaylokenov/MyTested.WebApi) - fluent testing framework for ASP.NET Web API 2
 - [AspNet.Mvc.TypedRouting](https://github.com/ivaylokenov/AspNet.Mvc.TypedRouting) - typed routing and link generation for ASP.NET Core MVC
 - [ASP.NET MVC 5 Lambda Expression Helpers](https://github.com/ivaylokenov/ASP.NET-MVC-Lambda-Expression-Helpers) - typed expression based link generation for ASP.NET MVC 5
	
## How to use

Make sure to check out [the documentation](https://github.com/ivaylokenov/MyTested.HttpServer/tree/master/documentation) for full list of available features.
You can also check out [the provided samples](https://github.com/ivaylokenov/MyTested.HttpServer/tree/master/samples) for real-life server testing.

Basically you can create a test case by using the fluent API the library provides. You are given a static `MyHttpServer` class from which all assertions can be easily configured.

```c#
namespace MyApp.Tests
{
	using MyTested.HttpServer;
	
	using NUnit.Framework;

    [TestFixture]
    public class MyServerShould
    {
		private const string BaseAddress = "http://mytestedasp.net";
	
		[TestFixtureSetUp]
		public void Init()
		{
			MyHttpServer.IsLocatedAt(BaseAddress);
		}
	
        [Test]
        public void ReturnStatusCodeOk()
        {
            MyHttpServer
                .WorkingRemotely()
                .WithHttpRequestMessage(req => req.WithMethod(HttpMethod.Get))
                .ShouldReturnHttpResponseMessage()
                .WithStatusCode(HttpStatusCode.OK);
        }
	}
}
```

The example uses NUnit but you can use whatever testing framework you want.
Basically, MyTested.HttpServer throws an unhandled exception if the assertion does not pass and the test fails.

## License

Code by Ivaylo Kenov. Copyright 2015-2016 Ivaylo Kenov.

This package has MIT license. Refer to the [LICENSE](https://github.com/ivaylokenov/MyTested.HttpServer/blob/master/LICENSE) for detailed information.
 
## Any questions, comments or additions?

If you have a feature request or bug report, leave an issue on the [issues page](https://github.com/ivaylokenov/MyTested.HttpServer/issues) or send a [pull request](https://github.com/ivaylokenov/MyTested.HttpServer/pulls). For general questions and comments, use the [StackOverflow](http://stackoverflow.com/) forum.
