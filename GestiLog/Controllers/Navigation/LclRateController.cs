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
    public class LclRateController : Controller
    {
        private DaoEntities db = new DaoEntities();

        // GET: LclRate
        [Authorize]
        public ActionResult Index()
        {
            var tarifarioLcl = db.TarifarioLcl.Include(t => t.AgenteNav).Include(t => t.CocargadorNav).Include(t => t.AreaNav).Include(t => t.FrecuenciaNav).Include(t => t.DivisaNav).Include(t => t.NavieraNav).Include(t => t.PuertoOrigenNav).Include(t => t.PuertoDestinoNav);
            return View(tarifarioLcl.ToList());
        }

        // GET: LclRate/Details/5
        [Authorize]
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TarifarioLcl tarifarioLcl = db.TarifarioLcl.Find(id);
            if (tarifarioLcl == null)
            {
                return HttpNotFound();
            }
            return View(tarifarioLcl);
        }

        // GET: LclRate/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.Agente = new SelectList(db.Agente, "Id", "Nombre");
            ViewBag.Cocargador = new SelectList(db.Agente, "Id", "Nombre");
            ViewBag.Area = new SelectList(db.Area, "Id", "Nombre");
            ViewBag.Frecuencia = new SelectList(db.Frecuencia, "Id", "Nombre");
            ViewBag.Divisa = new SelectList(db.Moneda, "Id", "Descripcion");
            ViewBag.Naviera = new SelectList(db.Naviera, "Id", "Nombre");
            ViewBag.PuertoOrigen = new SelectList(db.Puerto, "Id", "Nombre");
            ViewBag.PuertoDestino = new SelectList(db.Puerto, "Id", "Nombre");
            return View();
        }

        // POST: LclRate/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Consecutivo,Area,Naviera,Agente,Cocargador,PuertoOrigen,PuertoDestino,Frecuencia,TiempoTransito,VigenciaDesde,VigenciaHasta,Mercancia,Dg,Divisa")] TarifarioLcl tarifarioLcl)
        {
            if (ModelState.IsValid)
            {
                tarifarioLcl.Id = Guid.NewGuid();
                db.TarifarioLcl.Add(tarifarioLcl);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Agente = new SelectList(db.Agente, "Id", "Nombre", tarifarioLcl.Agente);
            ViewBag.Cocargador = new SelectList(db.Agente, "Id", "Nombre", tarifarioLcl.Cocargador);
            ViewBag.Area = new SelectList(db.Area, "Id", "Nombre", tarifarioLcl.Area);
            ViewBag.Frecuencia = new SelectList(db.Frecuencia, "Id", "Nombre", tarifarioLcl.Frecuencia);
            ViewBag.Divisa = new SelectList(db.Moneda, "Id", "Descripcion", tarifarioLcl.Divisa);
            ViewBag.Naviera = new SelectList(db.Naviera, "Id", "Nombre", tarifarioLcl.Naviera);
            ViewBag.PuertoOrigen = new SelectList(db.Puerto, "Id", "Nombre", tarifarioLcl.PuertoOrigen);
            ViewBag.PuertoDestino = new SelectList(db.Puerto, "Id", "Nombre", tarifarioLcl.PuertoDestino);
            return View(tarifarioLcl);
        }

        // GET: LclRate/Edit/5
        [Authorize]
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TarifarioLcl tarifarioLcl = db.TarifarioLcl.Find(id);
            if (tarifarioLcl == null)
            {
                return HttpNotFound();
            }
            ViewBag.Agente = new SelectList(db.Agente, "Id", "Nombre", tarifarioLcl.Agente);
            ViewBag.Cocargador = new SelectList(db.Agente, "Id", "Nombre", tarifarioLcl.Cocargador);
            ViewBag.Area = new SelectList(db.Area, "Id", "Nombre", tarifarioLcl.Area);
            ViewBag.Frecuencia = new SelectList(db.Frecuencia, "Id", "Nombre", tarifarioLcl.Frecuencia);
            ViewBag.Divisa = new SelectList(db.Moneda, "Id", "Descripcion", tarifarioLcl.Divisa);
            ViewBag.Naviera = new SelectList(db.Naviera, "Id", "Nombre", tarifarioLcl.Naviera);
            ViewBag.PuertoOrigen = new SelectList(db.Puerto, "Id", "Nombre", tarifarioLcl.PuertoOrigen);
            ViewBag.PuertoDestino = new SelectList(db.Puerto, "Id", "Nombre", tarifarioLcl.PuertoDestino);
            return View(tarifarioLcl);
        }

        // POST: LclRate/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Consecutivo,Area,Naviera,Agente,Cocargador,PuertoOrigen,PuertoDestino,Frecuencia,TiempoTransito,VigenciaDesde,VigenciaHasta,Mercancia,Dg,Divisa")] TarifarioLcl tarifarioLcl)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tarifarioLcl).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Agente = new SelectList(db.Agente, "Id", "Nombre", tarifarioLcl.Agente);
            ViewBag.Cocargador = new SelectList(db.Agente, "Id", "Nombre", tarifarioLcl.Cocargador);
            ViewBag.Area = new SelectList(db.Area, "Id", "Nombre", tarifarioLcl.Area);
            ViewBag.Frecuencia = new SelectList(db.Frecuencia, "Id", "Nombre", tarifarioLcl.Frecuencia);
            ViewBag.Divisa = new SelectList(db.Moneda, "Id", "Descripcion", tarifarioLcl.Divisa);
            ViewBag.Naviera = new SelectList(db.Naviera, "Id", "Nombre", tarifarioLcl.Naviera);
            ViewBag.PuertoOrigen = new SelectList(db.Puerto, "Id", "Nombre", tarifarioLcl.PuertoOrigen);
            ViewBag.PuertoDestino = new SelectList(db.Puerto, "Id", "Nombre", tarifarioLcl.PuertoDestino);
            return View(tarifarioLcl);
        }

        // GET: LclRate/Delete/5
        [Authorize]
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TarifarioLcl tarifarioLcl = db.TarifarioLcl.Find(id);
            if (tarifarioLcl == null)
            {
                return HttpNotFound();
            }
            return View(tarifarioLcl);
        }

        // POST: LclRate/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            TarifarioLcl tarifarioLcl = db.TarifarioLcl.Find(id);
            db.TarifarioLcl.Remove(tarifarioLcl);
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
