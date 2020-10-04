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
    public class MawbController : Controller
    {
        private DaoEntities db = new DaoEntities();
        private Guid module = Guid.Parse("32916188-AD05-4BCB-A89B-467260729CB5");

        // GET: Mawb
        [Authorize]
        public ActionResult Index()
        {
            var mawb = db.Mawb.Include(m => m.AerolineaNav).Include(m => m.AerolineaConexionNav).Include(m => m.AeropuertoOrigenNav).Include(m => m.AeropuertoDestinoNav).Include(m => m.AeropuertoConexionNav).Include(m => m.AgenteNav).Include(m => m.EmbalajeNav).Include(m => m.OperativoNav);
            return View(mawb.ToList());
        }

        // GET: Mawb/Details/5
        [Authorize]
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mawb mawb = db.Mawb.Find(id);
            if (mawb == null)
            {
                return HttpNotFound();
            }
            return View(mawb);
        }

        // GET: Mawb/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Mawb/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "Id,Do,Master,Agente,Aerolinea,Operativo,AeropuertoOrigen,AeropuertoDestino,Etd,FechaEstimadaArribo,Conexion,AerolineaConexion,AeropuertoConexion,FechaConexion,ViajeConexion,Peso,PesoCargable,DimensionLargo,DimensionAncho,DimensionAlto,Volumen,Embalaje,Piezas,Mercancia,Imo,S_Usuario,S_Creacion,Habilitado")] Mawb mawb)
        {
            if (ModelState.IsValid)
            {
                mawb.Id = Guid.NewGuid();
                mawb.Habilitado = true;
                mawb.S_Creacion = DateTime.Now;
                mawb.S_Usuario = Guid.Parse(User.Identity.GetUserId());
                db.Mawb.Add(mawb);
                db.SaveChanges();
                return RedirectToAction("Edit", new { id = mawb.Id });
            }

            return View(mawb);
        }

        // GET: Mawb/Edit/5
        [Authorize]
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Mawb mawb = db.Mawb.Where(w => w.Id == id).Include(i => i.HawbNav).FirstOrDefault();
            Session["Mawb"] = mawb;

            if (mawb == null)
            {
                return HttpNotFound();
            }

            List<Guid> hawbsId = mawb.HawbNav.Select(s => s.Id).ToList();
            ViewBag.NoticesCount = db.Historico.Where(w => hawbsId.Contains(w.Fuente)).Select(s => s.Comunicado).ToList();
            ViewBag.Notices = db.Comunicado.Where(w => w.Habilitado && w.Modulo == module).ToList();
            ViewBag.Aerolinea = new SelectList(db.Aerolinea, "Id", "Nombre", mawb.Aerolinea);
            ViewBag.AerolineaConexion = new SelectList(db.Aerolinea, "Id", "Nombre", mawb.AerolineaConexion);
            ViewBag.AeropuertoOrigen = new SelectList(db.Aeropuerto, "Id", "Nombre", mawb.AeropuertoOrigen);
            ViewBag.AeropuertoDestino = new SelectList(db.Aeropuerto, "Id", "Nombre", mawb.AeropuertoDestino);
            ViewBag.AeropuertoConexion = new SelectList(db.Aeropuerto, "Id", "Nombre", mawb.AeropuertoConexion);
            ViewBag.Agente = new SelectList(db.Agente, "Id", "Nombre", mawb.Agente);
            ViewBag.Embalaje = new SelectList(db.Embalaje, "Id", "Descripcion", mawb.Embalaje);
            ViewBag.Operativo = new SelectList(db.Operativo, "Id", "Nombre", mawb.Operativo);
            return View(mawb);
        }

        // POST: Mawb/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "Id,Do,Master,Agente,Aerolinea,Operativo,AeropuertoOrigen,AeropuertoDestino,Etd,FechaEstimadaArribo,Conexion,AerolineaConexion,AeropuertoConexion,FechaConexion,ViajeConexion,Peso,PesoCargable,DimensionLargo,DimensionAncho,DimensionAlto,Volumen,Embalaje,Piezas,Mercancia,Imo,S_Usuario,S_Creacion,Habilitado")] Mawb mawb)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mawb).State = EntityState.Modified;
                db.SaveChanges();
                // return RedirectToAction("Index");
            }

            mawb.HawbNav = db.Hawb.Where(w => w.Mawb == mawb.Id).ToList();

            List<Guid> hawbsId = mawb.HawbNav.Select(s => s.Id).ToList();
            ViewBag.NoticesCount = db.Historico.Where(w => hawbsId.Contains(w.Fuente)).Select(s => s.Comunicado).ToList();
            ViewBag.Notices = db.Comunicado.Where(w => w.Habilitado && w.Modulo == module).ToList();
            ViewBag.Aerolinea = new SelectList(db.Aerolinea, "Id", "Nombre", mawb.Aerolinea);
            ViewBag.AerolineaConexion = new SelectList(db.Aerolinea, "Id", "Nombre", mawb.AerolineaConexion);
            ViewBag.AeropuertoOrigen = new SelectList(db.Aeropuerto, "Id", "Nombre", mawb.AeropuertoOrigen);
            ViewBag.AeropuertoDestino = new SelectList(db.Aeropuerto, "Id", "Nombre", mawb.AeropuertoDestino);
            ViewBag.AeropuertoConexion = new SelectList(db.Aeropuerto, "Id", "Nombre", mawb.AeropuertoConexion);
            ViewBag.Agente = new SelectList(db.Agente, "Id", "Nombre", mawb.Agente);
            ViewBag.Embalaje = new SelectList(db.Embalaje, "Id", "Descripcion", mawb.Embalaje);
            ViewBag.Operativo = new SelectList(db.Operativo, "Id", "Nombre", mawb.Operativo);
            return View(mawb);
        }

        // GET: Mawb/Delete/5
        [Authorize]
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mawb mawb = db.Mawb.Find(id);
            if (mawb == null)
            {
                return HttpNotFound();
            }
            return View(mawb);
        }

        // POST: Mawb/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Mawb mawb = db.Mawb.Find(id);
            db.Mawb.Remove(mawb);
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
