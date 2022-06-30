using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using MobileMail.Models;


[assembly: OwinStartupAttribute(typeof(MobileMail.Startup))]
namespace MobileMail
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            createRolesandUsers();
        }

        private void createRolesandUsers()
        {
            MobileMailContainer bd = new MobileMailContainer();
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));


            // In Startup iam creating first Admin Role and creating a default Admin User     
            if (!roleManager.RoleExists("Admin"))
            {

                // first we create Admin rool    
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);

                //Here we create a Admin super user who will maintain the website                   

                var user = new ApplicationUser();
                user.UserName = "rsolis@mobilemail.com";
                user.Email = "rsolis@mobilemail.com";

                string userPWD = "123456";

                var chkUser = UserManager.Create(user, userPWD);

                //Add default User to Role Admin    
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, "Admin");

                    User U = new User();
                    U.AccountName = "rsolis";
                    U.Name = "Roberto Solis";
                    U.AlternativeMail = "nicaragua_6002@yahoo.es";
                    bd.Users.Add(U);
                    bd.SaveChanges();

                }
            }
            // creating Creating Employee role     
            if (!roleManager.RoleExists("User"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "User";
                roleManager.Create(role);

                //Here we create a Admin super user who will maintain the website                   

                var user = new ApplicationUser();
                user.UserName = "jmorales@mobilemail.com";
                user.Email = "jmorales@mobilemail.com";

                string userPWD = "123456";

                var chkUser = UserManager.Create(user, userPWD);

                //Add default User to Role User    
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, "User");

                    User U = new User();
                    U.AccountName = "jmorales";
                    U.Name = "Jose Morales";
                    U.AlternativeMail = "jmorales@yahoo.es";
                    bd.Users.Add(U);
                    bd.SaveChanges();

                }
            }

           



        }
    }
}
