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
using Microsoft.AspNet.Identity.EntityFramework;

namespace GestiLog.Controllers.Navigation
{
    public class HblController : Controller
    {
        private DaoEntities db = new DaoEntities();
        private Guid module = Guid.Parse("1E4CAD29-F6C1-4C9B-99C8-06B65E8464D8");

        // GET: Hbl
        [Authorize]
        public ActionResult Index(Guid id)
        {
            Session["Mbl"] = db.Mbl.Where(w => w.Id == id).FirstOrDefault();
            ViewBag.Mbl = ((Mbl)Session["Mbl"]).Do;
            ViewBag.MblId = ((Mbl)Session["Mbl"]).Id;
            var hbl = db.Hbl.Where(w => w.Mbl == id).Include(h => h.ClienteNav).Include(h => h.EmisionBlNav).Include(h => h.FleteNav).Include(h => h.HblTipoNav).Include(h => h.IncotermNav).Include(h => h.MblNav).Include(h => h.MotonaveNav).Include(h => h.MuelleNav).Include(h => h.NotifyNav).Include(h => h.ProveedorNav).Include(h => h.PuertoOrigenNav).Include(h => h.PuertoDestinoNav).Include(h => h.RangoDiaLibreNav).Include(h => h.SucursalNav);
            return View(hbl.ToList());
        }

        // GET: Hbl/Details/5
        [Authorize]
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Hbl hbl = db.Hbl.Find(id);
            if (hbl == null)
            {
                return HttpNotFound();
            }
            return View(hbl);
        }

