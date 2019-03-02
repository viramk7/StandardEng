using StandardEng.Common;
using StandardEng.Data.CustomModel;
using StandardEng.Data.DB;
using StandardEng.Data.Repository;
using StandardEng.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace StandardEng.Web.Controllers
{
    public class LoginController : Controller
    {
        #region private variables

        private readonly GenericRepository<tblUser> _dbRepository;

        private IFormsAuthenticationService FormsService { get; set; }

        #endregion

        #region Constructor
        public LoginController()
        {
            _dbRepository = new GenericRepository<tblUser>();
        }
        #endregion

        #region Public Methods
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        protected override void Initialize(RequestContext requestContext)
        {
            if (FormsService == null)
            {
                FormsService = new FormsAuthenticationService();
            }

            base.Initialize(requestContext);
        }

        public ActionResult LoginUser(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                tblUser logedInUser = _dbRepository.GetEntities().FirstOrDefault(m => m.Username == model.UserName && m.Password == model.Password);

                if (logedInUser == null)
                {
                    TempData[Enums.NotifyType.Error.GetDescription()] = "Invalid Login Credentials.";
                    return View("Index", model);
                }
                else
                {
                        SessionHelper.UserId = Convert.ToInt32(logedInUser.UserId);
                        SessionHelper.WelcomeUser = logedInUser.Name;
                        SessionHelper.RoleId = logedInUser.RoleId == null ? 0 : (int)logedInUser.RoleId;
                        SessionHelper.Email = logedInUser.UserEmail;
                        SessionHelper.UserAccessPermissions = CustomRepository.UserAccessPermissions(SessionHelper.RoleId, logedInUser.IsSuperAdmin);
                        CommonHelper.UserId = Convert.ToInt32(logedInUser.UserId);
                        FormsService.SignIn(logedInUser.Name, model.RememberMe);

                        TempData[Enums.NotifyType.Success.GetDescription()] = "Login Successfully.";
                        return RedirectToAction("Index", "Home");
                }

            }

            return View("Index", model);
        }

        public ActionResult Logout()
        {
            FormsService.SignOut();
            Session.Abandon();
            return RedirectToAction("Index", "Login");
        }

        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(string Email)
        {
            try
            {
                tblUser model = _dbRepository.GetEntities().FirstOrDefault(m => m.UserEmail == Email);
                string token = Guid.NewGuid().ToString();
                if (model != null)
                {
                    string link = Url.Action("ResetPassword", "Login", new RouteValueDictionary(new { Token = token }), System.Web.HttpContext.Current.Request.Url.Scheme);

                    string resetLink = "<a href='" + link + "'>" + link + "</a>";
                    string toEmail = model.UserEmail;


                    string bodyTemplate = System.IO.File.ReadAllText(Server.MapPath("~/Template/ForgotPassword.html"));


                    bodyTemplate = bodyTemplate.Replace("[@FirstName]", model.Name);
                    bodyTemplate = bodyTemplate.Replace("[@link]", resetLink);

                    Task task = new Task(() => EmailHelper.SendAsyncEmail(toEmail, "Forgot Password", bodyTemplate, true));
                    task.Start();


                    model.Token = token;
                    model.TokenExpiryDateTime = DateTime.Now.AddDays(1);

                    string result = _dbRepository.Update(model);

                    TempData[Enums.NotifyType.Success.GetDescription()] = "Email has been sent. Please check your email.";
                    return View("Index");
                }

                TempData[Enums.NotifyType.Error.GetDescription()] = "Invalid Email Address.";
                return View();
            }
            catch (Exception ex)
            {
                return Json(CommonHelper.GetErrorMessage(ex));
            }
        }

        [HttpGet]
        public ActionResult ResetPassword(string Token)
        {
            tblUser userModel = _dbRepository.GetEntities().FirstOrDefault(m => m.Token == Token);

            if (userModel.UserId > 0 && userModel.TokenExpiryDateTime >= DateTime.Now)
            {
                ResetPasswordModel model = new ResetPasswordModel { UserId = userModel.UserId };
                return View(model);
            }

            if (userModel.TokenExpiryDateTime < DateTime.Now)
            {
                TempData[Enums.NotifyType.Error.GetDescription()] = "Token for reset password has been expired.please try again to reset password.";
                return View("Index");
            }

            TempData[Enums.NotifyType.Error.GetDescription()] = "Something Went wrong. Please try again later.";
            return View("Index");
        }

        [HttpPost]
        public ActionResult ResetPassword(ResetPasswordModel userModel)
        {

            if (userModel.ConfirmPassword != userModel.Password)
            {
                TempData[Enums.NotifyType.Error.GetDescription()] = "Password and Confirm Password must be same.";
                return View("ResetPassword",userModel);
            }

            tblUser model = _dbRepository.GetEntities().FirstOrDefault(m => m.UserId == userModel.UserId);

            if (model != null)
            {
                model.Token = null;
                model.TokenExpiryDateTime = null;
                model.Password = userModel.Password;
                _dbRepository.Update(model);
            }
            if (model.UserId > 0)
            {
                TempData[Enums.NotifyType.Success.GetDescription()] = "Password reset successful.";
                return View("Index", new LoginModel());
            }
            TempData[Enums.NotifyType.Error.GetDescription()] = "Something went wrong. Please try again later.";
            return View("ResetPassword");
        }

        public ActionResult Error()
        {
            return View();
        }

        #endregion
    }

    #region Form Authentication
    public class FormsAuthenticationService : IFormsAuthenticationService
    {
        public void SignIn(string userName, bool createPersistentCookie)
        {
            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentException(string.Empty, "userName");
            }

            FormsAuthentication.SetAuthCookie(userName, createPersistentCookie);
        }

        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }
    }

    public interface IFormsAuthenticationService
    {
        void SignIn(string userName, bool createPersistentCookie);

        void SignOut();
    }
    #endregion
}