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
using GestiLog.Models;

namespace GestiLog.Controllers.Navigation
{
    public class MblController : Controller
    {
        private DaoEntities db = new DaoEntities();
        private Guid module = Guid.Parse("1E4CAD29-F6C1-4C9B-99C8-06B65E8464D8");

        // GET: Mbl
        [Authorize]
        public ActionResult Index()
        {
            var mbl = db.Mbl.Include(m => m.EmbalajeNav).Include(m => m.AgenteNav).Include(m => m.EmisionBlNav).Include(m => m.FleteNav).Include(m => m.MotonaveNav).Include(m => m.MuelleNav).Include(m => m.NavieraNav).Include(m => m.OperativoNav).Include(m => m.PuertoOrigenNav).Include(m => m.PuertoDestinoNav).Include(m => m.PuertoTransbordoNav).Include(m => m.RangoDiaLibreNav).Include(m => m.TipoEmbarqueNav);
            return View(mbl.ToList());
        }

        // GET: Mbl/Details/5
        [Authorize]
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mbl mbl = db.Mbl.Find(id);
            if (mbl == null)
            {
                return HttpNotFound();
            }
            return View(mbl);
        }

        // GET: Mbl/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Mbl/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "Id,Do,Master,Agente,Naviera,Operativo,TipoEmbarque,PuertoOrigen,PuertoDestino,Etd,Eta,Transbordo,FechaEstimadaArribo,PuertoTransbordo,MotonaveEstimadaArribo,ViajeEstimadoArribo,Peso,Volumen,Embalaje,Piezas,S_Usuario,S_Creacion,Habilitado,ExoneracionDeposito,ExoneracionDroopOff,DiaLibreNaviera,RangoDiaLibre,MuelleArribo,EmisionBl,NumeroManifiesto,Flete,RadicadoMuisca,Desconsolidacion,Finalizacion")] Mbl mbl)
        {
            if (ModelState.IsValid)
            {
                mbl.Id = Guid.NewGuid();
                mbl.Habilitado = true;
                mbl.S_Creacion = DateTime.Now;
                mbl.S_Usuario = Guid.Parse(User.Identity.GetUserId());
                db.Mbl.Add(mbl);
                db.SaveChanges();
                return RedirectToAction("Edit", new { id = mbl.Id });
            }

            return View(mbl);
        }

        // GET: Mbl/Edit/5
        [Authorize]
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mbl mbl = db.Mbl.Where(w => w.Id == id).Include(i => i.HblNav).Include(i => i.ContenedorNav).Include("ContenedorNav.TipoContenedorNav").FirstOrDefault();
            Session["Mbl"] = mbl;

            if (mbl == null)
            {
                return HttpNotFound();
            }

            // Traspaso de información entre objeto de base de datos a modelo.
            MblViewModel view = new MblViewModel()
            {
                Id = mbl.Id,
                Do = mbl.Do,
                Master = mbl.Master,
                Agente = mbl.Agente,
                Naviera = mbl.Naviera,
                Operativo = mbl.Operativo,
                TipoEmbarque = mbl.TipoEmbarque,
                PuertoOrigen = mbl.PuertoOrigen,
                PuertoDestino = mbl.PuertoDestino,
                Etd = mbl.Etd,
                Eta = mbl.Eta,
                Transbordo = mbl.Transbordo,
                FechaEstimadaArribo = mbl.FechaEstimadaArribo,
                PuertoTransbordo = mbl.PuertoTransbordo,
                MotonaveEstimadaArribo = mbl.MotonaveEstimadaArribo,
                ViajeEstimadoArribo = mbl.ViajeEstimadoArribo,
                Peso = mbl.Peso,
                Volumen = mbl.Volumen,
                Embalaje = mbl.Embalaje,
                Piezas = mbl.Piezas,
                ExoneracionDeposito = mbl.ExoneracionDeposito,
                ExoneracionDroopOff = mbl.ExoneracionDroopOff,
                DiaLibreNaviera = mbl.DiaLibreNaviera,
                RangoDiaLibre = mbl.RangoDiaLibre,
                MuelleArribo = mbl.MuelleArribo,
                EmisionBl = mbl.EmisionBl,
                NumeroManifiesto = mbl.NumeroManifiesto,
                Flete = mbl.Flete,
                RadicadoMuisca = mbl.RadicadoMuisca,
                Desconsolidacion = mbl.Desconsolidacion,
                Finalizacion = mbl.Finalizacion,
                S_Usuario = mbl.S_Usuario,
                S_Creacion = mbl.S_Creacion,
                Habilitado = mbl.Habilitado,
                Contenedores = mbl.ContenedorNav.ToList(),
                Hbls = mbl.HblNav.ToList()
            };

