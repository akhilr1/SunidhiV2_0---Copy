using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.StaticFiles;
using SunidhiV2_0.Code.Common;
using System.Text.Json.Serialization;

namespace SunidhiV2_0
{
    public class Startup
    {
        public static string ConnectionString { get; private set; }
        public IConfiguration Configuration { get; }
        public IHostingEnvironment HostingEnvironment { get; }


        public static string MailServerEnableSSL { get; private set; }
        public static string MailServerPort { get; private set; }
        public static string MailServerHost { get; private set; }
        public static string MailServerUserName { get; private set; }
        public static string SystemSenderEmailPassword { get; private set; }
        public static string SystemSenderEmailId { get; private set; }
        public static string AppSupportEmailId { get; private set; }
        public static string AppAdminEmailId { get; private set; }
        public static string SecureIV { get; private set; }
        public static string SecureKey { get; private set; }
        public static string SendAllTestMailsTo { get; private set; }
        public static string NumberOfUserToken { get; private set; }
        public static string Basepath { get; private set; }
        public static string TokenFile { get; private set; }
        public static string AnalyticsURL { get; private set; }
        public static string ShareLinkUserID { get; private set; }
        public static string GoogleAPIKey { get; private set; }
        public static string EmailTemplateImages { get; private set; }
        public static string DomainName { get; private set; }
        public static string ImWorksLink { get; set; }
        

        public Startup(IHostingEnvironment hostingEnvironment, IConfiguration configuration)
        {
            Configuration = configuration;
            HostingEnvironment = hostingEnvironment;
            Basepath = hostingEnvironment.ContentRootPath;
        }
        public Startup(IHostingEnvironment env)
        {
            Configuration = new ConfigurationBuilder().SetBasePath(env.ContentRootPath).AddJsonFile("appSettings.json").Build();
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //this is added for Vue CLI dev testing
            if (HostingEnvironment.IsDevelopment())
            {
                services.AddCors();
            }

            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                //  o.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(t => { t.Cookie.Name = "auth-cookie"; t.LoginPath = "/home/index"; });

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.AddResponseCompression();
            services.AddMvc().AddMvcOptions(t => { t.EnableEndpointRouting = false; });
            // services.AddControllers().AddJsonOptions(options => { options.JsonSerializerOptions.PropertyNamingPolicy = null; });

            // services.AddControllers().AddNewtonsoftJson();
            //services.AddRazorPages().AddNewtonsoftJson();

            services.AddControllersWithViews().AddNewtonsoftJson(); //For .net 3+
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromDays(7);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                options.Cookie.Name = "imworks-session";
            });
            //services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddCloudscribePagination();
            services.AddSingleton<IConfiguration>(Configuration);

            //services.AddControllers().AddJsonOptions(options =>
            // {
            //     //options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
            //     //options.JsonSerializerOptions.PropertyNamingPolicy = null;
            //     //options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());


            // });

            //For .net 6


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            ConnectionString = Configuration["ConnectionStrings:connstring"];

            MailServerEnableSSL = Configuration["ConfigAppsetting:MailServerEnableSSL"];
            MailServerPort = Configuration["ConfigAppsetting:MailServerPort"];
            MailServerHost = Configuration["ConfigAppsetting:MailServerHost"];
            MailServerUserName = Configuration["ConfigAppsetting:MailServerUserName"];
            SystemSenderEmailPassword = Configuration["ConfigAppsetting:SystemSenderEmailPassword"];
            SystemSenderEmailId = Configuration["ConfigAppsetting:SystemSenderEmailId"];
            AppSupportEmailId = Configuration["ConfigAppsetting:AppSupportEmailId"];
            AppAdminEmailId = Configuration["ConfigAppsetting:AppAdminEmailId"];
            SecureIV = Configuration["ConfigAppsetting:SecureIV"];
            SecureKey = Configuration["ConfigAppsetting:SecureKey"];
            SendAllTestMailsTo = Configuration["ConfigAppsetting:SendAllTestMailsTo"];
            NumberOfUserToken = Configuration["ConfigAppsetting:NumberOfUserToken"];
            TokenFile = Configuration["ConfigAppsetting:TokenFile"];
            AnalyticsURL = Configuration["ConfigAppsetting:AnalyticsURL"];
            ShareLinkUserID = Configuration["ConfigAppsetting:ShareLinkUserID"];
            GoogleAPIKey = Configuration["ConfigAppsetting:GoogleAPIKey"];
            
            //Email Images Path
            EmailTemplateImages = Configuration["ConfigAppsetting:EmailTemplateImages"];

            //Domain Name
            DomainName = Configuration["ConfigAppsetting:domainName"];
            ImWorksLink = Configuration["ConfigAppsetting:imWorksLink"];

            app.UseResponseCompression();
            //this is added for Vue CLI dev testing
            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
                app.UseCors(policy => policy
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .WithOrigins("http://localhost:8080")
                    .AllowCredentials());
            }
            if (env.IsDevelopment()) //use developer exception in dev 
            {
                app.UseDeveloperExceptionPage();
            }
            else //otherwise use ErrorsController which logs to TbErrorsLog
            {
                app.UseExceptionHandler("/Errors");
            }

            app.Use(async (context, next) =>
            {
                if (context.Request.Path.Value.StartsWith("/app"))
                {
                    var a = context.Request.Path.Value;
                    if (!a.Contains("."))
                    {
                        context.Request.Path = "/app/index.html";
                    }
                    a = context.Request.Path.Value;
                    context.Response.GetTypedHeaders().CacheControl =
                    new Microsoft.Net.Http.Headers.CacheControlHeaderValue()
                    {
                        Public = true,
                        MaxAge = TimeSpan.FromSeconds(-1)
                    };
                }
                await next.Invoke();
            });

            //app.UseExceptionHandler("/Errors");
            // app.UseStaticFiles();
            app.UseDefaultFiles();


            var provider = new FileExtensionContentTypeProvider();
            // Add new mappings
            provider.Mappings[".vue"] = "text/plain";
            app.UseStaticFiles(new StaticFileOptions()
            {
                ContentTypeProvider = provider,
                OnPrepareResponse = context =>
                {
                    // context.Context.Response.Headers["Access-Control-Allow-Credentials"] =
                    // "true";

                    // context.Context.Response.Headers["Expires"] = 
                    // DateTime.UtcNow.AddHours(12).ToString("R");
                }
            });

            app.UseCookiePolicy();
            app.UseSession();

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseAuthentication();

            /* */
            RequestContextManager.Instance = new RequestContextManager(app.ApplicationServices.GetService<IHttpContextAccessor>());



            app.UseMvc(routes =>
            {
                routes.MapRoute(
                   name: "default",
                   template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
