using CrudProject.Filters.ResourceFilters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CrudProject.Filters.ExceptionFilters
{
    public class HandleExceptionFilter : IExceptionFilter
    {

        private readonly ILogger<FeatureDisableResourceFilter> _logger;
        private readonly IHostEnvironment _hostEnvironment;
        public HandleExceptionFilter(ILogger<FeatureDisableResourceFilter> logger, IHostEnvironment hostEnvironment)
        { 
            _logger = logger;
            _hostEnvironment = hostEnvironment;
         }

        public void OnException(ExceptionContext context)
        {
            _logger.LogError("Exception filter {FilterName}.{MethodName}\n{ExceptionType}\n{ExceptionMesage}",
                nameof(HandleExceptionFilter), 
                nameof(OnException),
                context.Exception.GetType().ToString(), 
                nameof(FeatureDisableResourceFilter));

            if(_hostEnvironment.IsDevelopment())
            context.Result = new JsonResult(context.Exception.Message)
            {
                StatusCode = 500
            };

                //context.ExceptionHandled = true;
        } 
    }
}
