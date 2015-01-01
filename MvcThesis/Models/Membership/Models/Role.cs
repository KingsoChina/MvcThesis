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
    [Table("webpages_Roles")]
    public class Role
    {
        public Role()
        {
            Members = new List<MvcThesisMembership>();
            PermissionsInRoles = new List<PermissionsInRoles>();
        }

        [Key]
        [Display(Name = "ID")]
        public int RoleId { get; set; }

        [Required(ErrorMessage="请输入名称")]
        [Display(Name = "名称")]
        [StringLength(256)]
        public string RoleName { get; set; }

        public ICollection<MvcThesisMembership> Members { get; set; }

        [ForeignKey("RoleId")]
        public ICollection<PermissionsInRoles> PermissionsInRoles { set; get; }
    }
}
