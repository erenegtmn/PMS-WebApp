using RDChartSite.Common;
using RDChartSite.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RDChartSite.Inıt
{
    public class WebCommon : ICommon
    {
        public string GetCurrentEmail()
        {
            if (HttpContext.Current.Session["Login"] != null)
            {
                Users user = HttpContext.Current.Session["Login"] as Users;
                return user.Email;
            }
            return null;
        }

        public string GetCurrentPassword()
        {
            if (HttpContext.Current.Session["Login"] != null)
            {
                Users user = HttpContext.Current.Session["Login"] as Users;
                return user.Password;
            }
            return null;
        }

        public string GetCurrentUserName()
        {
           if(HttpContext.Current.Session["Login"] != null) 
            {
                Users user = HttpContext.Current.Session["Login"] as Users;
                return user.Name;
            }
            return "system";
        }
    }
}