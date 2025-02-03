using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CrudProject.Filters.AuthorizationFilter
{
    public class CookieAuthorizationFilter : IAuthorizationFilter
    {
        private readonly ILogger<CookieAuthorizationFilter> _logger;

        public CookieAuthorizationFilter(ILogger<CookieAuthorizationFilter> logger)
        {
            _logger = logger;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            _logger.LogInformation("Execution of {FilerName}.{MethodName}",
                nameof(CookieAuthorizationFilter), nameof(OnAuthorization));

            _logger.LogInformation("Short-curcuiting in the Authorization Filter");

            context.Result = new StatusCodeResult(StatusCodes.Status401Unauthorized);
        }
    }
}
