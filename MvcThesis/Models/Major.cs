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
    public class Major
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID")]
        public int MajorId { get; set; }

        [Display(Name = "专业名称")]
        public string Name { get; set; }
    }
}