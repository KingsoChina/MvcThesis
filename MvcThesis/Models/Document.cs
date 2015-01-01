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
    [Table("Document")]
    public class Document
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID")]
        public int DocumentId { get; set; }

        public DateTime LastUploadTime { get; set; }
        public Topic Topic { get; set; }
        
        public string Path { get; set; }
        public UserProfile Student { get; set; }

    }
}