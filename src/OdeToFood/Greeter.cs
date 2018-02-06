using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OdeToFood
{
    public interface IGreeter
    {
        string GetGreeting();
    }

    public class Greeter : IGreeter
    {
        private string _greeting;

        //LD we will pass the "configuration" whan instanciated. 
        // in this case is instanciated by dependency injection in "Startup.cs"
        // and receive "Configuration" because I declared in "startup.cs" -> "services.AddSingleton(Configuration);"
        // By doing this all the classes that need of "Configuration" will receive it.
        // In my case when in the "Configure" of "startup.cs" I want an "IGreeter" it will be instanciate and automatically 
        //.net will pass the "Configuration" object. 
        public Greeter(IConfiguration configuration)
        {
            _greeting =configuration["Greeting"];
        }

        public string GetGreeting()
        {
            return _greeting;
        }
    } 
}
