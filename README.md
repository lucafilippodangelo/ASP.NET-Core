# ASP.NET-Core-Playground

In this project I made my fingers dirty with dotnet core, it was the first time I used core(ver.1.0):

- dependency injection
- middleware 
  - http request pipeline
  - exceptions details
    

- controllers
  - conventional routes
  - attribute routes
  - action results --> dependency injection


- models
  - models and view models
    - dto(data transfer object)

- entity framework
  - db context implementation and mapping
  - configuration of entity framework services
    - migrations

- razor views
  - Layout Views
  - _ViewStart
  - _ViewImports
  - Tag Helpers
  - Partial Views
  - View Components

## project implementation description

### the project Json file and startup.cs

- "project.json" is useful to add all the dependencies
  - is possible add the string of the dependency just by typing for instance "mvc" or I can do right on the "solution" and then "manage nuget package"

//LD STEP1
- "startup.cs" is used instead of "Global.aspx" as a place where to write code that has to be executed at the startup of the application
  - the "Configure" method is where we build the http process pipeline, so we find how the application will respond to http request 
  - the "ConfigureService" method is the place where configure components for the application, for example is the place where configure the inversion of control container for the application.

### adding a configuration source - appsettings.json

- added "appsettings.json" and called it from "Startup.cs"
  - .net no longer use "web.config" file for configuration, the only reason why it still exist is to give to IIS instruction how to behave when an "http" request is coming. 

in "appsettings.json"

      {
            "Greeting": "A configurable hello!"
       }

in "Startup.cs"

             app.Run(async (context) =>
            {
                //LD STEP2 the code is to read from the 
                var message = Configuration["Greeting"];
                await context.Response.WriteAsync(message);
            });

### startup.cs and dependency injection

the purpose is: instead to get an information from the "appsettings.json" file in order to show it in the browser, we will get it from the class greather, by using interface "IGreeter" and dependency injection

//LD STEP4
- in "Startup.cs" -> "Configure" method, we give in input an "IGreeter" instance

         public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IGreeter, Greeter>();
        }


### startup and middleware

A middleware is a piece of "component" in the pipeline that knows how to process something, for instance, an "input request" pass between middleware, but I have to specify the next middleware of the chain

### using "IApplicationBuilder"

- remember that we set the "http request pipeline" and then the "middleware" in "startup.cs" -> "Configure" 
  - //LD STEP6 
    -example of default middleware to handle any unmanaged exception

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

- List of "services" that I get for free in the "Configure" method, can be USED in the CONSTRUCTOR of "Startup.cs"
            
            IApplicationBuilder app, 
            IHostingEnvironment env, 
            ILoggerFactory loggerFactory,

- //LD STEP5 example of how to generate an exception

- //LD STEP6 example of how to set different excaption display output per "environment"

### install nuget "Microsoft.AspNetCore.StaticFiles"

- add an HTML page under the folder "wwwroot" named "index.html", install a middleware "Microsoft.AspNetCore.StaticFiles" from nuget that look for an html page into the disk and then is able to process the request

- //LD STEP7
        
       // it will look at the file sistem into "wwwroot"
       app.UseStaticFiles();

- //LD STEP8
      app.UseDefaultFiles();

### Setting up ASP.NET MVC Middleware

steps:
  - install the nuget for MVC
  - //LD STEP9 configure the middleware for mvc
  - register the service //LD STEP10 adding the MVC service -> "services.AddMvc();"" 


### Controllers in the MVC framework, "conventional" routes

- //LD STEP11 I will configure "conventional" routes

         app.UseMvc(ConfigureRoutes);

and then:

         private void ConfigureRoutes(IRouteBuilder routeBuilder)
        {
            //LD this is the default route
            routeBuilder.MapRoute("Default", "{controller=Home}/{action=Index}/{id?}");
        }

- //LD STEP12 if MVC doesn't map any request, this is the default answer
            
      app.Run(ctx => ctx.Response.WriteAsync("Not found"));

