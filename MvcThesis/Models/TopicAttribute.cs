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
    [Table("TopicAttribute")]
    public class TopicAttribute
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.None)]
        [Display(Name = "ID")]
        public int SourceId { get; set; }

        [Display(Name = "设置项")]
        public int AttributeType { get; set; }

        [Display(Name = "设置值")]
        public string AttributeValue { get; set; }


    }
}