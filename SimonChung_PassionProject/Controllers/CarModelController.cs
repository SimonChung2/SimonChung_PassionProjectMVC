using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using SimonChung_PassionProject.Models;
using SimonChung_PassionProject.Models.ViewModels;


namespace SimonChung_PassionProject.Controllers
{
    public class CarModelController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static CarModelController()
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

        // GET: CarModel/List
        public ActionResult List()
        {
            //objective: to communicate with carmodeldata api method to retrieve list of car models
            //curl https://localhost:44366/api/carmodeldata/listcarmodels

            
            string url = "carmodeldata/listcarmodels";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<CarModel> carModels = response.Content.ReadAsAsync<IEnumerable<CarModel>>().Result;
            Debug.WriteLine("Number of car models:");
            Debug.WriteLine(carModels.Count());
            return View(carModels);
        }

        // GET: CarModel/Details/5
        public ActionResult Details(int id)
        {
            //objective: communicate with out car data api to retrieve one car
            //curl https://localhost:44366/api/carmodeldata/findcarmodel/{id}

            DetailsCarModel ViewModel = new DetailsCarModel();

            string url = "carmodeldata/findcarmodel/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            CarModel SelectedCarModel = response.Content.ReadAsAsync<CarModel>().Result;



            ViewModel.SelectedCarModel = SelectedCarModel;
            //showcase information about cars related to this car model
            //send a request to gather info about cars related to a particular car model id
            url = "cardata/listcarsforcarmodel/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<CarDto> RelatedCars = response.Content.ReadAsAsync<IEnumerable<CarDto>>().Result; 

            ViewModel.RelatedCars = RelatedCars;

            return View(ViewModel);
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
            return View();
        }

        // GET: CarModel/Create
        [HttpPost]
        [Authorize]
        public ActionResult Create(CarModel carmodel)
        {
            GetApplicationCookie();//get token credentials
            Debug.WriteLine("The JSON payload is: ");
            //Debug.WriteLine(car.Year);
            //objective: add a new car into our system using the API
            //curl -H "Content-Type:application/json" -d @car.json https://localhost:44366/api/carmodeldata/addcarmodel
            string url = "carmodeldata/addcarmodel";

            string jsonpayload = jss.Serialize(carmodel);

            Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
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


        // GET: CarModel/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            GetApplicationCookie();//get token credentials
            string url = "carmodeldata/findcarmodel/" + id;

            HttpResponseMessage response = client.GetAsync(url).Result;
            CarModel selectedcarmodel = response.Content.ReadAsAsync<CarModel>().Result;

            return View(selectedcarmodel);
        }

        // POST: CarModel/Update/5
        [HttpPost]
        [Authorize]
        public ActionResult Update(int id, CarModel carmodel)
        {
            GetApplicationCookie();//get token credentials
            string url = "carmodeldata/updatecarmodel/" + id;
            string jsonpayload = jss.Serialize(carmodel);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
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

        // GET: CarModel/Delete/5
        [Authorize]
        public ActionResult ConfirmDelete(int id)
        {
            GetApplicationCookie();//get token credentials
            string url = "carmodeldata/findcarmodel/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            CarModel selectedcarmodel = response.Content.ReadAsAsync<CarModel>().Result;
            return View(selectedcarmodel);
        }

        // POST: CarModel/Delete/5
        [HttpPost]
        [Authorize]
        public ActionResult Delete(int id)
        {
            GetApplicationCookie();//get token credentials
            string url = "carmodeldata/deletecarmodel/" + id;
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
