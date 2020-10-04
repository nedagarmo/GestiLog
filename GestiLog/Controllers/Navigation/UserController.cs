using GestiLog.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace GestiLog.Controllers.Navigation
{
    [Authorize]
    public class UserController : Controller
    {
        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: User
        public ActionResult Index()
        {
            return View(UserManager.Users.ToList());
        }

        public ActionResult Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ApplicationUser user = UserManager.Users.Where(w => w.Id == id).FirstOrDefault();

            if (user == null)
            {
                return HttpNotFound();
            }

            EditorUserViewModel userView = new EditorUserViewModel()
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email
            };

            ApplicationDbContext context = new ApplicationDbContext();
            ViewBag.Role = new SelectList(context.Roles.ToList(), "Name", "Name", UserManager.GetRoles(user.Id).FirstOrDefault());
            return View(userView);
        }

        //
        // POST: /User/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditorUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(model.Password))
                {
                    string code = await UserManager.GeneratePasswordResetTokenAsync(model.Id);
                    await UserManager.ResetPasswordAsync(model.Id, code, model.Password);
                }

                await UserManager.RemoveFromRolesAsync(model.Id);
                await UserManager.AddToRoleAsync(model.Id, model.Role);

                return RedirectToAction("Index");
            }

            ApplicationDbContext context = new ApplicationDbContext();
            ViewBag.Role = new SelectList(context.Roles.ToList(), "Name", "Name", model.Role);

            // Si llegamos a este punto, es que se ha producido un error y volvemos a mostrar el formulario
            return View(model);
        }
    }
}