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
    public class ContactController : Controller
    {
        private DaoEntities db = new DaoEntities();

        // GET: Contact
        [Authorize]
        [Route("Index/{entity}/{source}")]
        public ActionResult Index(string entity, Guid source)
        {
            List<Contacto> contacts = new List<Contacto>();
            contacts = db.Contacto.Where(w => w.Fuente == source && w.Entidad == entity).ToList();
            SetViewBag(entity, source);
            Session["entity"] = entity;
            Session["source"] = source;

            return View(contacts);
        }

        private void SetViewBag(string entity, Guid source)
        {
            switch (entity)
            {
                case "customer":
                    Cliente cliente = db.Cliente.Where(w => w.Id == source).FirstOrDefault();
                    ViewBag.Source = cliente;
                    ViewBag.Entity = "Cliente";
                    ViewBag.Command = entity;
                    ViewBag.Controller = "Customer";
                    break;
                case "shipping":
                    Naviera naviera = db.Naviera.Where(w => w.Id == source).FirstOrDefault();
                    ViewBag.Source = naviera;
                    ViewBag.Entity = "Naviera";
                    ViewBag.Command = entity;
                    ViewBag.Controller = "ShippingCompany";
                    break;
                case "agent":
                    Agente agente = db.Agente.Where(w => w.Id == source).FirstOrDefault();
                    ViewBag.Source = agente;
                    ViewBag.Entity = "Agente";
                    ViewBag.Command = entity;
                    ViewBag.Controller = "Agent";
                    break;
                default:
                    break;
            }
        }

        // GET: Contact/Details/5
        [Authorize]
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contacto contacto = db.Contacto.Find(id);
            SetViewBag(contacto.Entidad, contacto.Fuente);
            if (contacto == null)
            {
                return HttpNotFound();
            }
            return View(contacto);
        }

        // GET: Contact/Create
        [Authorize]
        public ActionResult Create()
        {
            string entity = Session["entity"].ToString();
            Guid source = Guid.Parse(Session["source"].ToString());
            SetViewBag(entity, source);
            ViewBag.Modulos = new MultiSelectList(db.Modulo, "Id", "Nombre");
            return View();
        }

        // POST: Contact/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nombre,Correo,Cargo,Celular,Fuente,Entidad,S_Usuario,S_Creacion,Modulos")] ContactViewModel registro)
        {
            if (ModelState.IsValid)
            {
                Contacto contacto = registro.ObtenerContacto();
                contacto.Id = Guid.NewGuid();
                contacto.Entidad = Session["entity"].ToString();
                contacto.Fuente = Guid.Parse(Session["source"].ToString());
                contacto.S_Creacion = DateTime.Now;
                contacto.S_Usuario = Guid.Parse(User.Identity.GetUserId());
                db.Contacto.Add(contacto);
                db.SaveChanges();
                return RedirectToAction("Index", new { entity = contacto.Entidad, source = contacto.Fuente });
            }

            SetViewBag(Session["entity"].ToString(), Guid.Parse(Session["source"].ToString()));
            ViewBag.Modulos = new MultiSelectList(db.Modulo, "Id", "Nombre", registro.Modulos);
            return View(registro);
        }

        // GET: Contact/Edit/5
        [Authorize]
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contacto contacto = db.Contacto.Where(w => w.Id == id).Include("ContactoModuloNav").Include("ContactoModuloNav.ModuloNav").FirstOrDefault();
            
            if (contacto == null)
            {
                return HttpNotFound();
            }

            ContactViewModel registro = new ContactViewModel() {
                Id = contacto.Id,
                Nombre = contacto.Nombre,
                Celular = contacto.Celular,
                Correo = contacto.Correo,
                Cargo = contacto.Cargo,
                Entidad = contacto.Entidad,
                Fuente = contacto.Fuente,
                S_Creacion = contacto.S_Creacion,
                S_Usuario = contacto.S_Usuario
            };

            registro.Modulos = contacto.ContactoModuloNav.Select(s => s.Modulo).ToList();
            ViewBag.Modulos = new MultiSelectList(db.Modulo, "Id", "Nombre", registro.Modulos);
            SetViewBag(contacto.Entidad, contacto.Fuente);
            return View(registro);
        }

        // POST: Contact/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nombre,Correo,Cargo,Celular,Fuente,Entidad,S_Usuario,S_Creacion,Modulos")] ContactViewModel registro)
        {
            if (ModelState.IsValid)
            {
                Contacto contacto = db.Contacto.Where(w => w.Id == registro.Id).Include("ContactoModuloNav").FirstOrDefault();

                contacto.Id = registro.Id;
                contacto.Nombre = registro.Nombre;
                contacto.Celular = registro.Celular;
                contacto.Correo = registro.Correo;
                contacto.Cargo = registro.Cargo;
                contacto.Entidad = registro.Entidad;
                contacto.Fuente = registro.Fuente;
                contacto.S_Creacion = registro.S_Creacion;
                contacto.S_Usuario = registro.S_Usuario;

                for (int i = 0; i < contacto.ContactoModuloNav.Count; i++)
                {
                    db.ContactoModulo.Remove(contacto.ContactoModuloNav.ElementAt(i));
                    db.SaveChanges();
                }

                if (registro.Modulos != null)
                {
                    foreach (var modulo in registro.Modulos)
                    {
                        contacto.ContactoModuloNav.Add(new ContactoModulo()
                        {
                            Id = Guid.NewGuid(),
                            Contacto = contacto.Id,
                            Modulo = db.Modulo.Where(w => w.Id == modulo).Select(s => s.Id).FirstOrDefault()
                        });
                    }
                }

                db.Entry(contacto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { entity = contacto.Entidad, source = contacto.Fuente });
            }

            ViewBag.Modulos = new MultiSelectList(db.Modulo, "Id", "Nombre", registro.Modulos);
            SetViewBag(Session["entity"].ToString(), Guid.Parse(Session["source"].ToString()));
            return View(registro);
        }

        // GET: Contact/Delete/5
        [Authorize]
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contacto contacto = db.Contacto.Find(id);
            SetViewBag(contacto.Entidad, contacto.Fuente);
            if (contacto == null)
            {
                return HttpNotFound();
            }
            return View(contacto);
        }

        // POST: Contact/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Contacto contacto = db.Contacto.Find(id);
            db.Contacto.Remove(contacto);
            db.SaveChanges();
            return RedirectToAction("Index", new { entity = contacto.Entidad, source = contacto.Fuente });
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
