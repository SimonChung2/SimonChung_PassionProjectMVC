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

        // GET: api/DealerData/ListDealers
        [HttpGet]
        public IEnumerable<Dealer> ListDealers()
        {
            List<Dealer> dealers = db.Dealers.ToList();
            return dealers;
        }

        // GET: api/DealerData/FindDealer/3
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

        // POST: api/DealerData/UpdateDealer/3
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

        // POST: api/DealerData/AddDealer
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

        // POST: api/DealerData/DeleteDealer/3
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