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
    public class MotorShipController : Controller
    {
        private DaoEntities db = new DaoEntities();

        // GET: MotorShip
        [Authorize]
        public ActionResult Index()
        {
            return View(db.Motonave.ToList());
        }

        // GET: MotorShip/Details/5
        [Authorize]
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Motonave motonave = db.Motonave.Find(id);
            if (motonave == null)
            {
                return HttpNotFound();
            }
            return View(motonave);
        }

        // GET: MotorShip/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: MotorShip/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nombre,S_Usuario,S_Creacion,Habilitado")] Motonave motonave)
        {
            if (ModelState.IsValid)
            {
                motonave.Id = Guid.NewGuid();
                motonave.S_Creacion = DateTime.Now;
                motonave.S_Usuario = Guid.Parse(User.Identity.GetUserId());
                db.Motonave.Add(motonave);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(motonave);
        }

        // GET: MotorShip/Edit/5
        [Authorize]
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Motonave motonave = db.Motonave.Find(id);
            if (motonave == null)
            {
                return HttpNotFound();
            }
            return View(motonave);
        }

        // POST: MotorShip/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nombre,S_Usuario,S_Creacion,Habilitado")] Motonave motonave)
        {
            if (ModelState.IsValid)
            {
                db.Entry(motonave).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(motonave);
        }

        // GET: MotorShip/Delete/5
        [Authorize]
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Motonave motonave = db.Motonave.Find(id);
            if (motonave == null)
            {
                return HttpNotFound();
            }
            return View(motonave);
        }

        // POST: MotorShip/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Motonave motonave = db.Motonave.Find(id);
            db.Motonave.Remove(motonave);
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
