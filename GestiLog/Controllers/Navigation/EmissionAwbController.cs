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
    public class EmissionAwbController : Controller
    {
        private DaoEntities db = new DaoEntities();

        // GET: EmissionAwb
        public ActionResult Index()
        {
            return View(db.EmisionAwb.ToList());
        }

        // GET: EmissionAwb/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmisionAwb emisionAwb = db.EmisionAwb.Find(id);
            if (emisionAwb == null)
            {
                return HttpNotFound();
            }
            return View(emisionAwb);
        }

        // GET: EmissionAwb/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EmissionAwb/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nombre,S_Usuario,S_Creacion,Habilitado")] EmisionAwb emisionAwb)
        {
            if (ModelState.IsValid)
            {
                emisionAwb.Id = Guid.NewGuid();
                emisionAwb.S_Creacion = DateTime.Now;
                emisionAwb.S_Usuario = Guid.Parse(User.Identity.GetUserId());
                db.EmisionAwb.Add(emisionAwb);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(emisionAwb);
        }

        // GET: EmissionAwb/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmisionAwb emisionAwb = db.EmisionAwb.Find(id);
            if (emisionAwb == null)
            {
                return HttpNotFound();
            }
            return View(emisionAwb);
        }

        // POST: EmissionAwb/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nombre,S_Usuario,S_Creacion,Habilitado")] EmisionAwb emisionAwb)
        {
            if (ModelState.IsValid)
            {
                db.Entry(emisionAwb).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(emisionAwb);
        }

        // GET: EmissionAwb/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmisionAwb emisionAwb = db.EmisionAwb.Find(id);
            if (emisionAwb == null)
            {
                return HttpNotFound();
            }
            return View(emisionAwb);
        }

        // POST: EmissionAwb/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            EmisionAwb emisionAwb = db.EmisionAwb.Find(id);
            db.EmisionAwb.Remove(emisionAwb);
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
