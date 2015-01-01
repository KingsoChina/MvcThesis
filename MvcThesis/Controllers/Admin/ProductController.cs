using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcThesis;

namespace MvcThesisSimplemembership.Controllers
{
    /// <summary>
    /// 这是一个产品权限的测试Controller
    /// </summary>
    /// 
    [Authorize]
    [InitializeSimpleMembership]
    public class ProductController : Controller
    {
        [MvcThesisAuthorize(permission="查询产品")]
        public ActionResult Index()
        {
            return View();
        }

        [MvcThesisAuthorize(permission="添加产品")]
        public ActionResult Create()
        {
            return View();
        }

        [MvcThesisAuthorize(permission="编辑产品")]
        public ActionResult Edit()
        {
            return View();
        }

        [MvcThesisAuthorize(permission="删除产品")]
        public ActionResult Delete()
        {
            return View();
        }

    }
}
