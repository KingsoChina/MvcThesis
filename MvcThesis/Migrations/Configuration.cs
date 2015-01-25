namespace MvcThesis.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<MvcThesis.MvcThesisMembershipContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(MvcThesis.MvcThesisMembershipContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            context.Settings.AddOrUpdate(p => p.Title,
                new Setting
                {
                    Title = "ָ����ʦ����ʱ��",
                    Content = DateTime.Now.ToString()
                },
                new Setting { Title = "������ʦ����ʱ��", Content = DateTime.Now.ToString() }
                );
            context.Majors.AddOrUpdate(p => p.Name,
                new Major { Name = "��Ϣ������ѧ" },
                new Major { Name = "Ӧ����ѧ" },
                new Major { Name = "ͳ��ѧ"}
             );
        }
    }
}
