using BattleAPI.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Shared;
using Shared.Helpers;
using Shared.Interfaces;
using Shared.Services;

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
                option.Configuration = Variables.REDIS_CONFIGURATION ?? "127.0.0.1";
                option.InstanceName = Variables.REDIS_INSTANCE ?? "master";
            });

            services.AddSingleton<ILoggingService, ConsoleLoggingService>();

            services.AddSingleton(Configuration);
            services.AddSingleton<IAuthCodeService, AuthCodeService>(service => {
                return new AuthCodeService(GenericHelpers.Combine(_env.ContentRootPath, "authcodes.json"));
            });
            services.AddSingleton<ICompanionService, CompanionService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BattleAPI v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }
    }
}
