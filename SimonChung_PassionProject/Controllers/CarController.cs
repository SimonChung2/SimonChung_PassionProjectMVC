using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Diagnostics;
using SimonChung_PassionProject.Models;
using System.Web.Script.Serialization;
using SimonChung_PassionProject.Models.ViewModels;

namespace SimonChung_PassionProject.Controllers
{
    public class CarController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static CarController()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false,
                //cookies are manually set in RequestHeader
                UseCookies = false
            };

            client = new HttpClient(handler);
            client.BaseAddress = new Uri("https://localhost:44366/api/");
        }

        //Code for GetApplicationCookie() method was retrieved from:
        //https://github.com/christinebittle/ZooApplication_5/blob/master/ZooApplication/Controllers/AnimalController.cs
        //Author: Christine Bittle
        /// <summary>
        /// Grabs the authentication cookie sent to this controller.
        /// For proper WebAPI authentication, you can send a post request with login credentials to the WebAPI and log the access token from the response. The controller already knows this token, so we're just passing it up the chain.
        /// 
        /// Here is a descriptive article which walks through the process of setting up authorization/authentication directly.
        /// https://docs.microsoft.com/en-us/aspnet/web-api/overview/security/individual-accounts-in-web-api
        /// </summary>
        private void GetApplicationCookie()
        {
            string token = "";
            //HTTP client is set up to be reused, otherwise it will exhaust server resources.
            //This is a bit dangerous because a previously authenticated cookie could be cached for
            //a follow-up request from someone else. Reset cookies in HTTP client before grabbing a new one.
            client.DefaultRequestHeaders.Remove("Cookie");
            if (!User.Identity.IsAuthenticated) return;

            HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies.Get(".AspNet.ApplicationCookie");
            if (cookie != null) token = cookie.Value;

            //collect token as it is submitted to the controller
            //use it to pass along to the WebAPI.
            Debug.WriteLine("Token Submitted is : " + token);
            if (token != "") client.DefaultRequestHeaders.Add("Cookie", ".AspNet.ApplicationCookie=" + token);

            return;
        }

        // GET: Car/List
        public ActionResult List()
        {
            //objective: communicate with out car data api to retrieve a list of cars
            //curl https://localhost:44366/api/cardata/listcars

          
            string url = "cardata/listcars";
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
           // Debug.WriteLine(response.StatusCode);

            IEnumerable<CarDto> cars = response.Content.ReadAsAsync<IEnumerable<CarDto>>().Result;

          //  Debug.WriteLine("Number of cars received:");
           // Debug.WriteLine(cars.Count());

            return View(cars);
        }

        // GET: Car/Details/5
        public ActionResult Details(int id)
        {
            //objective: communicate with out car data api to retrieve one car
            //curl https://localhost:44366/api/cardata/findcar/{id}

            
            string url = "cardata/findcar/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            CarDto selectedcar = response.Content.ReadAsAsync<CarDto>().Result;

            //Debug.WriteLine("car received:");
            //Debug.WriteLine(selectedcar.CarID);
            
            return View(selectedcar);
        }

        public ActionResult Error()
        {
            return View();
        }

        // GET: Car/New
        [Authorize]
        public ActionResult New()
        {
            GetApplicationCookie();//get token credentials
            CreateCar ViewModel= new CreateCar();
            //information about all the car models in the system
            //GET api/carmodeldata/listcarmodels
            string url = "carmodeldata/listcarmodels";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<CarModel> CarModelOptions = response.Content.ReadAsAsync<IEnumerable<CarModel>>().Result;
            ViewModel.CarModelOptions = CarModelOptions;

            //GET api/dealerdata/listdealers
            url = "dealerdata/listdealers";
            response = client.GetAsync(url).Result;
            IEnumerable<Dealer> DealerOptions = response.Content.ReadAsAsync<IEnumerable<Dealer>>().Result;
            ViewModel.DealerOptions = DealerOptions;

            return View(ViewModel);
        }

        // POST: Car/Create
        [HttpPost]
        [Authorize]
        public ActionResult Create(Car car)
        {
            GetApplicationCookie();//get token credentials
            Debug.WriteLine("The JSON payload is: ");
            //Debug.WriteLine(car.Year);
            //objective: add a new car into our system using the API
            //curl -H "Content-Type:application/json" -d @car.json https://localhost:44366/api/cardata/addcar
            string url = "cardata/addcar";

            string jsonpayload = jss.Serialize(car);

            Debug.WriteLine(jsonpayload);

            HttpContent content= new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType= "application/json";


            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");

            } else
            {
                return RedirectToAction("Error");
            }

        }

        // GET: Car/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            GetApplicationCookie();//get token credentials
            UpdateCar ViewModel = new UpdateCar();
            //the existing car information
            string url = "cardata/findcar/" + id;         
            HttpResponseMessage response = client.GetAsync(url).Result;
            CarDto SelectedCar = response.Content.ReadAsAsync<CarDto>().Result;
            ViewModel.SelectedCar = SelectedCar;


            //all carmodels to choose from when updating this car

            url = "carmodeldata/listcarmodels/";
            response = client.GetAsync(url).Result;
            IEnumerable<CarModel> CarModelOptions = response.Content.ReadAsAsync<IEnumerable<CarModel>>().Result;
            ViewModel.CarModelOptions = CarModelOptions;

            //all dealers to choose from when updating this car
            url = "dealerdata/listdealers/";
            response = client.GetAsync(url).Result;
            IEnumerable<Dealer> DealerOptions = response.Content.ReadAsAsync<IEnumerable<Dealer>>().Result;
            ViewModel.DealerOptions = DealerOptions;

            return View(ViewModel);
        }

        // POST: Car/Update/5
        [HttpPost]
        public ActionResult Update(int id, Car car)
        {
            string url = "cardata/updatecar/" + id;
            string jsonpayload = jss.Serialize(car);
            HttpContent content = new StringContent(jsonpayload);   
            content.Headers.ContentType.MediaType= "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            { 
                return RedirectToAction("Error"); 
            }

        }

        // GET: Car/ConfirmDelete/5
        [Authorize]
        public ActionResult ConfirmDelete(int id)
        {
            GetApplicationCookie();//get token credentials
            string url = "cardata/findcar/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            CarDto selectedcar = response.Content.ReadAsAsync<CarDto>().Result;
            return View(selectedcar);
        }

        // POST: Car/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "cardata/deletecar/" + id;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
    }
}
