using System;
using ChatApp.Business.Common.Enums;
using ChatApp.Business.Common.NavigationPages;
using ChatApp.Business.Common.SeedData;
using ChatApp.Business.Core.Interfaces;
using ChatApp.Business.Core.Repositories;
using ChatApp.Business.Core.Services;
using ChatApp.Business.Repository;
using BC = ChatApp.Business.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;
using ChatApp.DataAccess.DBContext;
using ChatApp.DataAccess.Models.Authentication;
using ChatApp.Hubs;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp
{
    public class Startup
    {
        private IConfiguration Configuration { get; }
        private string ConnectionStringDev => this.Configuration["Environment:DevEnvironment:ConnectionString"];
        private string ConnectionStringProd => this.Configuration["Environment:ProdEnvironment:ConnectionString"];
        private string AssemblyDataAccess => this.Configuration["Assembly:DataAccess"];


        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region Add Redis
            // Add Redis distributed cache TODO Figure out how to make it work when redis server is down until then don't use it
            //services.AddDistributedRedisCache(options =>
            //{
            //    options.Configuration = this.Configuration.GetConnectionString("redisServerUrl");
            //    options.ConfigurationOptions = new ConfigurationOptions()
            //    {
            //        AbortOnConnectFail = false,
            //        ConnectRetry = 3,
            //        EndPoints = { }

            //    };
            //    options.InstanceName = "master";
            //});

            #endregion

            #region Add SignalR
            // Add SignalR
            services.AddSignalR();

            #endregion

            #region Add DB service

            /*
             * Although migration is automated to be done on build and run of program the following commands(run them in Package Manager Console) can be used to do it manually:
             * 
             * 1. dotnet ef migrations add AddingMessageClientDatabase -s ChatApp -p ChatApp.DataAccess
             *  is adding new migration with changes which will be made to database
             * 2. dotnet ef database update AddingMessageClientDatabase -s ChatApp -p ChatApp.DataAccess
             *  is updating with changes from migration
             */
            services.AddDbContextPool<ApplicationDbContext>(options=> options.UseSqlServer(this.ConnectionStringDev,item=>item.MigrationsAssembly(this.AssemblyDataAccess)));

            #endregion

            #region Configure Identity Options

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredUniqueChars = 0;
                
            });
            #endregion

            #region Add Framework services
            services.AddIdentity<ApplicationUser, IdentityRole<Guid>>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
            services.AddControllersWithViews().AddRazorRuntimeCompilation().SetCompatibilityVersion(CompatibilityVersion.Version_3_0).AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());
            services.AddScoped<NavigationPage, NavigationPage>();
            services.AddTransient<SeedData, SeedData>();
            services.AddTransient<BC.Authentication.ApplicationUser, BC.Authentication.ApplicationUser>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<IMessageRepository,MessageRepository>();
            
            #endregion

            #region Add Policy.
            services.AddAuthorization(options => options.AddPolicy(Enum.GetName(typeof(EnumApplicationPolicy), EnumApplicationPolicy.AdministratorRoleGroup), policy => policy.RequireRole(Enum.GetName(typeof(EnumApplicationRoles), EnumApplicationRoles.Administrator))));
            #endregion
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
                
                // this way we add the hubs we created. Also need to create js code which handles opening connection doing work(e.g sending message to all users) and closing it.
                endpoints.MapHub<ChatHub>($"/{nameof(ChatHub)}");
            });

            using (IServiceScope scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                /*see link: https://jasonwatmore.com/post/2019/12/27/aspnet-core-automatic-ef-core-migrations-to-sql-database-on-startup*/
                scope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Database.Migrate();
                scope.ServiceProvider.GetService<SeedData>().InitSeedDataAsync();
            }

        }
    }
}
