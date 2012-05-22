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

        // Setting tables
        public DbSet<UnitType> UnitTypes { get; set; }
        public DbSet<UnitValue> UnitValues { get; set; }

        // Project tables
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectBlock> ProjectBlocks { get; set; }
        public DbSet<BlockPlot> BlockPlots { get; set; }

        // Purchase tables
        public DbSet<Category> Categorys { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }

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

            //one to one relationship with user mapping
            modelBuilder.Entity<User>()
            .HasOptional(u => u.Profile)
            .WithMany()
            .HasForeignKey(u => u.ProfileId);

            //one to one relationship with profile mapping
            modelBuilder.Entity<Profile>()
            .HasRequired(u => u.User)
            .WithMany()
            .HasForeignKey(u => u.UserName);

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
                                new Role {RoleName = "Admin"},
                                new Role {RoleName = "Account"},
                                new Role {RoleName = "Client"},
                                new Role {RoleName = "Manage"},
                                new Role {RoleName = "Purchase"},
                                new Role {RoleName = "Sale"},
                                new Role {RoleName = "Supliare"},
                                new Role {RoleName = "User"}
                            };

            roles.ForEach(r => context.Roles.Add(r));

            // Create some users.
            CreateUserWithRole("Faruk", "@123456", "faruk@syntechbd.com", "Admin", context);
            CreateUserWithRole("Ahmmed", "@123456", "ahmmed@syntechbd.com", "Account", context);
            CreateUserWithRole("Rasel", "@123456", "rasel@syntechbd.com", "User", context);
            CreateUserWithRole("Nur", "@123456", "nur@syntechbd.com", "Sale", context);
            CreateUserWithRole("Asraf", "@123456", "asraf@syntechbd.com", "Supliare", context);
            CreateUserWithRole("sunchoy", "@123456", "sunchoy@syntechbd.com", "Client", context);


            // will add more testdata soon

        }
    }

    #endregion
}

