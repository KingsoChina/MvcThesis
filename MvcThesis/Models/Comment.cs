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
    [Table("Comment")]
    public class Comment
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID")]
        public int CommentId { get; set; }

        [Display(Name = "内容"), Required(ErrorMessage="请输入内容")]
        public string Content { get; set; }

        [Display(Name = "评论时间"), Required]
        public DateTime CommentTime { get; set; }
        //0=导师意见，1=系评议意见，2=学院意见

        [Display(Name="评论级别"),Required]
        public int CommentLevel { get; set; }
        //1=选题，2=任务书，3=开题报告，4=毕业论文，5=评议书
        [Display(Name = "评论节点"), Required]
        public int CommentNode { get; set; }
        public virtual UserProfile Student { get; set; }

        public virtual UserProfile Teacher { get; set; }

        public virtual Topic Topic { get; set; }

    }
}
