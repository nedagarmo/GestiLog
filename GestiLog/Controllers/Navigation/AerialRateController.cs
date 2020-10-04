using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GestiLog.Entities;
using GestiLog.Models;

namespace GestiLog.Controllers.Navigation
{
    public class AerialRateController : Controller
    {
        private DaoEntities db = new DaoEntities();

        // GET: AerialRate
        [Authorize]
        public ActionResult Index()
        {
            var tarifarioAereo = db.TarifarioAereo.Include(t => t.AerolineaNav).Include(t => t.AeropuertoOrigenNav).Include(t => t.AeropuertoDestinoNav).Include(t => t.AgenteNav).Include(t => t.AreaNav).Include(t => t.FrecuenciaNav).Include(t => t.MonedaNav);
            return View(tarifarioAereo.ToList());
        }

        // GET: AerialRate/Details/5
        [Authorize]
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TarifarioAereo tarifarioAereo = db.TarifarioAereo.Find(id);
            if (tarifarioAereo == null)
            {
                return HttpNotFound();
            }
            return View(tarifarioAereo);
        }

        // GET: AerialRate/Create
        [Authorize]
        public ActionResult Create()
        {
            AerialRateViewModel model = new AerialRateViewModel();
            model.Items = new List<TarifarioAereoItem>();
            model.Items.Add(new TarifarioAereoItem());
            model.Vias = new List<TarifarioViaAerea>();
            ViewBag.Aerolinea = new SelectList(db.Aerolinea, "Id", "Nombre");
            ViewBag.AeropuertoOrigen = new SelectList(db.Aeropuerto, "Id", "Nombre");
            ViewBag.AeropuertoDestino = new SelectList(db.Aeropuerto, "Id", "Nombre");
            ViewBag.Puertos = new SelectList(db.Aeropuerto, "Id", "Nombre");
            ViewBag.Agente = new SelectList(db.Agente, "Id", "Nombre");
            ViewBag.Area = new SelectList(db.Area, "Id", "Nombre");
            ViewBag.Frecuencia = new SelectList(db.Frecuencia, "Id", "Nombre");
            ViewBag.Divisa = new SelectList(db.Moneda, "Id", "Descripcion");
            return View(model);
        }

        // POST: AerialRate/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Tarifario,Items,Vias")] AerialRateViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.Tarifario.Id = Guid.NewGuid();
                db.TarifarioAereo.Add(model.Tarifario);

                foreach (var item in model.Items)
                {
                    item.Id = Guid.NewGuid();
                    item.TarifarioAereo = model.Tarifario.Id;
                    db.TarifarioAereoItem.Add(item);
                }

                foreach (var via in model.Vias)
                {
                    via.Id = Guid.NewGuid();
                    via.TarifarioAereo = model.Tarifario.Id;
                    db.TarifarioViaAerea.Add(via);
                }

