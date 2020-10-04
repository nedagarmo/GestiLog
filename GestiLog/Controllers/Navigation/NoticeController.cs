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
    public class NoticeController : Controller
    {
        private DaoEntities db = new DaoEntities();

        // GET: Notice
        public ActionResult Index(Guid module)
        {
            ViewBag.Module = module;
            return View(db.Comunicado.Where(w => w.Modulo == module).ToList());
        }

        // GET: Notice/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comunicado comunicado = db.Comunicado.Find(id);
            if (comunicado == null)
            {
                return HttpNotFound();
            }
            return View(comunicado);
        }

        // GET: Notice/Create
        public ActionResult Create(Guid module)
        {
            ViewBag.AdjuntosMBL = new MultiSelectList(db.ComunicadoCampo.Where(w => w.Tipo == "file" && w.Entidad == "mbl" && w.Modulo == module), "Codigo", "Nombre");
            ViewBag.AdjuntosHBL = new MultiSelectList(db.ComunicadoCampo.Where(w => w.Tipo == "file" && w.Entidad == "hbl" && w.Modulo == module), "Codigo", "Nombre");

            return View(new NoticeViewModel() { Modulo = module });
        }

        // POST: Notice/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nombre,Codigo,Contenido,Modulo,S_Usuario,S_Creacion,Habilitado,AdjuntosMBL,AdjuntosHBL")] NoticeViewModel notice)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Comunicado comunicado = new Comunicado();
                    comunicado.Id = Guid.NewGuid();
                    comunicado.Codigo = notice.Codigo;
                    comunicado.Nombre = notice.Nombre;
                    comunicado.Contenido = notice.Contenido;
                    comunicado.Modulo = notice.Modulo;
                    comunicado.S_Creacion = DateTime.Now;
                    comunicado.S_Usuario = Guid.Parse(User.Identity.GetUserId());
                    comunicado.Habilitado = notice.Habilitado;

                    if (notice.AdjuntosMBL != null)
                    {
                        foreach (var adjunto in notice.AdjuntosMBL)
                        {
                            comunicado.ComunicadoAdjuntoNav.Add(new ComunicadoAdjunto()
                            {
                                Id = Guid.NewGuid(),
                                Comunicado = comunicado.Id,
                                Campo = adjunto,
                                Entidad = "mbl"
                            });
                        }
                    }

                    if (notice.AdjuntosHBL != null)
                    {
                        foreach (var adjunto in notice.AdjuntosHBL)
                        {
                            comunicado.ComunicadoAdjuntoNav.Add(new ComunicadoAdjunto()
                            {
                                Id = Guid.NewGuid(),
                                Comunicado = comunicado.Id,
                                Campo = adjunto,
                                Entidad = "hbl"
                            });
                        }
                    }

                    db.Comunicado.Add(comunicado);
                    db.SaveChanges();
                    return RedirectToAction("Index", new { module = notice.Modulo });
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "El código del comunicado ya existe, por favor, ingrese otro código.");
            }

            ViewBag.AdjuntosMBL = new MultiSelectList(db.ComunicadoCampo.Where(w => w.Tipo == "file" && w.Entidad == "mbl" && w.Modulo == notice.Modulo), "Codigo", "Nombre", notice.AdjuntosMBL);
            ViewBag.AdjuntosHBL = new MultiSelectList(db.ComunicadoCampo.Where(w => w.Tipo == "file" && w.Entidad == "hbl" && w.Modulo == notice.Modulo), "Codigo", "Nombre", notice.AdjuntosHBL);
            return View(notice);
        }

        // GET: Notice/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comunicado comunicado = db.Comunicado.Where(w => w.Id == id).Include(i => i.ComunicadoAdjuntoNav).FirstOrDefault();
            if (comunicado == null)
            {
                return HttpNotFound();
            }

            NoticeViewModel notice = new NoticeViewModel();
            notice.Id = comunicado.Id;
            notice.Codigo = comunicado.Codigo;
            notice.Nombre = comunicado.Nombre;
            notice.Contenido = comunicado.Contenido;
            notice.Modulo = comunicado.Modulo;
            notice.S_Creacion = comunicado.S_Creacion;
            notice.S_Usuario = comunicado.S_Usuario;
            notice.Habilitado = comunicado.Habilitado;

            notice.AdjuntosMBL = comunicado.ComunicadoAdjuntoNav.Where(w => w.Entidad == "mbl").Select(s => s.Campo).ToList();
            notice.AdjuntosHBL = comunicado.ComunicadoAdjuntoNav.Where(w => w.Entidad == "hbl").Select(s => s.Campo).ToList();

            ViewBag.AdjuntosMBL = new MultiSelectList(db.ComunicadoCampo.Where(w => w.Tipo == "file" && w.Entidad == "mbl" && w.Modulo == notice.Modulo), "Codigo", "Nombre", notice.AdjuntosMBL);
            ViewBag.AdjuntosHBL = new MultiSelectList(db.ComunicadoCampo.Where(w => w.Tipo == "file" && w.Entidad == "hbl" && w.Modulo == notice.Modulo), "Codigo", "Nombre", notice.AdjuntosMBL);
            return View(notice);
        }

        // POST: Notice/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nombre,Codigo,Contenido,S_Usuario,S_Creacion,Habilitado,AdjuntosMBL,AdjuntosHBL")] NoticeViewModel notice)
        {
            Comunicado comunicado = db.Comunicado.Where(w => w.Id == notice.Id).Include(i => i.ComunicadoAdjuntoNav).FirstOrDefault();
            try
            {
                if (ModelState.IsValid)
                {
                    comunicado.Nombre = notice.Nombre;
                    comunicado.Codigo = notice.Codigo;
                    comunicado.Contenido = notice.Contenido;
                    comunicado.Habilitado = notice.Habilitado;

                    for (int i = 0; i < comunicado.ComunicadoAdjuntoNav.Count; i++)
                    {
                        db.ComunicadoAdjunto.Remove(comunicado.ComunicadoAdjuntoNav.ElementAt(i));
                        db.SaveChanges();
                    }

                    if (notice.AdjuntosMBL != null)
                    {
                        foreach (var adjunto in notice.AdjuntosMBL)
                        {
                            comunicado.ComunicadoAdjuntoNav.Add(new ComunicadoAdjunto()
                            {
                                Id = Guid.NewGuid(),
                                Comunicado = comunicado.Id,
                                Campo = adjunto,
                                Entidad = "mbl"
                            });
                        }
                    }

                    if (notice.AdjuntosHBL != null)
                    {
                        foreach (var adjunto in notice.AdjuntosHBL)
                        {
                            comunicado.ComunicadoAdjuntoNav.Add(new ComunicadoAdjunto()
                            {
                                Id = Guid.NewGuid(),
                                Comunicado = comunicado.Id,
                                Campo = adjunto,
                                Entidad = "hbl"
                            });
                        }
                    }

                    db.Entry(comunicado).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index", new { module = comunicado.Modulo });
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "El código del comunicado ya existe, por favor, ingrese otro código.");
            }

            ViewBag.AdjuntosMBL = new MultiSelectList(db.ComunicadoCampo.Where(w => w.Tipo == "file" && w.Entidad == "mbl" && w.Modulo == comunicado.Modulo), "Codigo", "Nombre", notice.AdjuntosMBL);
            ViewBag.AdjuntosHBL = new MultiSelectList(db.ComunicadoCampo.Where(w => w.Tipo == "file" && w.Entidad == "hbl" && w.Modulo == comunicado.Modulo), "Codigo", "Nombre", notice.AdjuntosHBL);
            return View(notice);
        }

        // GET: Notice/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comunicado comunicado = db.Comunicado.Find(id);
            if (comunicado == null)
            {
                return HttpNotFound();
            }
            return View(comunicado);
        }

        // POST: Notice/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Comunicado comunicado = db.Comunicado.Find(id);
            db.Comunicado.Remove(comunicado);
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
