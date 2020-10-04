using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GestiLog.Entities;

namespace GestiLog.Controllers.Navigation
{
    public class ContainerController : Controller
    {
        private DaoEntities db = new DaoEntities();

        // GET: Container
        [Authorize]
        public ActionResult Index(Guid id)
        {
            Session["Mbl"] = db.Mbl.Where(w => w.Id == id).FirstOrDefault();
            ViewBag.Mbl = ((Mbl)Session["Mbl"]).Do;
            ViewBag.MblId = ((Mbl)Session["Mbl"]).Id;
            var contenedor = db.Contenedor.Where(w => w.Mbl == id).Include(c => c.MblNav).Include(c => c.TipoContenedorNav);
            return View(contenedor.ToList());
        }

        // GET: Container/Details/5
        [Authorize]
        public ActionResult Details(Guid? id)
        {
            ViewBag.Mbl = ((Mbl)Session["Mbl"]).Do;
            ViewBag.MblId = ((Mbl)Session["Mbl"]).Id;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contenedor contenedor = db.Contenedor.Where(w => w.Id == id).Include(i => i.TipoContenedorNav).FirstOrDefault();
            if (contenedor == null)
            {
                return HttpNotFound();
            }
            return View(contenedor);
        }

        // GET: Container/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.Mbl = ((Mbl)Session["Mbl"]).Do;
            ViewBag.MblId = ((Mbl)Session["Mbl"]).Id;
            ViewBag.Equipo = new SelectList(db.TipoContenedor, "Id", "Descripcion");
            return View();
        }

        // POST: Container/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Mbl,Equipo,Numero,Serial,Observacion")] Contenedor contenedor)
        {
            if (ModelState.IsValid)
            {
                contenedor.Id = Guid.NewGuid();
                contenedor.Mbl = ((Mbl)Session["Mbl"]).Id;
                db.Contenedor.Add(contenedor);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = contenedor.Mbl });
            }

            ViewBag.Mbl = ((Mbl)Session["Mbl"]).Do;
            ViewBag.MblId = ((Mbl)Session["Mbl"]).Id;
            ViewBag.Equipo = new SelectList(db.TipoContenedor, "Id", "Descripcion", contenedor.Equipo);
            return View(contenedor);
        }

        // GET: Container/Edit/5
        [Authorize]
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contenedor contenedor = db.Contenedor.Find(id);
            if (contenedor == null)
            {
                return HttpNotFound();
            }
            ViewBag.Mbl = ((Mbl)Session["Mbl"]).Do;
            ViewBag.MblId = ((Mbl)Session["Mbl"]).Id;
            ViewBag.Equipo = new SelectList(db.TipoContenedor, "Id", "Descripcion", contenedor.Equipo);
            return View(contenedor);
        }

        // POST: Container/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Mbl,Equipo,Numero,Serial,Observacion")] Contenedor contenedor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(contenedor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { id = contenedor.Mbl });
            }
            ViewBag.Mbl = ((Mbl)Session["Mbl"]).Do;
            ViewBag.MblId = ((Mbl)Session["Mbl"]).Id;
            ViewBag.Equipo = new SelectList(db.TipoContenedor, "Id", "Descripcion", contenedor.Equipo);
            return View(contenedor);
        }

        // GET: Container/Delete/5
        [Authorize]
        public ActionResult Delete(Guid? id)
        {
            ViewBag.Mbl = ((Mbl)Session["Mbl"]).Do;
            ViewBag.MblId = ((Mbl)Session["Mbl"]).Id;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contenedor contenedor = db.Contenedor.Where(w => w.Id == id).Include(i => i.TipoContenedorNav).FirstOrDefault();
            if (contenedor == null)
            {
                return HttpNotFound();
            }
            return View(contenedor);
        }

        // POST: Container/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Contenedor contenedor = db.Contenedor.Find(id);
            db.Contenedor.Remove(contenedor);
            db.SaveChanges();
            return RedirectToAction("Index", new { id = contenedor.Mbl });
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
