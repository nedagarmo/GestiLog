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
    public class AgentController : Controller
    {
        private DaoEntities db = new DaoEntities();

        // GET: Agent
        [Authorize]
        public ActionResult Index()
        {
            var agente = db.Agente.Include(a => a.AgenteNav_).Include(a => a.CiudadNav).Include(a => a.TipoOficinaNav).Include(a => a.TipoAgenteNav);
            return View(agente.ToList());
        }

        // GET: Agent/Details/5
        [Authorize]
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Agente agente = db.Agente.Find(id);
            if (agente == null)
            {
                return HttpNotFound();
            }
            return View(agente);
        }

        // GET: Agent/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.Sucursal = new SelectList(db.Agente, "Id", "Nombre");
            Guid country = Guid.Parse(WebConfigurationManager.AppSettings["SoftCountry"].ToString());
            Guid city = Guid.Parse(WebConfigurationManager.AppSettings["SoftCity"].ToString());
            ViewBag.Pais = new SelectList(db.Pais, "Id", "Nombre", country);
            ViewBag.Ciudad = new SelectList(db.Ciudad.Where(w => w.Pais == country), "Id", "Nombre", city);
            ViewBag.TipoOficina = new SelectList(db.TipoOficina, "Id", "Nombre");
            ViewBag.TipoAgente = new SelectList(db.TipoAgente, "Id", "Nombre");
            return View();
        }

        // POST: Agent/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nombre,Nit,Dv,Direccion,Ciudad,TipoOficina,TipoAgente,Sucursal,S_Usuario,S_Creacion,Habilitado")] Agente agente)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    agente.Id = Guid.NewGuid();
                    agente.S_Creacion = DateTime.Now;
                    agente.S_Usuario = Guid.Parse(User.Identity.GetUserId());
                    db.Agente.Add(agente);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "El NIT ya existe, por favor, ingrese uno diferente.");
            }

            ViewBag.Sucursal = new SelectList(db.Agente, "Id", "Nombre", agente.Sucursal);
            Ciudad city = db.Ciudad.Where(w => w.Id == agente.Ciudad).FirstOrDefault();

            ViewBag.Pais = new SelectList(db.Pais, "Id", "Nombre", city.Pais);
            ViewBag.Ciudad = new SelectList(db.Ciudad.Where(w => w.Pais == city.Pais), "Id", "Nombre", agente.Ciudad);
            ViewBag.TipoOficina = new SelectList(db.TipoOficina, "Id", "Nombre", agente.TipoOficina);
            ViewBag.TipoAgente = new SelectList(db.TipoAgente, "Id", "Nombre", agente.TipoAgente);
            return View(agente);
        }

        // GET: Agent/Edit/5
        [Authorize]
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Agente agente = db.Agente.Find(id);
            if (agente == null)
            {
                return HttpNotFound();
            }
            ViewBag.Sucursal = new SelectList(db.Agente, "Id", "Nombre", agente.Sucursal);
            Ciudad city = db.Ciudad.Where(w => w.Id == agente.Ciudad).FirstOrDefault();

            ViewBag.Pais = new SelectList(db.Pais, "Id", "Nombre", city.Pais);
            ViewBag.Ciudad = new SelectList(db.Ciudad.Where(w => w.Pais == city.Pais), "Id", "Nombre", agente.Ciudad);
            ViewBag.TipoOficina = new SelectList(db.TipoOficina, "Id", "Nombre", agente.TipoOficina);
            ViewBag.TipoAgente = new SelectList(db.TipoAgente, "Id", "Nombre", agente.TipoAgente);
            return View(agente);
        }

        // POST: Agent/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nombre,Nit,Dv,Direccion,Ciudad,TipoOficina,TipoAgente,Sucursal,S_Usuario,S_Creacion,Habilitado")] Agente agente)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(agente).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "El NIT ya existe, por favor, ingrese uno diferente.");
            }

            ViewBag.Sucursal = new SelectList(db.Agente, "Id", "Nombre", agente.Sucursal);
            Ciudad city = db.Ciudad.Where(w => w.Id == agente.Ciudad).FirstOrDefault();

            ViewBag.Pais = new SelectList(db.Pais, "Id", "Nombre", city.Pais);
            ViewBag.Ciudad = new SelectList(db.Ciudad.Where(w => w.Pais == city.Pais), "Id", "Nombre", agente.Ciudad);
            ViewBag.TipoOficina = new SelectList(db.TipoOficina, "Id", "Nombre", agente.TipoOficina);
            ViewBag.TipoAgente = new SelectList(db.TipoAgente, "Id", "Nombre", agente.TipoAgente);
            return View(agente);
        }

        // GET: Agent/Delete/5
        [Authorize]
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Agente agente = db.Agente.Find(id);
            if (agente == null)
            {
                return HttpNotFound();
            }
            return View(agente);
        }

        // POST: Agent/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Agente agente = db.Agente.Find(id);
            db.Agente.Remove(agente);
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
