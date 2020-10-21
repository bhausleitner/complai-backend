using ComplAI.Business.Managers;
using ComplAI.DataLayer.Context;
using ComplAI.DataLayer.Entity;
using ComplAI.DataLayer.Interfaces;
using ComplAI.DataLayer.MongoDbCollections;
using ComplAI.DataLayer.Persistence;
using ComplAI.DataLayer.Repositories;
using ComplAI.DataLayer.UnitOfWork;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ComplAI.API
{
    /// <summary>
    /// IOC / DI Configuration
    /// </summary>
    public class Startup
    {
        /// <inheritdoc />
        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            HostingEnvironment = hostingEnvironment;

        }

        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";


        /// <summary>
        /// Configuration Object that holds our dev/prod strings
        /// </summary>
        public IConfiguration Configuration { get; }
        public IHostingEnvironment HostingEnvironment { get; }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            
            services.AddScoped<IMongoContext, MongoContext>();
            services.AddScoped<IUnitOfWork, MongoDbUnitOfWork>();
            services.AddScoped(typeof(IRepository<>), typeof(MongoDbRepository<>));
            services.AddScoped(typeof(ICustomMongoRepository<>), typeof(CustomCollectionNameMongoDbRepository<>));
            // Custom-named collections that we do  not want to use the class name as the collection name
            services.AddScoped<IMongoCollectionName<EuDocumentEntity>, EuDocumentsCollection>();

            // MongoDb Configuration
            MongoDbPersistence.Configure();
                
            //services.AddNewtonsoftJson(options => options.UseMemberCasing()); ;

            services.AddScoped<RegulationManager>();
            services.AddScoped<EuDocumentsManager>();

            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                builder =>
                {
                    builder.WithOrigins("http://localhost:4200/");
                    builder.WithOrigins("https://dev-complai.eu.auth0.com");
                });
            });


            ConfigureAuthentication(services);


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "COMPLAI API", Version = "v1" });
                c.AddSecurityDefinition(CookieAuthenticationDefaults.AuthenticationScheme, new ApiKeyScheme()
                {
                    Type = "apiKey",
                    Description = CookieAuthenticationDefaults.AuthenticationScheme,
                    In = CookieAuthenticationDefaults.AuthenticationScheme,
                    Name = CookieAuthenticationDefaults.AuthenticationScheme
                });

                // Include XML comments in swagger documentation
                var filePath = Path.Combine(System.AppContext.BaseDirectory, "ComplAI.API.xml");
                c.IncludeXmlComments(filePath);
            });

            services.AddMvc();


        }

        private void ConfigureAuthentication(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => HostingEnvironment.IsProduction();
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            //services.AddScoped<CustomCookieAuthenticationEvents>();

            // Add authentication services
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                })
                .AddCookie(o =>
                {
                    o.LoginPath = "/authorize/login";
                    //o.EventsType = typeof(CustomCookieAuthenticationEvents);
                    
                    //o.Events.OnSignedIn= async context =>
                    //{
                    //    await Task.CompletedTask;
                    //};
                })
                .AddOpenIdConnect("Auth0", options =>
                {
                    // Set the authority to your Auth0 domain
                    options.Authority = $"https://{Configuration["Auth0:Domain"]}";

                    // Configure the Auth0 Client ID and Client Secret
                    options.ClientId = Configuration["Auth0:ClientId"];
                    options.ClientSecret = Configuration["Auth0:ClientSecret"];

                    // Set response type to code
                    options.ResponseType = "code";

                    // Configure the scope
                    options.Scope.Clear();
                    options.Scope.Add("openid");

                    // Set the callback path, so Auth0 will call back to http://localhost:3000/callback
                    // Also ensure that you have added the URL as an Allowed Callback URL in your Auth0 dashboard
                    options.CallbackPath = new PathString("/callback");

                    // Configure the Claims Issuer to be Auth0
                    options.ClaimsIssuer = "Auth0";

                    // Saves tokens to the AuthenticationProperties
                    options.SaveTokens = true;

                    options.Events = new OpenIdConnectEvents
                    {
                        // handle the logout redirection 
                        OnRedirectToIdentityProviderForSignOut = (context) =>
                        {
                            var logoutUri =
                                $"https://{Configuration["Auth0:Domain"]}/v2/logout?client_id={Configuration["Auth0:ClientId"]}";

                            var postLogoutUri = context.Properties.RedirectUri;
                            if (!string.IsNullOrEmpty(postLogoutUri))
                            {
                                if (postLogoutUri.StartsWith("/"))
                                {
                                    // transform to absolute
                                    var request = context.Request;
                                    postLogoutUri = request.Scheme + "://" + request.Host + request.PathBase +
                                                    postLogoutUri;
                                }

                                logoutUri += $"&returnTo={Uri.EscapeDataString(postLogoutUri)}";
                            }

                            context.Response.Redirect(logoutUri);
                            context.HandleResponse();

                            return Task.CompletedTask;
                        }
                    };
                });

        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline. 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
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

            app.UseCors(MyAllowSpecificOrigins);
            app.UseCookiePolicy();
            app.UseAuthentication();


            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "COMPLAI Api V1");
            });
            


        }

        //public class CustomCookieAuthenticationEvents : CookieAuthenticationEvents
        //{
        //    public override async Task ValidatePrincipal(CookieValidatePrincipalContext context)
        //    {
        //        var userPrincipal = context.Principal;

        //        // Look for the LastChanged claim.
        //        var lastChanged = (from c in userPrincipal.Claims
        //            where c.Type == "LastChanged"
        //            select c.Value).FirstOrDefault();
        //        await Task.CompletedTask;
        //        //if (string.IsNullOrEmpty(lastChanged))
        //        //{
        //        //    context.RejectPrincipal();

        //        //    await context.HttpContext.SignOutAsync(
        //        //        CookieAuthenticationDefaults.AuthenticationScheme);
        //        //}
        //    }

        //}
    }


}
