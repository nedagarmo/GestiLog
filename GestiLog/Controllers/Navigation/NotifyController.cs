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
using System.Web.Configuration;

namespace GestiLog.Controllers.Navigation
{
    public class NotifyController : Controller
    {
        private DaoEntities db = new DaoEntities();

        // GET: Notify
        [Authorize]
        public ActionResult Index()
        {
            var notify = db.Notify.Include(n => n.CiudadNav);
            return View(notify.ToList());
        }

        // GET: Notify/Details/5
        [Authorize]
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Notify notify = db.Notify.Find(id);
            if (notify == null)
            {
                return HttpNotFound();
            }
            return View(notify);
        }

        // GET: Notify/Create
        [Authorize]
        public ActionResult Create()
        {
            Guid country = Guid.Parse(WebConfigurationManager.AppSettings["SoftCountry"].ToString());
            Guid city = Guid.Parse(WebConfigurationManager.AppSettings["SoftCity"].ToString());
            ViewBag.Pais = new SelectList(db.Pais, "Id", "Nombre", country);
            ViewBag.Ciudad = new SelectList(db.Ciudad.Where(w => w.Pais == country), "Id", "Nombre", city);
            return View();
        }

        // POST: Notify/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "Id,Nombre,Nit,Dv,Telefono,Direccion,Ciudad,S_Usuario,S_Creacion,Habilitado")] Notify notify)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    notify.Id = Guid.NewGuid();
                    notify.S_Creacion = DateTime.Now;
                    notify.S_Usuario = Guid.Parse(User.Identity.GetUserId());
                    db.Notify.Add(notify);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "El NIT ya existe, por favor, ingrese uno diferente.");
            }

            Ciudad city = db.Ciudad.Where(w => w.Id == notify.Ciudad).FirstOrDefault();

            ViewBag.Pais = new SelectList(db.Pais, "Id", "Nombre", city.Pais);
            ViewBag.Ciudad = new SelectList(db.Ciudad.Where(w => w.Pais == city.Pais), "Id", "Nombre", notify.Ciudad);
            return View(notify);
        }

        // GET: Notify/Edit/5
        [Authorize]
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Notify notify = db.Notify.Find(id);
            if (notify == null)
            {
                return HttpNotFound();
            }

            Ciudad city = db.Ciudad.Where(w => w.Id == notify.Ciudad).FirstOrDefault();

            ViewBag.Pais = new SelectList(db.Pais, "Id", "Nombre", city.Pais);
            ViewBag.Ciudad = new SelectList(db.Ciudad.Where(w => w.Pais == city.Pais), "Id", "Nombre", notify.Ciudad);
            return View(notify);
        }

        // POST: Notify/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "Id,Nombre,Nit,Dv,Telefono,Direccion,Ciudad,S_Usuario,S_Creacion,Habilitado")] Notify notify)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(notify).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "El NIT ya existe, por favor, ingrese uno diferente.");
            }

            Ciudad city = db.Ciudad.Where(w => w.Id == notify.Ciudad).FirstOrDefault();

            ViewBag.Pais = new SelectList(db.Pais, "Id", "Nombre", city.Pais);
            ViewBag.Ciudad = new SelectList(db.Ciudad.Where(w => w.Pais == city.Pais), "Id", "Nombre", notify.Ciudad);
            return View(notify);
        }

        // GET: Notify/Delete/5
        [Authorize]
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Notify notify = db.Notify.Find(id);
            if (notify == null)
            {
                return HttpNotFound();
            }
            return View(notify);
        }

        // POST: Notify/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Notify notify = db.Notify.Find(id);
            db.Notify.Remove(notify);
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
