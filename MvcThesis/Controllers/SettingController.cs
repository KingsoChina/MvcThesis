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
            string CommentBookStartTime = db.Settings.Single(m => m.Title == "任务书开始时间").Content;
            string CommentBookEndTime = db.Settings.Single(m => m.Title == "任务书结束时间").Content;
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
        [Authorize(Roles = "系统管理员")]
        [HttpGet]
        public ActionResult Database()
        {
            return View();
        }
        [Authorize(Roles="系统管理员")]
        [HttpPost]
        public ActionResult ResetDatabase()
        {
            //db.UserProfiles.RemoveRange(db.UserProfiles.Where(m => m.UserName != "sxadmin").ToList());
            //db.Topics.RemoveRange(db.Topics.ToList());
            //db.Documents.RemoveRange(db.Documents.ToList());
            db.Database.Delete();
            db.SaveChanges();
            if (Directory.Exists(Server.MapPath("~/Upload/Document")))
            Directory.Delete(Server.MapPath("~/Upload/Document"), true);
            WebSecurity.Logout();
            return Json(new { status = 1, msg = "数据库已经回到初始状态" });
        }

    }
}
