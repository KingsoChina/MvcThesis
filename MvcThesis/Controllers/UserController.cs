using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;
using MvcThesis;
using System.Collections;
using System.Data.SqlClient;
using System.Configuration;

namespace MvcThesis.UI.AdminTools.Controllers
{

    //[AllowAnonymous]
    [InitializeSimpleMembership]
    [Authorize(Roles = "系统管理员")]
    public class UserController : Controller
    {
        private MvcThesisMembershipContext db = new MvcThesisMembershipContext();

        //
        // GET: /User/

        public ActionResult Index()
        {
            List<UserViewModel> userList = new List<UserViewModel>();
            List<MvcThesisMembership> list = new List<MvcThesisMembership>();
            using (MvcThesisMembershipContext db = new MvcThesisMembershipContext())
            {
                list = db.MvcThesisMemberships.Include("Roles").ToList();
                foreach (var bm in list)
                {
                    UserViewModel user = new UserViewModel();
                    user.User_ID = bm.UserId;
                    user.User_Name = db.UserProfiles.Single(e => e.UserId == bm.UserId).UserName;
                    user.Full_Name = db.UserProfiles.Single(e => e.UserId == bm.UserId).FullName;
                    user.JobTitle = db.UserProfiles.Single(e => e.UserId == bm.UserId).JobTitle;
                    user.Email = db.UserProfiles.Single(e => e.UserId == bm.UserId).Class;
                    var roleNameString = "";
                    for (int i = 0; i < bm.Roles.Count; i++)
                    {
                        if (i == 0)
                        {
                            roleNameString = bm.Roles.ElementAt(i).RoleName;
                        }
                        else
                        {
                            roleNameString = roleNameString + "," + bm.Roles.ElementAt(i).RoleName;
                        }
                    }
                    user.Role_Name = roleNameString;
                    userList.Add(user);
                }
            }
            ViewData["UserList"] = userList;
            ViewBag.Title = "账号管理";
            if (Request.IsAjaxRequest()) return PartialView(userList);
            return View(userList);
        }

        [AllowAnonymous]
        public ActionResult Create()
        {
            ViewData["RoleSelectList"] = GetRoleSelectList();
            if (Request.IsAjaxRequest()) return PartialView();
            return View();
        }

        //
        // POST: /User/Create

        //[AllowAnonymous]
        [HttpPost]
        public ActionResult Create(FormCollection collection, string[] roles)
        {
            if (ModelState.IsValid)
            {
                string userName = collection.Get("User_Name");
                string password = collection.Get("Password");
                string Name = collection.Get("Full_Name");
                if (db.UserProfiles.SingleOrDefault(e => e.UserName == userName) == null)
                {
                    WebSecurity.CreateUserAndAccount(userName, password, new { FullName = Name, MaxGuideNum = 0 });

                    if (roles != null)
                    {
                        foreach (var roleName in roles)
                        {
                            Roles.AddUserToRole(userName, roleName);
                        }
                    }
                    if (Request.IsAjaxRequest()) return Json(new { status = 1, msg = "添加成功" });
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewData["RoleSelectList"] = GetRoleSelectList();
                    TempData["ErrorMessage"] = "用户名已存在";
                }
            }
            if (Request.IsAjaxRequest()) return Json(new { status = 0, msg = TempData["ErrorMessage"] });
            return View();
        }
        [HttpGet]
        [MultipleResponseFormats]
        public ActionResult Import()
        {
            ViewData["RoleSelectList"] = GetRoleSelectList();
            return View();
        }

