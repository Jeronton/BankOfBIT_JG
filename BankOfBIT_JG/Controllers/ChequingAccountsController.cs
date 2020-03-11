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
    public class ChequingAccountsController : Controller
    {
        private BankOfBIT_JGContext db = new BankOfBIT_JGContext();

        // GET: ChequingAccounts
        public ActionResult Index()
        {
            var bankAccounts = db.ChequingAccounts.Include(c => c.AccountState).Include(c => c.Client);
            return View(bankAccounts.ToList());
        }

        // GET: ChequingAccounts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChequingAccount chequingAccount = db.ChequingAccounts.Find(id);
            if (chequingAccount == null)
            {
                return HttpNotFound();
            }
            return View(chequingAccount);
        }

        // GET: ChequingAccounts/Create
        public ActionResult Create()
        {
            // *JR Modified the display member
            ViewBag.AccountStateId = new SelectList(db.AccountStates, "AccountStateId", "Description");
            ViewBag.ClientId = new SelectList(db.Clients, "ClientId", "FullName");
            return View();
        }

        // POST: ChequingAccounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BankAccountId,AccountNumber,ClientId,AccountStateId,Balance,OpeningBalance,DateCreated,Notes,ChequingServiceCharges")] ChequingAccount chequingAccount)
        {
            if (ModelState.IsValid)
            {
                chequingAccount.SetNextAccountNumber();
                db.ChequingAccounts.Add(chequingAccount);
                db.SaveChanges();
                chequingAccount.ChangeState();
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            // *JR Modified DataTextField
            ViewBag.AccountStateId = new SelectList(db.AccountStates, "AccountStateId", "Description", chequingAccount.AccountStateId);
            ViewBag.ClientId = new SelectList(db.Clients, "ClientId", "FullName", chequingAccount.ClientId);
            return View(chequingAccount);
        }

        // GET: ChequingAccounts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChequingAccount chequingAccount = db.ChequingAccounts.Find(id);
            if (chequingAccount == null)
            {
                return HttpNotFound();
            }
            // *JR Modified DataTextField
            ViewBag.AccountStateId = new SelectList(db.AccountStates, "AccountStateId", "Description", chequingAccount.AccountStateId);
            ViewBag.ClientId = new SelectList(db.Clients, "ClientId", "FullName", chequingAccount.ClientId);
            return View(chequingAccount);
        }

        // POST: ChequingAccounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BankAccountId,AccountNumber,ClientId,AccountStateId,Balance,OpeningBalance,DateCreated,Notes,ChequingServiceCharges")] ChequingAccount chequingAccount)
        {
            if (ModelState.IsValid)
            {
                db.Entry(chequingAccount).State = EntityState.Modified;
                db.SaveChanges();
                chequingAccount.ChangeState();
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            // *JR Modified DataTextField
            ViewBag.AccountStateId = new SelectList(db.AccountStates, "AccountStateId", "Description", chequingAccount.AccountStateId);
            ViewBag.ClientId = new SelectList(db.Clients, "ClientId", "FullName", chequingAccount.ClientId);
            return View(chequingAccount);
        }

        // GET: ChequingAccounts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChequingAccount chequingAccount = db.ChequingAccounts.Find(id);
            if (chequingAccount == null)
            {
                return HttpNotFound();
            }
            return View(chequingAccount);
        }

        // POST: ChequingAccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ChequingAccount chequingAccount = db.ChequingAccounts.Find(id);
            db.ChequingAccounts.Remove(chequingAccount);
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
