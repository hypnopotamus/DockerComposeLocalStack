using System;
using System.Data;
using DockerComposeLocalStack.DataAccess;
using Messages;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace DockerComposeLocalStack
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
            services.AddSingleton(Configuration);

            services.AddHealthChecks()
                .AddSqlServer(Configuration.GetConnectionString("Master"), name: "Master")
                .AddSqlServer(Configuration.GetConnectionString("Messages"), name: "Messages")
                .AddCheck<SqlServerReadinessCheck>(nameof(SqlServerReadinessCheck));

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "DockerComposeLocalStack", Version = "v1"});
            });

            services.AddTransient<IDbConnection>(provider => new SqlConnection(provider.GetRequiredService<IConfiguration>().GetConnectionString("Messages")));
            services.AddTransient<IGetMessagesCommand, GetMessagesCommand>();
            services.AddTransient<Func<IGetMessagesCommand>>(provider => provider.GetRequiredService<IGetMessagesCommand>);
            services.AddTransient<ISaveMessageCommand, SaveMessagesCommand>();
            services.AddTransient<Func<ISaveMessageCommand>>(provider => provider.GetRequiredService<ISaveMessageCommand>);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DockerComposeLocalStack v1"));
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();

            Database.Create(Configuration.GetConnectionString("Messages"));

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/health");
                endpoints.MapControllers();
            });
        }
    }
}