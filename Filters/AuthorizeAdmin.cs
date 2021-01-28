using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Challenge.Models;

namespace Challenge.Filters
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class AuthorizeAdmin : AuthorizeAttribute
    {

        public AuthorizeAdmin() { }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            object user = HttpContext.Current.Session["User"];
            if (!(user is Admin))
            {
                // Si la session user no matchea con un admin, mandar al home
                filterContext.Result = new RedirectResult("~/Home/Index");
            }
            // Si matchea, no hacer nada
        }
    }
}