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

namespace GestiLog.Controllers
{
    public class PackagingController : Controller
    {
        private DaoEntities db = new DaoEntities();

        // GET: Packaging
        [Authorize]
        public ActionResult Index()
        {
            return View(db.Embalaje.ToList());
        }

        // GET: Packaging/Details/5
        [Authorize]
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Embalaje embalaje = db.Embalaje.Find(id);
            if (embalaje == null)
            {
                return HttpNotFound();
            }
            return View(embalaje);
        }

        // GET: Packaging/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Packaging/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "Id,Descripcion,Codigo,S_Usuario,S_Creacion,Habilitado")] Embalaje embalaje)
        {
            if (ModelState.IsValid)
            {
                embalaje.Id = Guid.NewGuid();
                embalaje.S_Creacion = DateTime.Now;
                embalaje.S_Usuario = Guid.Parse(User.Identity.GetUserId());
                db.Embalaje.Add(embalaje);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(embalaje);
        }

        // GET: Packaging/Edit/5
        [Authorize]
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Embalaje embalaje = db.Embalaje.Find(id);
            if (embalaje == null)
            {
                return HttpNotFound();
            }
            return View(embalaje);
        }

        // POST: Packaging/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "Id,Descripcion,Codigo,S_Usuario,S_Creacion,Habilitado")] Embalaje embalaje)
        {
            if (ModelState.IsValid)
            {
                db.Entry(embalaje).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(embalaje);
        }

        // GET: Packaging/Delete/5
        [Authorize]
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Embalaje embalaje = db.Embalaje.Find(id);
            if (embalaje == null)
            {
                return HttpNotFound();
            }
            return View(embalaje);
        }

        // POST: Packaging/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Embalaje embalaje = db.Embalaje.Find(id);
            db.Embalaje.Remove(embalaje);
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
