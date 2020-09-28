using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;

namespace WebApp
{
  public class Startup
  {

    public Startup(IConfiguration config)
    {
      Configuration = config;
    }

    public IConfiguration Configuration { get; set; }

    public void ConfigureServices(IServiceCollection services)
    {
      services.AddDbContext<DataContext>(opts =>
      {
        opts.UseSqlServer(Configuration[
            "ConnectionStrings:ProductConnection"]);
        // opts.EnableSensitiveDataLogging(true);
      });
      services.AddControllers();
      /* services.Configure<JsonOptions>(opts =>
      {
        opts.JsonSerializerOptions.IgnoreNullValues = true;
      }); */
      // services.AddControllers().AddNewtonsoftJson();
      services.Configure<MvcNewtonsoftJsonOptions>(opts =>
      {
        opts.SerializerSettings.NullValueHandling
          = Newtonsoft.Json.NullValueHandling.Ignore;
      });
      services.AddControllers().AddNewtonsoftJson().AddXmlSerializerFormatters();
      services.Configure<MvcOptions>(opts =>
      {
        opts.RespectBrowserAcceptHeader = true;
        opts.ReturnHttpNotAcceptable = true;
      });
      services.AddSwaggerGen(options =>
      {
        options.SwaggerDoc("v1",
        new OpenApiInfo { Title = "WebApp", Version = "v1" });
      });
    }

    public void Configure(IApplicationBuilder app, DataContext context)
    {

      app.UseDeveloperExceptionPage();
      app.UseStaticFiles();
      app.UseRouting();
      app.UseMiddleware<TestMiddleware>();
      app.UseEndpoints(endpoints =>
      {
        endpoints.MapGet("/", async context =>
        {
          await context.Response.WriteAsync("Hello World!");
        });
        endpoints.MapControllers();
      });
      app.UseSwagger();
      app.UseSwaggerUI(options =>
      {
        options.SwaggerEndpoint("/swagger/v1/swagger.json",
        "WebApp");
      });

      SeedData.SeedDatabase(context);

    }
  }
}
