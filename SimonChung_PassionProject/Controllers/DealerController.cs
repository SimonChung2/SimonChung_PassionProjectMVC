using SimonChung_PassionProject.Models;
using SimonChung_PassionProject.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace SimonChung_PassionProject.Controllers
{
    public class DealerController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static DealerController()
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

        // GET: Dealer/List
        public ActionResult List()
        {
            //objective: to communicate with dealerdata api method to retrieve list of dealers
            //curl https://localhost:44366/api/dealerdata/listdealers


            string url = "dealerdata/listdealers";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<Dealer> dealers = response.Content.ReadAsAsync<IEnumerable<Dealer>>().Result;
            //Debug.WriteLine("Number of car models:");
            //Debug.WriteLine(dealers.Count());


            return View(dealers);
        }

        // GET: Dealer/Details/5
        public ActionResult Details(int id)
        {
            //objective: communicate with out car data api to retrieve one car
            //curl https://localhost:44366/api/dealerdata/finddealer/{id}

            DetailsDealer ViewModel = new DetailsDealer();

            string url = "dealerdata/finddealer/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            Dealer SelectedDealer = response.Content.ReadAsAsync<Dealer>().Result;
            ViewModel.SelectedDealer= SelectedDealer;


            url = "cardata/listcarsfordealer/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<CarDto> RelatedCars = response.Content.ReadAsAsync<IEnumerable<CarDto>>().Result;
            ViewModel.RelatedCars = RelatedCars;

            return View(ViewModel);
        }

        public ActionResult Error()
        {
            return View();
        }

        // GET: Dealer/New
        [Authorize]
        public ActionResult New()
        {
            GetApplicationCookie();//get token credentials
            return View();
        }

        // GET: Dealer/Create
        [HttpPost]
        [Authorize] 
        public ActionResult Create(Dealer dealer)
        {
            GetApplicationCookie();//get token credentials
            Debug.WriteLine("The JSON payload is: ");
            //Debug.WriteLine(car.Year);
            //objective: add a new dealer into our system using the API
            //curl -H "Content-Type:application/json" -d @dealer.json https://localhost:44366/api/dealerdata/adddealer
            string url = "dealerdata/adddealer";

            string jsonpayload = jss.Serialize(dealer);

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


        // GET: Dealer/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            GetApplicationCookie();//get token credentials
            string url = "dealerdata/finddealer/" + id;

            HttpResponseMessage response = client.GetAsync(url).Result;
            Dealer selecteddealer = response.Content.ReadAsAsync<Dealer>().Result;

            return View(selecteddealer);
        }

        // POST: Dealer/Update/5
        [HttpPost]
        [Authorize]
        public ActionResult Update(int id, Dealer dealer)
        {
            GetApplicationCookie();//get token credentials
            string url = "dealerdata/updatedealer/" + id;
            string jsonpayload = jss.Serialize(dealer);
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

        // GET: Dealer/Delete/5
        [Authorize]
        public ActionResult ConfirmDelete(int id)
        {
            GetApplicationCookie();//get token credentials
            string url = "dealerdata/finddealer/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            Dealer selecteddealer = response.Content.ReadAsAsync<Dealer>().Result;
            return View(selecteddealer);
        }

        // POST: Dealer/Delete/5
        [HttpPost]
        [Authorize]
        public ActionResult Delete(int id)
        {
            GetApplicationCookie();//get token credentials
            string url = "dealerdata/deletedealer/" + id;
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
