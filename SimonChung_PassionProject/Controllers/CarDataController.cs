using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
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

        // GET: api/CarData/ListCars
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

        // GET: api/CarData/FindCar/3
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

        // POST: api/CarData/UpdateCar/3
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateCar(int id, Car car)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != car.CarID)
            {
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
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/CarData/AddCar
        [ResponseType(typeof(Car))]
        [HttpPost]

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

        // POST: api/CarData/DeleteCar/3
        [ResponseType(typeof(Car))]
        [HttpPost]
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