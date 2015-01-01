using System.Web;
using System.Web.Mvc;

namespace MvcThesis
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());

        }

    }

    public class MultipleResponseFormatsAttribute : ActionFilterAttribute 
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var request = filterContext.HttpContext.Request;
            var ViewResult = filterContext.Result as ViewResult;
            if (ViewResult == null) return;
            if (request.IsAjaxRequest() || request.QueryString["ajax"]=="1")
            {
                filterContext.Result = new PartialViewResult
                {
                    TempData = ViewResult.TempData,
                    ViewData = ViewResult.ViewData,
                    ViewName = ViewResult.ViewName
                };
            }
        }
        //public override void OnActionExecuting(ActionExecutingContext filterContext)
        //{
        //    if (filterContext.HttpContext.Request.Browser.to)
        //    base.OnActionExecuting(filterContext);
        //}
    }
    //public class MyGlobalAttribute : ActionFilterAttribute
    //{
    //    public override void OnActionExecuted(ActionExecutedContext filterContext)
    //    {
    //        base.OnActionExecuted(filterContext);
    //        var request = filterContext.HttpContext.Request;
    //    }
    //}
}