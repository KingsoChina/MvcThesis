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
    public class MvcThesisMembershipContext : DbContext
    {
        public MvcThesisMembershipContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<MvcThesisMembership> MvcThesisMemberships { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<PermissionsInRoles> PermissionsInRoles { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Major> Majors { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MvcThesisMembership>()
              .HasMany<Role>(r => r.Roles)
              .WithMany(u => u.Members)
              .Map(m =>
              {
                  m.ToTable("webpages_UsersInRoles");
                  m.MapLeftKey("UserId");
                  m.MapRightKey("RoleId");
              });
        }
    }
}
