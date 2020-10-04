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
    public class HawbController : Controller
    {
        private DaoEntities db = new DaoEntities();
        private Guid module = Guid.Parse("32916188-AD05-4BCB-A89B-467260729CB5");

        // GET: Hawb
        [Authorize]
        public ActionResult Index(Guid id)
        {
            Session["Mawb"] = db.Mawb.Where(w => w.Id == id).FirstOrDefault();
            ViewBag.Mawb = ((Mawb)Session["Mawb"]).Do;
            ViewBag.MawbId = ((Mawb)Session["Mawb"]).Id;
            var hawb = db.Hawb.Include(h => h.AeropuertoOrigenNav).Include(h => h.AeropuertoDestinoNav).Include(h => h.ClienteNav).Include(h => h.DisposicionNav).Include(h => h.EmbalajeNav).Include(h => h.EmisionAwbNav).Include(h => h.HawbTipoNav).Include(h => h.IncotermNav).Include(h => h.MawbNav).Include(h => h.NotifyNav).Include(h => h.ProveedorNav).Include(h => h.SucursalNav);
            return View(hawb.ToList());
        }

        // GET: Hawb/Details/5
        [Authorize]
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Hawb hawb = db.Hawb.Find(id);
            if (hawb == null)
            {
                return HttpNotFound();
            }
            return View(hawb);
        }

        // GET: Hawb/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.Mawb = ((Mawb)Session["Mawb"]).Do;
            ViewBag.MawbId = ((Mawb)Session["Mawb"]).Id;
            return View();
        }

        // POST: Hawb/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "Id,Mawb,Do,Hbl,Tipo,Cliente,Proveedor,Notify,Sucursal,Incoterm,AeropuertoOrigen,AeropuertoDestino,Peso,PesoCargable,DimensionLargo,DimensionAncho,DimensionAlto,Volumen,Embalaje,Piezas,Mercancia,Imo,Un,EntregaDocumentoFecha,EntregaDocumentoNombre,ConfirmacionArribo,Disposicion,EmisionAwb,RegistroAduanero,FechaRegistro,FechaLiberacion,QuienLibera,S_Usuario,S_Creacion,Habilitado")] Hawb hawb)
        {
            if (ModelState.IsValid)
            {
                hawb.Id = Guid.NewGuid();
                hawb.Mawb = ((Mawb)Session["Mawb"]).Id;
                hawb.Habilitado = true;
                hawb.S_Creacion = DateTime.Now;
                hawb.S_Usuario = Guid.Parse(User.Identity.GetUserId());
                db.Hawb.Add(hawb);
                db.SaveChanges();
                return RedirectToAction("Edit", new { id = hawb.Id });
            }

            return View(hawb);
        }

        // GET: Hawb/Edit/5
        [Authorize]
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Hawb hawb = db.Hawb.Find(id);
            if (hawb == null)
            {
                return HttpNotFound();
            }

            List<Contacto> selectedContacts = db.ContactoEnvio.Where(w => w.Entidad == "hawb" && w.Fuente == hawb.Id).Select(s => s.ContactoNav).ToList();

            if (selectedContacts.Count <= 0)
            {
                ViewBag.AlertNotice = "Los comunicados que envíe en este momento no tendrá destinatarios hasta que guarde la configuración de contactos del HAWB";
            }

            ViewBag.Mawb = ((Mawb)Session["Mawb"]).Do;
            ViewBag.Contacts = db.ContactoModulo.Where(w => w.Modulo == module && w.ContactoNav.Entidad == "customer" && w.ContactoNav.Fuente == hawb.Cliente).Select(s => s.ContactoNav).ToList();
            ViewBag.SelectedContacts = selectedContacts;
            ViewBag.NoticesCount = db.Historico.Where(w => w.Fuente == hawb.Id).Select(s => s.Comunicado).ToList();
            ViewBag.Notices = db.Comunicado.Where(w => w.Habilitado && w.Modulo == module).ToList();
            ViewBag.AeropuertoOrigen = new SelectList(db.Aeropuerto, "Id", "Nombre", hawb.AeropuertoOrigen);
            ViewBag.AeropuertoDestino = new SelectList(db.Aeropuerto, "Id", "Nombre", hawb.AeropuertoDestino);
            ViewBag.Cliente = new SelectList(db.Cliente, "Id", "Nombre", hawb.Cliente);
            ViewBag.Disposicion = new SelectList(db.Disposicion, "Id", "Nombre", hawb.Disposicion);
            ViewBag.Embalaje = new SelectList(db.Embalaje, "Id", "Descripcion", hawb.Embalaje);
            ViewBag.EmisionAwb = new SelectList(db.EmisionAwb, "Id", "Nombre", hawb.EmisionAwb);
            ViewBag.Tipo = new SelectList(db.HawbTipo, "Id", "Nombre", hawb.Tipo);
            ViewBag.Incoterm = new SelectList(db.Incoterm, "Id", "Descripcion", hawb.Incoterm);
            ViewBag.Notify = new SelectList(db.Notify, "Id", "Nombre", hawb.Notify);
            ViewBag.Proveedor = new SelectList(db.Proveedor, "Id", "Nombre", hawb.Proveedor);
            ViewBag.Sucursal = new SelectList(db.Sucursal, "Id", "Nombre", hawb.Sucursal);
            return View(hawb);
        }

        // POST: Hawb/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "Id,Mawb,Do,Hbl,Tipo,Cliente,Proveedor,Notify,Sucursal,Incoterm,AeropuertoOrigen,AeropuertoDestino,Peso,PesoCargable,DimensionLargo,DimensionAncho,DimensionAlto,Volumen,Embalaje,Piezas,Mercancia,Imo,Un,EntregaDocumentoFecha,EntregaDocumentoNombre,ConfirmacionArribo,Disposicion,EmisionAwb,RegistroAduanero,FechaRegistro,FechaLiberacion,QuienLibera,S_Usuario,S_Creacion,Habilitado")] Hawb hawb)
        {
            if (ModelState.IsValid)
            {
                bool flat = true;

                if ((((Mawb)Session["Mawb"]).HawbNav.Where(w => w.Id != hawb.Id).Sum(s => s.Peso) + hawb.Peso) > ((Mawb)Session["Mawb"]).Peso)
                {
                    ViewBag.Message = "Con el peso asignado a este HAWB se supera el peso del MAWB";
                    flat = false;
                }

                if ((((Mawb)Session["Mawb"]).HawbNav.Where(w => w.Id != hawb.Id).Sum(s => s.Volumen) + hawb.Volumen) > ((Mawb)Session["Mawb"]).Volumen)
                {
                    ViewBag.Message = "Con el volumen asignado a este HAWB se supera el volumen del MAWB";
                    flat = false;
                }

                if (flat)
                {
                    db.Entry(hawb).State = EntityState.Modified;
                    db.SaveChanges();
                    // return RedirectToAction("Index");
                }
            }

            List<Contacto> selectedContacts = db.ContactoEnvio.Where(w => w.Entidad == "hawb" && w.Fuente == hawb.Id).Select(s => s.ContactoNav).ToList();

            if (selectedContacts.Count <= 0)
            {
                ViewBag.AlertNotice = "Los comunicados que envíe en este momento no tendrá destinatarios hasta que guarde la configuración de contactos del HAWB";
            }

            ViewBag.Mawb = ((Mawb)Session["Mawb"]).Do;
            ViewBag.Contacts = db.ContactoModulo.Where(w => w.Modulo == module && w.ContactoNav.Entidad == "customer" && w.ContactoNav.Fuente == hawb.Cliente).Select(s => s.ContactoNav).ToList();
            ViewBag.SelectedContacts = selectedContacts;
            ViewBag.NoticesCount = db.Historico.Where(w => w.Fuente == hawb.Id).Select(s => s.Comunicado).ToList();
            ViewBag.Notices = db.Comunicado.Where(w => w.Habilitado && w.Modulo == module).ToList();
            ViewBag.AeropuertoOrigen = new SelectList(db.Aeropuerto, "Id", "Nombre", hawb.AeropuertoOrigen);
            ViewBag.AeropuertoDestino = new SelectList(db.Aeropuerto, "Id", "Nombre", hawb.AeropuertoDestino);
            ViewBag.Cliente = new SelectList(db.Cliente, "Id", "Nombre", hawb.Cliente);
            ViewBag.Disposicion = new SelectList(db.Disposicion, "Id", "Nombre", hawb.Disposicion);
            ViewBag.Embalaje = new SelectList(db.Embalaje, "Id", "Descripcion", hawb.Embalaje);
            ViewBag.EmisionAwb = new SelectList(db.EmisionAwb, "Id", "Nombre", hawb.EmisionAwb);
            ViewBag.Tipo = new SelectList(db.HawbTipo, "Id", "Nombre", hawb.Tipo);
            ViewBag.Incoterm = new SelectList(db.Incoterm, "Id", "Descripcion", hawb.Incoterm);
            ViewBag.Notify = new SelectList(db.Notify, "Id", "Nombre", hawb.Notify);
            ViewBag.Proveedor = new SelectList(db.Proveedor, "Id", "Nombre", hawb.Proveedor);
            ViewBag.Sucursal = new SelectList(db.Sucursal, "Id", "Nombre", hawb.Sucursal);
            return View(hawb);
        }

        // GET: Hawb/Delete/5
        [Authorize]
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Hawb hawb = db.Hawb.Find(id);
            if (hawb == null)
            {
                return HttpNotFound();
            }
            return View(hawb);
        }

        // POST: Hawb/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Hawb hawb = db.Hawb.Find(id);
            db.Hawb.Remove(hawb);
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
