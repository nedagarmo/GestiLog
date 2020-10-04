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
    public class FclRateController : Controller
    {
        private DaoEntities db = new DaoEntities();

        // GET: FclRate
        [Authorize]
        public ActionResult Index()
        {
            var tarifarioFcl = db.TarifarioFcl.Include(t => t.AgenteNav).Include(t => t.AreaNav).Include(t => t.FrecuenciaNav).Include(t => t.MonedaNav).Include(t => t.NavieraNav).Include(t => t.PuertoOrigenNav).Include(t => t.PuertoDestinoNav);
            return View(tarifarioFcl.ToList());
        }

        // GET: FclRate/Details/5
        [Authorize]
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TarifarioFcl tarifarioFcl = db.TarifarioFcl.Find(id);
            if (tarifarioFcl == null)
            {
                return HttpNotFound();
            }
            return View(tarifarioFcl);
        }

        // GET: FclRate/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.Agente = new SelectList(db.Agente, "Id", "Nombre");
            ViewBag.Area = new SelectList(db.Area, "Id", "Nombre");
            ViewBag.Frecuencia = new SelectList(db.Frecuencia, "Id", "Nombre");
            ViewBag.Divisa = new SelectList(db.Moneda, "Id", "Descripcion");
            ViewBag.Naviera = new SelectList(db.Naviera, "Id", "Nombre");
            ViewBag.PuertoOrigen = new SelectList(db.Puerto, "Id", "Nombre");
            ViewBag.PuertoDestino = new SelectList(db.Puerto, "Id", "Nombre");
            return View();
        }

        // POST: FclRate/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Consecutivo,Area,Naviera,Agente,PuertoOrigen,PuertoDestino,Frecuencia,TiempoTransito,VigenciaDesde,VigenciaHasta,Mercancia,Dg,Divisa")] TarifarioFcl tarifarioFcl)
        {
            if (ModelState.IsValid)
            {
                tarifarioFcl.Id = Guid.NewGuid();
                db.TarifarioFcl.Add(tarifarioFcl);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Agente = new SelectList(db.Agente, "Id", "Nombre", tarifarioFcl.Agente);
            ViewBag.Area = new SelectList(db.Area, "Id", "Nombre", tarifarioFcl.Area);
            ViewBag.Frecuencia = new SelectList(db.Frecuencia, "Id", "Nombre", tarifarioFcl.Frecuencia);
            ViewBag.Divisa = new SelectList(db.Moneda, "Id", "Descripcion", tarifarioFcl.Divisa);
            ViewBag.Naviera = new SelectList(db.Naviera, "Id", "Nombre", tarifarioFcl.Naviera);
            ViewBag.PuertoOrigen = new SelectList(db.Puerto, "Id", "Nombre", tarifarioFcl.PuertoOrigen);
            ViewBag.PuertoDestino = new SelectList(db.Puerto, "Id", "Nombre", tarifarioFcl.PuertoDestino);
            return View(tarifarioFcl);
        }

        // GET: FclRate/Edit/5
        [Authorize]
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TarifarioFcl tarifarioFcl = db.TarifarioFcl.Find(id);
            if (tarifarioFcl == null)
            {
                return HttpNotFound();
            }
            ViewBag.Agente = new SelectList(db.Agente, "Id", "Nombre", tarifarioFcl.Agente);
            ViewBag.Area = new SelectList(db.Area, "Id", "Nombre", tarifarioFcl.Area);
            ViewBag.Frecuencia = new SelectList(db.Frecuencia, "Id", "Nombre", tarifarioFcl.Frecuencia);
            ViewBag.Divisa = new SelectList(db.Moneda, "Id", "Descripcion", tarifarioFcl.Divisa);
            ViewBag.Naviera = new SelectList(db.Naviera, "Id", "Nombre", tarifarioFcl.Naviera);
            ViewBag.PuertoOrigen = new SelectList(db.Puerto, "Id", "Nombre", tarifarioFcl.PuertoOrigen);
            ViewBag.PuertoDestino = new SelectList(db.Puerto, "Id", "Nombre", tarifarioFcl.PuertoDestino);
            return View(tarifarioFcl);
        }

        // POST: FclRate/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Consecutivo,Area,Naviera,Agente,PuertoOrigen,PuertoDestino,Frecuencia,TiempoTransito,VigenciaDesde,VigenciaHasta,Mercancia,Dg,Divisa")] TarifarioFcl tarifarioFcl)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tarifarioFcl).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Agente = new SelectList(db.Agente, "Id", "Nombre", tarifarioFcl.Agente);
            ViewBag.Area = new SelectList(db.Area, "Id", "Nombre", tarifarioFcl.Area);
            ViewBag.Frecuencia = new SelectList(db.Frecuencia, "Id", "Nombre", tarifarioFcl.Frecuencia);
            ViewBag.Divisa = new SelectList(db.Moneda, "Id", "Descripcion", tarifarioFcl.Divisa);
            ViewBag.Naviera = new SelectList(db.Naviera, "Id", "Nombre", tarifarioFcl.Naviera);
            ViewBag.PuertoOrigen = new SelectList(db.Puerto, "Id", "Nombre", tarifarioFcl.PuertoOrigen);
            ViewBag.PuertoDestino = new SelectList(db.Puerto, "Id", "Nombre", tarifarioFcl.PuertoDestino);
            return View(tarifarioFcl);
        }

        // GET: FclRate/Delete/5
        [Authorize]
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TarifarioFcl tarifarioFcl = db.TarifarioFcl.Find(id);
            if (tarifarioFcl == null)
            {
                return HttpNotFound();
            }
            return View(tarifarioFcl);
        }

        // POST: FclRate/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            TarifarioFcl tarifarioFcl = db.TarifarioFcl.Find(id);
            db.TarifarioFcl.Remove(tarifarioFcl);
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
