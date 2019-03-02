using StandardEng.Data.DB;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace StandardEng.Web.Common
{
    public static class HtmlHelperExtensions
    {
        public static string SetStatusClientTemplate(this HtmlHelper helper, string isActive, string controllerName, string action, string parameter, string id, string gridId, string entityName)
        {

            string deactivteMessage = "Are you sure you want to deactivate this " + entityName;


            string activteMessage = "Are you sure you want to activate this " + entityName;

            var deactiveAttributes = " onclick='changeStatus(" + @"""" + controllerName + @"""" + ", " + @"""" +
                                    action + @"""" + ", " + @"""""" + ", "
                                    + @"""" + parameter + @"""" + ", " + @"""" + deactivteMessage + @"""" + ", " + id
                                    + ", " + @"""" + gridId + @"""" + @")'";

            var activeAttributes = " onclick='changeStatus(" + @"""" + controllerName + @"""" + ", " + @"""" +
                                   action + @"""" + ", " + @"""""" + ", "
                                   + @"""" + parameter + @"""" + ", " + @"""" + activteMessage + @"""" + ", " + id
                                   + ", " + @"""" + gridId + @"""" + @")'";


            return "# if (" + isActive + ")    {#" +
                     @"<a class='k-button' " + deactiveAttributes + @"><span class='k-icon k-i-check'></span></a>" +
                     "#}else { #" +
                     @"<a class='k-button' " + activeAttributes + @"><span class='k-icon k-i-close'></span></a>"
                     + "#}#";
        }

        public static MvcHtmlString GenerateMenu(this HtmlHelper helper)
        {
            List<Get_UserAccessPermissions_Result> parentMenuList = SessionHelper.UserAccessPermissions.Where(x => x.ParentMenuId == null).OrderBy(item => item.DisplayOrder).ToList();

            List<Get_UserAccessPermissions_Result> childMenuList = SessionHelper.UserAccessPermissions.Where(x => x.ParentMenuId != null).OrderBy(item => item.DisplayOrder).ToList();

            if (parentMenuList.Any())
            {
                TagBuilder ul = new TagBuilder("ul");
                ul.MergeAttribute("class", "nav nav-sidebar");
                ul.MergeAttribute("data-nav-type", "accordion");

                StringBuilder sb = new StringBuilder();

                TagBuilder maintagfirstspan = itag("icon-menu");

                TagBuilder divtag = new TagBuilder("div");
                divtag.MergeAttribute("class", "text-uppercase font-size-xs line-height-xs");
                divtag.InnerHtml = "Main";

                TagBuilder liWithformaintag = new TagBuilder("li");
                liWithformaintag.InnerHtml = string.Format(CultureInfo.InvariantCulture, "{0}{1}", Convert.ToString(divtag), Convert.ToString(maintagfirstspan));
                liWithformaintag.AddCssClass("nav-item-header");

                sb.Append(Convert.ToString(liWithformaintag));

                foreach (Get_UserAccessPermissions_Result menu in parentMenuList)
                {
                    List<Get_UserAccessPermissions_Result> childList = childMenuList.Where(x => x.ParentMenuId == menu.MenuId).ToList();


                    if (childList.Any())
                    {
                        StringBuilder sbChild = new StringBuilder();

                        TagBuilder liWithChild = new TagBuilder("li");
                        liWithChild.AddCssClass("nav-item nav-item-submenu");

                        TagBuilder firstSpan = itag(menu.ImagePath);

                        TagBuilder secondSpan = SpanTag("");
                        secondSpan.InnerHtml = Convert.ToString(menu.MenuName);

                        TagBuilder aLink = AnchorLink(menu.Action, menu.Controller, false);
                        aLink.InnerHtml = string.Format(CultureInfo.InvariantCulture, "{0}{1}", Convert.ToString(firstSpan), Convert.ToString(secondSpan));

                        TagBuilder ulChild = new TagBuilder("ul");
                        ulChild.AddCssClass("nav nav-group-sub");
                        ulChild.MergeAttribute("data-submenu-title", Convert.ToString(menu.MenuName));

                        foreach (Get_UserAccessPermissions_Result childMenu in childList)
                        {

                            TagBuilder firstSpanchild = itag(childMenu.ImagePath);

                            TagBuilder secondSpanchild = SpanTag("");
                            secondSpanchild.InnerHtml = Convert.ToString(childMenu.MenuName);

                            TagBuilder aLinkchild = AnchorLink(childMenu.Action, childMenu.Controller, false);
                            aLinkchild.InnerHtml = string.Format(CultureInfo.InvariantCulture, "{0}{1}", Convert.ToString(firstSpanchild), Convert.ToString(secondSpanchild));

                            TagBuilder liWithforchild = new TagBuilder("li");
                            liWithforchild.InnerHtml = Convert.ToString(aLinkchild);
                            liWithforchild.AddCssClass("nav-item");

                            //TagBuilder liChildaLink = AnchorLink(childMenu.Action, childMenu.Controller, false);

                            //TagBuilder ChildSpan = SpanTag("sidenav-icon " + childMenu.ImagePath);

                            //liChildaLink.InnerHtml = Convert.ToString(ChildSpan);
                            //liChildaLink.InnerHtml += Convert.ToString(childMenu.MenuName);

                            //TagBuilder liChild = new TagBuilder("li");

                            //liChild.InnerHtml = Convert.ToString(liChildaLink);

                            sbChild.Append(Convert.ToString(liWithforchild));

                        }

                        ulChild.InnerHtml = string.Format(CultureInfo.InvariantCulture, "{0}", Convert.ToString(sbChild));
                        liWithChild.InnerHtml = string.Format(CultureInfo.InvariantCulture, "{0}{1}", Convert.ToString(aLink), Convert.ToString(ulChild));
                        sb.Append(Convert.ToString(liWithChild));
                    }
                    else
                    {
                        TagBuilder firstSpan = itag(menu.ImagePath);

                        TagBuilder secondSpan = SpanTag("");
                        secondSpan.InnerHtml = Convert.ToString(menu.MenuName);

                        TagBuilder aLink = AnchorLink(menu.Action, menu.Controller, false);
                        aLink.InnerHtml = string.Format(CultureInfo.InvariantCulture, "{0}{1}", Convert.ToString(firstSpan), Convert.ToString(secondSpan));

                        TagBuilder liWithChild = new TagBuilder("li");
                        liWithChild.InnerHtml = Convert.ToString(aLink);
                        liWithChild.AddCssClass("nav-item");
                        sb.Append(Convert.ToString(liWithChild));
                    }
                }

                ul.InnerHtml = sb.ToString();
                return MvcHtmlString.Create(ul.ToString());
            }

            return MvcHtmlString.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="controllerName"></param>
        /// <param name="isParent"></param>
        /// <returns></returns>
        private static TagBuilder AnchorLink(string actionName, string controllerName, bool isParent)
        {
            TagBuilder aLink = new TagBuilder("a");
            aLink.MergeAttribute("class", "nav-link");

            //if (isParent)
            //{
            //    aLink.MergeAttribute("class", "dropdown-toggle");
            //    aLink.MergeAttribute("data-action", "click-trigger");
            //}

            if (string.IsNullOrEmpty(actionName) || string.IsNullOrEmpty(controllerName))
            {
                aLink.MergeAttribute("href", "javascript:void(0);");
            }
            else
            {
                aLink.MergeAttribute("id", controllerName);
                aLink.MergeAttribute("href", WebHelper.SiteRootPathUrl + "/" + controllerName + "/" + actionName);
            }
            return aLink;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        private static TagBuilder SpanTag(string className)
        {
            TagBuilder span = new TagBuilder("span");
            span.MergeAttribute("class", className);
            return span;
        }

        private static TagBuilder itag(string className)
        {
            TagBuilder itag = new TagBuilder("i");
            itag.MergeAttribute("class", className);
            return itag;
        }
    }
}