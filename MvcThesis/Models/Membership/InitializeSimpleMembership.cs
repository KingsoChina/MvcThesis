using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading;
using System.Web.Mvc;
using System.Web;
using WebMatrix.WebData;
using System.Web.Security;

namespace MvcThesis
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class InitializeSimpleMembership : ActionFilterAttribute
    {
        private static SimpleMembershipInitializer _initializer;
        private static object _initializerLock = new object();
        private static bool _isInitialized;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Ensure ASP.NET Simple Membership is initialized only once per app start
            LazyInitializer.EnsureInitialized(ref _initializer, ref _isInitialized, ref _initializerLock);
        }

        private class SimpleMembershipInitializer
        {
            public SimpleMembershipInitializer()
            {
                Database.SetInitializer<MvcThesisMembershipContext>(new ThesisInitializer());

                try
                {
                    using (var context = new MvcThesisMembershipContext())
                    {
                        if (!context.Database.Exists())
                        {
                            // Create the SimpleMembership database without Entity Framework migration schema
                            ((IObjectContextAdapter)context).ObjectContext.CreateDatabase();
                        }
                    }
                    if(!WebSecurity.Initialized)
                    WebSecurity.InitializeDatabaseConnection("DefaultConnection", "UserProfile", "UserId", "UserName", autoCreateTables: true);
                    if (!WebSecurity.UserExists("sxadmin")) {
                        WebSecurity.CreateUserAndAccount("sxadmin", "111111", new { MaxGuideNum = 0 });
                        Roles.AddUserToRole("sxadmin", "系统管理员");
                    }
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("The ASP.NET Simple Membership database could not be initialized. For more information, please see http://go.microsoft.com/fwlink/?LinkId=256588", ex);
                }
            }
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class MvcThesisAuthorizeAttribute : AuthorizeAttribute
    {
        private bool _authorize;

        private bool _isPermissionFail = false;

        public string permission { get; set; }

        public MvcThesisAuthorizeAttribute()
        {
            if (HttpContext.Current.User.Identity.Name != "")
            {
                _authorize = true;
                
            }
            else
            {
                _authorize = false;
            }
        }

        //public MvcThesisAuthorizeAttribute(string permission)
        //{
        //    if (HttpContext.Current.User.Identity.Name != "")
        //    {
        //        _authorize = PermissionManager.CheckUserHasPermision(HttpContext.Current.User.Identity.Name, permission);
        //        if (_authorize == false)
        //        {
        //            _isPermissionFail = true;
        //        }
        //    }
        //    else
        //    {
        //        _authorize = false;
        //    }
        //    //_authorize = true;
        //}

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException("HttpContext");
            }
            if (!httpContext.User.Identity.IsAuthenticated)
            {
                return false;
            }
            else
            {
                _authorize = PermissionManager.CheckUserHasPermision(HttpContext.Current.User.Identity.Name, permission);
                if (_authorize == false)
                {
                    _isPermissionFail = true;
                    return false;
                }
                return true;
            }
           // return false;
        }
        //protected override bool AuthorizeCore(HttpContextBase httpContext)
        //{
        //     return _authorize;
        //}

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (_isPermissionFail)
            {
                filterContext.HttpContext.Response.Redirect("/Admin/PermissionError");
            }
            else
            {
                base.HandleUnauthorizedRequest(filterContext);
            }
            
        }
    }
}
