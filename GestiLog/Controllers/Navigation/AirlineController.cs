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
    public class AirlineController : Controller
    {
        private DaoEntities db = new DaoEntities();

        // GET: Airline
        [Authorize]
        public ActionResult Index()
        {
            var aerolinea = db.Aerolinea.Include(a => a.CiudadNav);
            return View(aerolinea.ToList());
        }

        // GET: Airline/Details/5
        [Authorize]
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Aerolinea aerolinea = db.Aerolinea.Where(w => w.Id == id).Include(i => i.CiudadNav).FirstOrDefault();
            if (aerolinea == null)
            {
                return HttpNotFound();
            }
            return View(aerolinea);
        }

        // GET: Airline/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.Ciudad = new SelectList(db.Ciudad, "Id", "Nombre");
            ViewBag.TarifarioEscala = new SelectList(db.TarifarioEscala, "Id", "Nombre");
            return View();
        }

        // POST: Airline/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nombre,Codigo,Nit,Dv,Direccion,Telefono,Correo,Ciudad,Representante,TarifarioEscala,S_Usuario,S_Creacion,Habilitado")] Aerolinea aerolinea)
        {
            if (ModelState.IsValid)
            {
                aerolinea.Id = Guid.NewGuid();
                aerolinea.S_Creacion = DateTime.Now;
                aerolinea.S_Usuario = Guid.Parse(User.Identity.GetUserId());
                db.Aerolinea.Add(aerolinea);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Ciudad = new SelectList(db.Ciudad, "Id", "Nombre", aerolinea.Ciudad);
            ViewBag.TarifarioEscala = new SelectList(db.TarifarioEscala, "Id", "Nombre", aerolinea.TarifarioEscala);
            return View(aerolinea);
        }

        // GET: Airline/Edit/5
        [Authorize]
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Aerolinea aerolinea = db.Aerolinea.Find(id);
            if (aerolinea == null)
            {
                return HttpNotFound();
            }
            ViewBag.Ciudad = new SelectList(db.Ciudad, "Id", "Nombre", aerolinea.Ciudad);
            ViewBag.TarifarioEscala = new SelectList(db.TarifarioEscala, "Id", "Nombre", aerolinea.TarifarioEscala);
            return View(aerolinea);
        }

        // POST: Airline/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nombre,Codigo,Nit,Dv,Direccion,Telefono,Correo,Ciudad,Representante,TarifarioEscala,S_Usuario,S_Creacion,Habilitado")] Aerolinea aerolinea)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aerolinea).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Ciudad = new SelectList(db.Ciudad, "Id", "Nombre", aerolinea.Ciudad);
            ViewBag.TarifarioEscala = new SelectList(db.TarifarioEscala, "Id", "Nombre", aerolinea.TarifarioEscala);
            return View(aerolinea);
        }

        // GET: Airline/Delete/5
        [Authorize]
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Aerolinea aerolinea = db.Aerolinea.Where(w => w.Id == id).Include(i => i.CiudadNav).FirstOrDefault();
            if (aerolinea == null)
            {
                return HttpNotFound();
            }
            return View(aerolinea);
        }

        // POST: Airline/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Aerolinea aerolinea = db.Aerolinea.Find(id);
            db.Aerolinea.Remove(aerolinea);
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
