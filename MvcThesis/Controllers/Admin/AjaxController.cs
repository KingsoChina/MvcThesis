using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;
using System.Web.Security;
using MvcThesis;

namespace MvcThesis.UI.AdminTools.Controllers
{
    //[Authorize(Roles = "系统管理员")]
    [AllowAnonymous]
    [InitializeSimpleMembership]
    public class AjaxController : Controller
    {
        public JsonResult SetPermission(string id)
        {
            int roleId = int.Parse(id.Split(new string[] { "," }, StringSplitOptions.None)[0]);
            int permissionId = int.Parse(id.Split(new string[] { "," }, StringSplitOptions.None)[1]);

            using (MvcThesisMembershipContext db = new MvcThesisMembershipContext())
            {
                PermissionsInRoles pir = new PermissionsInRoles();
                pir.RoleId = roleId;
                pir.PermissionId = permissionId;
                db.PermissionsInRoles.Add(pir);
                db.SaveChanges();
            }
            JsonResult result = new JsonResult();
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        public JsonResult RemovePermission(string id)
        {
            int roleId = int.Parse(id.Split(new string[] { "," }, StringSplitOptions.None)[0]);
            int permissionId = int.Parse(id.Split(new string[] { "," }, StringSplitOptions.None)[1]);

            using (MvcThesisMembershipContext db = new MvcThesisMembershipContext())
            {
                PermissionsInRoles pir = db.PermissionsInRoles.Single(e => e.PermissionId == permissionId && e.RoleId == roleId);
                db.PermissionsInRoles.Remove(pir);
                db.SaveChanges();
            }
            JsonResult result = new JsonResult();
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        public JsonResult SetRole(string id)
        {
            int userId = int.Parse(id.Split(new string[] { "," }, StringSplitOptions.None)[0]);
            string roleName = id.Split(new string[] { "," }, StringSplitOptions.None)[1];
            

            using (MvcThesisMembershipContext db = new MvcThesisMembershipContext())
            {
                UserProfile user = db.UserProfiles.Single(e=>e.UserId == userId);
                Roles.AddUserToRole(user.UserName, roleName);
            }
            JsonResult result = new JsonResult();
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        public JsonResult RemoveRole(string id)
        {
            int userId = int.Parse(id.Split(new string[] { "," }, StringSplitOptions.None)[0]);
            string roleName = id.Split(new string[] { "," }, StringSplitOptions.None)[1];


            using (MvcThesisMembershipContext db = new MvcThesisMembershipContext())
            {
                UserProfile user = db.UserProfiles.Single(e => e.UserId == userId);
                Roles.RemoveUserFromRole(user.UserName, roleName);
            }
            JsonResult result = new JsonResult();
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }
    }
}
