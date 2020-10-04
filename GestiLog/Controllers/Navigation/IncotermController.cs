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
    public class IncotermController : Controller
    {
        private DaoEntities db = new DaoEntities();

        // GET: Incoterm
        [Authorize]
        public ActionResult Index()
        {
            return View(db.Incoterm.ToList());
        }

        // GET: Incoterm/Details/5
        [Authorize]
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Incoterm incoterm = db.Incoterm.Find(id);
            if (incoterm == null)
            {
                return HttpNotFound();
            }
            return View(incoterm);
        }

        // GET: Incoterm/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Incoterm/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Descripcion,Codigo,Observacion,S_Usuario,S_Creacion,Habilitado")] Incoterm incoterm)
        {
            if (ModelState.IsValid)
            {
                incoterm.Id = Guid.NewGuid();
                incoterm.S_Creacion = DateTime.Now;
                incoterm.S_Usuario = Guid.Parse(User.Identity.GetUserId());
                db.Incoterm.Add(incoterm);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(incoterm);
        }

        // GET: Incoterm/Edit/5
        [Authorize]
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Incoterm incoterm = db.Incoterm.Find(id);
            if (incoterm == null)
            {
                return HttpNotFound();
            }
            return View(incoterm);
        }

        // POST: Incoterm/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Descripcion,Codigo,Observacion,S_Usuario,S_Creacion,Habilitado")] Incoterm incoterm)
        {
            if (ModelState.IsValid)
            {
                db.Entry(incoterm).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(incoterm);
        }

        // GET: Incoterm/Delete/5
        [Authorize]
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Incoterm incoterm = db.Incoterm.Find(id);
            if (incoterm == null)
            {
                return HttpNotFound();
            }
            return View(incoterm);
        }

        // POST: Incoterm/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Incoterm incoterm = db.Incoterm.Find(id);
            db.Incoterm.Remove(incoterm);
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
