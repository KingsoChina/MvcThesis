using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using WebMatrix.WebData;
using System.Web.Security;

namespace MvcThesis
{
    public class ThesisInitializer : DropCreateDatabaseIfModelChanges<MvcThesisMembershipContext>
    {
        protected override void Seed(MvcThesisMembershipContext context)
        {
            var Settings = new List<Setting>{
                new Setting{Title = "公告", Content="欢迎使用毕业设计（论文）管理系统"},
                new Setting{Title = "选题开始时间",Content=DateTime.Now.ToString()},
                new Setting{Title = "选题结束时间",Content=DateTime.Now.ToString()},
                new Setting{Title = "任务书开始时间",Content=DateTime.Now.ToString()},
                new Setting{Title = "任务书结束时间",Content=DateTime.Now.ToString()},
                new Setting{Title = "开题报告开始时间",Content=DateTime.Now.ToString()},
                new Setting{Title = "开题报告结束时间",Content=DateTime.Now.ToString()},
                new Setting{Title = "毕业论文开始时间",Content=DateTime.Now.ToString()},
                new Setting{Title = "毕业论文结束时间",Content=DateTime.Now.ToString()},
                new Setting{Title = "评议书开始时间",Content=DateTime.Now.ToString()},
                new Setting{Title = "评议书结束时间",Content=DateTime.Now.ToString()},
            };
            Settings.ForEach(e =>context.Settings.Add(e));
            context.SaveChanges();
            var RolesList = new List<Role>
            {
                new Role{RoleName="系统管理员"},
                new Role{RoleName="院长"},
                new Role{RoleName="院长助理"},
                new Role{RoleName="教师"},
                new Role{RoleName="学生"}
            };
            RolesList.ForEach(e => context.Roles.Add(e));
            context.SaveChanges();
        }
    }
}