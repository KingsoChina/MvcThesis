using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;

namespace MvcThesis.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class HomeController : Controller
    {
        private MvcThesisMembershipContext db = new MvcThesisMembershipContext();
        public ActionResult Index()
        {
            if (Roles.IsUserInRole("院长") || Roles.IsUserInRole("系统管理员"))
            {
                int StuNum = Roles.GetUsersInRole("学生").Count();
                int TecNum = Roles.GetUsersInRole("教师").Count();
                int DeanNum = Roles.GetUsersInRole("院长").Count();
                IList<Topic> TopicList = db.Topics.ToList();//查出所有课题，在View统计
                string[] StudentArr = Roles.GetUsersInRole("学生");
                IList<UserProfile> StudentList = db.UserProfiles.Where(m => StudentArr.Contains(m.UserName)).ToList();
                ViewBag.StuNum = StuNum;//学生数量
                ViewBag.TecNum = TecNum;//教师数量
                ViewBag.DeanNum = DeanNum;//院长数量
                ViewBag.TopicList = TopicList;
                ViewBag.StudentList = StudentList;
                return View("StasticIndex");
            }
            else
            {
                ViewBag.Notice = ThesisHelper.C("公告");
                UserProfile User = db.UserProfiles.Single(m => m.UserId == WebSecurity.CurrentUserId);
                return View(User);
            }
            
        }


        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (WebSecurity.IsAuthenticated) return RedirectToAction("Index");
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid && WebSecurity.Login(model.UserName, model.Password, model.RememberMe))
            {
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "用户名或密码错误");
            return View(model);
        }

        public ActionResult LogOff()
        {
            WebSecurity.Logout();
            return RedirectToAction("Login");
        }

        //个人信息
        public ActionResult MyProfile()
        {
           int UserId = WebSecurity.GetUserId(User.Identity.Name);
           UserProfile user = db.UserProfiles.Single(m => m.UserId == UserId);
           if (Roles.IsUserInRole("教师")) return View("Profile-Tec", user);
            return View("Profile",user);
        }

        [HttpPost]
        public ActionResult MyProfile(string Phone,string ShortPhone, string changepasswordcheck)
        {
            Regex PhoneReg = new Regex(@"^(13[0-9]|15[7-9]|153|156|18[7-9])[0-9]{8}$");
            if (!PhoneReg.IsMatch(Phone)) return Json(new { status = 0, msg = "请输入正确的手机号" });
            int UserId = WebSecurity.GetUserId(User.Identity.Name);
            UserProfile user = db.UserProfiles.Single(m => m.UserId == UserId);
            user.Phone = Phone;
            user.ShortPhone = ShortPhone;
            if (Request.Form["ForTec"] == "yes")
            {
                string Direction = Request.Form["Direction"];
                string Email = Request.Form["Email"];
                user.Direction = Direction;
                user.Email = Email;
            }
            db.SaveChanges();
            if (changepasswordcheck == "1") {
                string pwd = Request.Form["pwd"].ToString();
                if (Request.Form["newpwd"] != Request.Form["renewpwd"])
                    return Json(new { status = 0, msg = "两次输入密码不正确" });
                bool IsChange = WebSecurity.ChangePassword(user.UserName, pwd, Request.Form["newpwd"]);
                if (!IsChange) return Json(new { status = 0, msg = "原始密码不正确" });
                WebSecurity.Logout();
            }
            
            return Json(new { status = 1, msg = "修改成功" });
        }
    }
}
