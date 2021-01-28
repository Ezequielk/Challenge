using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Challenge.Controllers
{
    public class AccessController : Controller
    {
        public ActionResult Logout()
        {
            Session["User"] = null;
            return RedirectToAction("Login");
        }
        public ActionResult Login()
        {
            if(Session["User"] != null)
            {
                // Si hay sesion, redirigir al home
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Login(string DNI, string file_number, string username, string password)
        {
            try
            {
                using (Models.DBContainer db = new Models.DBContainer())
                {
                    object dbUser;
                    
                    if (username != "")
                    {
                        // Uso linq para obtener el admin
                        dbUser = (from user in db.Admins
                                  where user.username == username && user.password == password
                                  select user).FirstOrDefault();
                    }
                    else
                    {
                        // Uso linq para obtener el student
                        dbUser = (from user in db.Students
                                  where user.DNI == DNI && user.file_number == file_number
                                  select user).FirstOrDefault();
                    }
                    
                    if (dbUser == null)
                    {
                        ViewData["Error"] = "Incorrect data";
                        return View();
                    }

                    // El usuario existe, lo guardo en Session
                    Session["User"] = dbUser;
                }

                return RedirectToAction("Index", "Home");
            }
            catch (Exception exc)
            {
                // Si hubo un error, capturo el mensaje del mismo para mostrarlo en la vista
                ViewData["Error"] = exc.Message;
                return View();
            }
        }
    }
}