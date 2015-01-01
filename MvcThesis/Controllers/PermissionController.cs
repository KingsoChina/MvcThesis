using System;
using System.Collections.Generic;
//using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcThesis;

namespace MvcThesis.UI.AdminTools.Controllers
{
    //[Authorize(Roles = "系统管理员")]
    [AllowAnonymous]
    [InitializeSimpleMembership]
    public class PermissionController : Controller
    {
        private MvcThesisMembershipContext db = new MvcThesisMembershipContext();

        public ActionResult Index()
        {
            if (Request.IsAjaxRequest()) return PartialView(db.Permissions.ToList());
            return View(db.Permissions.ToList());
        }

        //
        // GET: /Permission/Create

        public ActionResult Create()
        {
            return PartialView();
        }

        //
        // POST: /Permission/Create

        [HttpPost]
        public ActionResult Create(Permission permission)
        {
            if (!ModelState.IsValid)
            {
                //List<string> errors = 
                return Json(new { status = 0, msg = "" });
            }
            else
            {
                if (db.Permissions.SingleOrDefault(e => e.PermissionName == permission.PermissionName) == null)
                {
                    db.Permissions.Add(permission);
                    db.SaveChanges();
                    if (Request.IsAjaxRequest()) return Json(new { status = 1, msg = "添加成功" });
                    return View(permission);
                }
                else
                {
                    TempData["ErrorMessage"] = "权限名称已存在";
                    if (Request.IsAjaxRequest()) return Json(new { status = 0, msg = TempData["ErrorMessage"] });
                    return View(permission);
                }
            }


        }

        //
        // GET: /Permission/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Permission permission = db.Permissions.Find(id);
            if (permission == null)
            {
                return HttpNotFound();
            }
            if (Request.IsAjaxRequest()) return PartialView(permission);
            return View(permission);
        }

        //
        // POST: /Permission/Edit/5

        [HttpPost]
        public ActionResult Edit(Permission permission)
        {
            if (ModelState.IsValid)
            {
                if (db.Permissions.SingleOrDefault(e => e.PermissionName == permission.PermissionName) == null)
                {
                    db.Entry(permission).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["ErrorMessage"] = "权限名称已存在";
                }

            }
            return View(permission);
        }

        //
        // GET: /Permission/Delete/5

        public ActionResult Delete(int id)
        {
            if (db.Permissions.SingleOrDefault(e => e.PermissionId == id) != null)
            {
                Permission permission = db.Permissions.Find(id);
                db.Permissions.Remove(permission);
                db.SaveChanges();
                return Json(new { status = 1, msg = "删除成功" });
            }
            return Json(new { status = 0, msg = "不存在此权限" });
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}