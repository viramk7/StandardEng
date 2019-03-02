using StandardEng.Data.DB;
using StandardEng.Data.Repository;
using StandardEng.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace StandardEng.Web.Controllers
{
    public class BaseController : Controller
    {
        // GET: Base
        protected override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            filterContext.HttpContext.Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
            filterContext.HttpContext.Response.Cache.SetValidUntilExpires(false);
            filterContext.HttpContext.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            filterContext.HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            filterContext.HttpContext.Response.Cache.SetNoStore();
            base.OnResultExecuting(filterContext);
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            if (Request.IsAuthenticated)
            {

                string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName.ToLower();
                if (!controllerName.Contains("error") && !controllerName.Contains("nopagefound"))
                {
                    #region Handle Session Time Out
                    if (SessionHelper.UserId == 0)
                    {
                        SetSessionValues(filterContext);
                    }
                    #endregion
                }
                else
                {
                    RedirectToLoginPage(filterContext);
                }
            }
            else
            {
                RedirectToLoginPage(filterContext);
            }
        }

        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
            var cookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (cookie != null)
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
                FormsAuthenticationTicket newTicket = FormsAuthentication.RenewTicketIfOld(ticket);
                if (newTicket != null && newTicket.Expiration != ticket.Expiration)
                {
                    string encryptedTicket = FormsAuthentication.Encrypt(newTicket);

                    cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket)
                    {
                        Path = FormsAuthentication.FormsCookiePath
                    };
                    Response.Cookies.Add(cookie);
                }
            }
        }


        protected override void OnException(ExceptionContext filterContext)
        {
            var url = new UrlHelper(filterContext.RequestContext);
            var loginUrl = url.Action("Error", "Login");
            if (loginUrl != null) filterContext.HttpContext.Response.Redirect(loginUrl, true);
        }

        private void RedirectToLoginPage(ActionExecutingContext filterContext)
        {
            var url = new UrlHelper(filterContext.RequestContext);
            var loginUrl = url.Action("Index", "Login");
            if (loginUrl != null) filterContext.HttpContext.Response.Redirect(loginUrl, true);
        }



        /// <summary>
        /// Set Session Values
        /// </summary>
        /// <param name="filterContext"></param>
        private void SetSessionValues(ActionExecutingContext filterContext)
        {
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie == null)
            {
                RedirectToLoginPage(filterContext);
            }
            else
            {
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                if (authTicket == null || authTicket.Expired)
                {
                    RedirectToLoginPage(filterContext);
                }
                else
                {

                    string userName = authTicket.Name;

                    GenericRepository<tblUser> repository = new GenericRepository<tblUser>();

                    tblUser logedInUser = repository.GetEntities().FirstOrDefault(u => u.Username == userName);

                    if (logedInUser == null)
                    {
                        RedirectToLoginPage(filterContext);
                    }
                    else if (logedInUser.IsActive == false)
                    {
                        RedirectToLoginPage(filterContext);
                    }
                    else
                    {
                        SessionHelper.UserId = Convert.ToInt32(logedInUser.UserId);
                        SessionHelper.WelcomeUser = logedInUser.Name;
                        SessionHelper.RoleId = logedInUser.RoleId == null ? 0 : (int)logedInUser.RoleId;
                        SessionHelper.Email = logedInUser.UserEmail;
                        SessionHelper.UserAccessPermissions = CustomRepository.UserAccessPermissions(SessionHelper.RoleId, logedInUser.IsSuperAdmin);
                    }
                }
            }
        }
    }
}