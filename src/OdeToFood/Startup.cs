using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Routing;
using OdeToFood.Services;
using OdeToFood.Entities;
using Microsoft.EntityFrameworkCore;

namespace OdeToFood
{

    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            //LD STEP1
            //LD I declared this constructor because I want execute some code
            //before the methods "ConfigureServices" and "Configure"
            var builder = new ConfigurationBuilder()
                           .SetBasePath(env.ContentRootPath) //LD this is fluent code
                           //LD so I can add a "configuration file" and say at startup to consider it
                           .AddJsonFile("appsettings.json")
                           .AddEnvironmentVariables();

            Configuration = builder.Build(); 
        }

        public IConfiguration Configuration { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            //LD STEP4 need an api to register a service, we are saying to dot.net
            // that any time you need of an instance of "IGreeter" you have to pass
            //the concrete class "Greeter" for implementation
            services.AddSingleton(Configuration);
            services.AddSingleton<IGreeter, Greeter>();//LD that's the DEPENDENCY INJECTION

            //LD STEP10 adding the MVC service, so now I have available this MVC COMPONENT
            services.AddMvc();

            //LD STEP16 INJECT "InMemoryRestaurentData" any time you have "IRestaurantData" in a controller
            //services.AddScoped<IRestaurantData, InMemoryRestaurantData>();
            //LS STEP21
            services.AddScoped<IRestaurantData, SqlRestaurantData >();
            //LD we specify which connection string use for the specific "DbContext", then we get the info we need by queryng "Configuration", 
            // remember that we hawe setted in that method a reference to "appsettings.json"
            services.AddDbContext<OdeToFoodDbContext>(options =>options.UseSqlServer(Configuration.GetConnectionString("OdeToFood")));

        }

        // This method gets called by the runtime. 
        //Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, 
            IHostingEnvironment env, 
            ILoggerFactory loggerFactory,
            IGreeter greeter)
        {
            
            loggerFactory.AddConsole();


            //LD this default MIDDLEWARE is useful to handle any unmanaged exception
            //LD detect any error in any responce in the pipeline.
            if (env.IsDevelopment()) //LD STEP6 the dafault environment is "Development", so this flag will be "true"
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //LD if the "environment" is in PRODUCTION we can answer with a specific CUSTOM ERROR or a page or a controller with a customized view
                app.UseExceptionHandler(new ExceptionHandlerOptions
                {
                    ExceptionHandler = context => context.Response.WriteAsync("Cazzo un errore")
                });
            }


            ////LD STEP7 now I can use the MIDDLEWARE
            //// it will look at the file sistem into "wwwroot" and will show the content of "index.html"
            //app.UseStaticFiles();

            
            
            //LD STEP8
            app.UseDefaultFiles();


            ////LD STEP9 configure the environment for MVC
            ////this MIDDLEWARE look for an http request and try to map it to a method in a class
            //app.UseMvcWithDefaultRoute();

            //LD STEP11
            app.UseMvc(ConfigureRoutes);

            //LD STEP12 if MVC doesn't map any request, this is the default answer
            app.Run(ctx => ctx.Response.WriteAsync("Controller and Actions Not found"));


            ////LD this middleware will be called just if I call a specific path "http://localhost:60195/welcomepath"
            //app.UseWelcomePage("/welcomepath");



            ////LD default MIDDLEWARE present any time to RESPOND TO ANY HTTP REQUEST 
            //app.Run(async (context) =>
            //{

            //    //LD STEP5
            //    //throw new Exception("Something went wrong..");


            //    ////LD STEP2 the code is to read from the 
            //    //var message = Configuration["Greeting"];

            //    //LD STEP3 now I get the message from a class, but still don't know how to receive the parameter "IGreeter"
            //    var message = greeter.GetGreeting();

            //    await context.Response.WriteAsync(message);
            //});








        }

        private void ConfigureRoutes(IRouteBuilder routeBuilder)
        {
            //LD this is the default route
            routeBuilder.MapRoute("Default",
                "{controller=Home}/{action=Index}/{id?}");
        }

    }
}
