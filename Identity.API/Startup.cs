using Identity.API.Configuration;
using Identity.API.Data;
using Identity.API.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Identity.API
{
    public class Startup
    {
        private readonly IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = _config.GetConnectionString("DefaultConnection");
            var migrationAssembly = typeof(Startup).Assembly.GetName().Name;
            services.AddDbContext<ApplicationDbContext>(config =>
            {
                //SQL Server connection
                // config.UseSqlServer(connectionString);
                config.UseInMemoryDatabase("Memory");
            });
            
            services.AddIdentity<AppUser, IdentityRole<int>>(config =>
                {
                    config.Password.RequiredLength = 8;
                    config.Password.RequireDigit = false;
                    config.Password.RequireNonAlphanumeric = true;
                    config.Password.RequireUppercase = true;
                    config.User.RequireUniqueEmail = true;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(config =>
            {
                config.Cookie.Name = "Identity.Cookie";
                config.LoginPath = "/Auth/Login";
            });
            
            
            /////IdentityServer - EF Core + SQLServer
            // services.AddIdentityServer()
            //     .AddAspNetIdentity<AppUser>()
            //     // this adds the config data from DB (clients, resources)
            //     .AddConfigurationStore(options =>
            //     {
            //         options.ConfigureDbContext = builder =>
            //             builder.UseSqlServer(connectionString, 
            //                 sql => sql.MigrationsAssembly(migrationAssembly));
            //     })
            //     // this adds the operational data from DB (codes, tokens, consents)
            //     .AddOperationalStore(options =>
            //     {
            //         options.ConfigureDbContext = builder =>
            //             builder.UseSqlServer(connectionString, 
            //                 sql => sql.MigrationsAssembly(migrationAssembly));
            //     })
            //     .AddDeveloperSigningCredential();


            services.AddIdentityServer()
                .AddAspNetIdentity<AppUser>()
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddInMemoryApiResources(Config.GetResources())
                .AddInMemoryApiScopes(Config.GetScopes())
                .AddInMemoryClients(Config.GetClients())
                .AddDeveloperSigningCredential();
            
            

            services.AddControllersWithViews().AddRazorRuntimeCompilation();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();
            app.UseIdentityServer();
            
            
            app.UseRouting();
            

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}