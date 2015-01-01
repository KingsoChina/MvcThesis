using System;
using System.Xml;
using System.IO;
using System.Web;
using System.Web.Caching;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Security;
using System.Linq;
using WebMatrix.WebData;

namespace MvcThesis
{
    public class PermissionManager
    {
        public static bool CheckUserHasPermision(string userName, string permissionName)
        {
            List<Role> roleList = new List<Role>();
            List<PermissionsInRoles> permissionsInRolesList = new List<PermissionsInRoles>();
            using (MvcThesisMembershipContext db = new MvcThesisMembershipContext())
            {
                roleList = db.Roles.AsEnumerable<Role>().ToList<Role>();

            }

            using (MvcThesisMembershipContext db = new MvcThesisMembershipContext())
            {
                permissionsInRolesList = db.PermissionsInRoles
                                            .Include("Permission").Include("Role")
                                            .AsEnumerable<PermissionsInRoles>().ToList<PermissionsInRoles>();
            }

            string[] currentRoles = Roles.GetRolesForUser(userName);
            foreach (var roleName in currentRoles)
            {
                List<Permission> permissionList = permissionsInRolesList.Where(e => e.Role.RoleName == roleName)
                                                                            .Select(e => e.Permission).ToList<Permission>();

                foreach (var permission in permissionList)
                {
                    if (permission.PermissionName == permissionName)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
