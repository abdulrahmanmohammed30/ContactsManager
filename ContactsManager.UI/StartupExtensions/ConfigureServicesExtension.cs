using ContactsManager.Core.ServiceContract;
using ContactsManager.Core.Services;
using ContactsManager.Infrastructure.Repositories;
using CrudProject.Filters.ActionFilters;
using CrudProject.Filters.ExceptionFilters;
using Entities.Context;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;


namespace CrudProject.StartupExtensions
{
    public static class ConfigureservicesExtension
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configurations) {
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer("Server=WARRIOR; Database=ContactsDb; Integrated Security=True; Trust Server Certificate=True"));
            services.AddScoped<IPersonRepository, PersonRepository>();
            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<ICountryGetterService, CountryGetterService>();          
            services.AddScoped<ICountryAdderService, CountryAdderService>();
           //services.AddScoped<IPersonGetterService, PersonGetterService>();
            services.AddScoped<PersonGetterService>();
            services.AddScoped<IPersonSorterService, PersonSorterService>();
            services.AddScoped<IPersonAdderService, PersonAdderService>();
            services.AddScoped<IPersonUpdaterService, PersonUpdaterService>();
            services.AddScoped<IPersonDeleterService, PersonDeleterService>();
            services.AddScoped<IPersonGetterService, PersonGetterServiceWithFewExcelFields>();
            services.AddScoped<HandleExceptionFilter>();
            
            services.AddScoped<ExecutionTimeActionFilter>();
            return services;
        }
    }
}
