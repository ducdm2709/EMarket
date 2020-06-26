using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using EMarket.Areas.Admin.Models;
using Microsoft.EntityFrameworkCore;
using EMarket.Areas.Admin.Filters;
using EMarket.Areas.Client.Services;
using EMarket.Services.PayPal;
using Microsoft.Extensions.Logging;
using EMarket.Middlewares;
using EMarket.Services.MailChimp;

namespace EMarket
{
    public class Startup
    {
        private readonly ILogger _logger;
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration, ILogger<Startup> logger)
        {
            Configuration = configuration;
            _logger = logger;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                //options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.AddDbContext<EMarketContext>(options=> {
                options.UseSqlServer(Configuration.GetConnectionString("EMarket"));
            });
            services.AddSession();
            
            services.AddScoped<SessionFilter>();
            _logger.LogInformation("Added Session filter to startup services");

            services.AddScoped<HelperService>();
            _logger.LogInformation("Added Helper to startup services");

            services.AddSingleton<IPayPalPayment, PayPalPayment>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.Configure<PayPalAuthOptions>(Configuration.GetSection("PayPalPayment"));
            _logger.LogInformation("Added PayPalAuthorization Options. This can be retrieved via configuration.");


            services.Configure<MailchimpOptions>(Configuration.GetSection("Mailchimp"));
            _logger.LogInformation("Added Mailchimp Options. This can be retrieved via configuration.");

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            
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
                _logger.LogInformation("In Development environment");
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseSession();

            app.UseHitCounter();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "areas",
                    template: "{area:exists}/{controller=HangHoa}/{action=Index}/{id?}");

                routes.MapAreaRoute(
                    name: "Client",
                    areaName: "Client",
                    template: "{controller=HangHoa}/{action=Index}/{id?}");

                routes.MapAreaRoute(
                 name: "Admin",
                 areaName: "Admin", 
                 template: "{controller=HangHoa}/{action=Index}/{id?}");

            });
        }
    }
}
