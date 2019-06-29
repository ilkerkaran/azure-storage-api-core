using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using azure_storage_api_core.Models;
using azure_storage_api_core.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace azure_storage_api_core
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
            //DI Configurations
            ConfigureDependencies(services);
            ConfigureSettings(services);

            services.AddOptions();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        public void ConfigureSettings(IServiceCollection services)
        {
            services.Configure<AzureStorageSettings>(Configuration.GetSection(nameof(AzureStorageSettings)));
            services.AddSingleton<AzureStorageSettings>();
        }

        public void ConfigureDependencies(IServiceCollection services)
        {
            services.AddScoped<IStorageService, AzureStorageService>();





        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseMvc();
        }
    }
}
