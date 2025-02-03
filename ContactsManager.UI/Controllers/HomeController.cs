using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace CrudProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet("/error")]
        public IActionResult Error()
        {
            #region  errorfeature
            //{
            //    // The current working epresssion details 
            //    // A feature is additional details about the current working application 
            //    // and we are accessing the same from the HttpContext 
            //    // from another perspective, whenever an exceptino occurs, no matter 
            //    // whether you have added the cache block or not, the current working 
            //    // exception details are automatically added into these features 
            //    // as IExceptionHandlerPathFeature, so you can access that feature from 
            //    // anywhere in the entire application, no matter whether it's a controller 
            //    // or a a filter or something else 
            //    // for example, you can use the same code to access the current working 
            //    // exception details from the service as well 
            //    IExceptionHandlerPathFeature? exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            //    if (exceptionHandlerPathFeature != null && exceptionHandlerPathFeature.Error != null)
            //    {
            //        ViewBag.ErrorMessage = exceptionHandlerPathFeature.Error.Message;
            //    }
            #endregion

            IExceptionHandlerPathFeature? exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            if (exceptionHandlerPathFeature != null && exceptionHandlerPathFeature.Error != null)
            {
                var path = exceptionHandlerPathFeature.Path;
                var exception = exceptionHandlerPathFeature.Error;

                _logger.LogError(exception, "An exception has occcured at the following path: {Path}", path);

                ViewBag.ErrorMessage = exception.Message;
                ViewBag.Path = path;
            }

            return View();
        }
    }
}
