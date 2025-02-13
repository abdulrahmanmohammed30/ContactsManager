﻿using Serilog;
namespace CrudProject.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class CustomExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly  ILogger<CustomExceptionHandlingMiddleware> _logger;
       //private readonly IDiagnosticContext _diagnosticContext;

        
                
        public CustomExceptionHandlingMiddleware(RequestDelegate next, ILogger<CustomExceptionHandlingMiddleware> logger)
            /*,IDiagnosticContext diagnosticContext)*/
        {
            _next = next;
            _logger = logger;
            //_diagnosticContext = diagnosticContext;

        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }                                                                                                   
            catch (Exception ex)
            {

                if (ex.InnerException != null)
                {
                    _logger.LogError("{ExceptionType} {ExceptionMessage}",
                    ex.InnerException.GetType().ToString(),
                    ex.InnerException.Message);

                }
                 else
                {
                    _logger.LogError("{ExceptionType} {ExceptionMessage}",
                    ex.GetType().ToString(),
                    ex.Message);
                }
                //await httpContext.Response.WriteAsync("Error occurred");
                throw;
            }
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ExceptionHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionHandlingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomExceptionHandlingMiddleware>();
        }
    }
}
