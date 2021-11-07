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
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;


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
            services.AddControllers();

            services.AddDbContext<IdentityContext>(opts => {
                opts.UseMySql(Configuration["ConnectionStrings:IdentityConnection"]);
                opts.UseSnakeCaseNamingConvention();
            });
            services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<IdentityContext>();
            services.Configure<IdentityOptions>(opts => {
                opts.Password.RequireDigit = false;
                opts.Password.RequiredLength = 1;
                opts.Password.RequiredUniqueChars = 0;
                opts.Password.RequireLowercase = false;
                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequireUppercase = false;
            });

            services.AddAuthentication(opts => {
                opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(opts => {
                opts.RequireHttpsMetadata = false;
                opts.SaveToken = true;
                opts.TokenValidationParameters = new TokenValidationParameters {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["jwtSecret"])),
                    ValidateAudience = false,
                    ValidateIssuer = false
                };
                opts.Events = new JwtBearerEvents {
                    OnTokenValidated = async ctx => {
                        var userMgr = ctx.HttpContext.RequestServices.GetRequiredService<UserManager<IdentityUser>>();
                        var signinMgr = ctx.HttpContext.RequestServices.GetRequiredService<SignInManager<IdentityUser>>();
                        string email = ctx.Principal.FindFirst(ClaimTypes.Email).Value;
                        IdentityUser idUser = await userMgr.FindByEmailAsync(email);
                        ctx.Principal = await signinMgr.CreateUserPrincipalAsync(idUser);
                    }
                };
            });
        }

        public void Configure(IApplicationBuilder app, DataContext context) {
            app.UseDeveloperExceptionPage();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapGet("/", async context => {
                    await context.Response.WriteAsync("Hello World!");
                });
                endpoints.MapControllers(); // enable mvc
            });
            SeedData.SeedDatabase(context); 
            IdentitySeedData.SeedDatabase(app);
        }
    }
}
