using System;
using System.Collections.Generic;
using AutoMapper;
using BLL.Helpers;
using BLL.Helpers.Interfaces;
using BLL.Helpers.Mapping;
using BLL.Helpers.Mapping.Interfaces;
using BLL.Services;
using BLL.Services.Interfaces;
using DAL.Repositories;
using DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DAL.DBModels;
using Newtonsoft.Json;
using Stripe;

namespace PaymentAPI
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.AddAutoMapper(options => options.AddProfile<AutoMapperProfile>(), typeof(Startup));
            
            StripeConfiguration.ApiKey = Configuration["Stripe:SecretKey"];
            StripeConfiguration.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            
            services.AddDbContext<PaymentsDbContext>(options => options.UseSqlServer(Configuration.GetSection("ConnectionStrings:DefaultConnection").Value));

            services.AddTransient<IMappingProvider, MappingProvider>();
            services.AddTransient<IRetryHelper, RetryHelper>();
            services.AddTransient<IPaymentProvider, PaymentProvider>();


            services.AddTransient<IPaymentService, PaymentService>();
            services.AddTransient<IPaymentRepository, PaymentRepository>();

            services.AddScoped<MappingStripeSucceeded<Charge>>();
            services.AddScoped<MappingPaymentFailed<string>>();

            services.AddTransient<MappingResolver>(serviceProvider => key =>
            {
                switch (key)
                {
                    case PaymentServiceConstants.PaymentMappingType.Stripe_Succeeded:
                        return serviceProvider.GetService<MappingStripeSucceeded<Charge>>();
                    case PaymentServiceConstants.PaymentMappingType.Failed:
                        return serviceProvider.GetService<MappingPaymentFailed<string>>();
                    default:
                        throw new KeyNotFoundException();
                }
            });

            services.AddScoped<PaymentAuthentication>();
            services.AddScoped<PaymentCapture>();
            services.AddScoped<PaymentCharge>();

            services.AddTransient<ServiceResolver>(serviceProvider => key =>
            {
                switch (key)
                {
                    case PaymentServiceConstants.PaymentType.Auth:
                        return serviceProvider.GetService<PaymentAuthentication>();
                    case PaymentServiceConstants.PaymentType.Capture:
                        return serviceProvider.GetService<PaymentCapture>();
                    case PaymentServiceConstants.PaymentType.Charge:
                        return serviceProvider.GetService<PaymentCharge>();
                    default:
                        throw new KeyNotFoundException();
                }
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
           
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
