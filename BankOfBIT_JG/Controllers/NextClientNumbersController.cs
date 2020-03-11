using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BankOfBIT_JG.Models;

namespace BankOfBIT_JG.Controllers
{
    public class NextClientNumbersController : Controller
    {
        private BankOfBIT_JGContext db = new BankOfBIT_JGContext();

        // GET: NextClientNumbers
        public ActionResult Index()
        {
            return View(NextClientNumber.GetInstance());
        }

        // GET: NextClientNumbers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NextClientNumber nextClientNumber = db.NextClientNumbers.Find(id);
            if (nextClientNumber == null)
            {
                return HttpNotFound();
            }
            return View(nextClientNumber);
        }

        // GET: NextClientNumbers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: NextClientNumbers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "NextClientNumberId,NextAvailableNumber")] NextClientNumber nextClientNumber)
        {
            if (ModelState.IsValid)
            {
                db.NextClientNumbers.Add(nextClientNumber);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(nextClientNumber);
        }

        // GET: NextClientNumbers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NextClientNumber nextClientNumber = db.NextClientNumbers.Find(id);
            if (nextClientNumber == null)
            {
                return HttpNotFound();
            }
            return View(nextClientNumber);
        }

        // POST: NextClientNumbers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "NextClientNumberId,NextAvailableNumber")] NextClientNumber nextClientNumber)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nextClientNumber).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(nextClientNumber);
        }

        // GET: NextClientNumbers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NextClientNumber nextClientNumber = db.NextClientNumbers.Find(id);
            if (nextClientNumber == null)
            {
                return HttpNotFound();
            }
            return View(nextClientNumber);
        }

        // POST: NextClientNumbers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NextClientNumber nextClientNumber = db.NextClientNumbers.Find(id);
            db.NextClientNumbers.Remove(nextClientNumber);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
