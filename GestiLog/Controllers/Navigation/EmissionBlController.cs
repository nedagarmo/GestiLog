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
    public class EmissionBlController : Controller
    {
        private DaoEntities db = new DaoEntities();

        // GET: EmissionBl
        [Authorize]
        public ActionResult Index()
        {
            return View(db.EmisionBl.ToList());
        }

        // GET: EmissionBl/Details/5
        [Authorize]
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmisionBl emisionBl = db.EmisionBl.Find(id);
            if (emisionBl == null)
            {
                return HttpNotFound();
            }
            return View(emisionBl);
        }

        // GET: EmissionBl/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: EmissionBl/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nombre,S_Usuario,S_Creacion,Habilitado")] EmisionBl emisionBl)
        {
            if (ModelState.IsValid)
            {
                emisionBl.Id = Guid.NewGuid();
                emisionBl.S_Creacion = DateTime.Now;
                emisionBl.S_Usuario = Guid.Parse(User.Identity.GetUserId());
                db.EmisionBl.Add(emisionBl);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(emisionBl);
        }

        // GET: EmissionBl/Edit/5
        [Authorize]
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmisionBl emisionBl = db.EmisionBl.Find(id);
            if (emisionBl == null)
            {
                return HttpNotFound();
            }
            return View(emisionBl);
        }

        // POST: EmissionBl/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nombre,S_Usuario,S_Creacion,Habilitado")] EmisionBl emisionBl)
        {
            if (ModelState.IsValid)
            {
                db.Entry(emisionBl).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(emisionBl);
        }

        // GET: EmissionBl/Delete/5
        [Authorize]
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmisionBl emisionBl = db.EmisionBl.Find(id);
            if (emisionBl == null)
            {
                return HttpNotFound();
            }
            return View(emisionBl);
        }

        // POST: EmissionBl/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            EmisionBl emisionBl = db.EmisionBl.Find(id);
            db.EmisionBl.Remove(emisionBl);
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
