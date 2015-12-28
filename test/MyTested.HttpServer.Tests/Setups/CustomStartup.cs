#if DNX451

namespace MyTested.HttpServer.Tests.Setups
{
    using Owin;
    using System;

    public class CustomStartup
    {
        public void Configuration(IAppBuilder app)
        {
            app.Run(context =>
            {
                if (context.Request.Method == "GET" && context.Request.Uri.OriginalString.EndsWith("/headers"))
                {
                    context.Response.StatusCode = 200;
                    return context.Response.WriteAsync("OK!");
                }

                context.Response.StatusCode = 404;
                return context.Response.WriteAsync("Not found!");
            });
        }
    }
}
#endif