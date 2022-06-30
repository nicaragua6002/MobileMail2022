using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using MobileMail.Models;
using Microsoft.AspNet.Identity.Owin;

namespace MobileMail
{
    /// <summary>
    /// Summary description for MobileMailWS
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class MobileMailWS : System.Web.Services.WebService
    {
        MobileMailContainer bd = new MobileMailContainer();
        ApplicationDbContext context = new ApplicationDbContext();
      
        [WebMethod]
        public ResultRegister Register(string Name, string AccountName, Nullable<byte> ProfilePhoto, string AlternativeMail,string Pass)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            ResultRegister Value = new ResultRegister();



            User Existe = bd.Users.Where(x => x.AccountName == AccountName).FirstOrDefault();

            if(Existe==null)
            {
              var user = new ApplicationUser();
            user.UserName = AccountName+"@mobilemail.com";
            user.Email = AccountName+"@mobilemail.com";

            string userPWD = Pass;

            var chkUser = UserManager.Create(user, userPWD);

            //Add default User to Role User    
            if (chkUser.Succeeded)
            {
                var result1 = UserManager.AddToRole(user.Id, "User");

                User U = new User();
                U.AccountName = AccountName;
                U.Name = Name;
                U.AlternativeMail = AlternativeMail;
                U.ProfilePhoto = ProfilePhoto;
                bd.Users.Add(U);
                bd.SaveChanges();
                Value.Band = true;
                Value.Mensaje = "Se creo con exito";

              }
            else
                {
                    Value.Band = false;
                    Value.Mensaje = "Ocurrio un error no se pudo crear";
                }
            }
            else
            {
                Value.Band = false;
                Value.Mensaje = "Ya existe un usuario con esa cuenta";
            }
           
            return Value;
        }

        [WebMethod]
        public bool Login(string AccountName, string Pass)
        {
            return Validar(AccountName, Pass);
        }

        bool Validar(string AccountName, string Pass)
        {

            // No cuenta los errores de inicio de sesión para el bloqueo de la cuenta
            // Para permitir que los errores de contraseña desencadenen el bloqueo de la cuenta, cambie a shouldLockout: true
            // var  result = SignInManager.PasswordSignInAsync(user, pass, false, shouldLockout: false);
            var result = HttpContext.Current.GetOwinContext().Get<ApplicationSignInManager>().PasswordSignIn(AccountName, Pass, false, false);


            if (result == SignInStatus.Success)
            {
                return true;
            }
            else
                return false;
        }


        //Bandeja de entrada
        [WebMethod]
        public List<MailSW> InBox(string AccountName, string Pass)
        {
            if (Validar(AccountName, Pass))
                return bd.Mails.Where(x => x.To == AccountName).Select(x => new MailSW()
                {
                    Id = x.Id,
                    From = x.From,
                    To = x.To,
                    Content = x.Content,
                    Date = x.Date,
                    Status = x.Status
                }).ToList();
            else
                return null;
        }

        //Bandeja de Salida
        [WebMethod]
        public List<MailSW> OutBox(string AccountName, string Pass)
        {
            if (Validar(AccountName, Pass))
                return bd.Mails.Where(x => x.From == AccountName).Select(x => new MailSW()
            {
                Id = x.Id,
                From = x.From,
                To = x.To,
                Content = x.Content,
                Date = x.Date,
                Status = x.Status
            }).ToList();
            else
                return null;
        }

        //Agregar un email
        [WebMethod]
        public int AddMail(string AccountName, string Pass, MailSW x)
        {
            if (Validar(AccountName, Pass))
            {
                bd.Mails.Add(new Mail()
                {
                    Id = x.Id,
                    From = x.From,
                    To = x.To,
                    Content = x.Content,
                    Date = x.Date,
                    Status = x.Status
                });
                return bd.SaveChanges();
            }  
            else
                return 0;
        }
        public class ResultRegister
        {
            public bool Band { get; set; }
            public string Mensaje { get; set; }
        }

        public  class MailSW
        {
        
            public int Id { get; set; }
            public string From { get; set; }
            public string To { get; set; }
            public string Content { get; set; }
            public string Date { get; set; }
            public string Status { get; set; }

          
        }
    }
}
