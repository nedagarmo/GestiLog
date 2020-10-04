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
    public class BoardingTypeController : Controller
    {
        private DaoEntities db = new DaoEntities();

        // GET: BoardingType
        [Authorize]
        public ActionResult Index()
        {
            return View(db.TipoEmbarque.ToList());
        }

        // GET: BoardingType/Details/5
        [Authorize]
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoEmbarque tipoEmbarque = db.TipoEmbarque.Find(id);
            if (tipoEmbarque == null)
            {
                return HttpNotFound();
            }
            return View(tipoEmbarque);
        }

        // GET: BoardingType/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: BoardingType/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nombre,S_Usuario,S_Creacion,Habilitado")] TipoEmbarque tipoEmbarque)
        {
            if (ModelState.IsValid)
            {
                tipoEmbarque.Id = Guid.NewGuid();
                tipoEmbarque.S_Creacion = DateTime.Now;
                tipoEmbarque.S_Usuario = Guid.Parse(User.Identity.GetUserId());
                db.TipoEmbarque.Add(tipoEmbarque);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tipoEmbarque);
        }

        // GET: BoardingType/Edit/5
        [Authorize]
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoEmbarque tipoEmbarque = db.TipoEmbarque.Find(id);
            if (tipoEmbarque == null)
            {
                return HttpNotFound();
            }
            return View(tipoEmbarque);
        }

        // POST: BoardingType/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nombre,S_Usuario,S_Creacion,Habilitado")] TipoEmbarque tipoEmbarque)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tipoEmbarque).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tipoEmbarque);
        }

        // GET: BoardingType/Delete/5
        [Authorize]
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoEmbarque tipoEmbarque = db.TipoEmbarque.Find(id);
            if (tipoEmbarque == null)
            {
                return HttpNotFound();
            }
            return View(tipoEmbarque);
        }

        // POST: BoardingType/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            TipoEmbarque tipoEmbarque = db.TipoEmbarque.Find(id);
            db.TipoEmbarque.Remove(tipoEmbarque);
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
