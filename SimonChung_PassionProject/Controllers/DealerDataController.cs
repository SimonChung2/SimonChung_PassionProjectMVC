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
    public class DealerDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all dealers in the system
        /// </summary>
        /// <returns>
        /// CONTENT: all dealers in the database
        /// </returns>
        /// <example>
        /// GET: api/DealerData/ListDealers
        /// </example>

        [HttpGet]
        public IEnumerable<Dealer> ListDealers()
        {
            List<Dealer> dealers = db.Dealers.ToList();
            return dealers;
        }

        /// <summary>
        /// Returns a dealer in the system
        /// </summary>
        /// <param name="id">DealerID</param>
        /// <returns>
        /// CONTENT: A dealer in the system matching with the DealerID primary key
        /// </returns>
        /// <example>
        /// GET: api/DealerData/FindDealer/3
        /// </example>

        [ResponseType(typeof(Dealer))]
        [HttpGet]
        public IHttpActionResult FindDealer(int id)
        {
            Dealer dealer = db.Dealers.Find(id);
            if (dealer == null)
            {
                return NotFound();
            }

            return Ok(dealer);
        }

        /// <summary>
        /// Updates a particular dealer in the system with POST Data input
        /// </summary>
        /// <param name="id">DealerID</param>
        /// <param name="dealer">JSON form data of a dealer</param>
        /// <returns>
        /// Status codes: 204 (Success), 400 (Bad Request), 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/DealerData/UpdateDealer/3
        /// FORM DATA; Dealer JSON Object
        /// </example>

        [ResponseType(typeof(void))]
        [HttpPost]
        [Authorize]
        public IHttpActionResult UpdateDealer(int id, Dealer dealer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != dealer.DealerID)
            {
                return BadRequest();
            }

            db.Entry(dealer).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DealerExists(id))
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
        /// Adds a dealer to the system
        /// </summary>
        /// <param name="dealer">JSON form data of a car</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: DealerID, Dealer Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/DealerData/AddDealer
        /// FORM DATA: Dealer Json Object
        /// </example>

        [ResponseType(typeof(Dealer))]
        [HttpPost]
        [Authorize]
        public IHttpActionResult AddDealer(Dealer dealer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Dealers.Add(dealer);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = dealer.DealerID }, dealer);
        }

        /// <summary>
        ///  Deletes a dealer from the system according to it's ID
        /// </summary>
        /// <param name="id">DealerID</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 400 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/DealerData/DeleteDealer/3
        /// </example>

        [ResponseType(typeof(Dealer))]
        [HttpPost]
        [Authorize]
        public IHttpActionResult DeleteDealer(int id)
        {
            Dealer dealer = db.Dealers.Find(id);
            if (dealer == null)
            {
                return NotFound();
            }

            db.Dealers.Remove(dealer);
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

        private bool DealerExists(int id)
        {
            return db.Dealers.Count(e => e.DealerID == id) > 0;
        }
    }
}