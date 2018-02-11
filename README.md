# ASP.NET-Core-Playground

In this project I made my fingers dirty with the followins:

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

# !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

------------------------------------------------------------------------------
Building Your first ASP.NET Core Application
------------------------------------------------------------------------------

###############################################
the trainer shows how to do all the settings, he shows as well the place where to set INVERSION OF CONTROL
###############################################

+ I did all the settings
- - https://www.microsoft.com/net/core#macos

--------------------------------------------------------------------------------
THE PROJECT JSON FILE
--------------------------------------------------------------------------------
+ "project.json" is useful to add all the dependencies
- - I can add the string of the dependency just by typing for instance "mvc" or I can do right on the "solution" and then "MANAGE NUGET PACKAGE"

+ "STARTUP.cs" is used instead of "GLOBAL.aspx" as a place where to write code that has to be executed at the startup of the application
- - the "Configure" METHOD is where we build the HTTP PROCESS PIPELINE, so we find how the application will respond to HTTP REQUEST 
- - the "ConfigureService" METHOD (MIN 7:45) is the place where configure COMPONENTS for the application, for example is the place where configure the INVERSION OF CONTROL container for the application.

--------------------------------------------------------------------------------
ADDING A CONFIGURATION SOURCE 
--------------------------------------------------------------------------------

##################################################
added "appsettings.json" and called it from "Startup.cs"
##################################################

+ .net no longer use "web.config" file for configuration, the only reason why it still exist is to give to IIS instruction how to behave when an "http" request is coming. "WEB.CONFIG" IS NO LONGER USED FOR CONFIGURATIONS

+ MIN 1:15 we add our personal configuration file with JSON, by selecting the template "ASP.NET Configuration File". -> "appsettings.json"

- - The purpose is store a setting in a file and use it t runtime, then we have to update the "Startup.cs" file, to tell to .net that we have a CUSTOM CONFIGURATION FILE
- - MIN 6:08 I can jump here to see the code to write in order to READ THE CONFIGURATION FILE


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

--------------------------------------------------------------------------------------
CREATING A GREETING SERVICE
--------------------------------------------------------------------------------------

**
the purpose of this chapter is: instead to get an information from the "appsettings.json" file in order to show it in the browser, we will get it from the class greather, by using interface "IGreeter" and DEPENDENCY INJECTION
**

+ in "Startup.cs" -> "Configure" method, we give in input an "IGreeter" instance

         public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IGreeter, Greeter>();
        }


# !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

--------------------------------------------------------------------------------------------
STARTUP AND MIDDLEWARE
--------------------------------------------------------------------------------------------

--------------------------------------------------------------------------------------------
HOW MIDDLEWARE WORKS
--------------------------------------------------------------------------------------------
##########################################################
+DEFINITION: MIDDLEWARE is a piece of COMPONENT in the PIPELINE that knows how to process something
+ + for instance, an "input request" pass between MIDDLEWARE, but I have to specify the next middleware of the chain
##########################################################

- we need to set COMPONENTS to let the application behave as expected and answers to the REQUESTS

--------------------------------------------------------------------------------------------
USING "IApplicationBuilder"
--------------------------------------------------------------------------------------------
#########################################################
remember that we set the HTTP REQUEST PIPELINE in "startup.cs" -> "Configure" 

#########################################################
- we set MIDDLEWARE in "startup.cs" -> "Configure" 

- the trainer present all the default MIDDLEWARE

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


- MIN 2:00 each NUGET PACKAGE contain some MIDDLEWARE usable by "IApplicationBuilder"

- MIN 6:00 the order how middleware are called is important

#########################################################
most of the MIDDLEWARE are available by installation of NUGET PACKAGE, usually as EXTENSION METHODS of "IApplicationBuilder"
#########################################################

-------------------------------------------------------------------------------------------
SHOWING EXCEPTION DETAILS, MIDDLEWARE TO MATCH THE ENVIRONMENT.
-------------------------------------------------------------------------------------------

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

