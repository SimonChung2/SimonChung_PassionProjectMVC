using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using SimonChung_PassionProject.Models;

namespace SimonChung_PassionProject.Controllers
{
    public class CarDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        /// <summary>
        /// Returns all cars in the system
        /// </summary>
        /// <returns>
        /// CONTENT: all cars in the database, including their associated car models and dealers
        /// </returns>
        /// <example>
        /// GET: api/CarData/ListCars
        /// </example>
        [HttpGet]
        public IEnumerable<CarDto> ListCars()
        {
           List<Car> Cars = db.Cars.ToList();
           List<CarDto> carDtos= new List<CarDto>();

            Cars.ForEach(a => carDtos.Add(new CarDto()
            {
                CarID=a.CarID,
                Year=a.Year,
                Price=a.Price,
                Mileage=a.Mileage,

                CarModelName= a.CarModels.ModelName,
                CarMakeName= a.CarModels.Make,

                DealerName= a.Dealers.DealerName

            }));
            return carDtos;
        }

        /// <summary>
        /// Gathers information about cars of a particular car model
        /// </summary>
        /// <returns>
        /// CONTENT: cars in the database that match with a particular car model ID
        /// </returns>
        /// <param name="id"> ModelID</param>
        /// <example>
        /// GET: api/CarData/ListCarsForCarModel
        /// </example>

        [HttpGet]
        public IEnumerable<CarDto> ListCarsForCarModel(int id)
        {
            List<Car> Cars = db.Cars.Where(a=>a.ModelID==id).ToList();
            List<CarDto> carDtos = new List<CarDto>();

            Cars.ForEach(a => carDtos.Add(new CarDto()
            {
                CarID = a.CarID,
                Year = a.Year,
                Price = a.Price,
                Mileage = a.Mileage,

                CarModelName = a.CarModels.ModelName,
                CarMakeName = a.CarModels.Make,

                DealerName = a.Dealers.DealerName

            }));
            return carDtos;
        }

        /// <summary>
        /// Gathers information about cars available at a particular dealer
        /// </summary>
        /// <returns>
        /// CONTENT: cars in the database that match with a particular dealer ID
        /// </returns>
        /// <param name="id"> DealerID</param>
        /// <example>
        /// GET: api/CarData/ListCarsForDealer
        /// </example>


        [HttpGet]
        public IEnumerable<CarDto> ListCarsForDealer(int id)
        {
            List<Car> Cars = db.Cars.Where(a => a.DealerID == id).ToList();
            List<CarDto> carDtos = new List<CarDto>();

            Cars.ForEach(a => carDtos.Add(new CarDto()
            {
                CarID = a.CarID,
                Year = a.Year,
                Price = a.Price,
                Mileage = a.Mileage,

                CarModelName = a.CarModels.ModelName,
                CarMakeName = a.CarModels.Make,

                DealerName = a.Dealers.DealerName

            }));
            return carDtos;
        }

        /// <summary>
        /// Returns a car in the system
        /// </summary>
        /// <param name="id">CarID</param>
        /// <returns>
        /// CONTENT: A car in the system matching with the CarID primary key
        /// </returns>
        /// <example>
        /// GET: api/CarData/FindCar/3
        /// </example>

        [ResponseType(typeof(Car))]
        [HttpGet]
        public IHttpActionResult FindCar(int id)
        {
            Car car = db.Cars.Find(id);
            CarDto carDto = new CarDto()
            {
                CarID = car.CarID,
                Year = car.Year,
                Price = car.Price,
                Mileage = car.Mileage,

                CarModelName = car.CarModels.ModelName,
                CarMakeName = car.CarModels.Make,

                DealerName = car.Dealers.DealerName
            };
            if (car == null)
            {
                return NotFound();
            }

            return Ok(carDto);
        }


        /// <summary>
        /// Updates a particular car in the system with POST Data input
        /// </summary>
        /// <param name="id">CarID</param>
        /// <param name="car">JSON form data of a car</param>
        /// <returns>
        /// Status codes: 204 (Success), 400 (Bad Request), 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/CarData/UpdateCar/3
        /// FORM DATA; Car JSON Object
        /// </example>

        [ResponseType(typeof(void))]
        [HttpPost]
        [Authorize]
        public IHttpActionResult UpdateCar(int id, Car car)
        {
            if (!ModelState.IsValid)
            {
                Debug.WriteLine("ModelState Invalid");
                return BadRequest(ModelState);
            }

            if (id != car.CarID)
            {
                Debug.WriteLine("id variable != car.CarID" + id);
                return BadRequest();
            }

            db.Entry(car).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CarExists(id))
                {
                    Debug.WriteLine("Car with that ID not found");
                    return NotFound();
                }
                else
                {
                    Debug.WriteLine("Car found:" + id);
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Adds a car to the system
        /// </summary>
        /// <param name="car">JSON form data of a car</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: CarID, Car Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/CarData/AddCar
        /// FORM DATA: Car Json Object
        /// </example>

        [ResponseType(typeof(Car))]
        [HttpPost]
        [Authorize]

        public IHttpActionResult AddCar(Car car)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Cars.Add(car);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = car.CarID }, car);
        }

        /// <summary>
        ///  Deletes a car from the system according to it's ID
        /// </summary>
        /// <param name="id">CarID</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 400 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/CarData/DeleteCar/3
        /// </example>


        [ResponseType(typeof(Car))]
        [HttpPost]
        [Authorize]
        public IHttpActionResult DeleteCar(int id)
        {
            Car car = db.Cars.Find(id);
            if (car == null)
            {
                return NotFound();
            }

            db.Cars.Remove(car);
            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CarExists(int id)
        {
            return db.Cars.Count(e => e.CarID == id) > 0;
        }
    }
}