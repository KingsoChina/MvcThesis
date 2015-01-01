using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcThesis
{
    [Table("UserProfile")]
    public class UserProfile
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID")]
        public int UserId { get; set; }

        [Display(Name = "用户名"),Required]
        [StringLength(50)]
        public string UserName { get; set; }

        [Display(Name = "姓名")]
        [StringLength(15)]
        public string FullName { get; set; }

        [Display(Name = "学院")]
        [StringLength(50)]
        public string Institute { get; set; }

        [Display(Name = "专业")]
        [StringLength(50)]
        public string Major { get; set; }

        [Display(Name = "班级")]
        [StringLength(50)]
        public string Class { get; set; }

        [Display(Name = "手机")]
        [StringLength(11)]
        public string Phone { get; set; }

        [Display(Name = "短号")]
        [StringLength(10)]
        public string ShortPhone { get; set; }

        [Display(Name = "邮件地址")]
        [StringLength(250)]
        public string Email { get; set; }

        [Display(Name = "QQ")]
        [StringLength(20)]
        public string QQ { get; set; }

        [Display(Name = "职称")]
        [StringLength(250)]
        public string JobTitle { get; set; }

        //[Display(Name = "学位")]
        //[StringLength(250)]
        //public string Degree { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Display(Name = "指导人数")]
        public int MaxGuideNum { get; set; }

        [Display(Name = "研究方向")]
        public string Direction { get; set; }
        //学生一个
        public virtual Topic MyTopic { get; set; }
        //教师多个
        [Display(Name = "课题")]
        public virtual ICollection<Topic> Topics { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }

    }
}
