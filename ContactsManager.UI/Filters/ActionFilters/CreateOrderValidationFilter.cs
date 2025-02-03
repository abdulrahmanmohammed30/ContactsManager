using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.ComponentModel.DataAnnotations;
using RealWorldApp.Controllers;

namespace CrudProject.Filters.ActionFilters
{
    public class CreateOrderValidationFilter : IActionFilter
    {
        private readonly ILogger<CreateOrderValidationFilter> _logger;

        public CreateOrderValidationFilter(ILogger<CreateOrderValidationFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ActionArguments.TryGetValue("order", out var orderObj))
                throw new Exception("order propertry was not found");

            if (orderObj is null)
            {
                _logger.LogError("Order data is null.");
                context.Result = new BadRequestObjectResult("Order data cannot be null.");
                return;
            }

            var order = orderObj as Order;

            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(order);
            if (!Validator.TryValidateObject(order, validationContext, validationResults, true))
            {
                _logger.LogWarning("Order validation failed: {Errors}", string.Join(", ", validationResults.Select(v => v.ErrorMessage)));
                context.Result = new BadRequestObjectResult(validationResults);
            }


            if (Order.Orders.Any(o => o.Id == order.Id))
            {
                _logger.LogError("Order with ID {OrderId} already exists.", order.Id);
                context.Result = new ConflictObjectResult("Order with the same ID already exists.");
            }
        }
    }
}
