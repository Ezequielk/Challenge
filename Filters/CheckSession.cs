using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Challenge.Controllers;
using Challenge.Models;

namespace Challenge.Filters
{
    public class CheckSession : ActionFilterAttribute
    {
        private object user;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Llamo al método base
            base.OnActionExecuting(filterContext);

            // Obtengo la session "User"
            user = HttpContext.Current.Session["User"];

            // Si no hay usuario logueado se redirige al login, salvo que ya se esté allí
            if (user == null)
            {
                if (!(filterContext.Controller is AccessController))
                {
                    filterContext.HttpContext.Response.Redirect("~/Access/Login");
                }
            }
        }
    }
}