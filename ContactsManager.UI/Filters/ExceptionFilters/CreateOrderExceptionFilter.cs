using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CrudProject.Filters.ExceptionFilters
{
    public class CreateOrderExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<CreateOrderExceptionFilter> _logger;

        public CreateOrderExceptionFilter(ILogger<CreateOrderExceptionFilter> logger)
        {
            _logger = logger;   
        }

        public void OnException(ExceptionContext context)
        {
            _logger.LogError(context.Exception, "An unexpected error occurred while creating order.");

            context.Result = new ObjectResult("An unexpected error occurred while creating order.") { StatusCode = 500 };

            context.ExceptionHandled = true;
        }
    }
}
