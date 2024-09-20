using RDChartSite.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RDChartSite.Filters
{
    public class AuthAdmin : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (HttpContext.Current.Session["Login"] != null)
            {
                Users user = HttpContext.Current.Session["Login"] as Users;
                if (user.IsAdmin != true)
                {
                    filterContext.Result = new RedirectResult("/Home/AccesDenied");
                }
            }
        }
    }
}