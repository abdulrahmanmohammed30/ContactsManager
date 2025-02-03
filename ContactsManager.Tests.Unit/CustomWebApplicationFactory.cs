using Entities.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ContactsManager.Tests.Unit
{

    /// <summary>
    /// This piece of code executes after executing the complete program.cs file 
    /// That means all the services that are added here that are by default available 
    /// So it enbludes with the DbContext 
    /// But by default the DbContext is added by SqlServer 
    /// </summary>
    internal class CustomWebApplicationFactory:WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);

            // by default, Asp.net core web projets runs in production environment 
            // unless you change it in the launch settings 
            // but here we are trying to change the enivroment autoamtically with
            // this method 
            builder.UseEnvironment("test");

            // configure your own services that you neeed to execute the test 
            builder.ConfigureServices(Services =>
            {
                // when you add the DbContext, it automatically generates a service called 
                // DbContectOptions and in the following method, we are searching for the same  
                // if the service descriptor was not found, it returns null 
                var descripter = Services.SingleOrDefault(temp => temp.ServiceType == typeof(DbContextOptions<AppDbContext>));

                // remove the service since it was registered to useSQlServer 
                // but we want to register it to use in-memory storage 
                if (descripter != null)
                {
                    Services.Remove(descripter);
                }

                Services.AddDbContext<AppDbContext>(options=>options.UseInMemoryDatabase("DatabaseForTesting"));
            }) ;
        }


    }
}
