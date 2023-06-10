using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Diagnostics;
using SimonChung_PassionProject.Models;
using System.Web.Script.Serialization;

namespace SimonChung_PassionProject.Controllers
{
    public class CarController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static CarController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44366/api/cardata/");
        }
        // GET: Car/List
        public ActionResult List()
        {
            //objective: communicate with out car data api to retrieve a list of cars
            //curl https://localhost:44366/api/cardata/listcars

          
            string url = "listcars";
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The response code is ");
            Debug.WriteLine(response.StatusCode);

            IEnumerable<CarDto> cars = response.Content.ReadAsAsync<IEnumerable<CarDto>>().Result;

            Debug.WriteLine("Number of cars received:");
            Debug.WriteLine(cars.Count());

            return View(cars);
        }

        // GET: Car/Details/5
        public ActionResult Details(int id)
        {
            //objective: communicate with out car data api to retrieve one car
            //curl https://localhost:44366/api/cardata/findcar/{id}

            
            string url = "findcar/" +id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The response code is ");
            Debug.WriteLine(response.StatusCode);

            CarDto selectedcar = response.Content.ReadAsAsync<CarDto>().Result;

            Debug.WriteLine("car received:");
            Debug.WriteLine(selectedcar.CarID);
            
            return View(selectedcar);
        }

        public ActionResult Error()
        {
            return View();
        }

        // GET: Car/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Car/Create
        [HttpPost]
        public ActionResult Create(Car car)
        {
            Debug.WriteLine("The JSON payload is: ");
            //Debug.WriteLine(car.Year);
            //objective: add a new car into our system using the API
            //curl -H "Content-Type:application/json" -d @car.json https://localhost:44366/api/cardata/addcar
            string url = "addcar";

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
        public ActionResult Edit(int id)
        {
            string url = "cardata/findcar/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            CarDto selectedcar = response.Content.ReadAsAsync<CarDto>().Result;
            return View(selectedcar);
        }

        // POST: Car/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Car/ConfirmDelete/5
        public ActionResult ConfirmDelete(int id)
        {
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
