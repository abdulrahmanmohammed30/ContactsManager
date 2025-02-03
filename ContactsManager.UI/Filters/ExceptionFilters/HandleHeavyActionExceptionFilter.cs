using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CrudProject.Filters.ExceptionFilters
{
    public class HandleHeavyActionExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<HandleHeavyActionExceptionFilter> _logger;

        public HandleHeavyActionExceptionFilter(ILogger<HandleHeavyActionExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            _logger.LogError("Data retrieval failed");
            context.Result=new BadRequestObjectResult("No data available");
        }
    }
}
