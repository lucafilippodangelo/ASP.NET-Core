using Microsoft.AspNetCore.Mvc;
using OdeToFood.Entities;
using OdeToFood.Services;
using OdeToFood.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OdeToFood.Controllers
{

    public class HomeController : Controller
    {
        private IRestaurantData _restaurantData;
        private IGreeter _greeter;

        public HomeController(IRestaurantData restaurantData, IGreeter greeter)//LD STEP16
        {
            _restaurantData = restaurantData;
            _greeter = greeter;
        }
            
        public IActionResult Index()
        {
            ////LD OPTION1
            //LD TEST15
            //var listOfRestaurant = new List<Restaurant>();
            //listOfRestaurant.Add(new Restaurant { Id = 1, Name = "The House of Kobe" });
            //return View(listOfRestaurant);

            ////LD OPTION2
            //var listOfRestaurant = _restaurantData.GetAll();
            //return View(listOfRestaurant);

            //LD OPTION3 
            //LD SPEP17 I do assignement for the submodels
            var model = new HomePageViewModel();
            model.Restaurants = _restaurantData.GetAll();
            model.CurrentMessage = _greeter.GetGreeting();

            return View(model);

        }
 

        public IActionResult Details(int id)
        {
            var model = _restaurantData.Get(id);
            if (model == null)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(RestaurantEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var newRestaurant = new Restaurant();
                newRestaurant.Cuisine = model.Cuisine;
                newRestaurant.Name = model.Name;
                newRestaurant = _restaurantData.Add(newRestaurant);

                return RedirectToAction("Details", new { id = newRestaurant.Id });
            }
            return View();
        }


    }




}
