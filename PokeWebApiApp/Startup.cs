using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using PokeWebApiApp.Models;
using System;
using Microsoft.Extensions.Logging;

namespace PokeWebApiApp
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
            // Passando a versão do MySql
            var serverVersion = new MySqlServerVersion(new System.Version(8, 0, 27));
            // Pegando a string de conexão do banco
            var connection = Configuration["ConnectionMySql:MySqlConnectionString"];

            services.AddControllers();

            // Passando a string de conexão e a versão do MySql
            services.AddDbContext<PokemonContext>(
                opt => opt
                .UseMySql(connection, serverVersion)
                // Configurações de log(mensagens de erro em sequência)
                .LogTo(Console.WriteLine, LogLevel.Information)
                .EnableDetailedErrors()
            );

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PokeWebApiApp", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PokeWebApiApp v1"));
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
