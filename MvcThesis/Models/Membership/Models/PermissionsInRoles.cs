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
    [Table("webpages_PermissionsInRoles")]
    public class PermissionsInRoles
    {
        [Key]
        public int Id { get; set; }

        public int RoleId { get; set; }
        public int PermissionId { get; set; }

        [Column("RoleId"), InverseProperty("PermissionsInRoles")]
        public Role Role { get; set; }

        [Column("PermissionId"), InverseProperty("PermissionsInRoles")]
        public Permission Permission { get; set; }
    }
}
