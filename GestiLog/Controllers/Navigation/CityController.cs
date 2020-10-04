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
    public class CityController : Controller
    {
        private DaoEntities db = new DaoEntities();

        // GET: City
        [Authorize]
        public ActionResult Index()
        {
            var ciudad = db.Ciudad.Include(c => c.PaisNav);
            return View(ciudad.ToList());
        }

        // GET: City/Details/5
        [Authorize]
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ciudad ciudad = db.Ciudad.Find(id);
            if (ciudad == null)
            {
                return HttpNotFound();
            }
            return View(ciudad);
        }

        // GET: City/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.Pais = new SelectList(db.Pais, "Id", "Nombre");
            return View();
        }

        // POST: City/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nombre,Codigo,Pais,S_Usuario,S_Creacion,Habilitado")] Ciudad ciudad)
        {
            if (ModelState.IsValid)
            {
                ciudad.Id = Guid.NewGuid();
                ciudad.S_Creacion = DateTime.Now;
                ciudad.S_Usuario = Guid.Parse(User.Identity.GetUserId());
                db.Ciudad.Add(ciudad);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Pais = new SelectList(db.Pais, "Id", "Nombre", ciudad.Pais);
            return View(ciudad);
        }

        // GET: City/Edit/5
        [Authorize]
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ciudad ciudad = db.Ciudad.Find(id);
            if (ciudad == null)
            {
                return HttpNotFound();
            }
            ViewBag.Pais = new SelectList(db.Pais, "Id", "Nombre", ciudad.Pais);
            return View(ciudad);
        }

        // POST: City/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nombre,Codigo,Pais,S_Usuario,S_Creacion,Habilitado")] Ciudad ciudad)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ciudad).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Pais = new SelectList(db.Pais, "Id", "Nombre", ciudad.Pais);
            return View(ciudad);
        }

        // GET: City/Delete/5
        [Authorize]
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ciudad ciudad = db.Ciudad.Find(id);
            if (ciudad == null)
            {
                return HttpNotFound();
            }
            return View(ciudad);
        }

        // POST: City/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Ciudad ciudad = db.Ciudad.Find(id);
            db.Ciudad.Remove(ciudad);
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
