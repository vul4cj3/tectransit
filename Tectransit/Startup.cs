using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Tectransit.Modles;

namespace Tectransit
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
            var connString = Configuration.GetSection("ConnectionString")["tecConn"];
            services.AddDbContext<TECTRANSITDBContext>(cfg => cfg.UseSqlServer(connString));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSession();
            //get client IP info
            services.AddHttpContextAccessor();
            services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();

            // In production, the Angular files will be served from this directory
            //services.AddSpaStaticFiles(configuration =>
            //{
            //    configuration.RootPath = "TecTransit/dist/TecTransit";
            //});

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto | ForwardedHeaders.All;
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseForwardedHeaders();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            //app.UseSpaStaticFiles();
            
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"admin/dist/admin/assets")),
                RequestPath = new PathString("/assets")
            });
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"TecTransit/dist/TecTransit/assets")),
                RequestPath = new PathString("/res/assets")
            });

            app.UseSession();
            
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            //app.UseSpa(spa =>
            //{
            //    // To learn more about options for serving an Angular SPA from ASP.NET Core,
            //    // see https://go.microsoft.com/fwlink/?linkid=864501

            //    spa.Options.SourcePath = "ClientApp";

            //    if (env.IsDevelopment())
            //    {
            //        spa.UseAngularCliServer(npmScript: "start");
            //    }
            //});

            //router setting
            app.Map("/admin", admin =>
            {
                string clientApp1Path = env.IsDevelopment() ? "admin" : @"admin/dist/admin";

                // Each map gets its own physical path for it to map the static files to. 
                StaticFileOptions clientApp1Dist = new StaticFileOptions()
                {
                    FileProvider = new PhysicalFileProvider(
                            Path.Combine(Directory.GetCurrentDirectory(), clientApp1Path)
                        )
                };

                // Each map its own static files otherwise it will only ever serve index.html no matter the filename 
                admin.UseSpaStaticFiles(clientApp1Dist);
                admin.UseSpa(spa =>
                {
                    spa.Options.SourcePath = "admin";
                    if (env.IsDevelopment())
                    {
                        // it will use package.json & will search for start command to run
                        spa.UseAngularCliServer(npmScript: "start");
                        //spa.Options.DefaultPageStaticFileOptions = clientApp1Dist;
                    }
                    else
                    {
                        spa.Options.DefaultPageStaticFileOptions = clientApp1Dist;
                    }
                });
            }).Map("", client =>
            {

                string clientApp2Path = env.IsDevelopment() ? "TecTransit" : @"TecTransit/dist/TecTransit";

                // Each map gets its own physical path for it to map the static files to. 
                StaticFileOptions clientApp1Dist = new StaticFileOptions()
                {
                    FileProvider = new PhysicalFileProvider(
                            Path.Combine(Directory.GetCurrentDirectory(), clientApp2Path)
                        )
                };

                // Each map its own static files otherwise it will only ever serve index.html no matter the filename 
                client.UseSpaStaticFiles(clientApp1Dist);
                client.UseSpa(spa =>
                {
                    spa.Options.SourcePath = "TecTransit";
                    if (env.IsDevelopment())
                    {
                        // it will use package.json & will search for start command to run
                        spa.UseAngularCliServer(npmScript: "start");
                        //spa.Options.DefaultPageStaticFileOptions = clientApp1Dist;
                    }
                    else
                    {
                        spa.Options.DefaultPageStaticFileOptions = clientApp1Dist;
                    }
                });
            });
        }
    }
}