### Controllers in the MVC framework, "attribute" routes

- is possible use an attribute directly in the controller
  - //LD STEP13 [Route("company/[controller]/[action]")]
    
### Controllers in the MVC framework, "action results"

- usually any controller inherit from "Controller" in order to be able to use mvc features like "IActionResult" so send something back to the client. The result is incapsulated in an object that implement the "IActionResult" interface.

- //LD STEP14 code to send back an object by "ObjectResult" 

    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var model = new Restaurant { Id = 1, Name = "The House of Kobe" };
            return new ObjectResult(model);
        }
    }

- the client will receive {"id":1,"name":"The House of Kobe"}


### Rendering Views

     public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var listOfRestaurant = new List<Restaurant>();
            listOfRestaurant.Add(new Restaurant { Id = 1, Name = "The House of Kobe" });
            return View(listOfRestaurant);

        }
    }

- creation of a "Services" folder for all the interactions with the database

- //LD STEP15 in preparation to "dependency injection" creation of a class to interact with data and straight an interface that I will pass to the controller that will never see for example the sql implementation. 

     public interface IRestaurantData
    {
        IEnumerable<Restaurant> GetAll();
    }
    public class InMemoryRestaurantData : IRestaurantData
    {
        public InMemoryRestaurantData()
        {
            _restaurants = new List<Restaurant>
            {
                new Restaurant { Id = 1, Name="The House of Kobe" },
                new Restaurant { Id = 2, Name="LJ's and the Kat" },
                new Restaurant { Id = 3, Name="King's Contrivance" }
            };
        }
        public IEnumerable<Restaurant> GetAll()
        {
            return _restaurants;
        }
        List<Restaurant> _restaurants;
    }

- //LD STEP16 dependency injection
  - go in "Startup.cs" -> "ConfigureServices" method

      services.AddScoped<IRestaurantData, InMemoryRestaurantData>();

- "AddScoped" mean that I will have a new instance for any HTTP REQUEST

          public class HomeController : Controller
    {
        private IRestaurantData _restaurantData;
        public HomeController(IRestaurantData restaurantData)//LD STEP16
        {
            _restaurantData = restaurantData; 
        }
            
        public IActionResult Index()
        {
            var listOfRestaurant = _restaurantData.GetAll();
            return View(listOfRestaurant);
        }
    }


### models and viewmodels

- important to know the difference between "entity model" that is exactly the schema of the database and "viewmodel"(or DTO) that is what we will render in html in view, so what the user will see
  - a "dto" can show informations from two entity models and ofc is not persisted

- //LD SPEP17 example

- //STEP18 add a property to "RestaurantData.cs" repositoty and return it. Using it in the "details" action of the "Home" controller.

### Entity Framework, installation

- execute the console command
     
      install-package Microsoft.entityFrameworkCore.SqlServer
      install-package Microsoft.entityFrameworkCore.Tools -Pre

and then in my "project.json" setting file I got:

      "Microsoft.EntityFrameworkCore.SqlServer": "1.1.0"
      "Microsoft.EntityFrameworkCore.Tools": "1.1.0-preview4-final"

### Entity Framework, implementing "DbContext.cs"

