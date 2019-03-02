using StandardEng.Data.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StandardEng.Web.Common
{
    public class SessionHelper
    {
        #region Public Variables
        public static int UserId
        {
            get
            {
                return HttpContext.Current.Session["UserId"] == null ? 0 : (int)HttpContext.Current.Session["UserId"];
            }
            set { HttpContext.Current.Session["UserId"] = value; }
        }

        public static int RoleId
        {
            get
            {
                return HttpContext.Current.Session["RoleId"] == null ? 0 : (int)HttpContext.Current.Session["RoleId"];
            }
            set { HttpContext.Current.Session["RoleId"] = value; }
        }

        public static string WelcomeUser
        {
            get
            {
                return HttpContext.Current.Session["WelcomeUser"] == null
                    ? "Guest"
                    : (string)HttpContext.Current.Session["WelcomeUser"];
            }
            set { HttpContext.Current.Session["WelcomeUser"] = value; }
        }

        public static string Email
        {
            get { return Convert.ToString(HttpContext.Current.Session["Email"]); }

            set { HttpContext.Current.Session["Email"] = value; }
        }


        public static bool IsSuperAdmin
        {
            get
            {
                return HttpContext.Current.Session["IsSuperAdmin"] != null &&
                       (bool)HttpContext.Current.Session["IsSuperAdmin"];
            }
            set { HttpContext.Current.Session["IsSuperAdmin"] = value; }
        }

        public static List<Get_UserAccessPermissions_Result> UserAccessPermissions
        {
            get
            {
                return HttpContext.Current.Session["UserAccessPermissions"] == null
                    ? new List<Get_UserAccessPermissions_Result>()
                    : HttpContext.Current.Session["UserAccessPermissions"] as
                        List<Get_UserAccessPermissions_Result>;
            }

            set { HttpContext.Current.Session["UserAccessPermissions"] = value; }
        }


        #endregion
    }
}