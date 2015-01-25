using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;

namespace MvcThesis.Controllers
{
    [Authorize(Roles = "教师,院长,系统管理员")]
    [InitializeSimpleMembership]
    public class TecController : Controller
    {
        private MvcThesisMembershipContext db = new MvcThesisMembershipContext();
        //
        // GET: /Tec/

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ScoreGuide(int id)
        {
            Topic topic = db.Topics.SingleOrDefault(m => m.TopicId == id);
            return View(topic);
        }
        [HttpPost]
        public ActionResult ScoreGuide(string UsualScore, string ReviewScore, int TopicId)
        {
            int UScore = Convert.ToInt32(UsualScore);
            int RScore = Convert.ToInt32(ReviewScore);
            if (UScore < 0 || UScore > 100)
                return Json(new { status = 0, msg = "平时成绩分值格式不正确" });
            if (RScore < 0 || RScore > 100)
                return Json(new { status = 0, msg = "评阅成绩分值格式不正确" });
            Topic topic = db.Topics.SingleOrDefault(m => m.TopicId == TopicId);
            if (topic == null) return Json(new { status = 0, msg = "出错" });
            topic.UsualScore = UScore;
            topic.ReviewScore = RScore;
            db.SaveChanges();
            return Json(new { status = 1, msg = "评分成功" });
        }

        [MultipleResponseFormats]
        public ActionResult StuDetail(int id)
        {
            UserProfile Student = db.UserProfiles.SingleOrDefault(m => m.UserId == id);
            return View(Student);
        }
        //确认同意指导申请
        public ActionResult ConfirmChosen(int id)
        {
            Topic topic = db.Topics.SingleOrDefault(m => m.TopicId == id);
            //获取之前被拒绝的课题,删除之
            IList DenyList = db.Topics.Where(m => m.Student.UserId == topic.Student.UserId && m.IsTeacherAgree == -1).ToList();
            if (DenyList.Count > 0)
            {
                foreach (var Deny in DenyList)
                {
                    Topic DenyTopic = Deny as Topic;
                    db.Comments.Remove(db.Comments.Single(m => m.Topic.TopicId == DenyTopic.TopicId));
                    db.Topics.Remove(DenyTopic);
                }
            }
            topic.IsTeacherAgree = 1;
            db.SaveChanges();
            return Json(new { status = 1, msg = "成功确认！等待辅导员同意" });
        }
        [HttpPost]
        //申报课题
        public ActionResult ConfirmTopic(int id)
        {
            Topic topic = db.Topics.SingleOrDefault(m => m.TopicId == id);
            if (WebSecurity.CurrentUserId != topic.Teacher.UserId) return Json(new { status = 0, msg = "系统错误" });
            topic.IsTeacherAgree = 1;
            db.SaveChanges();
            return Json(new { status = 1, msg = "已申报论题" + topic.Title + ",等待上级同意" });
        }
        [MultipleResponseFormats]
        public ActionResult DenyChosen(int id)
        {
            Topic topic = db.Topics.SingleOrDefault(m => m.TopicId == id);
            if (topic.Teacher.UserId != WebSecurity.CurrentUserId) return HttpNotFound();
            ViewData["TopicId"] = id;
            return View();
        }

        //拒绝指导申请
        [HttpPost]
        public ActionResult DenyChosen(Comment comment, int topicid)
        {
            Topic Topic = db.Topics.Single(m => m.TopicId == topicid);
            if (Topic.Teacher.UserId != WebSecurity.CurrentUserId) return HttpNotFound();
            Topic.IsTeacherAgree = -1;

            comment.Topic = Topic;
            comment.Student = Topic.Student;
            comment.Teacher = Topic.Teacher;
            comment.CommentNode = 1;
            comment.CommentTime = DateTime.Now;
            if (!ModelState.IsValid) return Json(new { status = 0, msg = "输入内容不符" });
            Topic.Comments.Add(comment);
            Topic.Teacher = null;
            db.SaveChanges();
            return Json(new { status = 1, msg = "已拒绝其请求" });
        }
        //教师出题列表
        [MultipleResponseFormats]
        public ActionResult TopicList()
        {
            UserProfile techer = db.UserProfiles.SingleOrDefault(m => m.UserId == WebSecurity.CurrentUserId);
            IEnumerable TopicList = db.Topics.Where(m => m.Teacher.UserId == techer.UserId && m.Status == 0).OrderByDescending(m => m.CreateTime).ToList();
            int count = techer.Topics.Count();
            if (count >= techer.MaxGuideNum)
                ViewBag.MaxNum = 1;

            return View(TopicList);
        }

        [MultipleResponseFormats]
        public ActionResult SetTopic()
        {
            ViewData["MajorList"] = ThesisHelper.getMajor();
            return View();
        }

        [HttpPost]
        public ActionResult SetTopic(Topic topic,string[] ApplyClass)
        {
            if (ApplyClass.Length < 1) return Json(new { status = 0, msg = "未选中任何专业" });
            UserProfile Tec = db.UserProfiles.SingleOrDefault(m => m.UserId == WebSecurity.CurrentUserId);
            //判断是否已经超过可指导数量
            if (Tec.Topics.Count > Tec.MaxGuideNum)
                return Json(new { status = 1, msg = "您不能再发布课题了" });
            //判断输入内容是否合法
            if (!ModelState.IsValid) return Json(new { status = 0, msg = "输入数据不符合规则" });
            Topic ExitTopic = db.Topics.SingleOrDefault(m => m.Title == topic.Title);
            //判断是否有重复
            if (ExitTopic == null)
            {
                topic.CreateTime = DateTime.Now;
                topic.Teacher = Tec;//课题关联教师
                topic.ApplyClass = string.Join(",", ApplyClass);
                //topic.IsTeacherAgree = 1;
                Tec.Topics.Add(topic);//教师关联课题
                db.Topics.Add(topic);
                db.SaveChanges();
                return Json(new { status = 1, msg = "发布成功" });
            }
            if (ExitTopic.Status == 1) return Json(new { status = 0, msg = "此论题往年已被使用！" });
            return Json(new { status = 0, msg = "已存在此课题！" });
        }

        [MultipleResponseFormats]
        public ActionResult EditTopic(int id)
        {
            Topic Topic = db.Topics.Single(m => m.TopicId == id);
            ViewData["MajorList"] = ThesisHelper.getMajor();
            ViewData["SelectedMajor"] = Topic.ApplyClass.Split(',');
            return View(Topic);
        }
        [HttpPost]
        public ActionResult EditTopic(Topic topic,string[] ApplyClass)
        {
            if (!ModelState.IsValid) return Json(new { status = 0, msg = "提交数据有误" });
            Topic Topic = db.Topics.Single(m => m.TopicId == topic.TopicId);
            Topic.ApplyClass =string.Join(",",ApplyClass);
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
        public ActionResult Delete(int id)
        {
            db.Topics.Remove(db.Topics.SingleOrDefault(m => m.TopicId == id));
            db.SaveChanges();
            return Json(new { status = 1, msg = "删除成功" });
        }

        [Authorize(Roles = "教师,院长,系统管理员,院长助理")]
        [MultipleResponseFormats]
        public ActionResult TopicDetail(int id)
        {
            Topic topic = db.Topics.SingleOrDefault(m => m.TopicId == id);
            return View(topic);
        }
        [MultipleResponseFormats]
        public ActionResult DocumentDetail(int id)
        {
            Topic Topic = db.Topics.Single(m => m.TopicId == id);
            return View(Topic);
        }
    }
}
