using BattleAPI.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Shared;
using Shared.Helpers;
using Shared.Interfaces;
using Shared.Services;
using System.Net.NetworkInformation;
using System.Collections.Generic;

namespace BattleAPI
{
    public class Startup
    {
        private readonly IWebHostEnvironment _env;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers().AddNewtonsoftJson();
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "BattleAPI", Version = "v1" });
            });

            services.AddDistributedRedisCache(option =>
            {
                option.Configuration = Variables.REDIS_CONFIGURATION ?? "127.0.0.1,abortConnect=false,connectTimeout=500";
                option.InstanceName = Variables.REDIS_INSTANCE ?? "master";
            });

            services.AddSingleton<ILoggingService, ConsoleLoggingService>();

            services.AddSingleton(Configuration);
            services.AddSingleton<IAuthCodeService, AuthCodeService>();
            services.AddSingleton<ICompanionService, CompanionService>();
            services.AddSingleton<IPersonaService, PersonaService>();

            if (Variables.PLAYERCOUNT_LOGGING_ENABLED)
            {
                services.AddHostedService<PlayerCountWorker>();
            }

            services.AddHealthChecks()
                .AddRedis(Variables.REDIS_CONFIGURATION ?? "127.0.0.1,abortConnect=false,connectTimeout=500")
                .AddCheck("battlelog", () =>
                {
                    Dictionary<string, object> data = new();
                    try
                    {
                        using var ping = new Ping();

                        var reply = ping.Send("battlelog.battlefield.com");

                        data.Add("roundTripTime", reply.RoundtripTime);
                        if (reply.Status != IPStatus.Success)
                        {
                            return HealthCheckResult.Unhealthy("Ping is unhealthy", null, data);
                        }

                        if (reply.RoundtripTime > 100)
                        {
                            return HealthCheckResult.Degraded("Ping is degraded", null, data);
                        }

                        return HealthCheckResult.Healthy("Ping is healthy", data);
                    }
                    catch
                    {
                        return HealthCheckResult.Unhealthy("Ping is unhealthy", null, data);
                    }
                })
                .AddCheck("companion", () => {
                    Dictionary<string, object> data = new();
                    try
                    {
                        using var ping = new Ping();

                        var reply = ping.Send("companion-api.battlefield.com");
                        data.Add("roundTripTime", reply.RoundtripTime);
                        if (reply.Status != IPStatus.Success)
                        {
                            return HealthCheckResult.Unhealthy("Ping is unhealthy", null, data);
                        }

                        if (reply.RoundtripTime > 100)
                        {
                            return HealthCheckResult.Degraded("Ping is degraded", null, data);
                        }

                        return HealthCheckResult.Healthy("Ping is healthy", data);
                    }
                    catch
                    {
                        return HealthCheckResult.Unhealthy("Ping is unhealthy", null, data);
                    }
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ICompanionService companionService)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            if (env.IsDevelopment() || Variables.SWAGGER_ENABLED)
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BattleAPI v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapHealthChecks("/health");
                endpoints.MapControllers();
            });
        }
    }
}
