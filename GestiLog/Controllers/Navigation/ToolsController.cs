using GestiLog.Entities;
using GestiLog.Helpers;
using GestiLog.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace GestiLog.Controllers.Navigation
{
    public class ToolsController : Controller
    {
        private DaoEntities db = new DaoEntities();

        // POST: Tools/GetFiles/Command
        [HttpPost]
        [Authorize]
        public ActionResult GetFiles(string command, string entity, Guid source)
        {
            var iurl = WebConfigurationManager.AppSettings["SoftUrl"].ToString();
            UploaderViewModel response = new UploaderViewModel();
            List<Adjunto> adjuntos = db.Adjunto.Where(w => w.Campo == command && w.Entidad == entity && w.Fuente == source && w.Habilitado).ToList();

            foreach (var adjunto in adjuntos)
            {
                response.initialPreview.Add(iurl + "/Uploads/" + command + "/" + adjunto.Nombre);
                response.initialPreviewConfig.Add(new FileUploaded()
                {
                    caption = adjunto.Nombre,
                    key = adjunto.Id.ToString(),
                    size = adjunto.Tamano,
                    url = Url.Action("DeleteFile", "Tools", new { id = adjunto.Id }),
                    width = "120px"
                });
            }

            return Json(response);
        }

        // POST: Tools/UploadFile/Command
        [HttpPost]
        [Authorize]
        public ActionResult UploadFile(string command, string entity, Guid source)
        {
            UploaderViewModel response = new UploaderViewModel();
            var iurl = WebConfigurationManager.AppSettings["SoftUrl"].ToString();

            if (Request.Files.Count > 0)
            {
                Adjunto attached = new Adjunto();
                attached.Id = Guid.NewGuid();

                if (!Directory.Exists(Server.MapPath("~/Uploads/" + command)))
                {
                    Directory.CreateDirectory(Server.MapPath("~/Uploads/" + command));
                }

                int i = 0;
                while (i < Request.Files.Count)
                {
                    var file = Request.Files[i];
                    if (file != null && file.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var path = Path.Combine(Server.MapPath("~/Uploads/" + command), fileName);

                        if (System.IO.File.Exists(path))
                        {
                            string rename = attached.Id.ToString().Substring(0, 8);
                            fileName = rename + "-" + fileName;
                            path = Path.Combine(Server.MapPath("~/Uploads/" + command), fileName);
                        }

                        file.SaveAs(path);
                        attached.Nombre = fileName;
                        attached.Entidad = entity;
                        attached.Campo = command;
                        attached.Fuente = source;
                        attached.Tamano = file.ContentLength;
                        attached.S_Creacion = DateTime.Now;
                        attached.S_Usuario = Guid.Parse(User.Identity.GetUserId());
                        attached.Habilitado = true;

                        db.Adjunto.Add(attached);
                        db.SaveChanges();


                        response.initialPreview.Add(iurl + "/Uploads/" + command + "/" + fileName);
                        response.initialPreviewConfig.Add(new FileUploaded()
                        {
                            caption = fileName,
                            key = attached.Id.ToString(),
                            size = file.ContentLength,
                            url = Url.Action("DeleteFile", "Tools", new { id = attached.Id }),
                            width = "120px"
                        });
                    }

                    i++;
                }

            }

            return Json(response);
        }

        [Authorize]
        public ActionResult DeleteFile(Guid? id)
        {
            Adjunto attached = db.Adjunto.Find(id);
            attached.Habilitado = false;
            db.Entry(attached).State = EntityState.Modified;
            db.SaveChanges();

            return Json(new { });
        }

        // POST: Tools/GetFields/Entity
        [HttpPost]
        [Authorize]
        public ActionResult GetFields(string entity)
        {
            List<ISelectViewModel> response = new List<ISelectViewModel>();
            List<ComunicadoCampo> campos = db.ComunicadoCampo.Where(w => w.Entidad == entity && w.Tipo != "file" && w.Habilitado).ToList();

            foreach (var campo in campos)
            {
                response.Add(new ISelectViewModel()
                {
                    label = campo.Nombre,
                    value = campo.Codigo
                });
            }

            return Json(response);
        }

        // GET: Tools/SendNotice/5
        [HttpPost]
        [Authorize]
        public ActionResult SendNotice(Guid notice, string entity, Guid source, Guid module)
        {
            Comunicado comunicado = db.Comunicado.Where(w => w.Id == notice).Include(i => i.ComunicadoAdjuntoNav).FirstOrDefault();
            if (comunicado == null)
            {
                return Json(0);
            }

            if (entity.Equals("mbl"))
            {
                Mbl mbl = db.Mbl.Where(w => w.Id == source).Include(i => i.HblNav).FirstOrDefault();
                foreach (var hbl in mbl.HblNav)
                {
                    ProcessNoticeBl(comunicado, hbl, mbl, module);
                }
            }
            else if (entity.Equals("hbl"))
            {
                Hbl hbl = db.Hbl.Where(w => w.Id == source).Include(i => i.MblNav).FirstOrDefault();
                ProcessNoticeBl(comunicado, hbl, hbl.MblNav, module);
            }
            else if (entity.Equals("mawb"))
            {
                Mawb mawb = db.Mawb.Where(w => w.Id == source).Include(i => i.HawbNav).FirstOrDefault();
                foreach (var hawb in mawb.HawbNav)
                {
                    ProcessNoticeAwb(comunicado, hawb, mawb, module);
                }
            }
            else if (entity.Equals("hawb"))
            {
                Hawb hawb = db.Hawb.Where(w => w.Id == source).Include(i => i.MawbNav).FirstOrDefault();
                ProcessNoticeAwb(comunicado, hawb, hawb.MawbNav, module);
            }

            return Json(1);
        }

        private void ProcessNoticeBl(Comunicado comunicado, Hbl hbl, Mbl mbl, Guid module)
        {
            string pattern = "<i.*>(.*)<\\/i>";
            MatchCollection matches = Regex.Matches(comunicado.Contenido, pattern);
            string contenido = comunicado.Contenido;
            foreach (Match tags in matches)
            {
                string codigo = tags.Groups[1].Value.Substring(1);
                ComunicadoCampo campo = db.ComunicadoCampo.Where(w => w.Codigo == codigo).FirstOrDefault();
                if (campo != null)
                {
                    string valor = string.Empty;
                    if (campo.Entidad == "mbl")
                    {
                        valor = db.ObtenerValor(mbl.Id.ToString(), campo.Campo, campo.Entidad, campo.Tipo == "list", campo.Fuente).FirstOrDefault();
                    }
                    else if (campo.Entidad == "hbl")
                    {
                        valor = db.ObtenerValor(hbl.Id.ToString(), campo.Campo, campo.Entidad, campo.Tipo == "list", campo.Fuente).FirstOrDefault();
                    }
                    contenido = contenido.Replace(tags.Groups[0].Value, valor);
                }
            }

            List<Contacto> destinatarios = db.ContactoEnvio.Where(w => w.ContactoNav.Entidad == "customer" && w.ContactoNav.Fuente == hbl.Cliente && w.Fuente == hbl.Id)
                                                            .Select(s => s.ContactoNav).ToList();

            List<string> cuentas = destinatarios.Select(s => s.Correo).ToList();

            Email email = new Email();

            Historico historico = new Historico()
            {
                Id = Guid.NewGuid(),
                Comunicado = comunicado.Id,
                Contenido = contenido,
                Destinatario = string.Join(";", cuentas),
                Entidad = "hbl",
                Fuente = hbl.Id,
                S_Creacion = DateTime.Now,
                S_Usuario = Guid.Parse(User.Identity.GetUserId())
            };

            comunicado.ComunicadoAdjuntoNav.ToList().ForEach(i => {
                List<Adjunto> archivos = new List<Adjunto>();
                if (i.Entidad == "mbl")
                {
                    archivos = db.Adjunto.Where(w => w.Entidad == i.Entidad && w.Campo == i.Campo && w.Fuente == mbl.Id && w.Habilitado).ToList();
                }
                else if(i.Entidad == "hbl")
                {
                    archivos = db.Adjunto.Where(w => w.Entidad == i.Entidad && w.Campo == i.Campo && w.Fuente == hbl.Id && w.Habilitado).ToList();
                }

                foreach (var archivo in archivos)
                {
                    email.Attachments.Add(new Attachment(Server.MapPath("~/Uploads/" + archivo.Campo + "/" + archivo.Nombre)));
                    historico.HistoricoAdjuntoNav.Add(new HistoricoAdjunto() {
                        Id = Guid.NewGuid(),
                        Historico = historico.Id,
                        Adjunto = archivo.Id
                    });
                }
            });

            email.Send(cuentas, comunicado.Nombre, contenido);

            db.Historico.Add(historico);
            db.SaveChanges();
        }

        private void ProcessNoticeAwb(Comunicado comunicado, Hawb hawb, Mawb mawb, Guid module)
        {
            string pattern = "<i.*>(.*)<\\/i>";
            MatchCollection matches = Regex.Matches(comunicado.Contenido, pattern);
            string contenido = comunicado.Contenido;
            foreach (Match tags in matches)
            {
                string codigo = tags.Groups[1].Value.Substring(1);
                ComunicadoCampo campo = db.ComunicadoCampo.Where(w => w.Codigo == codigo).FirstOrDefault();
                if (campo != null)
                {
                    string valor = string.Empty;
                    if (campo.Entidad == "mawb")
                    {
                        valor = db.ObtenerValor(mawb.Id.ToString(), campo.Campo, campo.Entidad, campo.Tipo == "list", campo.Fuente).FirstOrDefault();
                    }
                    else if (campo.Entidad == "hawb")
                    {
                        valor = db.ObtenerValor(hawb.Id.ToString(), campo.Campo, campo.Entidad, campo.Tipo == "list", campo.Fuente).FirstOrDefault();
                    }
                    contenido = contenido.Replace(tags.Groups[0].Value, valor);
                }
            }

            List<Contacto> destinatarios = db.ContactoEnvio.Where(w => w.ContactoNav.Entidad == "customer" && w.ContactoNav.Fuente == hawb.Cliente && w.Fuente == hawb.Id)
                                                            .Select(s => s.ContactoNav).ToList();

            List<string> cuentas = destinatarios.Select(s => s.Correo).ToList();

            Email email = new Email();

            Historico historico = new Historico()
            {
                Id = Guid.NewGuid(),
                Comunicado = comunicado.Id,
                Contenido = contenido,
                Destinatario = string.Join(";", cuentas),
                Entidad = "hawb",
                Fuente = hawb.Id,
                S_Creacion = DateTime.Now,
                S_Usuario = Guid.Parse(User.Identity.GetUserId())
            };

            comunicado.ComunicadoAdjuntoNav.ToList().ForEach(i => {
                List<Adjunto> archivos = new List<Adjunto>();
                if (i.Entidad == "mawb")
                {
                    archivos = db.Adjunto.Where(w => w.Entidad == i.Entidad && w.Campo == i.Campo && w.Fuente == mawb.Id && w.Habilitado).ToList();
                }
                else if (i.Entidad == "hawb")
                {
                    archivos = db.Adjunto.Where(w => w.Entidad == i.Entidad && w.Campo == i.Campo && w.Fuente == hawb.Id && w.Habilitado).ToList();
                }

                foreach (var archivo in archivos)
                {
                    email.Attachments.Add(new Attachment(Server.MapPath("~/Uploads/" + archivo.Campo + "/" + archivo.Nombre)));
                    historico.HistoricoAdjuntoNav.Add(new HistoricoAdjunto()
                    {
                        Id = Guid.NewGuid(),
                        Historico = historico.Id,
                        Adjunto = archivo.Id
                    });
                }
            });

            email.Send(cuentas, comunicado.Nombre, contenido);

            db.Historico.Add(historico);
            db.SaveChanges();
        }

        // POST: Tools/SaveSelectedContacts/5
        [HttpPost]
        [Authorize]
        public ActionResult SaveSelectedContacts(string contacts, string entity, Guid source)
        {
            try
            {
                string[] selected_contacts = contacts.Split(',');
                db.ContactoEnvio.RemoveRange(db.ContactoEnvio.Where(w => w.Entidad == entity && w.Fuente == source));
                db.SaveChanges();

                foreach (var contact_id in selected_contacts)
                {
                    db.ContactoEnvio.Add(new ContactoEnvio()
                    {
                        Id = Guid.NewGuid(),
                        Contacto = Guid.Parse(contact_id),
                        Entidad = entity,
                        Fuente = source
                    });
                }

                db.SaveChanges();

                return Json(1);
            }
            catch (Exception ex)
            {
                return Json(0);
            }
        }

        // GET: Tools/GetCities/5
        [Authorize]
        public JsonResult GetCities(Guid country)
        {
            return Json(new SelectList(db.Ciudad.Where(w => w.Pais == country), "Id", "Nombre"), JsonRequestBehavior.AllowGet);
        }
    }
}