note that the repository class get automatically the "context" "OdeToFoodDbContext", it's enought the setting in "Startup.cs"

       public class SqlRestaurantData : IRestaurantData
       {
             private OdeToFoodDbContext _context;
             public SqlRestaurantData(OdeToFoodDbContext context)
             {
                   _context = context;
             }

- each "DbContext" class will give me access to a single database, by changing connection string can point to a different specific database.
  - inside "DbContext" we create "DbSet<T>" where "T" is a type of entity. In order to refer to a specific model. Each "DbSet" will map a table in the database
  - //LD STEP20 "DbContext" class 

        public DbSet<Restaurant> Restaurants { get; set; }

----------------------------------------------------------------------------------------------
CONFIGURING THE ENTITY FRAMEWORK SERVICES
----------------------------------------------------------------------------------------------
- we put the CONNECTION STRING into the JSON CONFIGURATION FILE and the "Startup.cs"

- MIN 0:49, let's go in the "Startup.cs" -> method "ConfigureServices"

      //LS STEP21
      services.AddScoped<IRestaurantData, SqlRestaurantData >();

now we have to configure the "DbContext"

      //LD we specify which connection string use for the specific "DbContext"
            services.AddDbContext<OdeToFoodDbContext>(options =>options.UseSqlServer(Configuration.GetConnectionString("OdeToFood")));

- MIN 3:17 "AppSetting.json" 


        "ConnectionStrings": {
    "OdeToFood": "Data Source=LUCA;Integrated Security=False;User ID=sa;Password=Luca111q;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"
  
------------------------------------------------------------------------------------------
ENTITY FRAMEWORK MIGRATIONS
------------------------------------------------------------------------------------------

1 - "add-migration" is the code to create the DB SCHEMA

2 - "update-database" 

- if I have a property called "Id", entity framework will assume that is the "Primary Key".

# !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

---------------------------------------------------------------------------------------------
RAZOR VIEWS
---------------------------------------------------------------------------------------------
##########################################################
---------------------------------------------------------------------------------------------
LAYOUT VIEWS
---------------------------------------------------------------------------------------------
- MIN 3.40 he talks about @RenderBody(), is the place where we expect to see all our content.

- MIN 5:00 "@RenderSection()" useful when I want render some specific view for instance in the footer section.

- - to do that, the "_Layout" view

      <!DOCTYPE html>

<html>
<head>
    <meta name="viewport" content="width=device-width" />
    <title>@ViewBag.Title</title>
</head>
<body>
    <div>
        @RenderBody()
    </div>
    <footer>
        @RenderSection("footer", required: false)  
        @await Component.InvokeAsync("Greeting")      
    </footer>
</body>
</html>

- - after that, update the "Index" view. To render in this section of "_Layout"

          <footer>
        @RenderSection("footer", required: false)  
        @await Component.InvokeAsync("Greeting")      
        </footer>

I need to add a section in the "Index.cshtml" view

       @section footer{
             teste minchia
        }

"CTRL+K" -> "CTRL+K" to format the layout

---------------------------------------------------------------------------------------------
_ViewStart
---------------------------------------------------------------------------------------------

it's a place where specify code that I want execute for each view. The code inside this specific view recognized by the framework is executed before the main index view
      @{
            Layout = "~/Views/Shared/_Layout.cshtml";
      }

---------------------------------------------------------------------------------------------
_ViewImports
---------------------------------------------------------------------------------------------
is the place where we specify all the imports, like:

      @using OdeToFood.Entities
      @addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers

in order to don't repeat it in each view.

---------------------------------------------------------------------------------------------
Tag Helpers
---------------------------------------------------------------------------------------------
- first of all install the "razor.tools"

---------------------------------------------------------------------------------------------
Partial Views
https://app.pluralsight.com/player?course=aspdotnet-core-fundamentals&author=scott-allen&name=aspdotnet-core-fundamentals-m6&clip=6&mode=live
---------------------------------------------------------------------------------------------

- use inside my view 
      @html.Partial("NameOfThePartialView",ModelIWantPass)

the partial view rely on model from the parent view, instead a "ViewComponent" can have it's own model. to use it

      @Component.InvokeAsync("Luchino") //luchino is the name

---------------------------------------------------------------------------------------------
View Component
https://app.pluralsight.com/player?course=aspdotnet-core-fundamentals&author=scott-allen&name=aspdotnet-core-fundamentals-m6&clip=7&mode=live
---------------------------------------------------------------------------------------------

- "ViewComponents" doesn't rely on the parent to get the model.
- - the framework will look for a class 
     
      "LuchinoViewComponent" that inherit from "ViewComponent"

in this new class I have to specify the "Invoke()" method

# !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
# !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
# !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!