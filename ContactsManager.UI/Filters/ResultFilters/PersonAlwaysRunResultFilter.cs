using CrudProject.Filters.ResourceFilters;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CrudProject.Filters.ResultFilters
{
    public class PersonAlwaysRunResultFilter : IAlwaysRunResultFilter
    {
        private readonly ILogger<FeatureDisableResourceFilter> _logger;
        public PersonAlwaysRunResultFilter(ILogger<FeatureDisableResourceFilter> logger)
        {
            _logger = logger;
        }

        public void OnResultExecuted(ResultExecutedContext context)
        {
            if (context.Filters.OfType<SkipFilter>().Any()) return;

            _logger.LogInformation("{FilterName}.{MethodName} - before", nameof(PersonAlwaysRunResultFilter),
            nameof(OnResultExecuting));
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            if (context.Filters.OfType<SkipFilter>().Any()) return;

            _logger.LogInformation("{FilterName}.{MethodName} - before", nameof(PersonAlwaysRunResultFilter),
                nameof(OnResultExecuting));

        }
    }
}
