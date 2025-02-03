using CrudProject.Filters.ActionFilters;
using CrudProject.Filters.AuthorizationFilter;
using CrudProject.Filters.ExceptionFilters;
using Microsoft.AspNetCore.Mvc;

namespace CrudProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HeavyController : ControllerBase
    {
        [HttpGet("heavy-action")]
        [TypeFilter(typeof(CustomAuthorizationFilter))]
        [TypeFilter(typeof(ExecutionTimeActionFilter),Arguments =new object[] {-30}, Order = 1)]
        [TypeFilter(typeof(HandleHeavyActionExceptionFilter))]
        public async Task<IActionResult> HeavyAction()
        {
            // Simulate data retrieval from in-memory storage
            var data = Enumerable.Range(1, 1000).Select(i => $"Item {i}").ToList();
            await Task.Delay(2000); // Simulating heavy processing

            // Data validation
            if (data == null || !data.Any())
                throw new Exception("Data retrieval failed");

            return Ok(data);
        }
    }
}
