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
        /// <summary>
        /// Returns all car models in the system
        /// </summary>
        /// <returns>
        /// CONTENT: all car models in the database
        /// </returns>
        /// <example>
        /// GET: api/CarModelData/ListCarModels
        /// </example>

        [HttpGet]
        public IEnumerable<CarModel> ListCarModels()
        {
            List<CarModel> carModels = db.CarModels.ToList();
            return carModels;
        }

        /// <summary>
        /// Returns a car model in the system
        /// </summary>
        /// <param name="id">ModelID</param>
        /// <returns>
        /// CONTENT: A car model in the system matching with the ModelID primary key
        /// </returns>
        /// <example>
        /// GET: api/CarModelData/FindCarModel/3
        /// </example>

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

        /// <summary>
        /// Updates a particular car model in the system with POST Data input
        /// </summary>
        /// <param name="id">ModelID</param>
        /// <param name="carModel">JSON form data of a car model</param>
        /// <returns>
        /// Status codes: 204 (Success), 400 (Bad Request), 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/CarModelData/UpdateCarModel/3
        /// FORM DATA; Car Model JSON Object
        /// </example>
        
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

        /// <summary>
        /// Adds a car model to the system
        /// </summary>
        /// <param name="carModel">JSON form data of a car</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: ModelID, Car Model Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/CarModelData/AddCarModel
        /// FORM DATA: Car Model Json Object
        /// </example>

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

        /// <summary>
        ///  Deletes a car model from the system according to it's ID
        /// </summary>
        /// <param name="id">ModelID</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 400 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/CarModelData/DeleteCarModel/3
        /// </example>

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