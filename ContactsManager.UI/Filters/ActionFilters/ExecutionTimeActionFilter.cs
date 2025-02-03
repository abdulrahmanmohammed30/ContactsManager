using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace CrudProject.Filters.ActionFilters
{
    public class ExecutionTimeActionFilterFactoryAttribute : Attribute, IFilterFactory
    {
        public bool IsReusable => false;
        public int Order { get; set; }
        
        public ExecutionTimeActionFilterFactoryAttribute(int order)
        {
            Order = order;
        }
        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            var instance = serviceProvider.GetService<ExecutionTimeActionFilter>();
            instance.Order = Order;
            return instance;
        }
    }

    public class ExecutionTimeActionFilter:IActionFilter, IOrderedFilter
    {
        private readonly ILogger<ExecutionTimeActionFilter> _logger;
        private long _startTime;
        public int Order { get; set; }

        public ExecutionTimeActionFilter(ILogger<ExecutionTimeActionFilter> logger)
        {
            _logger = logger;
        }


        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation($"Action completed in {Stopwatch.GetElapsedTime(_startTime)}ms");
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _startTime = Stopwatch.GetTimestamp();
            _logger.LogInformation("Action started");
        }
    }
}
