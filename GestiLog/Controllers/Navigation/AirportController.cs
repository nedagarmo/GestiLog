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
    public class AirportController : Controller
    {
        private DaoEntities db = new DaoEntities();

        // GET: Airport
        [Authorize]
        public ActionResult Index()
        {
            var aeropuerto = db.Aeropuerto.Include(a => a.CiudadNav);
            return View(aeropuerto.ToList());
        }

        // GET: Airport/Details/5
        [Authorize]
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Aeropuerto aeropuerto = db.Aeropuerto.Find(id);
            if (aeropuerto == null)
            {
                return HttpNotFound();
            }
            return View(aeropuerto);
        }

        // GET: Airport/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.Ciudad = new SelectList(db.Ciudad, "Id", "Nombre");
            return View();
        }

        // POST: Airport/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nombre,Codigo,Ciudad,S_Usuario,S_Creacion,Habilitado")] Aeropuerto aeropuerto)
        {
            if (ModelState.IsValid)
            {
                aeropuerto.Id = Guid.NewGuid();
                aeropuerto.S_Creacion = DateTime.Now;
                aeropuerto.S_Usuario = Guid.Parse(User.Identity.GetUserId());
                db.Aeropuerto.Add(aeropuerto);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Ciudad = new SelectList(db.Ciudad, "Id", "Nombre", aeropuerto.Ciudad);
            return View(aeropuerto);
        }

        // GET: Airport/Edit/5
        [Authorize]
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Aeropuerto aeropuerto = db.Aeropuerto.Find(id);
            if (aeropuerto == null)
            {
                return HttpNotFound();
            }
            ViewBag.Ciudad = new SelectList(db.Ciudad, "Id", "Nombre", aeropuerto.Ciudad);
            return View(aeropuerto);
        }

        // POST: Airport/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nombre,Codigo,Ciudad,S_Usuario,S_Creacion,Habilitado")] Aeropuerto aeropuerto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aeropuerto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Ciudad = new SelectList(db.Ciudad, "Id", "Nombre", aeropuerto.Ciudad);
            return View(aeropuerto);
        }

        // GET: Airport/Delete/5
        [Authorize]
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Aeropuerto aeropuerto = db.Aeropuerto.Find(id);
            if (aeropuerto == null)
            {
                return HttpNotFound();
            }
            return View(aeropuerto);
        }

        // POST: Airport/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Aeropuerto aeropuerto = db.Aeropuerto.Find(id);
            db.Aeropuerto.Remove(aeropuerto);
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
