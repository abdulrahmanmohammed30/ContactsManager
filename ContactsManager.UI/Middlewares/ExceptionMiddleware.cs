    using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CrudProject.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next; 
        }

        public async Task Invoke(HttpContext httpContext)
        {

            try
            {
                await _next(httpContext);
            } catch (Exception ex)
            {

            }
            httpContext.Response.StatusCode = 500;
            await httpContext.Response.WriteAsync("An error has occured on the server");
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
