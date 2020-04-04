using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using ElCamino.AspNetCore.Identity.AzureTable.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using XI.Portal.AzureTableLogging;
using XI.Portal.Web.Constants;
using XI.Portal.Web.Data;
using IdentityRole = ElCamino.AspNetCore.Identity.AzureTable.Model.IdentityRole;
using IdentityUser = ElCamino.AspNetCore.Identity.AzureTable.Model.IdentityUser;

namespace XI.Portal.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddIdentity<IdentityUser, IdentityRole>(options => { options.User.RequireUniqueEmail = true; })
                .AddAzureTableStores<ApplicationAuthDbContext>(() =>
                {
                    var config = new IdentityConfiguration
                    {
                        StorageConnectionString = Configuration["ApplicationAuthDbContext:StorageConnectionString"],
                        LocationMode = "PrimaryOnly",
                        IndexTableName = "AspNetIndex",
                        RoleTableName = "AspNetRoles",
                        UserTableName = "AspNetUsers",
                        TablePrefix = ""
                    };
                    return config;
                })
                .AddDefaultTokenProviders()
                .CreateAzureTablesIfNotExists<ApplicationAuthDbContext>();

            services.AddAuthentication(options => options.DefaultChallengeScheme = "XtremeIdiots")
                .AddOAuth("XtremeIdiots", options =>
                {
                    options.ClientId = Configuration["XtremeIdiotsAuth:ClientId"];
                    options.ClientSecret = Configuration["XtremeIdiotsAuth:ClientSecret"];
                    options.CallbackPath = new PathString("/signin-xtremeidiots");

                    options.AuthorizationEndpoint = "https://www.xtremeidiots.com/oauth/authorize/";
                    options.TokenEndpoint = "https://www.xtremeidiots.com/oauth/token/";
                    options.UserInformationEndpoint = "https://www.xtremeidiots.com/api/core/me";

                    options.ClaimActions.MapAll();

                    options.Scope.Add("profile");

                    options.Events = new OAuthEvents
                    {
                        OnCreatingTicket = async context =>
                        {
                            var request =
                                new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
                            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            request.Headers.Authorization =
                                new AuthenticationHeaderValue("Bearer", context.AccessToken);

                            var response = await context.Backchannel.SendAsync(request,
                                HttpCompletionOption.ResponseHeadersRead, context.HttpContext.RequestAborted);
                            response.EnsureSuccessStatusCode();

                            var contentAsString = await response.Content.ReadAsStringAsync();
                            var user = JsonDocument.Parse(contentAsString);

                            context.RunClaimActions(user.RootElement);
                        }
                    };
                });

            services.AddLogging(
                logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                    logging.AddAzureTableLogger(options =>
                    {
                        options.CreateTableIfNotExists = true;
                        options.LogTableName = Configuration["Logging:AzureTableLogger:LogTableName"];
                        options.ConnectionString = Configuration["Logging:AzureTableLogger:ConnectionString"];
                    });
                });

            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    "default",
                    "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}