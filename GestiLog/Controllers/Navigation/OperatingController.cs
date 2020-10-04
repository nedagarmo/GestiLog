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
    public class OperatingController : Controller
    {
        private DaoEntities db = new DaoEntities();

        // GET: Operating
        [Authorize]
        public ActionResult Index()
        {
            return View(db.Operativo.ToList());
        }

        // GET: Operating/Details/5
        [Authorize]
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Operativo operativo = db.Operativo.Find(id);
            if (operativo == null)
            {
                return HttpNotFound();
            }
            return View(operativo);
        }

        // GET: Operating/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Operating/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nombre,S_Usuario,S_Creacion,Habilitado")] Operativo operativo)
        {
            if (ModelState.IsValid)
            {
                operativo.Id = Guid.NewGuid();
                operativo.S_Creacion = DateTime.Now;
                operativo.S_Usuario = Guid.Parse(User.Identity.GetUserId());
                db.Operativo.Add(operativo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(operativo);
        }

        // GET: Operating/Edit/5
        [Authorize]
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Operativo operativo = db.Operativo.Find(id);
            if (operativo == null)
            {
                return HttpNotFound();
            }
            return View(operativo);
        }

        // POST: Operating/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nombre,S_Usuario,S_Creacion,Habilitado")] Operativo operativo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(operativo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(operativo);
        }

        // GET: Operating/Delete/5
        [Authorize]
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Operativo operativo = db.Operativo.Find(id);
            if (operativo == null)
            {
                return HttpNotFound();
            }
            return View(operativo);
        }

        // POST: Operating/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Operativo operativo = db.Operativo.Find(id);
            db.Operativo.Remove(operativo);
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
