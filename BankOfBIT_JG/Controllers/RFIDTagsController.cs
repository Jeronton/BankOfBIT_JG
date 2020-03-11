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
    public class RFIDTagsController : Controller
    {
        private BankOfBIT_JGContext db = new BankOfBIT_JGContext();

        // GET: RFIDTags
        public ActionResult Index()
        {
            var rFIDTags = db.RFIDTags.Include(r => r.Client);
            return View(rFIDTags.ToList());
        }

        // GET: RFIDTags/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RFIDTag rFIDTag = db.RFIDTags.Find(id);
            if (rFIDTag == null)
            {
                return HttpNotFound();
            }
            return View(rFIDTag);
        }

        // GET: RFIDTags/Create
        public ActionResult Create()
        {
            ViewBag.ClientId = new SelectList(db.Clients, "ClientId", "FullName");
            return View();
        }

        // POST: RFIDTags/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RFIDTagId,CardNumber,ClientId")] RFIDTag rFIDTag)
        {
            if (ModelState.IsValid)
            {
                db.RFIDTags.Add(rFIDTag);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ClientId = new SelectList(db.Clients, "ClientId", "FullName", rFIDTag.ClientId);
            return View(rFIDTag);
        }

        // GET: RFIDTags/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RFIDTag rFIDTag = db.RFIDTags.Find(id);
            if (rFIDTag == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClientId = new SelectList(db.Clients, "ClientId", "FullName", rFIDTag.ClientId);
            return View(rFIDTag);
        }

        // POST: RFIDTags/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RFIDTagId,CardNumber,ClientId")] RFIDTag rFIDTag)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rFIDTag).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClientId = new SelectList(db.Clients, "ClientId", "FullName", rFIDTag.ClientId);
            return View(rFIDTag);
        }

        // GET: RFIDTags/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RFIDTag rFIDTag = db.RFIDTags.Find(id);
            if (rFIDTag == null)
            {
                return HttpNotFound();
            }
            return View(rFIDTag);
        }

        // POST: RFIDTags/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RFIDTag rFIDTag = db.RFIDTags.Find(id);
            db.RFIDTags.Remove(rFIDTag);
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
