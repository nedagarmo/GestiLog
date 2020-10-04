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
    public class PortController : Controller
    {
        private DaoEntities db = new DaoEntities();

        // GET: Port
        [Authorize]
        public ActionResult Index()
        {
            var puerto = db.Puerto.Include(p => p.PaisNav);
            return View(puerto.ToList());
        }

        // GET: Port/Details/5
        [Authorize]
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Puerto puerto = db.Puerto.Find(id);
            if (puerto == null)
            {
                return HttpNotFound();
            }
            return View(puerto);
        }

        // GET: Port/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.Pais = new SelectList(db.Pais, "Id", "Nombre");
            return View();
        }

        // POST: Port/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "Id,Nombre,Codigo,Pais,S_Usuario,S_Creacion,Habilitado")] Puerto puerto)
        {
            if (ModelState.IsValid)
            {
                puerto.Id = Guid.NewGuid();
                puerto.S_Creacion = DateTime.Now;
                puerto.S_Usuario = Guid.Parse(User.Identity.GetUserId());
                db.Puerto.Add(puerto);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Pais = new SelectList(db.Pais, "Id", "Nombre", puerto.Pais);
            return View(puerto);
        }

        // GET: Port/Edit/5
        [Authorize]
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Puerto puerto = db.Puerto.Find(id);
            if (puerto == null)
            {
                return HttpNotFound();
            }
            ViewBag.Pais = new SelectList(db.Pais, "Id", "Nombre", puerto.Pais);
            return View(puerto);
        }

        // POST: Port/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "Id,Nombre,Codigo,Pais,S_Usuario,S_Creacion,Habilitado")] Puerto puerto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(puerto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Pais = new SelectList(db.Pais, "Id", "Nombre", puerto.Pais);
            return View(puerto);
        }

        // GET: Port/Delete/5
        [Authorize]
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Puerto puerto = db.Puerto.Find(id);
            if (puerto == null)
            {
                return HttpNotFound();
            }
            return View(puerto);
        }

        // POST: Port/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Puerto puerto = db.Puerto.Find(id);
            db.Puerto.Remove(puerto);
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
