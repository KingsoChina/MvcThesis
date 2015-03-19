using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;

namespace MvcThesis.Controllers
{
    [InitializeSimpleMembership]
    [Authorize]
    public class SettingController : Controller
    {
        private MvcThesisMembershipContext db = new MvcThesisMembershipContext();
        //
        // GET: /Setting/
        [MultipleResponseFormats]
        public ActionResult Index()
        {
            string Notice = db.Settings.Single(m => m.Title == "公告").Content;
            string TopicStartTime = db.Settings.Single(m => m.Title == "选题开始时间").Content;
            string TopicEndTime = db.Settings.Single(m => m.Title == "选题结束时间").Content;
            string MissionBookStartTime = db.Settings.Single(m => m.Title == "任务书开始时间").Content;
            string MissionBookEndTime = db.Settings.Single(m => m.Title == "任务书结束时间").Content;
            string ReportStartTime = db.Settings.Single(m => m.Title == "开题报告开始时间").Content;
            string ReportEndTime = db.Settings.Single(m => m.Title == "开题报告结束时间").Content;
            string ThesisStartTime = db.Settings.Single(m => m.Title == "毕业论文开始时间").Content;
            string ThesisEndTime = db.Settings.Single(m => m.Title == "毕业论文结束时间").Content;
            string CommentBookStartTime = db.Settings.Single(m => m.Title == "评议书开始时间").Content;
            string CommentBookEndTime = db.Settings.Single(m => m.Title == "评议书结束时间").Content;
            ViewData["Notice"] = Notice;
            ViewData["TopicStartTime"] = TopicStartTime;
            ViewData["TopicEndTime"] = TopicEndTime;
            ViewData["MissionBookStartTime"] = MissionBookStartTime;
            ViewData["MissionBookEndTime"] = MissionBookEndTime;
            ViewData["ReportStartTime"] = ReportStartTime;
            ViewData["ReportEndTime"] = ReportEndTime;
            ViewData["ThesisStartTime"] = ThesisStartTime;
            ViewData["ThesisEndTime"] = ThesisEndTime;
            ViewData["CommentBookStartTime"] = CommentBookStartTime;
            ViewData["CommentBookEndTime"] = CommentBookEndTime;
            return View();
        }
        public ActionResult SetTime()
        {
            string TopicStartTime = Request.Form["TopicStartTime"];
            string TopicEndTime = Request.Form["TopicEndTime"];
            string MissionBookStartTime = Request.Form["MissionBookStartTime"];
            string MissionBookEndTime = Request.Form["MissionBookEndTime"];
            string ReportStartTime = Request.Form["ReportStartTime"];
            string ReportEndTime = Request.Form["ReportEndTime"];
            string ThesisStartTime = Request.Form["ThesisStartTime"];
            string ThesisEndTime = Request.Form["ThesisEndTime"];
            string CommentBookStartTime = Request.Form["CommentBookStartTime"];
            string CommentBookEndTime = Request.Form["CommentBookEndTime"];
            db.Settings.Single(m => m.Title == "选题开始时间").Content = TopicStartTime;
            db.Settings.Single(m => m.Title == "选题结束时间").Content = TopicEndTime;
            db.Settings.Single(m => m.Title == "任务书开始时间").Content = MissionBookStartTime;
            db.Settings.Single(m => m.Title == "任务书结束时间").Content = MissionBookEndTime;
            db.Settings.Single(m => m.Title == "开题报告开始时间").Content = ReportStartTime;
            db.Settings.Single(m => m.Title == "开题报告结束时间").Content = ReportEndTime;
            db.Settings.Single(m => m.Title == "毕业论文开始时间").Content = ThesisStartTime;
            db.Settings.Single(m => m.Title == "毕业论文结束时间").Content = ThesisEndTime;
            db.Settings.Single(m => m.Title == "评议书开始时间").Content = CommentBookStartTime;
            db.Settings.Single(m => m.Title == "评议书结束时间").Content = CommentBookEndTime;
            db.SaveChanges();
            return Json(new { status = 1, msg = "更新成功" });
        }
        [ValidateInput(false)]
        public ActionResult SetNotice(string Notice)
        {
            db.Settings.Single(m => m.Title == "公告").Content = Notice;
            db.SaveChanges();
            return Json(new { status = 1, msg = "更新成功" });
        }
        [MultipleResponseFormats]
        [Authorize(Roles = "系统管理员")]
        [HttpGet]
        public ActionResult Database()
        {
            return View();
        }

        [Authorize(Roles = "系统管理员")]
        [MultipleResponseFormats]
        [HttpPost]
        public ActionResult ResetDatabase()
        {
            IList<Topic> TopicList = db.Topics.ToList();
            foreach (var topic in TopicList)
            {
                topic.Student = null;
                topic.Status = 1;//1表示往年论题
                if ((DateTime.Now.Year - topic.CreateTime.Year) > 4) db.Topics.Remove(topic);//删除4年前的论题
            }
            db.Comments.RemoveRange(db.Comments);
            db.Documents.RemoveRange(db.Documents);
            db.SaveChanges();
            string[] StuArr = Roles.GetUsersInRole("学生");
            if (StuArr.Length > 0)
                Roles.RemoveUsersFromRole(StuArr, "学生");
            //删除学生
            foreach (var user in StuArr)
            {
                ((SimpleMembershipProvider)Membership.Provider).DeleteAccount(user);
                ((SimpleMembershipProvider)Membership.Provider).DeleteUser(user, true);
            }
            string DocPath = "~/Upload/Document";
            if (Directory.Exists(Server.MapPath(DocPath)))
                Directory.Delete(Server.MapPath(DocPath), true);
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(Server.MapPath(DocPath));
            if (!dir.Exists)
                dir.Create();
            return Json(new { status = 1, msg = "数据库已经回到初始状态" });
        }
        [HttpGet]
        public ActionResult LoginImg()
        {
            ViewData["Img_url"] = ThesisHelper.C("登陆界面").ToString();
            return View();
        }
        [HttpPost]
        public ActionResult LoginImgHandler()
        {
            HttpPostedFileBase f = Request.Files[0];
            string relative_url = ThesisHelper.UpLoadImg(f, "background", "bg");
            ThesisHelper.C("登陆界面", relative_url);
            string url = Url.Content(relative_url);
            return Json(new { status = 1, url = url });
        }

    }
}