            List<Guid> hblsId = mbl.HblNav.Select(s => s.Id).ToList();
            ViewBag.NoticesCount = db.Historico.Where(w => hblsId.Contains(w.Fuente)).Select(s => s.Comunicado).ToList();
            ViewBag.Notices = db.Comunicado.Where(w => w.Habilitado && w.Modulo == module).ToList();
            ViewBag.Embalaje = new SelectList(db.Embalaje, "Id", "Descripcion", mbl.Embalaje);
            ViewBag.Agente = new SelectList(db.Agente, "Id", "Nombre", mbl.Agente);
            ViewBag.EmisionBl = new SelectList(db.EmisionBl, "Id", "Nombre", mbl.EmisionBl);
            ViewBag.Flete = new SelectList(db.Flete, "Id", "Nombre", mbl.Flete);
            ViewBag.MotonaveEstimadaArribo = new SelectList(db.Motonave, "Id", "Nombre", mbl.MotonaveEstimadaArribo);
            ViewBag.MuelleArribo = new SelectList(db.Muelle, "Id", "Nombre", mbl.MuelleArribo);
            ViewBag.Naviera = new SelectList(db.Naviera, "Id", "Nombre", mbl.Naviera);
            ViewBag.Operativo = new SelectList(db.Operativo, "Id", "Nombre", mbl.Operativo);
            ViewBag.PuertoOrigen = new SelectList(db.Puerto, "Id", "Nombre", mbl.PuertoOrigen);
            ViewBag.PuertoDestino = new SelectList(db.Puerto, "Id", "Nombre", mbl.PuertoDestino);
            ViewBag.PuertoTransbordo = new SelectList(db.Puerto, "Id", "Nombre", mbl.PuertoTransbordo);
            ViewBag.RangoDiaLibre = new SelectList(db.RangoDiaLibre.Select(s => new { Id = s.Id, Rango = s.Inicial + " - " + s.Final }), "Id", "Rango", mbl.RangoDiaLibre);
            ViewBag.TipoEmbarque = new SelectList(db.TipoEmbarque, "Id", "Nombre", mbl.TipoEmbarque);
            return View(view);
        }

        // POST: Mbl/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "Id,Do,Master,Agente,Naviera,Operativo,TipoEmbarque,PuertoOrigen,PuertoDestino,Etd,Eta,Transbordo,FechaEstimadaArribo,PuertoTransbordo,MotonaveEstimadaArribo,ViajeEstimadoArribo,Peso,Volumen,Embalaje,Piezas,S_Usuario,S_Creacion,Habilitado,ExoneracionDeposito,ExoneracionDroopOff,DiaLibreNaviera,RangoDiaLibre,MuelleArribo,EmisionBl,NumeroManifiesto,Flete,RadicadoMuisca,Desconsolidacion,Finalizacion")] MblViewModel view)
        {
            Mbl mbl = db.Mbl.Where(w => w.Id == view.Id).Include(i => i.HblNav).Include(i => i.ContenedorNav).FirstOrDefault();

            // Traspaso de información entre modelo a objeto de base de datos.
            mbl.Do = view.Do;
            mbl.Master = view.Master;
            mbl.Agente = view.Agente;
            mbl.Naviera = view.Naviera;
            mbl.Operativo = view.Operativo;
            mbl.TipoEmbarque = view.TipoEmbarque;
            mbl.PuertoOrigen = view.PuertoOrigen;
            mbl.PuertoDestino = view.PuertoDestino;
            mbl.Etd = view.Etd;
            mbl.Eta = view.Eta;
            mbl.Transbordo = view.Transbordo;
            mbl.FechaEstimadaArribo = view.FechaEstimadaArribo;
            mbl.PuertoTransbordo = view.PuertoTransbordo;
            mbl.MotonaveEstimadaArribo = view.MotonaveEstimadaArribo;
            mbl.ViajeEstimadoArribo = view.ViajeEstimadoArribo;
            mbl.Peso = view.Peso;
            mbl.Volumen = view.Volumen;
            mbl.Embalaje = view.Embalaje;
            mbl.Piezas = view.Piezas;
            mbl.ExoneracionDeposito = view.ExoneracionDeposito;
            mbl.ExoneracionDroopOff = view.ExoneracionDroopOff;
            mbl.DiaLibreNaviera = view.DiaLibreNaviera;
            mbl.RangoDiaLibre = view.RangoDiaLibre;
            mbl.MuelleArribo = view.MuelleArribo;
            mbl.EmisionBl = view.EmisionBl;
            mbl.NumeroManifiesto = view.NumeroManifiesto;
            mbl.Flete = view.Flete;
            mbl.RadicadoMuisca = view.RadicadoMuisca;
            mbl.Desconsolidacion = view.Desconsolidacion;
            mbl.Finalizacion = view.Finalizacion;
            mbl.Habilitado = view.Habilitado;

            if (ModelState.IsValid)
            {
                db.Entry(mbl).State = EntityState.Modified;
                db.SaveChanges();
                //return RedirectToAction("Index");
            }

            List<Guid> hblsId = mbl.HblNav.Select(s => s.Id).ToList();
            ViewBag.NoticesCount = db.Historico.Where(w => hblsId.Contains(w.Fuente)).Select(s => s.Comunicado).ToList();
            ViewBag.Notices = db.Comunicado.Where(w => w.Habilitado && w.Modulo == module).ToList();
            ViewBag.Embalaje = new SelectList(db.Embalaje, "Id", "Descripcion", mbl.Embalaje);
            ViewBag.Agente = new SelectList(db.Agente, "Id", "Nombre", mbl.Agente);
            ViewBag.EmisionBl = new SelectList(db.EmisionBl, "Id", "Nombre", mbl.EmisionBl);
            ViewBag.Flete = new SelectList(db.Flete, "Id", "Nombre", mbl.Flete);
            ViewBag.MotonaveEstimadaArribo = new SelectList(db.Motonave, "Id", "Nombre", mbl.MotonaveEstimadaArribo);
            ViewBag.MuelleArribo = new SelectList(db.Muelle, "Id", "Nombre", mbl.MuelleArribo);
            ViewBag.Naviera = new SelectList(db.Naviera, "Id", "Nombre", mbl.Naviera);
            ViewBag.Operativo = new SelectList(db.Operativo, "Id", "Nombre", mbl.Operativo);
            ViewBag.PuertoOrigen = new SelectList(db.Puerto, "Id", "Nombre", mbl.PuertoOrigen);
            ViewBag.PuertoDestino = new SelectList(db.Puerto, "Id", "Nombre", mbl.PuertoDestino);
            ViewBag.PuertoTransbordo = new SelectList(db.Puerto, "Id", "Nombre", mbl.PuertoTransbordo);
            ViewBag.RangoDiaLibre = new SelectList(db.RangoDiaLibre.Select(s => new { Id = s.Id, Rango = s.Inicial + " - " + s.Final }), "Id", "Rango", mbl.RangoDiaLibre);
            ViewBag.TipoEmbarque = new SelectList(db.TipoEmbarque, "Id", "Nombre", mbl.TipoEmbarque);

            view.Hbls = mbl.HblNav.ToList();
            view.Contenedores = mbl.ContenedorNav.ToList();
            return View(view);
        }

        // GET: Mbl/Delete/5
        [Authorize]
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mbl mbl = db.Mbl.Find(id);
            if (mbl == null)
            {
                return HttpNotFound();
            }
            return View(mbl);
        }

        // POST: Mbl/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Mbl mbl = db.Mbl.Find(id);
            db.Mbl.Remove(mbl);
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
