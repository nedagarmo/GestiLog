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
    public class NoticeFieldController : Controller
    {
        private DaoEntities db = new DaoEntities();

        // GET: NoticeField
        [Authorize]
        public ActionResult Index()
        {
            var comunicadoCampo = db.ComunicadoCampo.Include(c => c.ModuloNav);
            return View(comunicadoCampo.ToList());
        }

        // GET: NoticeField/Details/5
        [Authorize]
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ComunicadoCampo comunicadoCampo = db.ComunicadoCampo.Find(id);
            if (comunicadoCampo == null)
            {
                return HttpNotFound();
            }
            return View(comunicadoCampo);
        }

        // GET: NoticeField/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.Modulo = new SelectList(db.Modulo, "Id", "Nombre");
            return View();
        }

        // POST: NoticeField/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nombre,Codigo,Entidad,Habilitado,Campo,Tipo,Fuente,Modulo")] ComunicadoCampo comunicadoCampo)
        {
            if (ModelState.IsValid)
            {
                comunicadoCampo.Id = Guid.NewGuid();
                db.ComunicadoCampo.Add(comunicadoCampo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Modulo = new SelectList(db.Modulo, "Id", "Nombre", comunicadoCampo.Modulo);
            return View(comunicadoCampo);
        }

        // GET: NoticeField/Edit/5
        [Authorize]
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ComunicadoCampo comunicadoCampo = db.ComunicadoCampo.Find(id);
            if (comunicadoCampo == null)
            {
                return HttpNotFound();
            }
            ViewBag.Modulo = new SelectList(db.Modulo, "Id", "Nombre", comunicadoCampo.Modulo);
            return View(comunicadoCampo);
        }

        // POST: NoticeField/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nombre,Codigo,Entidad,Habilitado,Campo,Tipo,Fuente,Modulo")] ComunicadoCampo comunicadoCampo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(comunicadoCampo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Modulo = new SelectList(db.Modulo, "Id", "Nombre", comunicadoCampo.Modulo);
            return View(comunicadoCampo);
        }

        // GET: NoticeField/Delete/5
        [Authorize]
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ComunicadoCampo comunicadoCampo = db.ComunicadoCampo.Find(id);
            if (comunicadoCampo == null)
            {
                return HttpNotFound();
            }
            return View(comunicadoCampo);
        }

        // POST: NoticeField/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            ComunicadoCampo comunicadoCampo = db.ComunicadoCampo.Find(id);
            db.ComunicadoCampo.Remove(comunicadoCampo);
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
