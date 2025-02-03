using CrudProject.Filters.AuthorizationFilter;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace CrudProject.Filters.ResultFilters
{
    public class TokenResultFilter : IResultFilter
    {
        private readonly ILogger<TokenResultFilter> _logger;

        public TokenResultFilter(ILogger<TokenResultFilter> logger)
        {
            _logger = logger;
        }

        public void OnResultExecuted(ResultExecutedContext context)
        {
            //context.HttpContext.Response.Cookies.Append("Auth-Key", "auth-value");
            _logger.LogInformation("Execution of {FilerName}.{MethodName}",
                nameof(TokenResultFilter), nameof(OnResultExecuted));
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            _logger.LogInformation("Execution of {FilerName}.{MethodName}",
        nameof(TokenResultFilter), nameof(OnResultExecuting));
        }
    }
}
