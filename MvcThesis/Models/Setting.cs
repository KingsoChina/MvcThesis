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
    [Table("Setting")]
    public class Setting
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID")]
        public int SettingId { get; set; }

        [Display(Name = "标题"), Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Display(Name = "内容"), Required]
        public string Content { get; set; }
    }
}