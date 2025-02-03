using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CrudProject.Filters.AuthorizationFilter
{
    class User
    {
        public int Id { get; set; }
        public string IsRole { get; set; }
        public bool IsAuthenticated { get; set; }
    }

    public class CustomAuthorizationFilter : IAsyncAuthorizationFilter
    {
        private readonly ILogger<CustomAuthorizationFilter> _logger;
        public CustomAuthorizationFilter(ILogger<CustomAuthorizationFilter> logger)
        {
            _logger = logger;
        }

        public Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (context.HttpContext.Items.ContainsKey("User")
                && context.HttpContext.Items["User"] is User user 
                && user.IsRole == "Admin" 
                && user.IsAuthenticated == true)
            {
                return Task.CompletedTask;
            }

            _logger.LogWarning("Unauthorized access attempt");
            context.Result = new UnauthorizedResult();
            return Task.CompletedTask;
        }
    }
}
