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
    public class FreightsController : Controller
    {
        private DaoEntities db = new DaoEntities();

        // GET: Freights
        [Authorize]
        public ActionResult Index()
        {
            return View(db.Flete.ToList());
        }

        // GET: Freights/Details/5
        [Authorize]
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Flete flete = db.Flete.Find(id);
            if (flete == null)
            {
                return HttpNotFound();
            }
            return View(flete);
        }

        // GET: Freights/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Freights/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nombre,S_Usuario,S_Creacion,Habilitado")] Flete flete)
        {
            if (ModelState.IsValid)
            {
                flete.Id = Guid.NewGuid();
                flete.S_Creacion = DateTime.Now;
                flete.S_Usuario = Guid.Parse(User.Identity.GetUserId());
                db.Flete.Add(flete);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(flete);
        }

        // GET: Freights/Edit/5
        [Authorize]
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Flete flete = db.Flete.Find(id);
            if (flete == null)
            {
                return HttpNotFound();
            }
            return View(flete);
        }

        // POST: Freights/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nombre,S_Usuario,S_Creacion,Habilitado")] Flete flete)
        {
            if (ModelState.IsValid)
            {
                db.Entry(flete).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(flete);
        }

        // GET: Freights/Delete/5
        [Authorize]
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Flete flete = db.Flete.Find(id);
            if (flete == null)
            {
                return HttpNotFound();
            }
            return View(flete);
        }

        // POST: Freights/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Flete flete = db.Flete.Find(id);
            db.Flete.Remove(flete);
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
