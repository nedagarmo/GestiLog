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
    public class OfficeTypeController : Controller
    {
        private DaoEntities db = new DaoEntities();

        // GET: OfficeType
        [Authorize]
        public ActionResult Index()
        {
            return View(db.TipoOficina.ToList());
        }

        // GET: OfficeType/Details/5
        [Authorize]
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoOficina tipoOficina = db.TipoOficina.Find(id);
            if (tipoOficina == null)
            {
                return HttpNotFound();
            }
            return View(tipoOficina);
        }

        // GET: OfficeType/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: OfficeType/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nombre,Codigo,S_Usuario,S_Creacion,Habilitado")] TipoOficina tipoOficina)
        {
            if (ModelState.IsValid)
            {
                tipoOficina.Id = Guid.NewGuid();
                tipoOficina.S_Creacion = DateTime.Now;
                tipoOficina.S_Usuario = Guid.Parse(User.Identity.GetUserId());
                db.TipoOficina.Add(tipoOficina);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tipoOficina);
        }

        // GET: OfficeType/Edit/5
        [Authorize]
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoOficina tipoOficina = db.TipoOficina.Find(id);
            if (tipoOficina == null)
            {
                return HttpNotFound();
            }
            return View(tipoOficina);
        }

        // POST: OfficeType/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nombre,Codigo,S_Usuario,S_Creacion,Habilitado")] TipoOficina tipoOficina)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tipoOficina).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tipoOficina);
        }

        // GET: OfficeType/Delete/5
        [Authorize]
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoOficina tipoOficina = db.TipoOficina.Find(id);
            if (tipoOficina == null)
            {
                return HttpNotFound();
            }
            return View(tipoOficina);
        }

        // POST: OfficeType/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            TipoOficina tipoOficina = db.TipoOficina.Find(id);
            db.TipoOficina.Remove(tipoOficina);
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
