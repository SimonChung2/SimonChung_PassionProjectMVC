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
    public class CarModelDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/CarModelData/ListCarModels
        [HttpGet]
        public IEnumerable<CarModel> ListCarModels()
        {
            List<CarModel> carModels = db.CarModels.ToList();
            return carModels;
        }

        // GET: api/CarModelData/FindCarModel/3
        [ResponseType(typeof(CarModel))]
        [HttpGet]
        public IHttpActionResult FindCarModel(int id)
        {
            CarModel carModel = db.CarModels.Find(id);
            if (carModel == null)
            {
                return NotFound();
            }

            return Ok(carModel);
        }

        // POST: api/CarModelData/UpdateCarModel/3
        [ResponseType(typeof(void))]
        [HttpPost]
        [Authorize]
        public IHttpActionResult UpdateCarModel(int id, CarModel carModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != carModel.ModelID)
            {
                return BadRequest();
            }

            db.Entry(carModel).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CarModelExists(id))
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

        // POST: api/CarModelData/AddCarModel
        [ResponseType(typeof(CarModel))]
        [HttpPost]
        [Authorize]

        public IHttpActionResult AddCarModel(CarModel carModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.CarModels.Add(carModel);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = carModel.ModelID }, carModel);
        }

        // POST: api/CarModelData/DeleteCarModel/3
        [ResponseType(typeof(CarModel))]
        [HttpPost]
        [Authorize]

        public IHttpActionResult DeleteCarModel(int id)
        {
            CarModel carModel = db.CarModels.Find(id);
            if (carModel == null)
            {
                return NotFound();
            }

            db.CarModels.Remove(carModel);
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

        private bool CarModelExists(int id)
        {
            return db.CarModels.Count(e => e.ModelID == id) > 0;
        }
    }
}