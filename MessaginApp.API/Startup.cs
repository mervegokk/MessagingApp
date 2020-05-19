using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MessaginApp.API.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace MessaginApp.API
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
            services.AddDbContext<DataContext>(x=>x.UseSqlite
            (Configuration.GetConnectionString("DefaultConnection")));

            //Scoped: Bu yaşam tipi ile bir objeyi register ettiğimizde, Container bize ilgili Request sonlana kadar aynı objeyi verir, yeni bir request geldiğinde yeni bir obje oluşturulur.
            //Transient  -Singleton türleride vardır.
            services.AddScoped<IAuthRepository,AuthRepository>();
            
            services.AddControllers();

            services.AddAuthentication(
                JwtBearerDefaults.AuthenticationScheme).AddJwtBearer( options=>{
                    options.TokenValidationParameters=new TokenValidationParameters{
                        ValidateIssuerSigningKey=true,
                        IssuerSigningKey=new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(Configuration.GetSection("Appsetting=Token").Value)),
                        ValidateIssuer=false,
                        ValidateAudience=true
                };
                }
                );
            
        
            
        
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();

           app.UseCors(x=>x.AllowAnyOrigin().AllowAnyMethod());

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