        // GET: Hbl/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.Mbl = ((Mbl)Session["Mbl"]).Do;
            ViewBag.MblId = ((Mbl)Session["Mbl"]).Id;
            return View();
        }

        // POST: Hbl/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "Id,Mbl,Do,Hbl1,Proveedor,Cliente,Notify,Sucursal,ExoneracionDeposito,ExoneracionDroopOff,DiaLibreCliente,RangoDiaLibre,FechaCargue,FechaInstruccion,FechaNotificacion,Tipo,PuertoOrigen,PuertoDestino,Etd,Eta,MotonaveEstimadaArribo,ViajeEstimadoArribo,Peso,Volumen,DescripcionMercancia,Peligrosa,Incoterm,Imo,Un,MuelleArribo,EmisionBl,Flete,Valor,ServicioCliente,FechaLiberacion,QuienLibera,S_Usuario,S_Creacion,Habilitado")] Hbl hbl)
        {
            if (ModelState.IsValid)
            {
                hbl.Id = Guid.NewGuid();
                hbl.Mbl = ((Mbl)Session["Mbl"]).Id;
                hbl.Habilitado = true;
                hbl.S_Creacion = DateTime.Now;
                hbl.S_Usuario = Guid.Parse(User.Identity.GetUserId());
                db.Hbl.Add(hbl);
                db.SaveChanges();
                return RedirectToAction("Edit", new { id = hbl.Id });
            }

            return View(hbl);
        }

        // GET: Hbl/Edit/5
        [Authorize]
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Hbl hbl = db.Hbl.Find(id);
            if (hbl == null)
            {
                return HttpNotFound();
            }

            var context = new IdentityDbContext();
            var userList = context.Users.ToList();

            List<Contacto> selectedContacts = db.ContactoEnvio.Where(w => w.Entidad == "hbl" && w.Fuente == hbl.Id).Select(s => s.ContactoNav).ToList();

            if(selectedContacts.Count <= 0)
            {
                ViewBag.AlertNotice = "Los comunicados que envíe en este momento no tendrá destinatarios hasta que guarde la configuración de contactos del HBL";
            }

            ViewBag.Mbl = ((Mbl)Session["Mbl"]).Do;
            ViewBag.Contacts = db.ContactoModulo.Where(w => w.Modulo == module && w.ContactoNav.Entidad == "customer" && w.ContactoNav.Fuente == hbl.Cliente).Select(s => s.ContactoNav).ToList();
            ViewBag.SelectedContacts = selectedContacts;
            ViewBag.NoticesCount = db.Historico.Where(w => w.Fuente == hbl.Id).Select(s => s.Comunicado).ToList();
            ViewBag.Notices = db.Comunicado.Where(w => w.Habilitado && w.Modulo == module).ToList();
            ViewBag.ServicioCliente = new SelectList(userList, "Id", "UserName", hbl.ServicioCliente);
            ViewBag.Cliente = new SelectList(db.Cliente, "Id", "Nombre", hbl.Cliente);
            ViewBag.EmisionBl = new SelectList(db.EmisionBl, "Id", "Nombre", hbl.EmisionBl);
            ViewBag.Flete = new SelectList(db.Flete, "Id", "Nombre", hbl.Flete);
            ViewBag.Tipo = new SelectList(db.HblTipo, "Id", "Nombre", hbl.Tipo);
            ViewBag.Incoterm = new SelectList(db.Incoterm, "Id", "Descripcion", hbl.Incoterm);
            ViewBag.MotonaveEstimadaArribo = new SelectList(db.Motonave, "Id", "Nombre", hbl.MotonaveEstimadaArribo);
            ViewBag.MuelleArribo = new SelectList(db.Muelle, "Id", "Nombre", hbl.MuelleArribo);
            ViewBag.Notify = new SelectList(db.Notify, "Id", "Nombre", hbl.Notify);
            ViewBag.Proveedor = new SelectList(db.Proveedor, "Id", "Nombre", hbl.Proveedor);
            ViewBag.PuertoOrigen = new SelectList(db.Puerto, "Id", "Nombre", hbl.PuertoOrigen);
            ViewBag.PuertoDestino = new SelectList(db.Puerto, "Id", "Nombre", hbl.PuertoDestino);
            ViewBag.RangoDiaLibre = new SelectList(db.RangoDiaLibre.Select(s => new { Id = s.Id, Rango = s.Inicial + " - " + s.Final }), "Id", "Rango", hbl.RangoDiaLibre);
            ViewBag.Sucursal = new SelectList(db.Sucursal, "Id", "Nombre", hbl.Sucursal);
            return View(hbl);
        }

        // POST: Hbl/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "Id,Mbl,Do,Hbl1,Proveedor,Cliente,Notify,Sucursal,ExoneracionDeposito,ExoneracionDroopOff,DiaLibreCliente,RangoDiaLibre,FechaCargue,FechaInstruccion,FechaNotificacion,Tipo,PuertoOrigen,PuertoDestino,Etd,Eta,MotonaveEstimadaArribo,ViajeEstimadoArribo,Peso,Volumen,DescripcionMercancia,Peligrosa,Incoterm,Imo,Un,MuelleArribo,EmisionBl,Flete,Valor,ServicioCliente,FechaLiberacion,QuienLibera,S_Usuario,S_Creacion,Habilitado")] Hbl hbl)
        {
            if (ModelState.IsValid)
            {
                bool flat = true;

                if((((Mbl)Session["Mbl"]).HblNav.Where(w => w.Id != hbl.Id).Sum(s => s.Peso) + hbl.Peso) > ((Mbl)Session["Mbl"]).Peso)
                {
                    ViewBag.Message = "Con el peso asignado a este HBL se supera el peso del MBL";
                    flat = false;
                }

                if ((((Mbl)Session["Mbl"]).HblNav.Where(w => w.Id != hbl.Id).Sum(s => s.Volumen) + hbl.Volumen) > ((Mbl)Session["Mbl"]).Volumen)
                {
                    ViewBag.Message = "Con el volumen asignado a este HBL se supera el volumen del MBL";
                    flat = false;
                }

                if (flat)
                {
                    db.Entry(hbl).State = EntityState.Modified;
                    db.SaveChanges();
                    // return RedirectToAction("Index");
                }
            }

            var context = new IdentityDbContext();
            var userList = context.Users.ToList();
            
            List<Contacto> selectedContacts = db.ContactoEnvio.Where(w => w.Entidad == "hbl" && w.Fuente == hbl.Id).Select(s => s.ContactoNav).ToList();

            if (selectedContacts.Count <= 0)
            {
                ViewBag.AlertNotice = "Los comunicados que envíe en este momento no tendrá destinatarios hasta que guarde la configuración de contactos del HBL";
            }

            ViewBag.Mbl = ((Mbl)Session["Mbl"]).Do;
            ViewBag.Contacts = db.ContactoModulo.Where(w => w.Modulo == module && w.ContactoNav.Entidad == "customer" && w.ContactoNav.Fuente == hbl.Cliente).Select(s => s.ContactoNav).ToList();
            ViewBag.SelectedContacts = selectedContacts;
            ViewBag.NoticesCount = db.Historico.Where(w => w.Fuente == hbl.Id).Select(s => s.Comunicado).ToList();
            ViewBag.Notices = db.Comunicado.Where(w => w.Habilitado && w.Modulo == module).ToList();
            ViewBag.ServicioCliente = new SelectList(userList, "Id", "UserName", hbl.ServicioCliente);
            ViewBag.Cliente = new SelectList(db.Cliente, "Id", "Nombre", hbl.Cliente);
            ViewBag.EmisionBl = new SelectList(db.EmisionBl, "Id", "Nombre", hbl.EmisionBl);
            ViewBag.Flete = new SelectList(db.Flete, "Id", "Nombre", hbl.Flete);
            ViewBag.Tipo = new SelectList(db.HblTipo, "Id", "Nombre", hbl.Tipo);
            ViewBag.Incoterm = new SelectList(db.Incoterm, "Id", "Descripcion", hbl.Incoterm);
            ViewBag.MotonaveEstimadaArribo = new SelectList(db.Motonave, "Id", "Nombre", hbl.MotonaveEstimadaArribo);
            ViewBag.MuelleArribo = new SelectList(db.Muelle, "Id", "Nombre", hbl.MuelleArribo);
            ViewBag.Notify = new SelectList(db.Notify, "Id", "Nombre", hbl.Notify);
            ViewBag.Proveedor = new SelectList(db.Proveedor, "Id", "Nombre", hbl.Proveedor);
            ViewBag.PuertoOrigen = new SelectList(db.Puerto, "Id", "Nombre", hbl.PuertoOrigen);
            ViewBag.PuertoDestino = new SelectList(db.Puerto, "Id", "Nombre", hbl.PuertoDestino);
            ViewBag.RangoDiaLibre = new SelectList(db.RangoDiaLibre.Select(s => new { Id = s.Id, Rango = s.Inicial + " - " + s.Final }), "Id", "Rango", hbl.RangoDiaLibre);
            ViewBag.Sucursal = new SelectList(db.Sucursal, "Id", "Nombre", hbl.Sucursal);
            return View(hbl);
        }

        // GET: Hbl/Delete/5
        [Authorize]
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Hbl hbl = db.Hbl.Find(id);
            if (hbl == null)
            {
                return HttpNotFound();
            }
            return View(hbl);
        }

        // POST: Hbl/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Hbl hbl = db.Hbl.Find(id);
            db.Hbl.Remove(hbl);
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
