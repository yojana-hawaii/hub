using hub.dal.interfaces.directory;
using hub.dal.repository.directory;
using hub.dbMigration.dbContext;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Security.Claims;

namespace hub.mvc
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
            services.AddDbContext<HubDbContext>(
                options => options.UseSqlServer(
                    Configuration.GetConnectionString("LocalDbConnection")
                    )
                );
            services.AddSingleton(Configuration);
            services.AddScoped<IEmployee, EmployeeRepository>();
            services.AddScoped<ILocation, LocationRepository>();
            services.AddScoped<IJobTitle, JobTitleRepository>();
            services.AddScoped<IDepartment, DepartmentRepository>();

            // use open id connect with adfs
            services.AddAuthentication(options =>
            {
                //  check if user has authentication cookie -> if they dont then default is open id connect
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddOpenIdConnect(options =>
            {
                // from appsettings.json or secret
                options.MetadataAddress = Configuration["Adfs:address"];
                options.ClientId = Configuration["Adfs:clientId"];

                options.SignInScheme = "Cookies";
                options.RequireHttpsMetadata = true;
                options.ResponseType = OpenIdConnectResponseType.Code;
                options.UsePkce = false;

                options.Scope.Clear();
                options.Scope.Add("openid");


                options.SaveTokens = true;
            })
            .AddCookie(options =>
            {
                options.AccessDeniedPath = "/";
            }
            );

            //mapping AD user to claims role.
            services.AddAuthorization(options =>
            {
                options.AddPolicy("TestAdmin", policyBuilder =>
                    policyBuilder.RequireClaim(ClaimTypes.Role, "Test Admin")
                );
                options.AddPolicy("TestUser", policyBuilder =>
                    policyBuilder.RequireClaim(ClaimTypes.Role, "Test User")
                );
            });
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