#########################################################
List of "services" that I get for free in the  "Configure" method
            
            IApplicationBuilder app, 
            IHostingEnvironment env, 
            ILoggerFactory loggerFactory,

but them can be USED in the CONSTRUCTOR of "Startup.cs"
#########################################################
- search for //LD STEP5 to generate an exception

- search for //LD STEP6 he talks about ENVIRONMENT, I can SET DIFFERENT EXCEPTION VIDEO OUTPUT per ENVIRONMENT

-------------------------------------------------------------------------------------------
SERVING FILES
-------------------------------------------------------------------------------------------
- he add an HTML page under the folder "wwwroot" named "index.html"

now I have to INSTALL a MIDDLEWARE that look for an html page inthe disk and then is able to PROCESS THAT REQUEST

+ MIN 1:40 installing of the NUGET "Microsoft.AspNetCore.StaticFiles", no now I can add another MIDDLEWARE

es. //LD STEP7
        
       // it will look at the file sistem into "wwwroot"
       app.UseStaticFiles();

es. //LD STEP8
      app.UseDefaultFiles();

--------------------------------------------------------------------------------------------
Setting uo ASP.NET MVC Middleware
--------------------------------------------------------------------------------------------

- first step is install the nugat for MVC
- second step //LD STEP9 configure the MIDDLEWARE for MVC
- thirth step is REGISTER THE SERVICE 

            //LD STEP10 adding the MVC service
            services.AddMvc(); 


# !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
-----------------------------------------------------------------------------------------
Controllers in the MVC framework
-----------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------
CONVENTIONAL ROUTES
-----------------------------------------------------------------------------------------
- in //LD STEP11 I will configure the routes

         app.UseMvc(ConfigureRoutes);

and then:

         private void ConfigureRoutes(IRouteBuilder routeBuilder)
        {
            //LD this is the default route
            routeBuilder.MapRoute("Default",
                "{controller=Home}/{action=Index}/{id?}");
        }

- //LD STEP12 if MVC doesn't map any request, this is the default answer
            
      app.Run(ctx => ctx.Response.WriteAsync("Not found"));

- of course i CAN DEFINE MANY ROUTE RULES for any specific and particular controller route


-----------------------------------------------------------------------------------------
ATTRIBUTE ROUTES
-----------------------------------------------------------------------------------------
- we can use an attribute directly in the controller

- FOLLOW ALL THE VIDEO, because the "route attributing" can be static, when I specify the name of the action or generic by using "TOKEN" PREDEFINED by MVC. For Instance [controller]

          //LD STEP13
          [Route("company/[controller]/[action]")]
    
#########################################################
WE CAN COMBINE BOTH TYPES OF ATTRIBUTES (MIN 4:70)
#########################################################

-------------------------------------------------------------------------------------------
ACTION RESULTS
-------------------------------------------------------------------------------------------
- usually any controller inherit from "Controller" in order to be able to use MVC features like "IActionResult" so SEND SOMETHING BACK TO THE CLIENT
- - usually the RESULT IS INCAPSULATED in an OBJECT THAT IMPLEMENT "IActionResult" INTERFACE.

- MIN 2:00, he talks about the CONTROLLER and all the informations that I can get from the "HttpObject".
- es. "this.context.request"

- MIN 3:40 "CONTENT" result

- MIN 7:00 after he create the code to SEND BACK AN OBJECT by "ObjectResult" //LD STEP14

    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var model = new Restaurant { Id = 1, Name = "The House of Kobe" };
            return new ObjectResult(model);
        }
    }

- the client will receive 
      {"id":1,"name":"The House of Kobe"}


-------------------------------------------------------------------------------------------
RENDERING VIEWS
-------------------------------------------------------------------------------------------

     public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var listOfRestaurant = new List<Restaurant>();
            listOfRestaurant.Add(new Restaurant { Id = 1, Name = "The House of Kobe" });
            return View(listOfRestaurant);

        }
    }

