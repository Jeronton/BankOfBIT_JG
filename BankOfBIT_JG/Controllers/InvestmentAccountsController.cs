﻿using System;
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
    public class InvestmentAccountsController : Controller
    {
        private BankOfBIT_JGContext db = new BankOfBIT_JGContext();

        // GET: InvestmentAccounts
        public ActionResult Index()
        {
            var bankAccounts = db.InvestmentAccounts.Include(i => i.AccountState).Include(i => i.Client);
            return View(bankAccounts.ToList());
        }

        // GET: InvestmentAccounts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InvestmentAccount investmentAccount = db.InvestmentAccounts.Find(id);
            if (investmentAccount == null)
            {
                return HttpNotFound();
            }
            return View(investmentAccount);
        }

        // GET: InvestmentAccounts/Create
        public ActionResult Create()
        {
            // *JR Modified the display member
            ViewBag.AccountStateId = new SelectList(db.AccountStates, "AccountStateId", "Description");
            ViewBag.ClientId = new SelectList(db.Clients, "ClientId", "FullName");
            return View();
        }

        // POST: InvestmentAccounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BankAccountId,AccountNumber,ClientId,AccountStateId,Balance,OpeningBalance,DateCreated,Notes,InterestRate")] InvestmentAccount investmentAccount)
        {
            if (ModelState.IsValid)
            {
                // *JR modified
                investmentAccount.SetNextAccountNumber();
                db.InvestmentAccounts.Add(investmentAccount);
                db.SaveChanges();
                investmentAccount.ChangeState();
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            // *JR Modified DataTextField
            ViewBag.AccountStateId = new SelectList(db.AccountStates, "AccountStateId", "Description", investmentAccount.AccountStateId);
            ViewBag.ClientId = new SelectList(db.Clients, "ClientId", "FullName", investmentAccount.ClientId);
            return View(investmentAccount);
        }

        // GET: InvestmentAccounts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InvestmentAccount investmentAccount = db.InvestmentAccounts.Find(id);
            if (investmentAccount == null)
            {
                return HttpNotFound();
            }
            // *JR Modified DataTextField
            ViewBag.AccountStateId = new SelectList(db.AccountStates, "AccountStateId", "Description", investmentAccount.AccountStateId);
            ViewBag.ClientId = new SelectList(db.Clients, "ClientId", "FullName", investmentAccount.ClientId);
            return View(investmentAccount);
        }

        // POST: InvestmentAccounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BankAccountId,AccountNumber,ClientId,AccountStateId,Balance,OpeningBalance,DateCreated,Notes,InterestRate")] InvestmentAccount investmentAccount)
        {
            if (ModelState.IsValid)
            {
                db.Entry(investmentAccount).State = EntityState.Modified;
                db.SaveChanges();
                investmentAccount.ChangeState();
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            // *JR Modified DataTextField
            ViewBag.AccountStateId = new SelectList(db.AccountStates, "AccountStateId", "Description", investmentAccount.AccountStateId);
            ViewBag.ClientId = new SelectList(db.Clients, "ClientId", "FullName", investmentAccount.ClientId);
            return View(investmentAccount);
        }

        // GET: InvestmentAccounts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InvestmentAccount investmentAccount = db.InvestmentAccounts.Find(id);
            if (investmentAccount == null)
            {
                return HttpNotFound();
            }
            return View(investmentAccount);
        }

        // POST: InvestmentAccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            InvestmentAccount investmentAccount = db.InvestmentAccounts.Find(id);
            db.InvestmentAccounts.Remove(investmentAccount);
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
