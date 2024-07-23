
namespace Project.API.Middlewares
{
    public class BufferingBodyMiddleware : IMiddleware
    {
        // Enable body to read multiple times !
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            context.Request.EnableBuffering();
            await next(context);
        }
    }
}
