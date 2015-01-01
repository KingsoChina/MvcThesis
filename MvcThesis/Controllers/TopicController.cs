using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MvcThesis.Controllers
{
    [InitializeSimpleMembership]
    [Authorize(Roles="系统管理员,院长,院长助理")]
    public class TopicController : Controller
    {
        private MvcThesisMembershipContext db = new MvcThesisMembershipContext();
        //
        // GET: /Dean/

        public ActionResult Index()
        {
            return View();
        }
        [MultipleResponseFormats]
        public ActionResult ConfirmTopicList()
        {
            IEnumerable TopicList = db.Topics.Where(m => m.IsTeacherAgree == 1 && m.IsDeanAgree==0).ToList();
            return View(TopicList);
        }
        [HttpPost]
        public ActionResult ConfirmTopic(int id)
        {
            Topic Topic = db.Topics.Single(m => m.TopicId == id);
            Topic.IsDeanAgree = 1;
            db.SaveChanges(); 
            return Json(new { status = 1, msg = "已通过课题" + Topic.Title });
        }

        //查看教师资料
        [MultipleResponseFormats]
        public ActionResult TecDetail(int id)
        {
            UserProfile Teacher = db.UserProfiles.SingleOrDefault(m => m.UserId == id);
            return View(Teacher);
        }
        [MultipleResponseFormats]
        public ActionResult StuDetail(int id)
        {
            UserProfile Student = db.UserProfiles.SingleOrDefault(m => m.UserId == id);
            return View(Student);
        }
        [MultipleResponseFormats]
        public ActionResult TopicList()
        {
            IEnumerable TopicList = db.Topics.ToList();
            return View(TopicList);
        }
        [Authorize(Roles="院长,系统管理员")]
        [MultipleResponseFormats]
        public ActionResult EditTopic(int id)
        {
            Topic Topic = db.Topics.Single(m => m.TopicId == id);
            string[] UserName = Roles.GetUsersInRole("教师");
            IEnumerable Tec = db.UserProfiles.Where(m => UserName.Contains(m.UserName)).ToList();//列出没有超出指导数量的教师
            ViewData["Tec"] = Tec;
            return View(Topic);
        }
        [HttpPost]
        [Authorize(Roles = "院长,系统管理员")]
        public ActionResult EditTopic(Topic topic,int TecId)
        {
            if (!ModelState.IsValid) return Json(new { status = 0, msg = "提交数据有误" });
            Topic Topic = db.Topics.Single(m => m.TopicId == topic.TopicId);
            Topic.ApplyClass = topic.ApplyClass;
            Topic.Teacher = db.UserProfiles.Single(m => m.UserId == TecId);
            Topic.Title = topic.Title;
            Topic.Source = topic.Source;
            Topic.Type = topic.Type;
            Topic.Condition = topic.Condition;
            Topic.Elaboration = topic.Elaboration;
            db.SaveChanges();
            return Json(new { status=1,msg="更改成功"});
        }
        [MultipleResponseFormats]
        public ActionResult TopicDocument(int id)
        {
            Topic topic = db.Topics.Single(m => m.TopicId == id);
            if (topic == null) return Json(new { status = 0, msg = "数据出错" });
            return View(topic);
        }
        [HttpPost]
        [Authorize(Roles = "院长,系统管理员")]
        public ActionResult CoverDocument(int id)
        {
            Document Doc = db.Documents.Single(m => m.DocumentId == id);
            HttpPostedFileBase f = Request.Files[0];
            ThesisHelper.CoverDoc(f, Doc.Path);
            return Json(new { status = 0, msg = "覆盖成功" });
        }

        public ActionResult ExcelStatistic()
        {
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[] { 
                new DataColumn("学号"), 
                new DataColumn("姓名"), 
                new DataColumn("班级"),
                new DataColumn("毕业论文（设计）题目"), 
                new DataColumn("指导老师"), 
                new DataColumn("教师职称")
            });
            List<Topic> TopicList = db.Topics.Where(m => m.Teacher!=null && m.Student !=null).ToList();
            foreach(var topic in TopicList){
                dt.Rows.Add(new object[] { 
                    topic.Student.UserName,
                    topic.Student.FullName,
                    topic.Student.Class,
                    topic.Title, 
                    topic.Teacher.FullName,
                    topic.Teacher.JobTitle
                });
            }
            NpoiHelper.ExportDataTableToExcel(dt, DateTime.Now.Year + "年毕业论文选题汇总.xls", "毕业论文选题汇总");
            return Json(new { status = 1, msg = "下载成功" });
        }

        //压缩文档文件夹
        public ActionResult ZipToDocument()
        {
            string filePath = "~/Down/毕业论文（设计）文档.zip";
            string dirPath = "~/Upload/Document";
            if(!Directory.Exists(Server.MapPath(dirPath)))Directory.CreateDirectory(dirPath);
            ThesisHelper.DocumentToZip(filePath, dirPath);
            return File(filePath, "application/zip", "毕业论文（设计）文档.zip");
        }
    }
}
