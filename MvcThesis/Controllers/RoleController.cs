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
    [InitializeSimpleMembership]
    [Authorize(Roles = "系统管理员")]
    //[AllowAnonymous]
    
    public class RoleController : Controller
    {
        private MvcThesisMembershipContext db = new MvcThesisMembershipContext();

        //
        // GET: /Role/

        [MultipleResponseFormats]
        public ActionResult Index()
        {
            return View(db.Roles.ToList());
        }

        //
        // GET: /Role/Details/5

        public ActionResult Details(int id = 0)
        {
            Role role = db.Roles.Find(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }

        //
        // GET: /Role/Create
        [MultipleResponseFormats]
        public ActionResult Create()
        {
            ViewData["PermissionSelectList"] = GetPermissionSelectList();
            return View();
        }

        //
        // POST: /Role/Create
        [HttpPost]
        public ActionResult Create(Role role, string[] permissions)
        {
            if (ModelState.IsValid)
            {
                if (db.Roles.SingleOrDefault(e => e.RoleName == role.RoleName) == null)
                {
                    Role currentRole = db.Roles.Add(role);
                    db.SaveChanges();
                    if (permissions != null)
                    {
                        foreach (var permissionId in permissions)
                        {
                            PermissionsInRoles pir = new PermissionsInRoles();
                            pir.RoleId = currentRole.RoleId;
                            pir.PermissionId = int.Parse(permissionId);
                            db.PermissionsInRoles.Add(pir);
                            db.SaveChanges();
                        }
                    }
                    return Json(new { status = 1, msg = "添加成功" });
                }
                return Json(new { status = 0, msg = "已存在此角色名" });
            }
            return Json(new { status = 0, msg = "数据非法" });
        }

        //
        // GET: /Role/Edit/5
        [MultipleResponseFormats]
        public ActionResult Edit(int id = 0)
        {
            Role role = db.Roles.Find(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }

        //
        // POST: /Role/Edit/5

        [HttpPost]
        public ActionResult Edit(Role role)
        {
            if (db.Roles.SingleOrDefault(e => e.RoleName == role.RoleName) == null)
            {
                if (ModelState.IsValid)
                {
                    db.Entry(role).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json(new { status = 1, msg = "修改成功" });
                }
                return Json(new { status = 1, msg = "数据非法" });
            }
            else
            {
                return Json(new { status = 0, msg = "已存在角色名称" });
            }
        }
        [MultipleResponseFormats]
        public ActionResult EditPermission(int id = 0)
        {
            Role role = db.Roles.Single(e => e.RoleId == id);
            List<SelectListItem> permissionSelectList = GetPermissionSelectList();

            List<Permission> permissionList = db.PermissionsInRoles
                                    .Include("Permissions").Include("Roles")
                                    .Where(e => e.RoleId == id)
                                    .Select(e => e.Permission).ToList<Permission>();

            foreach (var item in permissionSelectList)
            {
                foreach (var permission in permissionList)
                {
                    if (item.Value == permission.PermissionId.ToString())
                    {
                        item.Selected = true;
                    }
                }
            }

            ViewData["PermissionSelectList"] = permissionSelectList;
            return View(role);
        }

        [HttpPost]
        public ActionResult EditPermission(int RoleId, string[] permissions)
        {
            Role role = db.Roles.Single(e => e.RoleId == RoleId);
            foreach(var permission in db.PermissionsInRoles.Where(m => m.RoleId==RoleId).ToList()){
                db.PermissionsInRoles.Remove(permission);
            }
            PermissionsInRoles pir = new PermissionsInRoles();
            foreach (var permission in permissions)
            {
                pir.RoleId = RoleId;
                pir.PermissionId = Convert.ToInt32(permission);
                db.PermissionsInRoles.Add(pir);
                db.SaveChanges();
            }
            
            return Json(new { status = 1, msg = "修改成功" });

        }

        public ActionResult Delete(int id)
        {
            Role role = db.Roles.Find(id);
            db.Roles.Remove(role);
            db.SaveChanges();
            return Json(new { status = 1, msg = "删除成功" });
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        private List<SelectListItem> GetPermissionSelectList()
        {
            List<SelectListItem> permissionList = new List<SelectListItem>();
            using (MvcThesisMembershipContext db = new MvcThesisMembershipContext())
            {
                foreach (var permission in db.Permissions)
                {
                    SelectListItem item = new SelectListItem();
                    item.Value = permission.PermissionId.ToString();
                    item.Text = permission.PermissionName;
                    permissionList.Add(item);
                }
            }
            return permissionList;
        }
    }
}