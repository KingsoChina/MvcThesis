using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;
using System.Web.Security;
using System.Text;
using System.Web.Routing;

namespace MvcThesis
{
    public static class CheckBoxListExtensions
    {
        public static string CheckBoxList(this HtmlHelper htmlHelper, string name, List<SelectListItem> list)
        {
            return htmlHelper.CheckBoxList(name, list,
                ((IDictionary<string, object>)null));
        }

        public static string CheckBoxList(this HtmlHelper htmlHelper, string name, List<SelectListItem> list,
            object htmlAttributes)
        {
            return htmlHelper.CheckBoxList(name, list,
                ((IDictionary<string, object>)new RouteValueDictionary(htmlAttributes)));
        }

        public static string CheckBoxList(this HtmlHelper htmlHelper, string name, List<SelectListItem> list,
            IDictionary<string, object> htmlAttributes)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentException("The argument must have a value", "name");
            if (list == null)
                return "";
            if (list.Count < 1)
                return "";


            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < list.Count; i++)
            {
                
                SelectListItem item = list.ElementAt(i);
                TagBuilder div = new TagBuilder("div");
                div.AddCssClass("checkbox_holder");
                TagBuilder builder = new TagBuilder("input");
                if (item.Selected) builder.MergeAttribute("checked", "checked");
                builder.MergeAttributes<string, object>(htmlAttributes);
                builder.MergeAttribute("type", "checkbox");
                builder.MergeAttribute("value", item.Value);
                builder.MergeAttribute("name", name);
                builder.InnerHtml = item.Text;
                div.InnerHtml = builder.ToString(TagRenderMode.Normal);
                sb.Append(div.ToString(TagRenderMode.Normal));
                if ((i + 1) % 3 == 0 && i != 0)
                {
                    sb.Append("<br />");
                }
            }

            return sb.ToString();
        }

    }
}
