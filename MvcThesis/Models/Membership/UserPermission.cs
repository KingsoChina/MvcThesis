using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Web.Mvc;
using WebMatrix.WebData;
using MvcThesis;

namespace MvcThesis
{
    public class UserPermission
    {
        public static bool HasMaterialWVEditReleasedPermission
        {
            get
            {
                return PermissionManager.CheckUserHasPermision(HttpContext.Current.User.Identity.Name, "物料入库生效修改");
            }
        }

        public static bool HasMaterialSalesOrderEditReleasedPermission
        {
            get
            {
                return PermissionManager.CheckUserHasPermision(HttpContext.Current.User.Identity.Name, "物料销售生效修改");
            }
        }

        public static bool HasProductSalesOrderEditReleasedPermission
        {
            get
            {
                return PermissionManager.CheckUserHasPermision(HttpContext.Current.User.Identity.Name, "产品销售生效修改");
            }
        }

        public static bool HasPieceWorkPaymentEditReleasedPermission
        {
            get
            {
                return PermissionManager.CheckUserHasPermision(HttpContext.Current.User.Identity.Name, "计件工资生效修改");
            }
        }

        public static bool HasOddJobPaymentEditReleasedPermission
        {
            get
            {
                return PermissionManager.CheckUserHasPermision(HttpContext.Current.User.Identity.Name, "杂工费生效修改");
            }
        }

        public static bool HasDailyUsingCostEditReleasedPermission
        {
            get
            {
                return PermissionManager.CheckUserHasPermision(HttpContext.Current.User.Identity.Name, "领用费生效修改");
            }
        }
    }
}
