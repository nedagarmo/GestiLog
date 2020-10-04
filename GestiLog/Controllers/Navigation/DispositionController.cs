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
    public class DispositionController : Controller
    {
        private DaoEntities db = new DaoEntities();

        // GET: Disposition
        [Authorize]
        public ActionResult Index()
        {
            return View(db.Disposicion.ToList());
        }

        // GET: Disposition/Details/5
        [Authorize]
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Disposicion disposicion = db.Disposicion.Find(id);
            if (disposicion == null)
            {
                return HttpNotFound();
            }
            return View(disposicion);
        }

        // GET: Disposition/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Disposition/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nombre,S_Usuario,S_Creacion,Habilitado")] Disposicion disposicion)
        {
            if (ModelState.IsValid)
            {
                disposicion.Id = Guid.NewGuid();
                disposicion.S_Creacion = DateTime.Now;
                disposicion.S_Usuario = Guid.Parse(User.Identity.GetUserId());
                db.Disposicion.Add(disposicion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(disposicion);
        }

        // GET: Disposition/Edit/5
        [Authorize]
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Disposicion disposicion = db.Disposicion.Find(id);
            if (disposicion == null)
            {
                return HttpNotFound();
            }
            return View(disposicion);
        }

        // POST: Disposition/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nombre,S_Usuario,S_Creacion,Habilitado")] Disposicion disposicion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(disposicion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(disposicion);
        }

        // GET: Disposition/Delete/5
        [Authorize]
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Disposicion disposicion = db.Disposicion.Find(id);
            if (disposicion == null)
            {
                return HttpNotFound();
            }
            return View(disposicion);
        }

        // POST: Disposition/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Disposicion disposicion = db.Disposicion.Find(id);
            db.Disposicion.Remove(disposicion);
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
