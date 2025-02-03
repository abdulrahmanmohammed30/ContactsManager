using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CrudProject.Filters.ResourceFilters
{
    public class FeatureDisableResourceFilter : IResourceFilter
    {
        private readonly ILogger<FeatureDisableResourceFilter> _logger;
        private readonly bool _IsDisabled;
        public FeatureDisableResourceFilter(ILogger<FeatureDisableResourceFilter> logger, bool IsDisabled = true)
        {
            _logger = logger;
            _IsDisabled = IsDisabled;
        }

        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            _logger.LogInformation("{FilterName}.{MethodName} - after", nameof(FeatureDisableResourceFilter),
                nameof(OnResourceExecuted));
        }

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            
            _logger.LogInformation("{FilterName}.{MethodName} - before", nameof(FeatureDisableResourceFilter),
                nameof(OnResourceExecuting));

            //if (_IsDisabled == true)
            //    context.Result = new JsonResult(new {data= Enumerable.Range(0,50).Select(x=>new { Id= x + 1})}) ;
        }
    }
}

