using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;

namespace MvcThesis.Controllers
{
    [Authorize(Roles="学生")]
    [InitializeSimpleMembership]
    public class StuController : Controller
    {
        private MvcThesisMembershipContext db = new MvcThesisMembershipContext();
        //
        // GET: /Stu/
        [MultipleResponseFormats]
        public ActionResult TopicList()
        {
            DateTime TopicStartTime = Convert.ToDateTime(ThesisHelper.C("选题开始时间"));
            DateTime TopicEndTime = Convert.ToDateTime(ThesisHelper.C("选题结束时间"));
            ViewBag.TopicStatus = DateTime.Now < TopicStartTime ? -1 : DateTime.Now > TopicEndTime ? 1 : 0;
            if (ViewBag.TopicStatus != 0) return View();
            UserProfile Stu = db.UserProfiles.SingleOrDefault(m => m.UserId == WebSecurity.CurrentUserId);
            //判断是否已选课题或者老师已拒绝
            if (Stu.MyTopic != null && Stu.MyTopic.IsTeacherAgree !=-1)
            {
                return RedirectToAction("MyThesis");
            }
            IEnumerable TopicList = db.Topics.Where(m => m.Student == null && m.IsDeanAgree==1).ToList();
            return View(TopicList);
        }
        [MultipleResponseFormats]
        public ActionResult SetTopic()
        {
            string[] UserName = Roles.GetUsersInRole("教师");
            IEnumerable Tec = db.UserProfiles.Where(m => UserName.Contains(m.UserName)).Where(m => m.Topics.Count < m.MaxGuideNum).ToList();//列出没有超出指导数量的教师
            ViewData["Tec"] = Tec;
            return View();
        }
        [HttpPost]
        public ActionResult SetTopic(Topic topic, string TecId)
        {
            if (!ModelState.IsValid || TecId == null) return Json(new { status = 0, msg = "提交失败" });
            UserProfile Student = db.UserProfiles.SingleOrDefault(m => m.UserId == WebSecurity.CurrentUserId);//获取当前登陆学生对象
            int teacherid = Convert.ToInt32(TecId);
            UserProfile Teacher = db.UserProfiles.SingleOrDefault(m => m.UserId == teacherid);
            topic.Student = Student;//指定学生
            topic.Teacher = Teacher;//指定老师
            topic.IsFromStudent = true;
            topic.Source = "其他";
            topic.ApplyClass = Student.Major;
            topic.CreateTime = DateTime.Now;//添加时间
            Student.MyTopic = topic;//绑定学生
            db.Topics.Add(topic);
            db.SaveChanges();
            return Json(new { status = 1, msg = "自命题成功，等待老师同意" });
        }
        //查看教师资料
        [MultipleResponseFormats]
        public ActionResult TecDetail(int id)
        {
            UserProfile Teacher = db.UserProfiles.SingleOrDefault(m => m.UserId == id);
            return View(Teacher);
        }
        //查看课题详细
        [MultipleResponseFormats]
        public ActionResult TopicDetail(int id)
        {
            Topic Topic = db.Topics.Single(m => m.TopicId == id);
            return View(Topic);
        }
        //选择课题
        [HttpPost]
        public ActionResult ChooseTopic(int id)
        {
            Topic topic = db.Topics.SingleOrDefault(m => m.TopicId == id);
            if (topic.Student != null) return Json(new { status = 0, msg = "下手太晚，此课题已被选" });
            UserProfile Stu = db.UserProfiles.SingleOrDefault(m => m.UserId == WebSecurity.CurrentUserId);
            if (Stu.MyTopic != null && Stu.MyTopic.IsTeacherAgree !=-1) return Json(new { status = 0, msg = "您已经选择了一个课题" });
            try
            {
                //双向绑定
                topic.Student = Stu;
                Stu.MyTopic = topic;
                topic.IsTeacherAgree = 1;
                db.SaveChanges();
                return Json(new { status = 1, msg = "选题成功！" });
            }
            catch
            {
                return Json(new { status = 1, msg = "选题失败！被别人抢先一步了(┬＿┬)" });
            }
        }
        //显示学生申请或所选课题详情
        [MultipleResponseFormats]
        public ActionResult MyThesis()
        {
            DateTime MissionBookStartTime = Convert.ToDateTime(ThesisHelper.C("任务书开始时间"));
            DateTime MissionBookEndTime = Convert.ToDateTime(ThesisHelper.C("任务书结束时间"));
            ViewBag.MissionBookStatus = DateTime.Now < MissionBookStartTime ? -1 : DateTime.Now > MissionBookEndTime ? 1 : 0;
            DateTime ReportStartTime = Convert.ToDateTime(ThesisHelper.C("开题报告开始时间"));
            DateTime ReportEndTime = Convert.ToDateTime(ThesisHelper.C("开题报告结束时间"));
            ViewBag.ReportStatus = DateTime.Now < ReportStartTime ? -1 : DateTime.Now > ReportEndTime ? 1 : 0;
            DateTime ThesisStartTime = Convert.ToDateTime(ThesisHelper.C("毕业论文开始时间"));
            DateTime ThesisEndTime = Convert.ToDateTime(ThesisHelper.C("毕业论文结束时间"));
            ViewBag.ThesisStatus = DateTime.Now < ThesisStartTime ? -1 : DateTime.Now > ThesisEndTime ? 1 : 0;
            DateTime CommentBookStartTime = Convert.ToDateTime(ThesisHelper.C("评议书开始时间"));
            DateTime CommentBookEndTime = Convert.ToDateTime(ThesisHelper.C("评议书结束时间"));
            ViewBag.CommentBookStatus = DateTime.Now < CommentBookStartTime ? -1 : DateTime.Now > CommentBookEndTime ? 1 : 0;
            UserProfile Student = db.UserProfiles.Single(m => m.UserId == WebSecurity.CurrentUserId);
            return View(Student);
        }
        [MultipleResponseFormats]
        public ActionResult CommentList(int id)
        {
            UserProfile Student = db.UserProfiles.Single(m => m.UserId == WebSecurity.CurrentUserId);
            Topic Topic = Student.MyTopic;
            IEnumerable CommentList = Topic.Comments.Where(m => m.CommentNode ==id).ToList();
            return View(CommentList);
        }
        public ActionResult Upload(int type)
        {
            UserProfile Student = db.UserProfiles.Single(m => m.UserId == WebSecurity.CurrentUserId);
            switch (type)
            {
                case 1:
                    Student.MyTopic.MissionBook = DocumentUpload(Student, Student.MyTopic.MissionBook,"任务书");
                    break;
                case 2:
                    Student.MyTopic.Report = DocumentUpload(Student, Student.MyTopic.Report,"开题报告");
                    break;
                case 3:
                    Student.MyTopic.Thesis = DocumentUpload(Student, Student.MyTopic.Thesis,"毕业论文");
                    break;
                case 4:
                    Student.MyTopic.CommentBook = DocumentUpload(Student, Student.MyTopic.CommentBook,"评议书");
                    break;
            }
            db.SaveChanges();
            return this.Json(new { status=1,LastUploadTime = DateTime.Now.ToString() });
        }
        private Document DocumentUpload(UserProfile Student, Document document, string TypePath="")
        {
            if (document == null) {
                document = db.Documents.Create();
            }
            string Major = Student.Major;
            string Class = Student.Class;
            HttpPostedFileBase f = Request.Files[0];
            string virtualPath = ThesisHelper.UpLoadDoc(f, Major + "/" + Class + "/" + TypePath, Student.UserName + Student.FullName);//以专业/班级/文档类型/学号+姓名 进行存储
            document.Path = virtualPath;
            document.LastUploadTime = DateTime.Now;
            document.Student = Student;
            document.Topic = Student.MyTopic;
            db.SaveChanges();
            return document;
        }
    }
}
