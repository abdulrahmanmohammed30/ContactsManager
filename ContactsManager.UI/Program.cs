using CrudProject.Middlewares;
using CrudProject.StartupExtensions;
using Microsoft.AspNetCore.Http.Connections.Features;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

builder.Services.ConfigureServices(builder.Configuration);

var app = builder.Build();
if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    //app.UseExceptionHandler("/error");
    //app.UseExceptionHandler("/error");
    //app.UseExceptionHandlingMiddleware();

}
else
{
    app.UseExceptionHandler("/error");
    app.UseExceptionHandlingMiddleware();
} 

Rotativa.AspNetCore.RotativaConfiguration.Setup("wwwroot", wkhtmltopdfRelativePath: "Rotativa");
app.UseStaticFiles();
app.MapControllers();

app.Run(); 
