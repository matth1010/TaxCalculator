using System.Reflection;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using TaxCalculator.API.Models;
using TaxCalculator.BL.Interfaces;
using TaxCalculator.BL.Services;
using TaxCalculator.DAL.Context;
using TaxCalculator.DAL.Interfaces;
using TaxCalculator.DAL.Models;
using TaxCalculator.DAL.Repositories;
using Microsoft.AspNetCore.Diagnostics;
using Serilog.Extensions.Logging.File;

namespace TaxCalculator.API
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<TaxCalculatorDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddScoped<ITaxRateRepository, TaxRateRepository>();
            services.AddScoped<ITaxRecordRepository, TaxRecordRepository>();

            services.AddScoped<ITaxCalculatorService, TaxCalculatorService>();

            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<TaxCalculationRequest, TaxRecord>();
                config.CreateMap<TaxRecord, TaxCalculationResponse>();
                config.CreateMap<TaxRate, TaxRateResponse>();
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            services.AddLogging(builder =>
            {
                builder.AddConsole();
                builder.AddDebug();
                builder.AddFile("logs/log.txt");
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Tax Calculator API", Version = "v1" });

                string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddFile("logs/log.txt");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(errorApp =>
                {
                    errorApp.Run(async context =>
                    {
                        context.Response.StatusCode = 500;
                        context.Response.ContentType = "text/plain";

                        await context.Response.WriteAsync("An error occurred. Please try again later.");

                        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                        logger.LogError($"An exception occurred: {exceptionHandlerPathFeature.Error}");
                    });
                });
            }

            app.UseCors();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Tax Calculator API");
            });

            app.UseMiddleware<Middleware.ExceptionHandlerMiddleware>();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}