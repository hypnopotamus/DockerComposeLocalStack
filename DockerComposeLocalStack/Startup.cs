using System;
using DockerComposeLocalStack.DataAccess;
using Messages;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
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
            services.AddDbContext<MessagesDbContext>(o => o.UseSqlServer(Configuration.GetConnectionString("Messages")));

            services.AddHealthChecks()
                .AddSqlServer(Configuration.GetConnectionString("Master"))
                .AddDbContextCheck<MessagesDbContext>()
                .AddCheck<SqlServerReadinessCheck>(nameof(SqlServerReadinessCheck));

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "DockerComposeLocalStack", Version = "v1"});
            });

            services.AddTransient<IGetMessagesCommand, GetMessageCommand>();
            services.AddTransient<Func<IGetMessagesCommand>>(provider => provider.GetRequiredService<IGetMessagesCommand>);
            services.AddTransient<ISaveMessageCommand, SaveMessageCommand>();
            services.AddTransient<Func<ISaveMessageCommand>>(provider => provider.GetRequiredService<ISaveMessageCommand>);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, MessagesDbContext dbContext)
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

            dbContext.Database.EnsureCreated();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/health");
                endpoints.MapControllers();
            });
        }
    }
}