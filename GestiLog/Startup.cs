using GestiLog.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GestiLog.Startup))]
namespace GestiLog
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            PopulateInitialData();
        }

        private void PopulateInitialData()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));


            // In Startup iam creating first Admin Role and creating a default Admin User    
            if (!roleManager.RoleExists("Soporte"))
            {

                // first we create Admin rool   
                var role = new IdentityRole();
                role.Name = "Soporte";
                roleManager.Create(role);

                //Here we create a Admin super user who will maintain the website                  

                var user = new ApplicationUser();
                user.UserName = "Nedagarmo";
                user.Email = "nedagarmo@gmail.com";

                string clave = "Hi@123";

                var resultado = UserManager.Create(user, clave);

                //Add default User to Role Admin   
                if (resultado.Succeeded)
                {
                    UserManager.AddToRole(user.Id, "Soporte");
                }
            }

            // creating Creating Manager role    
            if (!roleManager.RoleExists("Administrador"))
            {
                var role = new IdentityRole();
                role.Name = "Administrador";
                roleManager.Create(role);
            }

            // creating Creating Employee role    
            if (!roleManager.RoleExists("Operador"))
            {
                var role = new IdentityRole();
                role.Name = "Operador";
                roleManager.Create(role);
            }
        }
    }
}