                db.SaveChanges();
                return RedirectToAction("Edit", new { id = model.Tarifario.Id });
            }

            ViewBag.Aerolinea = new SelectList(db.Aerolinea, "Id", "Nombre", model.Tarifario.Aerolinea);
            ViewBag.AeropuertoOrigen = new SelectList(db.Aeropuerto, "Id", "Nombre", model.Tarifario.AeropuertoOrigen);
            ViewBag.AeropuertoDestino = new SelectList(db.Aeropuerto, "Id", "Nombre", model.Tarifario.AeropuertoDestino);
            ViewBag.Puertos = new SelectList(db.Aeropuerto, "Id", "Nombre");
            ViewBag.Agente = new SelectList(db.Agente, "Id", "Nombre", model.Tarifario.Agente);
            ViewBag.Area = new SelectList(db.Area, "Id", "Nombre", model.Tarifario.Area);
            ViewBag.Frecuencia = new SelectList(db.Frecuencia, "Id", "Nombre", model.Tarifario.Frecuencia);
            ViewBag.Divisa = new SelectList(db.Moneda, "Id", "Descripcion", model.Tarifario.Divisa);
            return View(model);
        }

        // GET: AerialRate/Edit/5
        [Authorize]
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TarifarioAereo tarifarioAereo = db.TarifarioAereo.Where(w => w.Id == id).Include(i => i.TarifarioAereoItemNav).Include(i => i.TarifarioViaAereaNav).Include("TarifarioAereoItemNav.TarifarioEscalaItemNav").FirstOrDefault();
            if (tarifarioAereo == null)
            {
                return HttpNotFound();
            }

            AerialRateViewModel model = new AerialRateViewModel();
            model.Tarifario = tarifarioAereo;
            model.Items = tarifarioAereo.TarifarioAereoItemNav.OrderByDescending(o => o.TarifarioEscalaItemNav.Rango).ThenBy(o => o.TarifarioEscalaItemNav.Hasta).ToList();
            model.Vias = tarifarioAereo.TarifarioViaAereaNav.ToList();
            ViewBag.Aerolinea = new SelectList(db.Aerolinea, "Id", "Nombre", tarifarioAereo.Aerolinea);
            ViewBag.AeropuertoOrigen = new SelectList(db.Aeropuerto, "Id", "Nombre", tarifarioAereo.AeropuertoOrigen);
            ViewBag.AeropuertoDestino = new SelectList(db.Aeropuerto, "Id", "Nombre", tarifarioAereo.AeropuertoDestino);
            ViewBag.Puertos = db.Aeropuerto.ToList();
            ViewBag.Agente = new SelectList(db.Agente, "Id", "Nombre", tarifarioAereo.Agente);
            ViewBag.Area = new SelectList(db.Area, "Id", "Nombre", tarifarioAereo.Area);
            ViewBag.Frecuencia = new SelectList(db.Frecuencia, "Id", "Nombre", tarifarioAereo.Frecuencia);
            ViewBag.Divisa = new SelectList(db.Moneda, "Id", "Descripcion", tarifarioAereo.Divisa);
            ViewBag.Moneda = db.Moneda.ToList();

            return View(model);
        }

        // POST: AerialRate/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Tarifario,Items,Vias")] AerialRateViewModel model)
        {
            if (ModelState.IsValid)
            {
                db.Entry(model.Tarifario).State = EntityState.Modified;
                db.TarifarioAereoItem.RemoveRange(db.TarifarioAereoItem.Where(w => w.TarifarioAereo == model.Tarifario.Id));
                db.TarifarioViaAerea.RemoveRange(db.TarifarioViaAerea.Where(w => w.TarifarioAereo == model.Tarifario.Id));

                foreach (var item in model.Items)
                {
                    item.Id = Guid.NewGuid();
                    item.TarifarioAereo = model.Tarifario.Id;
                    db.TarifarioAereoItem.Add(item);
                }

                foreach (var via in model.Vias)
                {
                    via.Id = Guid.NewGuid();
                    via.TarifarioAereo = model.Tarifario.Id;
                    db.TarifarioViaAerea.Add(via);
                }

                db.SaveChanges();

                return RedirectToAction("Edit", new { id = model.Tarifario.Id });
            }

            TarifarioAereo tarifarioAereo = db.TarifarioAereo.Where(w => w.Id == model.Tarifario.Id).Include(i => i.TarifarioAereoItemNav).Include(i => i.TarifarioViaAereaNav).Include("TarifarioAereoItemNav.TarifarioEscalaItemNav").FirstOrDefault();
            model.Items = tarifarioAereo.TarifarioAereoItemNav.OrderByDescending(o => o.TarifarioEscalaItemNav.Rango).ThenBy(o => o.TarifarioEscalaItemNav.Hasta).ToList();
            model.Vias = tarifarioAereo.TarifarioViaAereaNav.ToList();
            ViewBag.Aerolinea = new SelectList(db.Aerolinea, "Id", "Nombre", model.Tarifario.Aerolinea);
            ViewBag.AeropuertoOrigen = new SelectList(db.Aeropuerto, "Id", "Nombre", model.Tarifario.AeropuertoOrigen);
            ViewBag.AeropuertoDestino = new SelectList(db.Aeropuerto, "Id", "Nombre", model.Tarifario.AeropuertoDestino);
            ViewBag.Puertos = db.Aeropuerto.ToList();
            ViewBag.Agente = new SelectList(db.Agente, "Id", "Nombre", model.Tarifario.Agente);
            ViewBag.Area = new SelectList(db.Area, "Id", "Nombre", model.Tarifario.Area);
            ViewBag.Frecuencia = new SelectList(db.Frecuencia, "Id", "Nombre", model.Tarifario.Frecuencia);
            ViewBag.Divisa = new SelectList(db.Moneda, "Id", "Descripcion", model.Tarifario.Divisa);
            ViewBag.Moneda = db.Moneda.ToList();
            return View(model);
        }

        // GET: AerialRate/Delete/5
        [Authorize]
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TarifarioAereo tarifarioAereo = db.TarifarioAereo.Find(id);
            if (tarifarioAereo == null)
            {
                return HttpNotFound();
            }
            return View(tarifarioAereo);
        }

        // POST: AerialRate/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            TarifarioAereo tarifarioAereo = db.TarifarioAereo.Find(id);
            db.TarifarioAereo.Remove(tarifarioAereo);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult AddTrack(int id)
        {
            ViewBag.Id = id;
            ViewBag.Puertos = new SelectList(db.Aeropuerto, "Id", "Nombre");
            return PartialView("~/Views/Shared/_TrackPartial.cshtml");
        }

        [Authorize]
        public ActionResult GetPriceTable(Guid id)
        {
            ViewBag.Moneda = new SelectList(db.Moneda, "Id", "Descripcion");

            Aerolinea aerolinea = db.Aerolinea.Where(w => w.Id == id).FirstOrDefault();
            List<TarifarioAereoItem> model = db.TarifarioEscalaItem.Where(w => w.TarifarioEscala == aerolinea.TarifarioEscala).OrderByDescending(o => o.Rango).ThenBy(o => o.Hasta).ToList().Select(s => new TarifarioAereoItem()
            {
                Id = Guid.NewGuid(),
                TarifarioEscalaItemNav = s
            }).ToList();

            return PartialView("_PriceTablePartial", model);
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
