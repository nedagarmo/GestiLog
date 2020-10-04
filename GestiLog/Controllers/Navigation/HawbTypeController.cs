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
    public class HawbTypeController : Controller
    {
        private DaoEntities db = new DaoEntities();

        // GET: HawbType
        [Authorize]
        public ActionResult Index()
        {
            return View(db.HawbTipo.ToList());
        }

        // GET: HawbType/Details/5
        [Authorize]
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HawbTipo hawbTipo = db.HawbTipo.Find(id);
            if (hawbTipo == null)
            {
                return HttpNotFound();
            }
            return View(hawbTipo);
        }

        // GET: HawbType/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: HawbType/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nombre,S_Usuario,S_Creacion,Habilitado")] HawbTipo hawbTipo)
        {
            if (ModelState.IsValid)
            {
                hawbTipo.Id = Guid.NewGuid();
                hawbTipo.S_Creacion = DateTime.Now;
                hawbTipo.S_Usuario = Guid.Parse(User.Identity.GetUserId());
                db.HawbTipo.Add(hawbTipo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(hawbTipo);
        }

        // GET: HawbType/Edit/5
        [Authorize]
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HawbTipo hawbTipo = db.HawbTipo.Find(id);
            if (hawbTipo == null)
            {
                return HttpNotFound();
            }
            return View(hawbTipo);
        }

        // POST: HawbType/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nombre,S_Usuario,S_Creacion,Habilitado")] HawbTipo hawbTipo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hawbTipo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(hawbTipo);
        }

        // GET: HawbType/Delete/5
        [Authorize]
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HawbTipo hawbTipo = db.HawbTipo.Find(id);
            if (hawbTipo == null)
            {
                return HttpNotFound();
            }
            return View(hawbTipo);
        }

        // POST: HawbType/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            HawbTipo hawbTipo = db.HawbTipo.Find(id);
            db.HawbTipo.Remove(hawbTipo);
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
