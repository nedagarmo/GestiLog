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
    public class ContainerTypeController : Controller
    {
        private DaoEntities db = new DaoEntities();

        // GET: ContainerType
        [Authorize]
        public ActionResult Index()
        {
            return View(db.TipoContenedor.ToList());
        }

        // GET: ContainerType/Details/5
        [Authorize]
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoContenedor tipoContenedor = db.TipoContenedor.Find(id);
            if (tipoContenedor == null)
            {
                return HttpNotFound();
            }
            return View(tipoContenedor);
        }

        // GET: ContainerType/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: ContainerType/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Descripcion,Codigo,Capacidad,S_Usuario,S_Creacion,Habilitado")] TipoContenedor tipoContenedor)
        {
            if (ModelState.IsValid)
            {
                tipoContenedor.Id = Guid.NewGuid();
                tipoContenedor.S_Creacion = DateTime.Now;
                tipoContenedor.S_Usuario = Guid.Parse(User.Identity.GetUserId());
                db.TipoContenedor.Add(tipoContenedor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tipoContenedor);
        }

        // GET: ContainerType/Edit/5
        [Authorize]
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoContenedor tipoContenedor = db.TipoContenedor.Find(id);
            if (tipoContenedor == null)
            {
                return HttpNotFound();
            }
            return View(tipoContenedor);
        }

        // POST: ContainerType/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Descripcion,Codigo,Capacidad,S_Usuario,S_Creacion,Habilitado")] TipoContenedor tipoContenedor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tipoContenedor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tipoContenedor);
        }

        // GET: ContainerType/Delete/5
        [Authorize]
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoContenedor tipoContenedor = db.TipoContenedor.Find(id);
            if (tipoContenedor == null)
            {
                return HttpNotFound();
            }
            return View(tipoContenedor);
        }

        // POST: ContainerType/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            TipoContenedor tipoContenedor = db.TipoContenedor.Find(id);
            db.TipoContenedor.Remove(tipoContenedor);
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
