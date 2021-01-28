using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Challenge.Models;

namespace Challenge.Filters
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class AuthorizeStudent : AuthorizeAttribute
    {
        public AuthorizeStudent() { }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            object user = HttpContext.Current.Session["User"];
            if (!(user is Student))
            {
                // Si la session user no matchea con un estudiante, mandar al home
                filterContext.Result = new RedirectResult("~/Home/Index");
            }
            // Si matchea, no hacer nada
        }
    }
}