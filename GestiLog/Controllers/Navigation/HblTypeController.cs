﻿using System;
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
    public class HblTypeController : Controller
    {
        private DaoEntities db = new DaoEntities();

        // GET: HblType
        [Authorize]
        public ActionResult Index()
        {
            return View(db.HblTipo.ToList());
        }

        // GET: HblType/Details/5
        [Authorize]
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HblTipo hblTipo = db.HblTipo.Find(id);
            if (hblTipo == null)
            {
                return HttpNotFound();
            }
            return View(hblTipo);
        }

        // GET: HblType/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: HblType/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nombre,S_Usuario,S_Creacion,Habilitado")] HblTipo hblTipo)
        {
            if (ModelState.IsValid)
            {
                hblTipo.Id = Guid.NewGuid();
                hblTipo.S_Creacion = DateTime.Now;
                hblTipo.S_Usuario = Guid.Parse(User.Identity.GetUserId());
                db.HblTipo.Add(hblTipo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(hblTipo);
        }

        // GET: HblType/Edit/5
        [Authorize]
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HblTipo hblTipo = db.HblTipo.Find(id);
            if (hblTipo == null)
            {
                return HttpNotFound();
            }
            return View(hblTipo);
        }

        // POST: HblType/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nombre,S_Usuario,S_Creacion,Habilitado")] HblTipo hblTipo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hblTipo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(hblTipo);
        }

        // GET: HblType/Delete/5
        [Authorize]
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HblTipo hblTipo = db.HblTipo.Find(id);
            if (hblTipo == null)
            {
                return HttpNotFound();
            }
            return View(hblTipo);
        }

        // POST: HblType/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            HblTipo hblTipo = db.HblTipo.Find(id);
            db.HblTipo.Remove(hblTipo);
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
