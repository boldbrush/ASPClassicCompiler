using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AspWebServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Console.Out.WriteLine("config WebRoot " + configuration["WebRoot"]);
        }

        private string WebRoot
        {
            get
            {
                var dir = Configuration["WebRoot"];
                if (dir.StartsWith(Path.PathSeparator))
                {
                    dir = Path.Join(Environment.CurrentDirectory, dir);
                }

                return dir;
            }
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // services.AddControllers();
            // services.TryAddSingleton<Microsoft.AspNetCore.DataProtection.IDataProtectionProvider, Microsoft.AspNetCore.DataProtection.>();
            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(10);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // var rt = new RouteCollection();
            
            // app.UseRouter(rt);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            // app.UseAuthorization();
            app.UseSession();

            
            AspHandler handler = new AspHandler(env, WebRoot);
            
            app.Use(async (context, next) =>
            {
                var path = context.Request.Path.Value;
                if (path.EndsWith("/"))
                {
                    path += "index.asp";
                }
                Console.Out.WriteLine("serving path " + path);

                if (path.EndsWith(".asp") || path.EndsWith("/"))
                {
                    context.Response.ContentType = "text/html; charset=UTF-8";
                    await handler.ProcessRequest(context);
                    return;
                }
                await next.Invoke();
                
                // Do work that doesn't write to the Response.
                // Do logging or other work that doesn't write to the Response.
            });
            app.UseStaticFiles();
            // app.UseEndpoints(async endpoints =>
            // {
            //     
            //     // endpoints.MapControllers();
            //     endpoints.MapGet("/hello", async context =>
            //     {
            //         
            //         await context.Response.WriteAsync("Hela");
            //     });
            // });
        }
    }
}