using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using PropertyManage.Domain;
using System.Web.Security;

namespace PropertyManage.Data
{
    public class AppDbContext : DbContext
    {
        // Membership tables
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Profile> Profiles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Maps to the expected many-to-many join table name for roles to users.
            modelBuilder.Entity<User>()
            .HasMany(u => u.Roles)
            .WithMany(r => r.Users)
            .Map(m =>
            {
                m.ToTable("RoleMemberships");
                m.MapLeftKey("UserName");
                m.MapRightKey("RoleName");
            });
        }

    }

    #region Initial data

    // Change the base class as follows if you want to drop and create the database during development:
    //public class DBInitializer : DropCreateDatabaseAlways<AppDbContext>
    //public class DBInitializer : CreateDatabaseIfNotExists<AppDbContext>
    public class DBInitializer : DropCreateDatabaseIfModelChanges<AppDbContext>
    {
        private static void CreateUserWithRole(string username, string password, string email, string rolename, AppDbContext context)
        {
            var status = new MembershipCreateStatus();

            Membership.CreateUser(username, password, email);
            if (status == MembershipCreateStatus.Success)
            {
                // Add the role.
                var user = context.Users.Find(username);
                var adminRole = context.Roles.Find(rolename);
                user.Roles = new List<Role> { adminRole };
            }
        }


        protected override void Seed(AppDbContext context)
        {
            // Create default roles.
            var roles = new List<Role>
                            {
                                new Role {RoleName = "Super Administrator"},
                                new Role {RoleName = "Administrator"},
                                new Role {RoleName = "User"}
                            };

            roles.ForEach(r => context.Roles.Add(r));

            // Create some users.
            CreateUserWithRole("Faruk", "@123456", "faruk@syntechbd.com", "Super Administrator", context);
            CreateUserWithRole("Siddik", "@123456", "siddik@syntechbd.com", "Administrator", context);
            CreateUserWithRole("Rasel", "@123456", "rasel@syntechbd.com", "User", context);
            CreateUserWithRole("Nur", "@123456", "nur@syntechbd.com", "User", context);
            CreateUserWithRole("Asraf", "@123456", "asraf@syntechbd.com", "User", context);
            CreateUserWithRole("sunchoy", "@123456", "sojoy@syntechbd.com", "User", context);


            // will add more testdata soon

        }
    }

    #endregion
}

