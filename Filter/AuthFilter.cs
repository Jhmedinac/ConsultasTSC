using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsultasTSC.Filter
{
    public class AuthFilter : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            string XHRUserName = "Username";
            string XHRPassword = "Password";


            if (!context.HttpContext.Request.Headers.TryGetValue(XHRUserName, out var UserName)
                ||
                !context.HttpContext.Request.Headers.TryGetValue(XHRPassword, out var Password)
                )
            {
                context.Result = new StatusCodeResult(401);
                return;
            }

            XHRUserName = SystemParameters.AppUserName;
            XHRPassword = SystemParameters.AppPassword;



            if (!(XHRUserName == UserName.ToString() && XHRPassword == Password.ToString()))
            {
                context.Result = new StatusCodeResult(401);
                return;
            }
        }
    }
}

