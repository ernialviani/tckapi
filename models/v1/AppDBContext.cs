using TicketingApi.Models.v1.Users;  
using Microsoft.EntityFrameworkCore;  
using System;  
using System.Collections.Generic;  
using System.Linq;  
using System.Threading.Tasks;  
using TicketingApi.Utils;
  
namespace TicketingApi.DBContexts  
{  
    public class AppDBContext : DbContext  
    {  
        public DbSet<User> Users { get; set; } 
        public DbSet<Sender> Senders { get; set; }  
        public DbSet<Role> Roles { get; set; } 
        public DbSet<UserRole> UserRoles { get; set; } 
        public DbSet<Department> Departments { get; set; }    
        public DbSet<UserDept> UserDeprts { get; set; } 
  
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)  
        {   
        }  
        // protected override void OnConfiguring(DbContextOptions optionsBuilder){

        // }

  
        protected override void OnModelCreating(ModelBuilder modelBuilder)  
        {  
            // Use Fluent API to configure  
  
            // Map entities to tables  
            modelBuilder.Entity<User>().ToTable("users");  
            // Configure Primary Keys  
            modelBuilder.Entity<User>().HasKey(u => u.Id).HasName("PK_Users");  
            // Configure indexes  
            modelBuilder.Entity<User>().HasIndex(u => u.Email).HasDatabaseName("idx_email");     
            // Configure columns   
            modelBuilder.Entity<User>().Property(u => u.Id).HasColumnName("id").HasColumnType("int").UseMySqlIdentityColumn().IsRequired();  
            modelBuilder.Entity<User>().Property(u => u.FirstName).HasColumnName("first_name").HasColumnType("nvarchar(50)").IsRequired();  
            modelBuilder.Entity<User>().Property(u => u.LastName).HasColumnName("last_name").HasColumnType("nvarchar(50)").IsRequired();  
            modelBuilder.Entity<User>().Property(u => u.Email).HasColumnName("email").HasColumnType("nvarchar(100)").IsRequired();  
            modelBuilder.Entity<User>().Property(u => u.Password).HasColumnName("password").HasColumnType("nvarchar(255)").IsRequired();  
            modelBuilder.Entity<User>().Property(u => u.Salt).HasColumnName("salt").HasColumnType("nvarchar(36)").IsRequired();  
            modelBuilder.Entity<User>().Property(u => u.Image).HasColumnName("image").HasColumnType("nvarchar(50)").IsRequired(false);  
            modelBuilder.Entity<User>().Property(u => u.CreationDateTime).HasColumnName("created_at").HasColumnType("timestamp").IsRequired();  
            modelBuilder.Entity<User>().Property(u => u.LastUpdateDateTime).HasColumnName("updated_at").HasColumnType("timestamp").IsRequired(false);  
  
            var salt =  CryptoUtil.GenerateSalt();;
            modelBuilder.Entity<User>().HasData(
                new { 
                    Id = 1,
                    FirstName = "vicky", 
                    LastName = "Epsylon", 
                    Email = "vicky.indiarto@epsylonhome.com", 
                    Password = CryptoUtil.HashMultiple("programmer3", salt), 
                    Salt=salt,
                    CreationDateTime = DateTime.Now
                    }
            );

            // Configure relationships  
            // modelBuilder.Entity<User>().HasOne<UserGroup>().WithMany().HasPrincipalKey(ug => ug.Id).HasForeignKey(u => u.UserGroupId).OnDelete(DeleteBehavior.NoAction).HasConstraintName("FK_Users_UserGroups");  
       
            modelBuilder.Entity<Role>().ToTable("roles");  
            modelBuilder.Entity<Role>().HasKey(u => u.Id).HasName("PK_Roles");  
            modelBuilder.Entity<Role>().HasIndex(u => u.Name).HasDatabaseName("idx_name");     
            modelBuilder.Entity<Role>().Property(u => u.Id).HasColumnName("id").HasColumnType("int").UseMySqlIdentityColumn().IsRequired();  
            modelBuilder.Entity<Role>().Property(u => u.Name).HasColumnName("name").HasColumnType("nvarchar(50)").IsRequired();  
            modelBuilder.Entity<Role>().Property(u => u.Desc).HasColumnName("desc").HasColumnType("nvarchar(50)").IsRequired(false);

            modelBuilder.Entity<Role>().HasData(
                new { Id = 1, Name = "SuperAdmin", Desc = "" },
                new { Id = 2, Name = "Leader", Desc = "" },
                new { Id = 3, Name = "Manager", Desc = "" },
                new { Id = 4, Name = "User", Desc = "" }
            );
           
            modelBuilder.Entity<Department>().ToTable("departments");   
            modelBuilder.Entity<Department>().HasKey(u => u.Id).HasName("PK_Depatments");  
            modelBuilder.Entity<Department>().HasIndex(u => u.Name).HasDatabaseName("idx_name");     
            modelBuilder.Entity<Department>().Property(u => u.Id).HasColumnName("id").HasColumnType("int").UseMySqlIdentityColumn().IsRequired();  
            modelBuilder.Entity<Department>().Property(u => u.Name).HasColumnName("name").HasColumnType("nvarchar(50)").IsRequired();  
            modelBuilder.Entity<Department>().Property(u => u.Desc).HasColumnName("desc").HasColumnType("nvarchar(50)").IsRequired(false);

            modelBuilder.Entity<Department>().HasData(
                new { Id = 1, Name = "Management", Desc = "" },
                new { Id = 2, Name = "CS", Desc = "" },
                new { Id = 3, Name = "Programmer", Desc = "" },
                new { Id = 4, Name = "Other", Desc = "" }
            );
                  
            modelBuilder.Entity<UserRole>().ToTable("user_roles");   
            modelBuilder.Entity<UserRole>().HasKey(u => u.Id).HasName("PK_UserRoles");  
            modelBuilder.Entity<UserRole>().HasIndex(u => u.UserId).HasDatabaseName("idx_userid");     
            modelBuilder.Entity<UserRole>().Property(u => u.Id).HasColumnName("id").HasColumnType("int").UseMySqlIdentityColumn().IsRequired();  
            modelBuilder.Entity<UserRole>().Property(u => u.UserId).HasColumnName("user_id").HasColumnType("int").IsRequired();  
            modelBuilder.Entity<UserRole>().Property(u => u.RoleId).HasColumnName("role_id").HasColumnType("int").IsRequired();  

             modelBuilder.Entity<UserRole>().HasData(
                new { Id = 1, UserId = 1, RoleId = 1 },
                new { Id = 2, UserId = 1, RoleId = 4 }
            );


            modelBuilder.Entity<UserDept>().ToTable("user_depatments");   
            modelBuilder.Entity<UserDept>().HasKey(u => u.Id).HasName("PK_UserDepts");  
            modelBuilder.Entity<UserDept>().HasIndex(u => u.UserId).HasDatabaseName("idx_userid");     
            modelBuilder.Entity<UserDept>().Property(u => u.Id).HasColumnName("id").HasColumnType("int").UseMySqlIdentityColumn().IsRequired();  
            modelBuilder.Entity<UserDept>().Property(u => u.UserId).HasColumnName("user_id").HasColumnType("int").IsRequired();  
            modelBuilder.Entity<UserDept>().Property(u => u.DepartmentId).HasColumnName("dept_id").HasColumnType("int").IsRequired();  

            modelBuilder.Entity<UserDept>().HasData(
                new { Id = 1, UserId = 1, DepartmentId = 1 },
                new { Id = 2, UserId = 1, DepartmentId = 3 }
            );

            modelBuilder.Entity<Sender>().ToTable("senders");  
            modelBuilder.Entity<Sender>().HasKey(u => u.Id).HasName("PK_Sender");  
            modelBuilder.Entity<Sender>().HasIndex(u => u.Email).HasDatabaseName("idx_sender");     
            modelBuilder.Entity<Sender>().Property(u => u.Id).HasColumnName("id").HasColumnType("int").UseMySqlIdentityColumn().IsRequired();  
            modelBuilder.Entity<Sender>().Property(u => u.FirstName).HasColumnName("first_name").HasColumnType("nvarchar(50)").IsRequired();  
            modelBuilder.Entity<Sender>().Property(u => u.LastName).HasColumnName("last_name").HasColumnType("nvarchar(50)").IsRequired();  
            modelBuilder.Entity<Sender>().Property(u => u.Email).HasColumnName("email").HasColumnType("nvarchar(100)").IsRequired();  
            modelBuilder.Entity<Sender>().Property(u => u.Password).HasColumnName("password").HasColumnType("nvarchar(255)").IsRequired();  
            modelBuilder.Entity<Sender>().Property(u => u.Salt).HasColumnName("salt").HasColumnType("nvarchar(36)").IsRequired();  
            modelBuilder.Entity<Sender>().Property(u => u.Image).HasColumnName("image").HasColumnType("nvarchar(50)").IsRequired(false);  
            modelBuilder.Entity<Sender>().Property(u => u.CreationDateTime).HasColumnName("created_at").HasColumnType("timestamp").IsRequired();  
            modelBuilder.Entity<Sender>().Property(u => u.LastUpdateDateTime).HasColumnName("updated_at").HasColumnType("timestamp").IsRequired(false);  
       
        }  
    }  
}  