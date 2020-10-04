using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GestiLog.Entities;
using Microsoft.AspNet.Identity;

namespace GestiLog.Controllers.Navigation
{
    public class PierController : Controller
    {
        private DaoEntities db = new DaoEntities();

        // GET: Pier
        [Authorize]
        public ActionResult Index()
        {
            return View(db.Muelle.ToList());
        }

        // GET: Pier/Details/5
        [Authorize]
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Muelle muelle = db.Muelle.Find(id);
            if (muelle == null)
            {
                return HttpNotFound();
            }
            return View(muelle);
        }

        // GET: Pier/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Pier/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nombre,S_Usuario,S_Creacion,Habilitado")] Muelle muelle)
        {
            if (ModelState.IsValid)
            {
                muelle.Id = Guid.NewGuid();
                muelle.S_Creacion = DateTime.Now;
                muelle.S_Usuario = Guid.Parse(User.Identity.GetUserId());
                db.Muelle.Add(muelle);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(muelle);
        }

        // GET: Pier/Edit/5
        [Authorize]
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Muelle muelle = db.Muelle.Find(id);
            if (muelle == null)
            {
                return HttpNotFound();
            }
            return View(muelle);
        }

        // POST: Pier/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nombre,S_Usuario,S_Creacion,Habilitado")] Muelle muelle)
        {
            if (ModelState.IsValid)
            {
                db.Entry(muelle).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(muelle);
        }

        // GET: Pier/Delete/5
        [Authorize]
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Muelle muelle = db.Muelle.Find(id);
            if (muelle == null)
            {
                return HttpNotFound();
            }
            return View(muelle);
        }

        // POST: Pier/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Muelle muelle = db.Muelle.Find(id);
            db.Muelle.Remove(muelle);
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
