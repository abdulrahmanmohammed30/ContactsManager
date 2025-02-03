using CrudProject.Filters.ActionFilters;
using CrudProject.Filters.ExceptionFilters;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace RealWorldApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(ILogger<OrdersController> logger)
        {
            _logger = logger;
        }

        [HttpPost("create")]
        //[TypeFilter(typeof(CreateOrderValidationFilter))]
        //[ExecutionTimeActionFilterFactory]
        [TypeFilter(typeof(CreateOrderExceptionFilter))]
        public IActionResult CreateOrder([FromBody] Order order)
        {
            Order.Orders.Add(order);
            return CreatedAtAction(nameof(CreateOrder), new { id = order.Id }, order);
        }
    }

    public class Order
    {
        public static readonly List<Order> Orders = new()
        {
            new Order { Id = 1, CustomerId = 101, Items = new List<string> { "Laptop", "Mouse" }, TotalAmount = 1200 },
            new Order { Id = 2, CustomerId = 102, Items = new List<string> { "Phone", "Headphones" }, TotalAmount = 900 },
            new Order { Id = 3, CustomerId = 103, Items = new List<string> { "Monitor", "Keyboard" }, TotalAmount = 500 }
        };

        [Required]
        public int Id { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "Order must contain at least one item.")]
        public List<string> Items { get; set; }

        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Total amount must be greater than zero.")]
        public decimal TotalAmount { get; set; }
    }
}
