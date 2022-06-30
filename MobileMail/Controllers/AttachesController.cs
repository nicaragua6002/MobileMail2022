using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MobileMail.Models;

namespace MobileMail.Controllers
{
    public class AttachesController : Controller
    {
        private MobileMailContainer db = new MobileMailContainer();

        // GET: Attaches
        public ActionResult Index()
        {
            var attaches = db.Attaches.Include(a => a.Mail);
            return View(attaches.ToList());
        }

        // GET: Attaches/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Attach attach = db.Attaches.Find(id);
            if (attach == null)
            {
                return HttpNotFound();
            }
            return View(attach);
        }

        // GET: Attaches/Create
        public ActionResult Create()
        {
            ViewBag.MailId = new SelectList(db.Mails, "Id", "From");
            return View();
        }

        // POST: Attaches/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Value,Type,MailId")] Attach attach)
        {
            if (ModelState.IsValid)
            {
                db.Attaches.Add(attach);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MailId = new SelectList(db.Mails, "Id", "From", attach.MailId);
            return View(attach);
        }

        // GET: Attaches/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Attach attach = db.Attaches.Find(id);
            if (attach == null)
            {
                return HttpNotFound();
            }
            ViewBag.MailId = new SelectList(db.Mails, "Id", "From", attach.MailId);
            return View(attach);
        }

        // POST: Attaches/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Value,Type,MailId")] Attach attach)
        {
            if (ModelState.IsValid)
            {
                db.Entry(attach).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MailId = new SelectList(db.Mails, "Id", "From", attach.MailId);
            return View(attach);
        }

        // GET: Attaches/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Attach attach = db.Attaches.Find(id);
            if (attach == null)
            {
                return HttpNotFound();
            }
            return View(attach);
        }

        // POST: Attaches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Attach attach = db.Attaches.Find(id);
            db.Attaches.Remove(attach);
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
