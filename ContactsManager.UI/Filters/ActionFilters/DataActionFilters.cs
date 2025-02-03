using CrudProject.Filters.ResourceFilters;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CrudProject.Filters.ActionFilters
{
    public class DataActionFilters : IActionFilter
    {
        private readonly ILogger<FeatureDisableResourceFilter> _logger;
        public DataActionFilters(ILogger<FeatureDisableResourceFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation("{FilterName}.{MethodName} - before", nameof(DataActionFilters),
 nameof(OnActionExecuted));

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation("{FilterName}.{MethodName} - before", nameof(DataActionFilters),
            nameof(OnActionExecuted));
        }
    }
}
