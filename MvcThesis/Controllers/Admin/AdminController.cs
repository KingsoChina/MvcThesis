using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;
using System.Web.Security;
using MvcThesis;

namespace MvcThesis.UI.TongmiWebsite.Controllers
{
    //[Authorize(Roles = "系统管理员")]
    [AllowAnonymous]
    [InitializeSimpleMembership]
    public class AdminController : Controller
    {

        //[AllowAnonymous]
        //public ActionResult Login(string returnUrl)
        //{
        //    ViewBag.ReturnUrl = returnUrl;
        //    return View();
        //}

        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public ActionResult Login(LoginModel model, string returnUrl)
        //{
        //    if (ModelState.IsValid && WebSecurity.Login(model.UserName, model.Password,model.RememberMe))
        //    {
        //        return RedirectToAction("Index");
        //    }

        //    ModelState.AddModelError("", "用户名或密码错误");
        //    return View(model);
        //}

        public ActionResult Index()
        {
            return View();
        }

        //public ActionResult LogOff()
        //{
        //    WebSecurity.Logout();
        //    return RedirectToAction("Login");
        //}

        public ActionResult PermissionError()
        {
            ViewBag.Message = "Your app description page.";
            return View();
        }
    }
}
