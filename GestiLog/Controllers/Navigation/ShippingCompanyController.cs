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
    public class ShippingCompanyController : Controller
    {
        private DaoEntities db = new DaoEntities();

        // GET: ShippingCompany
        [Authorize]
        public ActionResult Index()
        {
            var naviera = db.Naviera.Include(n => n.CiudadNav).Include(n => n.NavieraNav_).Include(n => n.TipoOficinaNav);
            return View(naviera.ToList());
        }

        // GET: ShippingCompany/Details/5
        [Authorize]
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Naviera naviera = db.Naviera.Find(id);
            if (naviera == null)
            {
                return HttpNotFound();
            }
            return View(naviera);
        }

        // GET: ShippingCompany/Create
        [Authorize]
        public ActionResult Create()
        {
            Guid country = Guid.Parse(WebConfigurationManager.AppSettings["SoftCountry"].ToString());
            Guid city = Guid.Parse(WebConfigurationManager.AppSettings["SoftCity"].ToString());
            ViewBag.Pais = new SelectList(db.Pais, "Id", "Nombre", country);
            ViewBag.Ciudad = new SelectList(db.Ciudad.Where(w => w.Pais == country), "Id", "Nombre", city);
            ViewBag.Sucursal = new SelectList(db.Naviera, "Id", "Nombre");
            ViewBag.TipoOficina = new SelectList(db.TipoOficina, "Id", "Nombre");
            return View();
        }

        // POST: ShippingCompany/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "Id,Nombre,Nit,Dv,Telefono,Direccion,Ciudad,TipoOficina,Sucursal,Representante,S_Usuario,S_Creacion,Habilitado")] Naviera naviera)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    naviera.Id = Guid.NewGuid();
                    naviera.S_Creacion = DateTime.Now;
                    naviera.S_Usuario = Guid.Parse(User.Identity.GetUserId());
                    db.Naviera.Add(naviera);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "El NIT ya existe, por favor, ingrese uno diferente.");
            }

            Ciudad city = db.Ciudad.Where(w => w.Id == naviera.Ciudad).FirstOrDefault();

            ViewBag.Pais = new SelectList(db.Pais, "Id", "Nombre", city.Pais);
            ViewBag.Ciudad = new SelectList(db.Ciudad.Where(w => w.Pais == city.Pais), "Id", "Nombre", naviera.Ciudad);
            ViewBag.Sucursal = new SelectList(db.Naviera, "Id", "Nombre", naviera.Sucursal);
            ViewBag.TipoOficina = new SelectList(db.TipoOficina, "Id", "Nombre", naviera.TipoOficina);
            return View(naviera);
        }

        // GET: ShippingCompany/Edit/5
        [Authorize]
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Naviera naviera = db.Naviera.Find(id);
            if (naviera == null)
            {
                return HttpNotFound();
            }
            Ciudad city = db.Ciudad.Where(w => w.Id == naviera.Ciudad).FirstOrDefault();

            ViewBag.Pais = new SelectList(db.Pais, "Id", "Nombre", city.Pais);
            ViewBag.Ciudad = new SelectList(db.Ciudad.Where(w => w.Pais == city.Pais), "Id", "Nombre", naviera.Ciudad);
            ViewBag.Sucursal = new SelectList(db.Naviera, "Id", "Nombre", naviera.Sucursal);
            ViewBag.TipoOficina = new SelectList(db.TipoOficina, "Id", "Nombre", naviera.TipoOficina);
            return View(naviera);
        }

        // POST: ShippingCompany/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "Id,Nombre,Nit,Dv,Telefono,Direccion,Ciudad,TipoOficina,Sucursal,Representante,S_Usuario,S_Creacion,Habilitado")] Naviera naviera)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(naviera).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "El NIT ya existe, por favor, ingrese uno diferente.");
            }

            Ciudad city = db.Ciudad.Where(w => w.Id == naviera.Ciudad).FirstOrDefault();

            ViewBag.Pais = new SelectList(db.Pais, "Id", "Nombre", city.Pais);
            ViewBag.Ciudad = new SelectList(db.Ciudad.Where(w => w.Pais == city.Pais), "Id", "Nombre", naviera.Ciudad);
            ViewBag.Sucursal = new SelectList(db.Naviera, "Id", "Nombre", naviera.Sucursal);
            ViewBag.TipoOficina = new SelectList(db.TipoOficina, "Id", "Nombre", naviera.TipoOficina);
            return View(naviera);
        }

        // GET: ShippingCompany/Delete/5
        [Authorize]
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Naviera naviera = db.Naviera.Find(id);
            if (naviera == null)
            {
                return HttpNotFound();
            }
            return View(naviera);
        }

        // POST: ShippingCompany/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Naviera naviera = db.Naviera.Find(id);
            db.Naviera.Remove(naviera);
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
