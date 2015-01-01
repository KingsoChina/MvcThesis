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
    [Authorize(Roles="教师")]
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
            IList DenyList =  db.Topics.Where(m =>m.Student.UserId==topic.Student.UserId && m.IsTeacherAgree==-1).ToList();
            if (DenyList.Count > 0)
            {
                foreach(var Deny in DenyList)
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
        public ActionResult DenyChosen(Comment comment,int topicid)
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
          IEnumerable TopicList = db.Topics.Where(m => m.Teacher.UserId == techer.UserId).ToList();
           int count = techer.Topics.Count();
          if ( count > techer.MaxGuideNum)
            ViewBag.MaxNum = 1;
            return View(TopicList);
        }

        [MultipleResponseFormats]
        public ActionResult SetTopic()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SetTopic(Topic topic) 
        {
            UserProfile Tec = db.UserProfiles.SingleOrDefault(m => m.UserId == WebSecurity.CurrentUserId);
            //判断是否已经超过可指导数量
            if(Tec.Topics.Count > Tec.MaxGuideNum)
                return Json(new{status=1,msg="您不能再发布课题了"});
            //判断输入内容是否合法
            if (!ModelState.IsValid) return Json(new { status = 0, msg = "输入数据不符合规则" });
            //判断是否有重复
            if (db.Topics.SingleOrDefault(m => m.Title == topic.Title && m.IsTeacherAgree!=-1) == null)
            {
                topic.CreateTime = DateTime.Now;
                topic.Teacher = Tec;//课题关联教师
                topic.IsTeacherAgree = 1;
                Tec.Topics.Add(topic);//教师关联课题
                db.Topics.Add(topic);
                db.SaveChanges();
                return Json(new { status = 1, msg = "发布成功" });
            }
            return Json(new { status = 0, msg = "已存在此课题" });
        }

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
