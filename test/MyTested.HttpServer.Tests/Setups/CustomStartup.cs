#if NET451

namespace MyTested.HttpServer.Tests.Setups
{
    using Owin;
    using System.Linq;

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

                if (context.Request.Method == "GET" && context.Request.Uri.OriginalString.EndsWith("/cookies"))
                {
                    context.Response.StatusCode = 200;
                    return context.Response.WriteAsync(string.Join("!", context.Request.Cookies.Select(c => $"{c.Key}+{c.Value}")));
                }

                context.Response.StatusCode = 404;
                return context.Response.WriteAsync("Not found!");
            });
        }
    }
}
#endif