-------------------------------------------------------------------------------------------
A TABLE FULL OF RESTAURANTS
-------------------------------------------------------------------------------------------
- He create a "Services" folder for all the INTERACTIONS WITH THE DATABASE

##########################################################
in preparation to "DEPENDENCY INJECTION" he create a class to interact with DATA and straight an INTERFACE that I will pass to the CONTROLLER that will never see for example the SQL IMPLEMENTATION. 
for instance: //LD STEP15
##########################################################

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

- DEPENDENCY INJECTION //LD STEP16

- - MIN 4:00 he shows how to SET THE CONTROLLER, go in "Startup.cs" -> "ConfigureServices" method

      services.AddScoped<IRestaurantData, InMemoryRestaurantData>();

"AddScoped" mean that I will have a new instance for any HTTP REQUEST

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


# !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

------------------------------------------------------------------------------------------
Models in the MVC framework
------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------
MODELS AND VIEW MODELS
------------------------------------------------------------------------------------------
########################################################
he just show HOW, WHEN to use it and give an EXAMPLE 
########################################################

- he do the DIFFERENCE between ENTITY MODEL that is exactly the schema of the database and VIEW MODEL(or DTO) that is what we will render in html in our VIEW, so what the user will see
- - a DTO can show informations from two ENTITY MODELS
 for instance
- - - so WE DON'T PERSIST THE "DTO"

- PRATICAL EXAMPLE min3:00 --> //LD SPEP17

------------------------------------------------------------------------------------------
DETAIL A RESTAURANT
------------------------------------------------------------------------------------------
//STEP18 
- add a property to "RestaurantData.cs" repositoty and return it. We are using it in the "details" action of the "Home" controller.

- min6:00 he add the html for the "details.cshtml" view

- min9:00 in redirect with C# 6 we can use the "nameof" operator

------------------------------------------------------------------------------------------
CREATE A RESTAURANT, ACCEPTING FORM INPUT, POST REDIRECT GET PATTERN
------------------------------------------------------------------------------------------
- he use the "RestaurantEditViewModel.cs"

- min 6:00 of "ACCEPTING FORM INPUT" he set the list of restaurant "STATIC" and the constructor NON PUBLIC

# !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

----------------------------------------------------------------------------------------------
ENTITY FRAMEWORK
----------------------------------------------------------------------------------------------
###########################################################
STEP1 WHEN WE HAVE TO USE A DATABASE
###########################################################
- I executed the console command
     
      install-package Microsoft.entityFrameworkCore.SqlServer
      install-package Microsoft.entityFrameworkCore.Tools -Pre

and then in my "project.json" setting file I got:

      "Microsoft.EntityFrameworkCore.SqlServer": "1.1.0"
      "Microsoft.EntityFrameworkCore.Tools": "1.1.0-preview4-final"

----------------------------------------------------------------------------------------------
IMPLEMENTING A DB CONTEXT

########################################################
note that the repository class get automatically the "context" "OdeToFoodDbContext", it's enought the setting in "Startup.cs"

       public class SqlRestaurantData : IRestaurantData
       {
             private OdeToFoodDbContext _context;

             public SqlRestaurantData(OdeToFoodDbContext context)
             {
                   _context = context;
             }

########################################################
----------------------------------------------------------------------------------------------
- EACH "DbContext" class will give me access to a single database, by chanhing CONNECTION STRING we can point to a different specific database.

- inside "DbContext" we create "DbSet<T>" where "T" is a type of entity. In order to refer to a specific MODEL. EACH "DbSet" will MAP a TABLE in the database
- - In this example a "DbSet" will be "restaurant".

- MIN 1:58 he insert the "DbContext" class //LD STEP20

        //LD STEP20
        public DbSet<Restaurant> Restaurants { get; set; }

- MIN 3:24, now we will create a second implementation of "IRestaurantData" that will work with the database

- MIN 6:00 "Context.SaveChanges"

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