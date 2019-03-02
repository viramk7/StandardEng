using StandardEng.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StandardEng.Web.Common
{
    public static class WebHelper
    {
        #region Contants

        public static readonly string ReportApiController = WebHelper.SiteRootPathUrl + "/api/reportsapi/";
        public static readonly string ReportTemplate = WebHelper.SiteRootPathUrl + "/Template/telerikReportViewerTemplate.html";

        public const string RegexEmail = @"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}";

        public const string DateFormate = "dd/MM/yyyy";

        public const string TimeFormate = "HH:mm";

        public const string CommonErrorMsg = "Something Went Wrong. Please Try Again Later.";

        public const int PageSize = 25;

        public static int[] PageSizes = { 25, 50, 100, 200, 500 };

        public const string PleaseSelect = "--Select--";

        public static readonly Dictionary<string, object> ActionCenterColumnStyleWithCanEdit = new Dictionary<string, object> { { "align", "center" }, { "style", "text-align:center;vertical-align:middle !important;" }, { "width", "80px" } };

        public static readonly Dictionary<string, object> ActionCenterColumnStyleWithCanStatus = new Dictionary<string, object> { { "align", "center" }, { "style", "text-align:center;vertical-align:middle !important;" }, { "width", "120px" } };

        public static readonly Dictionary<string, object> StatusColumnStyle = new Dictionary<string, object> { { "align", "center" }, { "style", "text-align:center;vertical-align:middle !important;" }, { "width", "60px" } };

        public static readonly Dictionary<string, object> SmallColumnStyle = new Dictionary<string, object> { { "align", "center" }, { "style", "text-align:center;vertical-align:middle !important;" }, { "width", "30px" } };
        #endregion

        #region Methods


        public static string SiteRootPathUrl
        {
            get
            {
                string msRootUrl;
                if (HttpContext.Current.Request.ApplicationPath != null && string.IsNullOrEmpty(HttpContext.Current.Request.ApplicationPath.Split('/')[1]))
                {
                    msRootUrl = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Host +
                                ":" +
                                HttpContext.Current.Request.Url.Port;
                }
                else
                {
                    msRootUrl = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Host + HttpContext.Current.Request.ApplicationPath;
                }

                return msRootUrl;
            }
        }

        public static string ShowAlertMessage(string message)
        {
            message = message.Replace("'", " ");
            var strString = @"<script type='text/javascript' language='javascript'>$(function() { showMessageOnly(' " + message + "' , 'popup-error'); })</script>";
            return strString;
        }

        public static string ShowAlertMessage(Exception exception)
        {
            string message = CommonHelper.GetErrorMessage(exception);
            message = message.Replace("'", " ");
            var strString = @"<script type='text/javascript' language='javascript'>$(function() { showMessageOnly(' " + message + "') , 'popup-error'; })</script>";
            return strString;
        }

        public static List<int> ConvertStringToIntList(string jointid)
        {

            if (string.IsNullOrEmpty(jointid))
            {
                return null;
            }

            return jointid.Split(',').Select(int.Parse).ToList();
        }

        /// <summary>
        /// IsUpper
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsUpper(this string value)
        {
            // Consider string to be uppercase if it has no lowercase letters.
            return value.All(t => !Char.IsLower(t));
        }



        #endregion
    }
    public class AuthorizationHelper
    {
        public static bool CanAdd(string controllerName)
        {
            return SessionHelper.UserAccessPermissions.Any(item => item.IsInsert == true && item.Controller != null && item.Controller.ToLower() == controllerName.ToLower());
        }

        public static bool CanEdit(string controllerName)
        {
            return SessionHelper.UserAccessPermissions.Any(item => item.IsEdit == true && item.Controller != null && item.Controller.ToLower() == controllerName.ToLower());
        }

        public static bool CanDelete(string controllerName)
        {
            return SessionHelper.UserAccessPermissions.Any(item => item.IsDelete == true && item.Controller != null && item.Controller.ToLower() == controllerName.ToLower());
        }

        public static bool CanChangeStatus(string controllerName)
        {
            return SessionHelper.UserAccessPermissions.Any(item => item.IsChangeStatus == true && item.Controller != null && item.Controller.ToLower() == controllerName.ToLower());
        }

        public static bool CanEditDelete(string controllerName)
        {
            return SessionHelper.UserAccessPermissions.Any(item => (item.IsDelete == true || item.IsEdit == true)

                                                                   && item.Controller != null && item.Controller.ToLower() == controllerName.ToLower());
        }

    }
}