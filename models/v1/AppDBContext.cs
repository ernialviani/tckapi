using TicketingApi.Models.v1.Users;  
using TicketingApi.Models.v1.Misc;
using TicketingApi.Models.v1.Tickets;
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
  
        //misc
        public virtual DbSet<App> Apps {get; set;}
        public virtual DbSet<Module> Modules {get; set;}
        public virtual DbSet<Team> Teams {get; set;}
        public virtual DbSet<TeamDetail> TeamDetails {get; set;}
        public virtual DbSet<Media> Medias {get; set;}
        
        //ticketing

        public virtual DbSet<Stat> Stats {get; set;}
        public virtual DbSet<Ticket> Tickets {get; set;}
        public virtual DbSet<TicketAssign> TicketAssigns {get; set;}
        public virtual DbSet<TicketDetail> TicketDetails {get; set;}
     

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
            modelBuilder.Entity<User>().Ignore(u => u.File);
            modelBuilder.Entity<User>().Property(u => u.Image).HasColumnName("image").HasColumnType("nvarchar(150)").IsRequired(false);  
           // modelBuilder.Entity<User>().Ignore(u => u.CreatedAt); //.HasColumnName("created_at").HasColumnType("datetime").IsRequired();  
           // modelBuilder.Entity<User>().Ignore(u => u.UpdateAt); //.HasColumnName("updated_at").HasColumnType("datetime").IsRequired(false);  

            modelBuilder.Entity<User>().Property(u => u.CreatedAt).HasColumnName("created_at").HasColumnType("datetime").IsRequired(false);  
            modelBuilder.Entity<User>().Property(u => u.UpdateAt).HasColumnName("updated_at").HasColumnType("datetime").IsRequired(false);  
  
            var salt =  CryptoUtil.GenerateSalt();;
            modelBuilder.Entity<User>().HasData(
                new { 
                    Id = 1,
                    FirstName = "vicky", 
                    LastName = "Epsylon", 
                    Email = "vicky.indiarto@epsylonhome.com", 
                    Password = CryptoUtil.HashMultiple("programmer3", salt), 
                    Salt=salt,
                    CreatedAt = DateTime.Now
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
            modelBuilder.Entity<Sender>().Property(u => u.CreatedAt).HasColumnName("created_at").HasColumnType("datetime").IsRequired(false);  
            modelBuilder.Entity<Sender>().Property(u => u.UpdatedAt).HasColumnName("updated_at").HasColumnType("datetime").IsRequired(false);  

            modelBuilder.Entity<App>().ToTable("apps");   
            modelBuilder.Entity<App>().HasKey(u => u.Id).HasName("PK_Apps");  
            modelBuilder.Entity<App>().HasIndex(u => u.Name).HasDatabaseName("idx_name");     
            modelBuilder.Entity<App>().Property(u => u.Id).HasColumnName("id").HasColumnType("int").UseMySqlIdentityColumn().IsRequired();  
            modelBuilder.Entity<App>().Property(u => u.Name).HasColumnName("name").HasColumnType("nvarchar(50)").IsRequired();  
            modelBuilder.Entity<App>().Property(u => u.Logo).HasColumnName("logo").HasColumnType("nvarchar(50)").IsRequired(false);  
            modelBuilder.Entity<App>().Property(u => u.Desc).HasColumnName("desc").HasColumnType("nvarchar(50)").IsRequired(false);

            modelBuilder.Entity<App>().HasData(
                new { Id = 1, Name = "SysAd", Desc = "Integrated Advertising System" },
                new { Id = 2, Name = "App2", Desc = "" },
                new { Id = 3, Name = "App3", Desc = "" },
                new { Id = 4, Name = "APP4", Desc = "" }
            );
                  
            modelBuilder.Entity<Module>().ToTable("modules");   
            modelBuilder.Entity<Module>().HasKey(u => u.Id).HasName("PK_Modules");  
            modelBuilder.Entity<Module>().HasIndex(u => u.Name).HasDatabaseName("idx_name");     
            modelBuilder.Entity<Module>().Property(u => u.Id).HasColumnName("id").HasColumnType("int").UseMySqlIdentityColumn().IsRequired();  
            modelBuilder.Entity<Module>().Property(u => u.Name).HasColumnName("name").HasColumnType("nvarchar(50)").IsRequired();   
            modelBuilder.Entity<Module>().Property(u => u.AppId).HasColumnName("app_id").HasColumnType("int").IsRequired();
            modelBuilder.Entity<Module>().Property(u => u.Desc).HasColumnName("desc").HasColumnType("nvarchar(150)").IsRequired(false);

            modelBuilder.Entity<Module>().HasData(
                new { Id = 1, Name = "Media",      AppId=1, Desc = "" },
                new { Id = 2, Name = "Production", AppId=1, Desc = "" },
                new { Id = 3, Name = "Finance",    AppId=1, Desc = "" },
                new { Id = 4, Name = "Others",     AppId=1, Desc = "" }
            );

            modelBuilder.Entity<Team>().ToTable("teams");   
            modelBuilder.Entity<Team>().HasKey(u => u.Id).HasName("PK_Teams");  
            modelBuilder.Entity<Team>().HasIndex(u => u.Name).HasDatabaseName("idx_name");     
            modelBuilder.Entity<Team>().Property(u => u.Id).HasColumnName("id").HasColumnType("int").UseMySqlIdentityColumn().IsRequired();  
            modelBuilder.Entity<Team>().Property(u => u.Name).HasColumnName("name").HasColumnType("nvarchar(50)").IsRequired();   
            modelBuilder.Entity<Team>().Property(u => u.Desc).HasColumnName("desc").HasColumnType("nvarchar(150)").IsRequired(false);  
            modelBuilder.Entity<Team>().Property(u => u.ManagerId).HasColumnName("manager_id").HasColumnType("int").IsRequired();   

            modelBuilder.Entity<Team>().HasData(
                new { Id = 1, Name = "TEAM CAP", ManagerId=1, Desc = "" }
            );

            modelBuilder.Entity<TeamDetail>().ToTable("team_details");   
            modelBuilder.Entity<TeamDetail>().HasKey(u => u.Id).HasName("PK_Teams_details");    
            modelBuilder.Entity<TeamDetail>().Property(u => u.Id).HasColumnName("id").HasColumnType("int").UseMySqlIdentityColumn().IsRequired();  
            modelBuilder.Entity<TeamDetail>().Property(u => u.TeamId).HasColumnName("team_id").HasColumnType("int").IsRequired().HasDefaultValue(0);  
            modelBuilder.Entity<TeamDetail>().Property(u => u.UserId).HasColumnName("user_id").HasColumnType("int").IsRequired().HasDefaultValue(0);  
           
            // modelBuilder.Entity<TeamDetail>().HasData(
            //     new { Id = 1, TeamId = 1, UserId=2}
            // );

            modelBuilder.Entity<Stat>().ToTable("stats");
            modelBuilder.Entity<Stat>().HasKey(u => u.Id).HasName("PK_Stat");  
            modelBuilder.Entity<Stat>().HasIndex(u => u.Name).HasDatabaseName("idx_name"); 
            modelBuilder.Entity<Stat>().Property(u => u.Id).HasColumnName("id").HasColumnType("int").UseMySqlIdentityColumn().IsRequired();  
            modelBuilder.Entity<Stat>().Property(u => u.Name).HasColumnName("name").HasColumnType("nvarchar(50)").IsRequired();   
            modelBuilder.Entity<Stat>().Property(u => u.Color).HasColumnName("color").HasColumnType("nvarchar(50)").IsRequired(false);   
            modelBuilder.Entity<Stat>().Property(u => u.Desc).HasColumnName("desc").HasColumnType("nvarchar(50)").IsRequired(false);

             modelBuilder.Entity<Stat>().HasData(
                new { Id = 1, Name = "New", Color= "Green", Desc = "" },
                new { Id = 2, Name = "Open", Color = "Orange", Desc = "" },
                new { Id = 3, Name = "In Progress", Color = "Red", Desc = "" },
                new { Id = 4, Name = "Resolved", Color = "Blue", Desc = "" },
                new { Id = 5, Name = "Rejected", Color = "Grey", Desc = "" }
            );

            modelBuilder.Entity<Ticket>().ToTable("tickets");
            modelBuilder.Entity<Ticket>().HasKey(u => u.Id).HasName("PK_Ticket");  
            modelBuilder.Entity<Ticket>().HasIndex(u => u.TicketNumber).HasDatabaseName("idx_TicketNumber"); 
            modelBuilder.Entity<Ticket>().Property(u => u.Id).HasColumnName("id").HasColumnType("int").UseMySqlIdentityColumn().IsRequired();  
            modelBuilder.Entity<Ticket>().Property(u => u.TicketNumber).HasColumnName("ticket_number").HasColumnType("nvarchar(50)").IsRequired();   
            modelBuilder.Entity<Ticket>().Property(u => u.Subject).HasColumnName("supject").HasColumnType("text").IsRequired();   
            modelBuilder.Entity<Ticket>().Property(u => u.Comment).HasColumnName("comment").HasColumnType("text").IsRequired();
            modelBuilder.Entity<Ticket>().Property(u => u.AppId).HasColumnName("app_id").HasColumnType("int").IsRequired();
            modelBuilder.Entity<Ticket>().Property(u => u.ModuleId).HasColumnName("module_id").HasColumnType("int").IsRequired();
            modelBuilder.Entity<Ticket>().Property(u => u.SenderMail).HasColumnName("sender_mail").HasColumnType("nvarchar(50)").IsRequired();
            modelBuilder.Entity<Ticket>().Property(u => u.StatId).HasColumnName("stat_id").HasColumnType("int").IsRequired();
            modelBuilder.Entity<Ticket>().Property(u => u.SolvedBy).HasColumnName("solved_by").HasColumnType("nvarchar(50)").IsRequired(false);
            modelBuilder.Entity<Ticket>().Property(u => u.RejectedBy).HasColumnName("rejected_by").HasColumnType("nvarchar(50)").IsRequired(false);
            modelBuilder.Entity<Ticket>().Property(u => u.RejectedReason).HasColumnName("rejected_reason").HasColumnType("text").IsRequired(false);
            modelBuilder.Entity<Ticket>().Property(u => u.CreatedAt).HasColumnName("created_at").HasColumnType("datetime").IsRequired(false);
            modelBuilder.Entity<Ticket>().Property(u => u.UpdatedAt).HasColumnName("updated_at").HasColumnType("datetime").IsRequired(false);

            
             modelBuilder.Entity<Ticket>().HasData(
                new { Id = 1, TicketNumber = "180620211", Subject= "Subject 1", Comment = "lorem ipsum dolor shit nyolibay knoper low", AppId = 1, ModuleId = 1, SenderMail = "vickynewonline@gmail.com", StatId=1, SolvedBy="", RejectedBy="", RejectedReason="", CreatedAt = DateTime.Now, UpdatedAt=DateTime.Now   },
                new { Id = 2, TicketNumber = "180620212", Subject= "Subject 2", Comment = "asdhjkahsdjas jasshdjkajksdas jashdjkahsjkd oashdasihsjskaslnsk", AppId = 1, ModuleId = 1, SenderMail = "vickynewonline@gmail.com", StatId=1, SolvedBy="", RejectedBy="", RejectedReason="", CreatedAt = DateTime.Now, UpdatedAt=DateTime.Now   },
                new { Id = 3, TicketNumber = "180620213", Subject= "Subject 3", Comment = "ksknnina  lasklk  klsnklna ksaiopoells;mlauw klnskoiskel aksnkadia; mkaskks ", AppId = 1, ModuleId = 1, SenderMail = "vickynewonline@gmail.com", StatId=1, SolvedBy="", RejectedBy="", RejectedReason="", CreatedAt = DateTime.Now, UpdatedAt=DateTime.Now   }
            );

            modelBuilder.Entity<TicketDetail>().ToTable("ticket_details");
            modelBuilder.Entity<TicketDetail>().HasKey(u => u.Id).HasName("PK_Ticket_details");  
            modelBuilder.Entity<TicketDetail>().Property(u => u.Id).HasColumnName("id").HasColumnType("int").UseMySqlIdentityColumn().IsRequired();  
            modelBuilder.Entity<TicketDetail>().Property(u => u.TicketId).HasColumnName("ticket_id").HasColumnType("int").IsRequired();   
            modelBuilder.Entity<TicketDetail>().Property(u => u.UserMail).HasColumnName("user_mail").HasColumnType("nvarchar(50)").IsRequired(false);   
            modelBuilder.Entity<TicketDetail>().Property(u => u.Comment).HasColumnName("comment").HasColumnType("text").IsRequired();
            modelBuilder.Entity<TicketDetail>().Property(u => u.Flag).HasColumnName("flag").HasColumnType("nvarchar(10)").IsRequired();
            modelBuilder.Entity<TicketDetail>().Property(u => u.CreatedAt).HasColumnName("created_at").HasColumnType("datetime").IsRequired(false);
            modelBuilder.Entity<TicketDetail>().Property(u => u.UpdatedAt).HasColumnName("updated_at").HasColumnType("datetime").IsRequired(false);

            // modelBuilder.Entity<TicketDetail>().HasData(
            //     new { Id = 1, TicketId = 1, UserMail= "vicky.indiarto@epsylonhome.com", Comment = "lorem ipsum dolor shit nyolibay knoper low", flag = false, CreatedAt = DateTime.Now, UpdatedAt=DateTime.Now   },
            //     new { Id = 2, TicketId = 1, UserMail= "", Comment = "asdhjkahsdjas jasshdjkajksdas jashdjkahsjkd oashdasihsjskaslnsk", flag = false, CreatedAt = DateTime.Now, UpdatedAt=DateTime.Now   },
            //     new { Id = 3, TicketId = 1, UserMail= "vicky.indiarto@epsylonhome.com", Comment = "ksknnina  lasklk  klsnklna ksaiopoells;mlauw klnskoiskel aksnkadia; mkaskks ", flag = false, CreatedAt = DateTime.Now, UpdatedAt=DateTime.Now   }
            // );

            modelBuilder.Entity<TicketAssign>().ToTable("ticket_assign");
            modelBuilder.Entity<TicketAssign>().HasKey(u => u.Id).HasName("PK_Ticket_assign");  
            modelBuilder.Entity<TicketAssign>().Property(u => u.Id).HasColumnName("id").HasColumnType("int").UseMySqlIdentityColumn().IsRequired();  
            modelBuilder.Entity<TicketAssign>().Property(u => u.TicketId).HasColumnName("ticket_id").HasColumnType("int").IsRequired();   
            modelBuilder.Entity<TicketAssign>().Property(u => u.TeamId).HasColumnName("team_id").HasColumnType("int").IsRequired(false);   
            modelBuilder.Entity<TicketAssign>().Property(u => u.TeamAt).HasColumnName("team_at").HasColumnType("datetime").IsRequired(false);
            modelBuilder.Entity<TicketAssign>().Property(u => u.UserId).HasColumnName("user_id").HasColumnType("int").IsRequired(false);
            modelBuilder.Entity<TicketAssign>().Property(u => u.UserAt).HasColumnName("user_at").HasColumnType("datetime").IsRequired(false);

            modelBuilder.Entity<TicketAssign>().HasData(
                new { Id = 1, TicketId = 1, TeamId = 1, TeamAt = DateTime.Now }
            );



       
        }  
    }  
}  