        [HttpPost]
        public ActionResult Import(string filename, string role)
        {
            if (role == string.Empty) return Json(new { status = 0, msg = "没有指定角色" });
            string realpath = Server.MapPath("~/Upload/Import/") + filename;
            DataSet ds = ThesisHelper.ExcelToDS(realpath);
            DataRow[] dr = ds.Tables[0].Select();
            int rowcount = ds.Tables[0].Rows.Count;
            if (role == "tec")//教师
            {
                for (int i = 0; i < dr.Length; i++)
                {
                    string tid = dr[i][0].ToString().Trim();//工号
                    if (tid == "" || tid.Length != 5) continue;
                    string name = dr[i][1].ToString().Trim();//姓名
                    string jobtitle = dr[i][2].ToString().Trim();//职称
                    string direction = dr[i][3].ToString().Trim();//研究方向
                    string tel = dr[i][4].ToString().Trim();//电话
                    string shorttel = dr[i][5].ToString().Trim();//短号
                    string email = dr[i][6].ToString().Trim();//邮箱
                    string QQ = dr[i][7].ToString().Trim();//QQ

                    if (db.UserProfiles.SingleOrDefault(e => e.UserName == tid) == null)
                    {
                        WebSecurity.CreateUserAndAccount(tid, "111111", new
                        { //添加账号信息
                            FullName = name,
                            MaxGuideNum = 8,
                            Direction = direction,
                            Phone = tel,
                            jobtitle = jobtitle,
                            ShortPhone = shorttel,
                            Email = email,
                            QQ = QQ
                        });
                        if (!Roles.RoleExists("教师")) Roles.CreateRole("教师");
                        Roles.AddUserToRole(tid, "教师");
                    }
                    else
                    {
                        UserProfile user = db.UserProfiles.Single(e => e.UserName == tid);
                        user.FullName = name;
                        user.Direction = direction;
                        user.Email = email;
                        user.Phone = tel;
                        user.JobTitle = jobtitle;
                        user.ShortPhone = shorttel;
                        user.MaxGuideNum = 8;
                        db.SaveChanges();
                    }
                }
            }
            else if (role == "stu")
            {
                for (int i = 0; i < dr.Length; i++)
                {
                    string sid = dr[i][0].ToString().Trim();//学号
                    if (sid == "" || sid.Length != 11) continue;
                    string name = dr[i][1].ToString().Trim();//姓名
                    string department = dr[i][2].ToString().Trim();//专业
                    string Class = dr[i][3].ToString().Trim();//班级
                    string tel = dr[i][4].ToString().Trim();//长号
                    string shorttel = dr[i][5].ToString().Trim();//短号
                    string Email = dr[i][6].ToString().Trim();//邮箱
                    string QQ = dr[i][7].ToString().Trim();//QQ

                    if (db.UserProfiles.SingleOrDefault(e => e.UserName == sid) == null)
                    {
                        WebSecurity.CreateUserAndAccount(sid, "111111", new
                        { //添加账号信息
                            FullName = name,
                            MaxGuideNum = 0,
                            Major = department,
                            Phone = tel,
                            ShortPhone = shorttel,
                            Class = Class,
                            Email = Email,
                            QQ = QQ
                        });
                        if (!Roles.RoleExists("学生")) Roles.CreateRole("学生");
                        Roles.AddUserToRole(sid, "学生");
                    }
                    else
                    {
                        UserProfile user = db.UserProfiles.Single(e => e.UserName == sid);
                        user.FullName = name;
                        user.Major = department;
                        user.Phone = tel;
                        user.Class = Class;
                        user.ShortPhone = shorttel;
                        user.QQ = QQ;
                        user.Email = Email;
                        db.SaveChanges();
                    }
                }
            }
            return Json(new { status = 1, msg = "导入成功！" });
        }

        [HttpPost]
        public ActionResult Upload()
        {
            HttpPostedFileBase f = Request.Files[0];
            string virtualPath = ThesisHelper.UpLoadXls(f);
            return this.Json(new { filename = Url.Content(virtualPath) });
        }

        //
        // GET: /User/Edit/5

        public ActionResult Edit(int id = 0)
        {
            List<SelectListItem> roleList = GetRoleSelectList();
            UserProfile vModel = db.UserProfiles.Single(m => m.UserId == id);
            MvcThesisMembership membership = db.MvcThesisMemberships.Include("Roles").Single(e => e.UserId == id);
            UserProfile user = db.UserProfiles.Single(e => e.UserId == id);
            db.SaveChanges();
            ViewData["RoleSelectList"] = roleList;
            if (Request.IsAjaxRequest()) return PartialView("_editForm", vModel);
            return View("_editForm", vModel);
        }

        //
        // POST: /User/Edit/5

