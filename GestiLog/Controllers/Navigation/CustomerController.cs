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
    public class CustomerController : Controller
    {
        private DaoEntities db = new DaoEntities();

        // GET: Customer
        [Authorize]
        public ActionResult Index()
        {
            var cliente = db.Cliente.Include(c => c.CiudadNav).Include(c => c.ClienteNav_).Include(c => c.TipoOficinaNav).Include(c => c.SectorNav);
            return View(cliente.ToList());
        }

        // GET: Customer/Details/5
        [Authorize]
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cliente cliente = db.Cliente.Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            return View(cliente);
        }

        // GET: Customer/Create
        [Authorize]
        public ActionResult Create()
        {
            Guid country = Guid.Parse(WebConfigurationManager.AppSettings["SoftCountry"].ToString());
            Guid city = Guid.Parse(WebConfigurationManager.AppSettings["SoftCity"].ToString());
            ViewBag.Pais = new SelectList(db.Pais, "Id", "Nombre", country);
            ViewBag.Ciudad = new SelectList(db.Ciudad.Where(w => w.Pais == country), "Id", "Nombre", city);
            ViewBag.Sector = new SelectList(db.Sector, "Id", "Nombre");
            ViewBag.Sucursal = new SelectList(db.Cliente, "Id", "Nombre");
            ViewBag.TipoOficina = new SelectList(db.TipoOficina, "Id", "Nombre");
            ViewBag.Id = new SelectList(db.Sector, "Id", "Nombre");
            return View();
        }

        // POST: Customer/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nombre,Nit,Dv,Telefono,Direccion,Ciudad,Sector,TipoOficina,Sucursal,Credito,S_Usuario,S_Creacion,Habilitado")] Cliente cliente)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    cliente.Id = Guid.NewGuid();
                    cliente.S_Creacion = DateTime.Now;
                    cliente.S_Usuario = Guid.Parse(User.Identity.GetUserId());
                    db.Cliente.Add(cliente);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "El NIT ya existe, por favor, ingrese uno diferente.");
            }

            Ciudad city = db.Ciudad.Where(w => w.Id == cliente.Ciudad).FirstOrDefault();

            ViewBag.Pais = new SelectList(db.Pais, "Id", "Nombre", city.Pais);
            ViewBag.Ciudad = new SelectList(db.Ciudad.Where(w => w.Pais == city.Pais), "Id", "Nombre", cliente.Ciudad);
            ViewBag.Sector = new SelectList(db.Sector, "Id", "Nombre", cliente.Sector);
            ViewBag.Sucursal = new SelectList(db.Cliente, "Id", "Nombre", cliente.Sucursal);
            ViewBag.TipoOficina = new SelectList(db.TipoOficina, "Id", "Nombre", cliente.TipoOficina);
            ViewBag.Id = new SelectList(db.Sector, "Id", "Nombre", cliente.Id);
            return View(cliente);
        }

        // GET: Customer/Edit/5
        [Authorize]
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cliente cliente = db.Cliente.Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }

            Ciudad city = db.Ciudad.Where(w => w.Id == cliente.Ciudad).FirstOrDefault();

            ViewBag.Pais = new SelectList(db.Pais, "Id", "Nombre", city.Pais);
            ViewBag.Ciudad = new SelectList(db.Ciudad.Where(w => w.Pais == city.Pais), "Id", "Nombre", cliente.Ciudad);
            ViewBag.Sector = new SelectList(db.Sector, "Id", "Nombre", cliente.Sector);
            ViewBag.Sucursal = new SelectList(db.Cliente, "Id", "Nombre", cliente.Sucursal);
            ViewBag.TipoOficina = new SelectList(db.TipoOficina, "Id", "Nombre", cliente.TipoOficina);
            ViewBag.Id = new SelectList(db.Sector, "Id", "Nombre", cliente.Id);
            return View(cliente);
        }

        // POST: Customer/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nombre,Nit,Dv,Telefono,Direccion,Ciudad,Sector,TipoOficina,Sucursal,Credito,S_Usuario,S_Creacion,Habilitado")] Cliente cliente)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(cliente).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "El NIT ya existe, por favor, ingrese uno diferente.");
            }

            Ciudad city = db.Ciudad.Where(w => w.Id == cliente.Ciudad).FirstOrDefault();

            ViewBag.Pais = new SelectList(db.Pais, "Id", "Nombre", city.Pais);
            ViewBag.Ciudad = new SelectList(db.Ciudad.Where(w => w.Pais == city.Pais), "Id", "Nombre", cliente.Ciudad);
            ViewBag.Sector = new SelectList(db.Sector, "Id", "Nombre", cliente.Sector);
            ViewBag.Sucursal = new SelectList(db.Cliente, "Id", "Nombre", cliente.Sucursal);
            ViewBag.TipoOficina = new SelectList(db.TipoOficina, "Id", "Nombre", cliente.TipoOficina);
            ViewBag.Id = new SelectList(db.Sector, "Id", "Nombre", cliente.Id);
            return View(cliente);
        }

        // GET: Customer/Delete/5
        [Authorize]
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cliente cliente = db.Cliente.Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            return View(cliente);
        }

        // POST: Customer/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Cliente cliente = db.Cliente.Find(id);
            db.Cliente.Remove(cliente);
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
