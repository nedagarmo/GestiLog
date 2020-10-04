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
    public class RangeDayOffController : Controller
    {
        private DaoEntities db = new DaoEntities();

        // GET: RangeDayOff
        [Authorize]
        public ActionResult Index()
        {
            return View(db.RangoDiaLibre.ToList());
        }

        // GET: RangeDayOff/Details/5
        [Authorize]
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RangoDiaLibre rangoDiaLibre = db.RangoDiaLibre.Find(id);
            if (rangoDiaLibre == null)
            {
                return HttpNotFound();
            }
            return View(rangoDiaLibre);
        }

        // GET: RangeDayOff/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: RangeDayOff/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Inicial,Final,S_Usuario,S_Creacion,Habilitado")] RangoDiaLibre rangoDiaLibre)
        {
            if (ModelState.IsValid)
            {
                rangoDiaLibre.Id = Guid.NewGuid();
                rangoDiaLibre.S_Creacion = DateTime.Now;
                rangoDiaLibre.S_Usuario = Guid.Parse(User.Identity.GetUserId());
                db.RangoDiaLibre.Add(rangoDiaLibre);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(rangoDiaLibre);
        }

        // GET: RangeDayOff/Edit/5
        [Authorize]
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RangoDiaLibre rangoDiaLibre = db.RangoDiaLibre.Find(id);
            if (rangoDiaLibre == null)
            {
                return HttpNotFound();
            }
            return View(rangoDiaLibre);
        }

        // POST: RangeDayOff/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Inicial,Final,S_Usuario,S_Creacion,Habilitado")] RangoDiaLibre rangoDiaLibre)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rangoDiaLibre).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(rangoDiaLibre);
        }

        // GET: RangeDayOff/Delete/5
        [Authorize]
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RangoDiaLibre rangoDiaLibre = db.RangoDiaLibre.Find(id);
            if (rangoDiaLibre == null)
            {
                return HttpNotFound();
            }
            return View(rangoDiaLibre);
        }

        // POST: RangeDayOff/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            RangoDiaLibre rangoDiaLibre = db.RangoDiaLibre.Find(id);
            db.RangoDiaLibre.Remove(rangoDiaLibre);
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
