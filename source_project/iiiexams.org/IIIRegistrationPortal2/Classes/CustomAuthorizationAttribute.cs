using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;

namespace IIIRegistrationPortal2
{
    /// <summary>
    /// checks if the user has permission to menu and if the session still exists
    /// </summary>
    public class AuthorizeExtAttribute : AuthorizeAttribute
    {
        Boolean HasSessionExpired = false;
        String action = String.Empty;
        String controller = String.Empty;
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            Boolean b = false;
            if (PortalSession.RoleCode != String.Empty)
            {
                b = base.AuthorizeCore(httpContext);
                if (b)
                {
                    b = PortalSession.HasAccess(controller, action);
                }
                else
                {//Nothing

                }
            }
            else
            {
                b = false;
                HasSessionExpired = true;
                httpContext.Session.Clear();
                httpContext.Session.Abandon();
            }
            return b;

            //Boolean b = base.AuthorizeCore(httpContext);
            //if (b)
            //{
            //    if (PortalSession.RoleCode != String.Empty)
            //    {
            //        b = PortalSession.HasAccess(controller, action);
            //    }
            //    else
            //    {
            //        b = false;
            //        HasSessionExpired = true;
            //        httpContext.Session.Clear();
            //        httpContext.Session.Abandon();
            //    }
            //}
            //else
            //{//Nothing

            //}
            //return b;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            action = filterContext.Controller.ControllerContext.RouteData.Values["action"].ToString();
            controller = filterContext.Controller.ControllerContext.RouteData.Values["controller"].ToString();
            base.OnAuthorization(filterContext);
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (HasSessionExpired)
            {
                filterContext.Result = new RedirectResult("../Home/Relogin");
            }
            else
            {
                filterContext.Result = new RedirectResult("../Home/UnauthorizedAccess");
            }
        }
    }

    /// <summary>
    /// checks if the session still exists... to be used for ajax calls
    /// </summary>
    public class AuthorizeAJAXAttribute: AuthorizeAttribute
    {
        Boolean HasSessionExpired = false;
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            //return base.AuthorizeCore(httpContext);
            Boolean b = false;
            //Errorlogger.LogInfo("AuthorizeAJAXAttribute", "AuthorizeCore", "Inside AuthorizeCore");
            if (!String.IsNullOrEmpty(PortalSession.RoleCode))
            {
                b = base.AuthorizeCore(httpContext);
            }
            else
            {
                b = false;
                HasSessionExpired = true;
                httpContext.Session.Clear();
                httpContext.Session.Abandon();
            }
            return b;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            //Errorlogger.LogInfo("CustomAuthorizationAttributeClass", "OnAuthorization", "Inside OnAuthorization");
            base.OnAuthorization(filterContext);
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            //Errorlogger.LogInfo("CustomAuthorizationAttributeClass", "HandleUnauthorizedRequest", "Inside HandleUnauthorizedRequest");
            var httpContext = filterContext.HttpContext;
            var request = httpContext.Request;
            var response = httpContext.Response;
            if (request.IsAjaxRequest())
            {
                if (HasSessionExpired)
                {
                    response.StatusCode = (int)HttpStatusCode.Unauthorized;
                }
                response.SuppressFormsAuthenticationRedirect = true;
                response.End();
            }

            base.HandleUnauthorizedRequest(filterContext);
        }
    }
}