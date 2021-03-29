using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using EmployeeUC.WebAPI.Context;
using EmployeeUC.WebAPI.PipelineBehaviours;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using FluentValidation;
using MediatR;
using MediatR.Pipeline;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace EmployeeUC.WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddDbContext<ApplicationContext>(options =>
    options.UseSqlServer(
        Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly(typeof(ApplicationContext).Assembly.FullName)));
            #region Swagger
            services.AddSwaggerGen(c =>
            {
                c.IncludeXmlComments(string.Format(@"{0}\EmployeeUC.WebAPI.xml", System.AppDomain.CurrentDomain.BaseDirectory));
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "EmployeeUC.WebAPI",
                });

            });
            #endregion
            services.AddScoped<IApplicationContext>(provider => provider.GetService<ApplicationContext>());

            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddControllers();

            services.AddValidatorsFromAssembly(typeof(Startup).Assembly);
            //Used for logging
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            #region Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "EmployeeUC.WebAPI");
            });
            #endregion

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            //Global excptional handling
            app.UseExceptionHandler(
             options =>
             {
                options.Run(
                async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "text/html";
                    var ex = context.Features.Get<IExceptionHandlerFeature>();
                    if (ex != null)
                    {
                        var err = $"<h1>Error: {ex.Error.Message}</h1>{ex.Error.StackTrace }";
                        await context.Response.WriteAsync(err).ConfigureAwait(false);
                    }
                });
             }
             );

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
