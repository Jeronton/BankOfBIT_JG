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
    public class NextTransactionNumbersController : Controller
    {
        private BankOfBIT_JGContext db = new BankOfBIT_JGContext();

        // GET: NextTransactionNumbers
        public ActionResult Index()
        {
            return View(NextTransactionNumber.GetInstance());
        }

        // GET: NextTransactionNumbers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NextTransactionNumber nextTransactionNumber = db.NextTransactionNumbers.Find(id);
            if (nextTransactionNumber == null)
            {
                return HttpNotFound();
            }
            return View(nextTransactionNumber);
        }

        // GET: NextTransactionNumbers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: NextTransactionNumbers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "NextTransactionNumberId,NextAvailableNumber")] NextTransactionNumber nextTransactionNumber)
        {
            if (ModelState.IsValid)
            {
                db.NextTransactionNumbers.Add(nextTransactionNumber);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(nextTransactionNumber);
        }

        // GET: NextTransactionNumbers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NextTransactionNumber nextTransactionNumber = db.NextTransactionNumbers.Find(id);
            if (nextTransactionNumber == null)
            {
                return HttpNotFound();
            }
            return View(nextTransactionNumber);
        }

        // POST: NextTransactionNumbers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "NextTransactionNumberId,NextAvailableNumber")] NextTransactionNumber nextTransactionNumber)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nextTransactionNumber).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(nextTransactionNumber);
        }

        // GET: NextTransactionNumbers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NextTransactionNumber nextTransactionNumber = db.NextTransactionNumbers.Find(id);
            if (nextTransactionNumber == null)
            {
                return HttpNotFound();
            }
            return View(nextTransactionNumber);
        }

        // POST: NextTransactionNumbers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NextTransactionNumber nextTransactionNumber = db.NextTransactionNumbers.Find(id);
            db.NextTransactionNumbers.Remove(nextTransactionNumber);
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
