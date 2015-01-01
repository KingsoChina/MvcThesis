using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MvcThesis
{
    public class UserViewModel
    {
        [Key]
        [Display(Name = "ID")]
        public int User_ID { set; get; }

        [Required(ErrorMessage = "请填写账号")]
        [Display(Name = "账号（学号/工号）")]
        public string User_Name { set; get; }

        [Required(ErrorMessage = "请填写密码")]
        [Display(Name = "密码")]
        public string Password { set; get; }

        [Required(ErrorMessage = "请填写姓名")]
        [Display(Name = "姓名")]
        public string Full_Name { set; get; }

        [Required(ErrorMessage = "请填写电子邮箱")]
        [Display(Name = "邮箱")]
        public string Email { set; get; }

        [Display(Name = "学院")]
        [StringLength(50)]
        public string Institute { set; get; }

        [Display(Name = "班级")]
        [StringLength(50)]
        public string Class { get; set; }

        [Display(Name = "手机")]
        [StringLength(11)]
        public string Phone { get; set; }

        [Display(Name = "短号")]
        [StringLength(10)]
        public string ShortPhone { get; set; }

        [Display(Name = "职称")]
        [StringLength(250)]
        public string JobTitle { get; set; }

        [Display(Name = "指导人数")]
        public int MaxGuideNum { get; set; }

        [Display(Name = "角色")]
        public string Role_Name { set; get; }

        [Required(ErrorMessage = "请填写新密码")]
        [Display(Name = "新密码")]
        public string NewPassword { set; get; }
    }
}
