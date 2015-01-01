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
    [Table("Topic")]
    public class Topic
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID")]
        public int TopicId { get; set; }

        [Display(Name = "标题"), Required(ErrorMessage="请输入标题")]
        [StringLength(100)]
        public string Title { get; set; }

        [Display(Name = "适用班级")]
        [StringLength(50)]
        public string ApplyClass { get; set; }

        [Display(Name = "课题来源")]
        [StringLength(50)]
        public string Source { get; set; }

        [Display(Name = "类型"), Required]
        [StringLength(50)]
        public string Type { get; set; }

        [Display(Name = "课题条件")]
        public string Condition { get; set; }

        [Display(Name = "阐述"),Required(ErrorMessage="请阐述一下课题")]
        public string Elaboration { get; set; }
        
        [DataType(DataType.DateTime)]
        [Display(Name = "添加日期")]
        public DateTime CreateTime { get; set; }

        [Display(Name = "是否学生命题"), Required]
        public bool IsFromStudent { get; set; }

        [Display(Name = "是否已被选"), Required]
        public bool IsChosen { get; set; }

        [Display(Name = "指导老师是否同意"),Required]
        public int IsTeacherAgree { get; set; }
 
        [Display(Name = "助理是否同意")]
        public int IsAssistAgree { get; set; }

        [Display(Name = "院长是否同意")]
        public int IsDeanAgree { get; set; }
        public virtual UserProfile Student { get; set; }

        [Display(Name = "评论/意见")]
        public virtual ICollection<Comment> Comments { get; set; }

        [Display(Name = "任务书")]
        public virtual Document MissionBook { get; set; }

        [Display(Name = "开题报告")]
        public virtual Document Report { get; set; }
        [Display(Name = "毕业论文")]
        public virtual Document Thesis { get; set; }
        [Display(Name = "评议书")]
        public virtual Document CommentBook { get; set; }

        public virtual UserProfile Teacher { get; set; }

        //时间戳，处理并发问题
        [Timestamp]
        public byte[] RowVersion { get; set; }

    }
}