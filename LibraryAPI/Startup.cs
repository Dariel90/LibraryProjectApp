using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using LibraryAPI.DataContext;
using Microsoft.EntityFrameworkCore;
using LibraryAPI.Data;

namespace LibraryAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureDevelopmentServices(IServiceCollection services)
        {
            services.AddDbContext<WebApiDbContext>(x => {
                x.UseLazyLoadingProxies();
                x.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            ConfigureServices(services);
        }

        public void ConfigureProductionServices(IServiceCollection services)
        {
            services.AddDbContext<WebApiDbContext>(x => {
                x.UseLazyLoadingProxies();
                x.UseMySql(Configuration.GetConnectionString("DefaultConnection"));
            });

            ConfigureServices(services);
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            AddSwagger(services);

            services.AddCors(opt => {
                opt.AddPolicy("CorsPolicy", policy => {
                    policy.AllowAnyHeader()
                        .AllowAnyMethod()
                        .WithOrigins(new[] {"http://localhost:36180", "http://localhost:5000" } );
                });
            });
            services.AddTransient<ILibraryRepository, LibraryRepository>();//Importando repositorio para CRUD del negocio principal
            ;
        }

        private void AddSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                var groupName = "v1";

                options.SwaggerDoc(groupName, new OpenApiInfo
                {
                    Title = $"LibraryAPI {groupName}",
                    Version = groupName,
                    Description = "Library API for the library management",
                    Contact = new OpenApiContact
                    {
                        Name = "Dariel's Company",
                        Email = string.Empty,
                        Url = new Uri("https://foo.com/"),
                    }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "LibraryAPI v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