        [HttpPost]
        public ActionResult Edit(FormCollection collection, string[] roles)
        {
            if (!ModelState.IsValid) return Json(new { status = 0, msg = "输入信息不合规范" });
            int id = int.Parse(collection.Get("UserId"));
            string UserName = collection.Get("UserName");
            string FullName = collection.Get("FullName");
            string Major = collection.Get("Major");
            string Class = collection.Get("Class");
            string email = collection.Get("Email");
            string Phone = collection.Get("Phone");
            string ShortPhone = collection.Get("ShortPhone");
            string JobTitle = collection.Get("JobTitle");
            int MaxGuideNum = collection.Get("MaxGuideNum") == null ? 0 : int.Parse(collection.Get("MaxGuideNum"));
            string Institute = collection.Get("Institute");
            UserProfile user = db.UserProfiles.Single(e => e.UserId == id);
            if (roles != null)
            {
                foreach (var roleName in roles)
                {
                    Roles.AddUserToRole(UserName, roleName);
                }
            }
            user.UserName = UserName;
            user.FullName = FullName;
            user.Email = email;
            user.Institute = Institute;
            user.JobTitle = JobTitle;
            user.Major = Major;
            user.MaxGuideNum = MaxGuideNum;
            user.Phone = Phone;
            user.ShortPhone = ShortPhone;
            user.Class = Class;
            db.SaveChanges();
            return Json(new { status = 1, msg = "更改成功" });
        }
        [MultipleResponseFormats]
        public ActionResult EditRole(int id = 0)
        {
            UserViewModel vModel = new UserViewModel();
            List<SelectListItem> roleList = GetRoleSelectList();
            using (MvcThesisMembershipContext db = new MvcThesisMembershipContext())
            {
                MvcThesisMembership membership = db.MvcThesisMemberships.Include("Roles").Single(e => e.UserId == id);
                UserProfile user = db.UserProfiles.Single(e => e.UserId == id);
                vModel.User_ID = user.UserId;
                vModel.User_Name = user.UserName;
                vModel.Email = user.Class;

                foreach (var item in roleList)
                {
                    foreach (var role in membership.Roles)
                    {
                        if (item.Value == role.RoleName)
                        {
                            item.Selected = true;
                        }
                    }
                }
            }
            ViewData["RoleSelectList"] = roleList;

            return View(vModel);
        }

        [HttpPost]
        public ActionResult EditRole(int User_ID, string[] roles)
        {
            UserProfile user = db.UserProfiles.Single(e => e.UserId == User_ID);
            string[] roleList = Roles.GetRolesForUser(user.UserName);
            if (roleList.Length > 0)
                Roles.RemoveUserFromRoles(user.UserName, roleList);
            if (roles != null)
            {
                foreach (var roleName in roles)
                {
                    Roles.AddUserToRole(user.UserName, roleName);
                }
            }
            return Json(new { status = 1, msg = "修改成功" });
        }

        //
        // Post: /User/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            Membership.DeleteUser(db.UserProfiles.Single(e => e.UserId == id).UserName);
            db.MvcThesisMemberships.Remove(db.MvcThesisMemberships.Single(e => e.UserId == id));
            db.SaveChanges();
            return Json(new { status = 1, msg = "删除成功" });
        }

        public ActionResult ResetPassword(int id)
        {
            UserProfile user = db.UserProfiles.Single(e => e.UserId == id);
            UserViewModel vModel = new UserViewModel();
            vModel.User_ID = user.UserId;
            vModel.User_Name = user.UserName;
            vModel.Email = user.Class;
            return PartialView(vModel);
        }

        [HttpPost]
        public ActionResult ResetPassword(FormCollection collection)
        {
            int id = int.Parse(collection.Get("User_ID"));
            string newPassword = collection.Get("NewPassword");
            UserProfile user = db.UserProfiles.Single(e => e.UserId == id);
            MvcThesisMembership membership = db.MvcThesisMemberships.Single(e => e.UserId == id);
            string passwordResetToken = WebSecurity.GeneratePasswordResetToken(user.UserName, 1440);
            membership.PasswordResetToken = passwordResetToken;
            WebSecurity.ResetPassword(passwordResetToken, newPassword);
            //TempData["SuccessMessage"] = "密码修改成功！";
            if (Request.IsAjaxRequest()) return Json(new { status = 1, msg = "修改成功" });
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        private List<SelectListItem> GetRoleSelectList()
        {
            List<SelectListItem> roleList = new List<SelectListItem>();
            using (MvcThesisMembershipContext db = new MvcThesisMembershipContext())
            {
                foreach (var role in db.Roles)
                {
                    SelectListItem item = new SelectListItem();
                    item.Value = role.RoleName;
                    item.Text = role.RoleName;
                    roleList.Add(item);
                }
            }
            return roleList;
        }
    }
}