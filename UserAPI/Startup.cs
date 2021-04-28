using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using UserAPI.Data;

namespace UserAPI
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
            /*
            var server = Configuration["DBServer"] ?? "ms-sql-server";
            var port = Configuration["DBPort"] ?? "1433";
            var user = Configuration["DBUser"] ?? "sa";
            var password = Configuration["DBPasswrod"] ?? "Roberto@123";
            var database = Configuration["Database"] ?? "Usuarios";
            */
            //Adiciona ao UserContext a conexão do banco
            /*
            services.AddDbContext<UserContext>(opt => 
                opt.UseSqlServer($"Server={server},{port};Initial Catalog={database};User ID ={user};Password={password}"));
            
            */
            services.AddDbContext<UserContext>(opt => 
                opt.UseSqlServer(Configuration.GetConnectionString("UserConnection")));
              
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "UserAPI", Version = "v1" });
            });

            //Adiciona a propiedades do mapper (não sei bem oq isso faz)
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            //Adiciona a dependencia?
            services.AddScoped<IUserRepo,SqlUserRepo>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "UserAPI v1"));
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
