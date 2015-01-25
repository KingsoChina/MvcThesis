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
    [Authorize(Roles = "系统管理员,院长,院长助理")]
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
            IEnumerable TopicList = db.Topics.Where(m => m.IsTeacherAgree == 1 && m.IsDeanAgree == 0).ToList();
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

        [HttpGet]
        public ActionResult AssignMarker(string id)
        {
            string[] ids = id.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            if (ids.Length < 1) return Json(new { status = 0, msg = "没有选中课题" }, JsonRequestBehavior.AllowGet);
            string[] UserName = Roles.GetUsersInRole("教师");
            IEnumerable Tec = db.UserProfiles.Where(m => UserName.Contains(m.UserName)).ToList();//列出没有超出指导数量的教师
            ViewData["Tec"] = Tec;
            if (ids.Length == 1)
            {
                int topicid = Convert.ToInt32(ids[0]);
                Topic topic = db.Topics.SingleOrDefault(m => m.TopicId == topicid);
                ViewData["topic"] = topic;
            }
            ViewData["id"] = string.Join(",", ids);
            return View();
        }
        [HttpPost]
        public ActionResult AssignMarker(string id, string CommentTeacherId, string AnswerTeacherId)
        {
            string[] ids = id.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            if (ids.Length < 1) return Json(new { status = 0, msg = "没有选中课题" });
            int CTID = Convert.ToInt32(CommentTeacherId);
            int ATID = Convert.ToInt32(AnswerTeacherId);
            UserProfile CommentTeacher = db.UserProfiles.SingleOrDefault(m => m.UserId == CTID);
            UserProfile AnswerTeacher = db.UserProfiles.SingleOrDefault(m => m.UserId == ATID);
            foreach (var topicId in ids)
            {
                int TID = Convert.ToInt32(topicId);
                Topic topic = db.Topics.SingleOrDefault(m => m.TopicId == TID);
                topic.CommentTeacher = CommentTeacher;
                topic.AnswerTeacher = AnswerTeacher;
                db.SaveChanges();
            }
            return Json(new { status = 1, msg = "设置成功！" });
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
        [HttpPost]
        public ActionResult ConfirmChosenTopic(string id)
        {
            string[] ids = id.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            if (ids.Length < 1) return Json(new { status = 0, msg = "没有选中课题" });
            IList<Topic> topicList = db.Topics.Where(m => ids.Contains(m.TopicId.ToString())).ToList();
            foreach (var topic in topicList)
            {
                topic.IsDeanAgree = 1;
            }
            db.SaveChanges();
            return Json(new { status = 1, msg = "操作成功" });
        }
        [MultipleResponseFormats]
        public ActionResult TopicList()
        {
            IEnumerable TopicList = db.Topics.Where(m => m.Status == 0).ToList();
            return View(TopicList);
        }
        [Authorize(Roles = "院长,系统管理员")]
        [MultipleResponseFormats]
        public ActionResult EditTopic(int id)
        {
            Topic Topic = db.Topics.Single(m => m.TopicId == id);
            string[] UserName = Roles.GetUsersInRole("教师");
            IEnumerable Tec = db.UserProfiles.Where(m => UserName.Contains(m.UserName)).ToList();//列出没有超出指导数量的教师
            ViewData["Tec"] = Tec;
            ViewData["MajorList"] = ThesisHelper.getMajor();
            ViewData["SelectedMajor"] = Topic.ApplyClass.Split(',');
            return View(Topic);
        }
        [HttpPost]
        [Authorize(Roles = "院长,系统管理员")]
        public ActionResult EditTopic(Topic topic, int TecId,string[] ApplyClass)
        {
            if (!ModelState.IsValid) return Json(new { status = 0, msg = "提交数据有误" });
            Topic Topic = db.Topics.Single(m => m.TopicId == topic.TopicId);
            Topic.ApplyClass = string.Join(",", ApplyClass);
            Topic.Teacher = db.UserProfiles.Single(m => m.UserId == TecId);
            Topic.Title = topic.Title;
            Topic.Source = topic.Source;
            Topic.Type = topic.Type;
            Topic.Condition = topic.Condition;
            Topic.Elaboration = topic.Elaboration;
            db.SaveChanges();
            return Json(new { status = 1, msg = "更改成功" });
        }
        //删除指定论题
        [HttpPost]
        [Authorize(Roles = "院长,系统管理员")]
        public ActionResult Delete(int id)
        {
            db.Topics.Remove(db.Topics.SingleOrDefault(m => m.TopicId == id));
            db.SaveChanges();
            return Json(new { status = 1, msg = "删除成功" });
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
            List<Topic> TopicList = db.Topics.Where(m => m.Teacher != null && m.Student != null).ToList();
            foreach (var topic in TopicList)
            {
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

        public ActionResult ExcelTopicDetail()
        {
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[]{
                new DataColumn("标题"),
                new DataColumn("适用班级"),
                new DataColumn("课题来源"),
                new DataColumn("类型"),
                new DataColumn("课题条件"),
                new DataColumn("阐述"),
                new DataColumn("指导老师"),
                new DataColumn("是否自命题"),
                new DataColumn("添加日期")
            });
            List<Topic> TopicList = db.Topics.ToList();
            foreach (var topic in TopicList)
            {
                dt.Rows.Add(new object[]{
                    topic.Title,
                    topic.ApplyClass,
                    topic.Source,
                    topic.Type,
                    topic.Condition,
                    topic.Elaboration,
                    topic.Teacher.FullName,
                    topic.IsFromStudent ? "是" : "否",
                    topic.CreateTime.ToString("yyyy/MM/dd H:mm:ss")
                });
            }
            int[] ColumnWidth = new int[] { 20, 15, 10, 10, 30, 30, 8, 5, 25 };//设定列宽
            NpoiHelper.ExportDataTableToExcel(dt, DateTime.Now.Year + "课题详细汇总.xls", "毕业论文课题详细汇总", ColumnWidth);
            return Json(new { status = 1, msg = "导出成功" });
        }

        //压缩文档文件夹
        public ActionResult ZipToDocument()
        {
            string filePath = "~/Down/毕业论文（设计）文档.zip";
            string dirPath = "~/Upload/Document";
            if (!Directory.Exists(Server.MapPath(dirPath))) Directory.CreateDirectory(dirPath);
            ThesisHelper.DocumentToZip(filePath, dirPath);
            return File(filePath, "application/zip", "毕业论文（设计）文档.zip");
        }
        [HttpPost]
        public ActionResult Upload()
        {
            HttpPostedFileBase f = Request.Files[0];
            string virtualPath = ThesisHelper.UpLoadXls(f);
            return this.Json(new { filename = Url.Content(virtualPath) });
        }
        [MultipleResponseFormats]
        [HttpGet]
        public ActionResult ImportData()
        {
            return View();
        }
        //导入信息
        [HttpPost]
        public ActionResult ImportData(string filename, string sheet)
        {
            if (sheet != "1" && sheet != "2") return Json(new { status = 1, msg = "参数错误" });
            string realpath = Server.MapPath("~/Upload/Import/") + filename;
            if (sheet == "1")
            {
                DataSet ds = ThesisHelper.ExcelToDS(realpath, "毕业论文课题详细汇总");
                DataRow[] dr = ds.Tables[0].Select();
                int rowcount = ds.Tables[0].Rows.Count;
                for (int i = 0; i < rowcount; i++)
                {
                    string Title = dr[i][0].ToString().Trim();
                    Topic topic = db.Topics.SingleOrDefault(m => m.Title == Title);
                    if (topic == null) { topic = db.Topics.Create(); }
                    string TeacherName = dr[i][6].ToString().Trim();
                    UserProfile Teacher = db.UserProfiles.SingleOrDefault(m => m.FullName == TeacherName);
                    topic.Title = dr[i][0].ToString().Trim();
                    topic.ApplyClass = dr[i][1].ToString().Trim();
                    topic.Source = dr[i][2].ToString().Trim();
                    topic.Type = dr[i][3].ToString().Trim();
                    topic.Condition = dr[i][4].ToString().Trim();
                    topic.Elaboration = dr[i][5].ToString().Trim();
                    topic.Teacher = Teacher;
                    topic.IsFromStudent = dr[i][7].ToString().Trim() == "是" ? true : false;
                    topic.CreateTime = Convert.ToDateTime(dr[i][8]);
                    topic.IsDeanAgree = 1;
                    if (!(topic.TopicId > 0)) db.Topics.Add(topic);
                    db.SaveChanges();
                }

            }
            else if (sheet == "2")
            {
                DataSet ds = ThesisHelper.ExcelToDS(realpath, "毕业论文选题汇总");
                DataRow[] dr = ds.Tables[0].Select();
                int rowcount = ds.Tables[0].Rows.Count;
                for (int i = 1; i < rowcount; i++)
                {
                    UserProfile Teacher = db.UserProfiles.SingleOrDefault(m => m.FullName == dr[i][0].ToString().Trim());
                    UserProfile Student = db.UserProfiles.SingleOrDefault(m => m.UserName == dr[i][4].ToString().Trim());
                    Topic Topic = db.Topics.SingleOrDefault(m => m.Title == dr[i][2].ToString().Trim());
                    if (Teacher == null || Student == null || Topic == null) continue;
                    Topic.Student = Student;
                    Student.MyTopic = Topic;
                    Topic.IsDeanAgree = 1;
                    db.SaveChanges();
                }
            }

            return Json(new { status = 1, msg = "导入完成" });
        }
    }
}
