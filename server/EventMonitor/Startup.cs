using EventMonitor.Business;
using EventMonitor.Hubs;
using EventMonitor.Interfaces;
using EventMonitor.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;

namespace EventMonitor
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
            services.AddSingleton<IEventBusiness, EventBusiness>();
            services.AddSingleton<IEventsAggregator, EventsAggregator>();
            services.AddSingleton<IEventsProcessor, EventsProcessor>();

            services.AddCors();

            services.AddSwaggerGen(c =>
                        {
                            c.SwaggerDoc("v1", new OpenApiInfo
                            {
                                Title = "Event Monitor API",
                                Version = "v1"
                            });
                        });

            services.AddControllers();

            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseSwagger();

                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Event Monitor API v1");
                });
            }

            app.UseRouting();

            app.UseCors(builder =>
            {
                builder.SetIsOriginAllowed(s => true);
                builder.AllowAnyMethod();
                builder.AllowAnyHeader();
                builder.AllowCredentials();
            });

            app.UseAuthorization();

            app.UseWebSockets(new WebSocketOptions
            {
                KeepAliveInterval = TimeSpan.FromSeconds(120),
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                endpoints.MapHub<EventHub>("/hub/events");
            });

            app.ApplicationServices.GetService<EventBusiness>();
            app.ApplicationServices.GetService<EventsAggregator>();
            app.ApplicationServices.GetService<EventsProcessor>();
        }
    }
}
