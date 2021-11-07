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
using BurgerShop.Models;

namespace BurgerShop
{
    public class Startup
    {
        public Startup(IConfiguration config) {
            Configuration = config;
        }

        public IConfiguration Configuration {get; set;}

        public void ConfigureServices(IServiceCollection services) {
            services.AddDbContext<DataContext>(opts => {
                opts.UseMySql(Configuration["ConnectionStrings:BurgerShopConnection"]);
                opts.UseSnakeCaseNamingConvention();
            });
        }

        public void Configure(IApplicationBuilder app, DataContext context) {
            app.UseDeveloperExceptionPage();
            app.UseRouting();

            app.UseEndpoints(endpoints => {
                endpoints.MapGet("/", async context => {
                    await context.Response.WriteAsync("Hello World!");
                });
            });
            SeedData.SeedDatabase(context);
        }
    }
}
