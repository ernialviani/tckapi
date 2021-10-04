using TicketingApi.Models.v1.Users;  
using TicketingApi.Models.v1.Misc;
using TicketingApi.Models.v1.Tickets;
using TicketingApi.Models.v1.CLogs;
using TicketingApi.Models.v1.Notifications;
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
        public DbSet<UserDept> UserDepts { get; set; } 
  
        //misc
        public  DbSet<App> Apps {get; set;}
        public  DbSet<Module> Modules {get; set;}
        public  DbSet<Team> Teams {get; set;}
        public  DbSet<TeamMember> TeamMembers {get; set;}
        public  DbSet<Media> Medias {get; set;}
        public DbSet<KBase> KBases {get; set;} 

        public DbSet<CLog> CLogs {get; set;} 
        public DbSet<CLogDetail> CLogDetails {get; set;} 
        public DbSet<CLogType> ClogTypes {get; set;} 


        
        public DbSet<Faq> Faqs {get; set;} 
        public DbSet<ClientGroup> ClientGroups {get; set;} 
        public DbSet<ClientDetail> ClientDetails {get; set;} 
        public DbSet<Verification> Verification {get; set;} 
        
        //ticketing
        public DbSet<Stat> Stats {get; set;}
        public DbSet<Ticket> Tickets {get; set;}
        public DbSet<TicketAssign> TicketAssigns {get; set;}
        public DbSet<TicketDetail> TicketDetails {get; set;}

        //NOTIF
        public DbSet<Notif> Notifs {get; set;}
        public DbSet<NotifRegister> NotifRegisters {get; set;}



     

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
            modelBuilder.Entity<User>().Property(u => u.Color).HasColumnName("color").HasColumnType("nvarchar(50)").IsRequired(false);  
            modelBuilder.Entity<User>().Property(u => u.Deleted).HasColumnName("deleted").HasColumnType("tinyint(1)").HasDefaultValue(0).IsRequired();
            modelBuilder.Entity<User>().Property(u => u.CreatedAt).HasColumnName("created_at").HasColumnType("datetime").IsRequired(false);  
            modelBuilder.Entity<User>().Property(u => u.UpdatedAt).HasColumnName("updated_at").HasColumnType("datetime").IsRequired(false);  
  
            var salt =  "a38f49ef-23ef-424b-81ed-7e55cc32e512";
            modelBuilder.Entity<User>().HasData(
                // admin programmer user
                new { Id = 1, FirstName = "Admin", LastName = "Super", Email = "adminsuper@epsylonhome.com", Password = "407CEFA8AD88C93B16D48CB8303F0585C2F78B93679C6AD301C536DA16A9D1523E20E4AF705AF4EB5A843BB8076B60494B7665C11A18412265948CA475E584E9", Salt=salt, Image="Users/adminsuper.jpg", CreatedAt =  new DateTime(2021, 8, 1) }
               
                // Leader CS
                // new { Id = 2, FirstName = "Leader", LastName = "CS", Email = "teamleadcs@epsylonhome.com", Password = CryptoUtil.HashMultiple("teamleadcs", salt), Salt=salt, Image="Users/teamleadcs.jpg", CreatedAt =  new DateTime(2021, 8, 1) },
                // // Leader Prg           
                // new { Id = 3, FirstName = "Leader", LastName = "DEV", Email = "teamleaddev@epsylonhome.com", Password = CryptoUtil.HashMultiple("teamleaddev", salt), Salt=salt,  Image="Users/teamleaddev.jpg", CreatedAt =  new DateTime(2021, 8, 1) },

                // // Manager CS
                // new { Id = 4, FirstName = "Manager", LastName = "CS", Email = "managercs@epsylonhome.com", Password = CryptoUtil.HashMultiple("managercs", salt), Salt=salt, Image="Users/managercs.jpg", CreatedAt =  new DateTime(2021, 8, 1) },              
                // // Manager Prg
                // new { Id = 5, FirstName = "Manager", LastName = "DEV", Email = "managerdev@epsylonhome.com", Password = CryptoUtil.HashMultiple("managerdev", salt), Salt=salt, Image="Users/managerdev.jpg", CreatedAt =  new DateTime(2021, 8, 1) },

                // //user cs 1
                // new { Id = 6, FirstName = "AUser", LastName = "CS1", Email = "ausercs1@epsylonhome.com", Password = CryptoUtil.HashMultiple("ausercs1", salt), Salt=salt, Color="#32a852",  CreatedAt =  new DateTime(2021, 8, 1) },
                // //user cs2
                // new { Id = 7, FirstName = "BUser", LastName = "DEV1", Email = "buserdev1@epsylonhome.com", Password = CryptoUtil.HashMultiple("buserdev1", salt), Salt=salt, Color="#91ebe9", CreatedAt =  new DateTime(2021, 8, 1) },
               
                // new { Id = 8, FirstName = "CUser", LastName = "CS2", Email = "cusercs2@epsylonhome.com", Password = CryptoUtil.HashMultiple("cusercs2", salt), Salt=salt, Color="#93f595", CreatedAt =  new DateTime(2021, 8, 1) },
               
                // new { Id = 9, FirstName = "DUser", LastName = "DEV2", Email = "duserdev2@epsylonhome.com", Password = CryptoUtil.HashMultiple("duserdev2", salt), Salt=salt, Color="#d9e868", CreatedAt =  new DateTime(2021, 8, 1) }
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
                new { Id = 1, Name = "Super Admin", Desc = "" },
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
                //admin
                new { Id = 1, UserId = 1, RoleId = 1 }
                // new { Id = 2, UserId = 1, RoleId = 4 },
                // // manager
                // new { Id = 3, UserId = 2, RoleId = 2 },
                // new { Id = 4, UserId = 3, RoleId = 2 },
                // //leader
                // new { Id = 5, UserId = 4, RoleId = 3 },
                // new { Id = 6, UserId = 5, RoleId = 3 },
                // //user
                // new { Id = 7, UserId = 6, RoleId = 4 },
                // new { Id = 8, UserId = 7, RoleId = 4 },
                // new { Id = 9, UserId = 8, RoleId = 4 },
                // new { Id = 10, UserId = 9, RoleId = 4 }
            );


            modelBuilder.Entity<UserDept>().ToTable("user_depatments");   
            modelBuilder.Entity<UserDept>().HasKey(u => u.Id).HasName("PK_UserDepts");  
            modelBuilder.Entity<UserDept>().HasIndex(u => u.UserId).HasDatabaseName("idx_userid");     
            modelBuilder.Entity<UserDept>().Property(u => u.Id).HasColumnName("id").HasColumnType("int").UseMySqlIdentityColumn().IsRequired();  
            modelBuilder.Entity<UserDept>().Property(u => u.UserId).HasColumnName("user_id").HasColumnType("int").IsRequired();  
            modelBuilder.Entity<UserDept>().Property(u => u.DepartmentId).HasColumnName("dept_id").HasColumnType("int").IsRequired();  

            modelBuilder.Entity<UserDept>().HasData(
                //admin
                new { Id = 1, UserId = 1, DepartmentId = 1 }
                // new { Id = 2, UserId = 1, DepartmentId = 3 },
                //
                // new { Id = 3, UserId = 2, DepartmentId = 2 },
                // new { Id = 4, UserId = 3, DepartmentId = 3 }, 
                
                // new { Id = 5, UserId = 4, DepartmentId = 2 },
                // new { Id = 6, UserId = 5, DepartmentId = 3 },

                // new { Id = 7, UserId = 6, DepartmentId = 2 },
                // new { Id = 8, UserId = 7, DepartmentId = 3 },
                // new { Id = 9, UserId = 8, DepartmentId = 2 },
                // new { Id = 10, UserId = 9, DepartmentId = 3 }
            );

            modelBuilder.Entity<Sender>().ToTable("senders");  
            modelBuilder.Entity<Sender>().HasKey(u => u.Id).HasName("PK_Sender");  
            modelBuilder.Entity<Sender>().HasIndex(u => u.Email).HasDatabaseName("idx_sender");     
            modelBuilder.Entity<Sender>().Property(u => u.Id).HasColumnName("id").HasColumnType("int").UseMySqlIdentityColumn().IsRequired();  
            modelBuilder.Entity<Sender>().Property(u => u.FirstName).HasColumnName("first_name").HasColumnType("nvarchar(50)").IsRequired();  
            modelBuilder.Entity<Sender>().Property(u => u.LastName).HasColumnName("last_name").HasColumnType("nvarchar(50)").IsRequired();  
            modelBuilder.Entity<Sender>().Property(u => u.Email).HasColumnName("email").HasColumnType("nvarchar(100)").IsRequired();  
            modelBuilder.Entity<Sender>().Property(u => u.Password).HasColumnName("password").HasColumnType("nvarchar(255)").IsRequired(false);  
            modelBuilder.Entity<Sender>().Property(u => u.Salt).HasColumnName("salt").HasColumnType("nvarchar(36)").IsRequired();  
            modelBuilder.Entity<Sender>().Ignore(u => u.File);
            modelBuilder.Entity<Sender>().Property(u => u.LoginStatus).HasColumnName("login_status").HasColumnType("tinyint(1)").IsRequired(false);
            modelBuilder.Entity<Sender>().Property(u => u.Image).HasColumnName("image").HasColumnType("nvarchar(150)").IsRequired(false);  
            modelBuilder.Entity<Sender>().Property(u => u.Color).HasColumnName("color").HasColumnType("nvarchar(50)").IsRequired(false);  
            modelBuilder.Entity<Sender>().Property(u => u.CreatedAt).HasColumnName("created_at").HasColumnType("datetime").IsRequired(false);  
            modelBuilder.Entity<Sender>().Property(u => u.UpdatedAt).HasColumnName("updated_at").HasColumnType("datetime").IsRequired(false);  

        
            modelBuilder.Entity<Sender>().HasData(
                new { Id = 1, FirstName = "AClient", LastName = "Satu", Email = "aclientsatu@gmail.com", Password = "", Salt="", Color="#d9e868" , CreatedAt =  new DateTime(2021, 8, 1) }
                // new { Id = 2, FirstName = "BClient", LastName = "Dua", Email = "bclientdua@gmail.com", Password = "", Salt="", Color="#91ebe9", CreatedAt =  new DateTime(2021, 8, 1) },
                // // logedin
                // new { Id = 3, FirstName = "CClient", LastName = "Tiga", Email = "cclienttiga@gmail.com", Password = CryptoUtil.HashMultiple("cclienttiga", salt), Salt=salt, Color="#32a852", LoginStatus=true, CreatedAt =  new DateTime(2021, 8, 1) }
            );


            modelBuilder.Entity<App>().ToTable("apps");   
            modelBuilder.Entity<App>().HasKey(u => u.Id).HasName("PK_Apps");  
            modelBuilder.Entity<App>().HasIndex(u => u.Name).HasDatabaseName("idx_name");     
            modelBuilder.Entity<App>().Property(u => u.Id).HasColumnName("id").HasColumnType("int").UseMySqlIdentityColumn().IsRequired();  
            modelBuilder.Entity<App>().Property(u => u.Name).HasColumnName("name").HasColumnType("nvarchar(50)").IsRequired(); 
            modelBuilder.Entity<App>().Ignore(u => u.File);
            modelBuilder.Entity<App>().Property(u => u.Logo).HasColumnName("logo").HasColumnType("nvarchar(50)").IsRequired(false);  
            modelBuilder.Entity<App>().Property(u => u.Desc).HasColumnName("desc").HasColumnType("nvarchar(50)").IsRequired(false);

            modelBuilder.Entity<App>().HasData(
                new { Id = 1, Name = "SysAd", Logo="Apps/Sysad.jpg", Desc = "Integrated Advertising System" }
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
            modelBuilder.Entity<Team>().Ignore(u => u.File);
            modelBuilder.Entity<Team>().Property(u => u.Image).HasColumnName("image").HasColumnType("nvarchar(150)").IsRequired(false);  
            modelBuilder.Entity<Team>().Property(u => u.CreateBy).HasColumnName("created_by").HasColumnType("nvarchar(150)").IsRequired(false);  
            modelBuilder.Entity<Team>().Property(u => u.ManagerId).HasColumnName("manager_id").HasColumnType("int").IsRequired();   
            modelBuilder.Entity<Team>().Property(u => u.CreatedAt).HasColumnName("created_at").HasColumnType("datetime").IsRequired(false);  
            modelBuilder.Entity<Team>().Property(u => u.UpdatedAt).HasColumnName("updated_at").HasColumnType("datetime").IsRequired(false);  
            modelBuilder.Entity<Team>().Property(u => u.Deleted).HasColumnName("deleted").HasColumnType("tinyint(1)").HasDefaultValue(0).IsRequired();
            modelBuilder.Entity<Team>().Property(u => u.Color).HasColumnName("color").HasColumnType("nvarchar(45)").IsRequired(false);  

            // modelBuilder.Entity<Team>().HasData(
            //     new { Id = 1, Name = "TEAM CS1", ManagerId=4, Desc = "" },
            //     new { Id = 2, Name = "TEAM PROG1", ManagerId=5, Desc = "" }
            // );

            modelBuilder.Entity<TeamMember>().ToTable("team_members");   
            modelBuilder.Entity<TeamMember>().HasKey(u => u.Id).HasName("PK_Team_Members");    
            modelBuilder.Entity<TeamMember>().Property(u => u.Id).HasColumnName("id").HasColumnType("int").UseMySqlIdentityColumn().IsRequired();  
            modelBuilder.Entity<TeamMember>().Property(u => u.Deleted).HasColumnName("deleted").HasColumnType("tinyint(1)").HasDefaultValue(0).IsRequired();
            modelBuilder.Entity<TeamMember>().Property(u => u.TeamId).HasColumnName("team_id").HasColumnType("int").IsRequired();
            modelBuilder.Entity<TeamMember>().Property(u => u.UserId).HasColumnName("user_id").HasColumnType("int").IsRequired();  
           
            // modelBuilder.Entity<TeamMember>().HasData(
            //     new { Id = 1, TeamId = 1, UserId=6},
            //     new { Id = 2, TeamId = 1, UserId=8},
            //     new { Id = 3, TeamId = 2, UserId=7},
            //     new { Id = 4, TeamId = 2, UserId=9}
            // );

            modelBuilder.Entity<Stat>().ToTable("stats");
            modelBuilder.Entity<Stat>().HasKey(u => u.Id).HasName("PK_Stat");  
            modelBuilder.Entity<Stat>().HasIndex(u => u.Name).HasDatabaseName("idx_name"); 
            modelBuilder.Entity<Stat>().Property(u => u.Id).HasColumnName("id").HasColumnType("int").UseMySqlIdentityColumn().IsRequired();  
            modelBuilder.Entity<Stat>().Property(u => u.Name).HasColumnName("name").HasColumnType("nvarchar(50)").IsRequired();   
            modelBuilder.Entity<Stat>().Property(u => u.Color).HasColumnName("color").HasColumnType("nvarchar(50)").IsRequired(false);   
            modelBuilder.Entity<Stat>().Property(u => u.Desc).HasColumnName("desc").HasColumnType("nvarchar(50)").IsRequired(false);

             modelBuilder.Entity<Stat>().HasData(
                new { Id = 1, Name = "New", Color= "Red", Desc = "bg-danger" },
                new { Id = 2, Name = "Open", Color = "Sky", Desc = "bg-info" },
                new { Id = 3, Name = "In Progress", Color = "Blue", Desc = "bg-primary" },
                new { Id = 4, Name = "Pending", Color = "Yellow", Desc = "bg-warning" },
                new { Id = 5, Name = "Solve", Color = "Green", Desc = "bg-success" },
                new { Id = 6, Name = "Reject", Color = "Grey", Desc = "bg-dark" }
            );

            modelBuilder.Entity<Ticket>().ToTable("tickets");
            modelBuilder.Entity<Ticket>().HasKey(u => u.Id).HasName("PK_Ticket");  
            modelBuilder.Entity<Ticket>().HasIndex(u => u.TicketNumber).HasDatabaseName("idx_TicketNumber"); 
            modelBuilder.Entity<Ticket>().Property(u => u.Id).HasColumnName("id").HasColumnType("int").UseMySqlIdentityColumn().IsRequired();  
            modelBuilder.Entity<Ticket>().Property(u => u.TicketNumber).HasColumnName("ticket_number").HasColumnType("nvarchar(50)").IsRequired();   
            modelBuilder.Entity<Ticket>().Property(u => u.Subject).HasColumnName("subject").HasColumnType("text").IsRequired();   
            modelBuilder.Entity<Ticket>().Property(u => u.Comment).HasColumnName("comment").HasColumnType("longtext").IsRequired();
            modelBuilder.Entity<Ticket>().Property(u => u.AppId).HasColumnName("app_id").HasColumnType("int").IsRequired();
            modelBuilder.Entity<Ticket>().Property(u => u.ModuleId).HasColumnName("module_id").HasColumnType("int").IsRequired();
            modelBuilder.Entity<Ticket>().Property(u => u.StatId).HasColumnName("stat_id").HasColumnType("int").IsRequired();
            modelBuilder.Entity<Ticket>().Property(u => u.SenderId).HasColumnName("sender_id").HasColumnType("int").IsRequired(false);
            modelBuilder.Entity<Ticket>().Property(u => u.UserId).HasColumnName("user_id").HasColumnType("int").IsRequired(false);
            modelBuilder.Entity<Ticket>().Property(u => u.TicketType).HasColumnName("ticket_type").HasColumnType("nvarchar(1)").IsRequired(false);
            modelBuilder.Entity<Ticket>().Property(u => u.PendingBy).HasColumnName("pending_by").HasColumnType("nvarchar(50)").IsRequired(false);
            modelBuilder.Entity<Ticket>().Property(u => u.PendingAt).HasColumnName("pending_at").HasColumnType("datetime").IsRequired(false);
            modelBuilder.Entity<Ticket>().Property(u => u.SolvedBy).HasColumnName("solved_by").HasColumnType("nvarchar(50)").IsRequired(false);
            modelBuilder.Entity<Ticket>().Property(u => u.SolvedAt).HasColumnName("solved_at").HasColumnType("datetime").IsRequired(false);
            modelBuilder.Entity<Ticket>().Property(u => u.RejectedBy).HasColumnName("rejected_by").HasColumnType("nvarchar(50)").IsRequired(false);
            modelBuilder.Entity<Ticket>().Property(u => u.RejectedAt).HasColumnName("rejected_at").HasColumnType("datetime").IsRequired(false);
            modelBuilder.Entity<Ticket>().Property(u => u.RejectedReason).HasColumnName("rejected_reason").HasColumnType("text").IsRequired(false);
            modelBuilder.Entity<Ticket>().Property(u => u.CreatedBy).HasColumnName("created_by").HasColumnType("nvarchar(100)").IsRequired(false);
            modelBuilder.Entity<Ticket>().Property(u => u.CreatedAt).HasColumnName("created_at").HasColumnType("datetime").IsRequired(false);
            modelBuilder.Entity<Ticket>().Property(u => u.UpdatedAt).HasColumnName("updated_at").HasColumnType("datetime").IsRequired(false);

            
            //  modelBuilder.Entity<Ticket>().HasData(
            //     new { Id = 1, TicketNumber = "180620211", Subject= "Ini Test Subject satu ", Comment = "lorem ipsu sdkskadn ksdnksin jdnskjdna jsandjkansdjkansd jndsajkdnajkd kasjndsndoqm dolor shit nyoasdasdaslibay knoper low", AppId = 1, ModuleId = 1, SenderId = 1, StatId=3, CreatedBy="daniel@gmail.com", TicketType="E", CreatedAt =  new DateTime(2021, 8, 1)  },
            //     new { Id = 2, TicketNumber = "180620212", Subject= "Subject for ticket number 2", Comment = "asdhjkahsdjas jasdjj sjadnajk jasnd jas d asndjka  skjdnaksjdn sshdjkajksdas jashdjkahsjkd oashdasihsjskaslnsk", AppId = 1, ModuleId = 1, SenderId = 2, StatId=1, CreatedBy="vickyindiary@yahoo.com", TicketType="E", CreatedAt =  new DateTime(2021, 8, 1)  },
            //     new { Id = 3, TicketNumber = "180620213", Subject= "Subjecsdskkks ksnkandkasndk t 3", Comment = "ksknnina  lasklk  klsnklna ksaiopoellss ksdoasjdandanwdwqo sdnskandjasd  jskdnjksanda asndndiqwioqdwq", AppId = 1, ModuleId = 1, SenderId = 3, StatId=1, CreatedBy="vickyindiary@yahoo.com", TicketType="E", CreatedAt =  new DateTime(2021, 8, 1)  },
            //     new { Id = 4, TicketNumber = "180620214", Subject= "BUG SYSAD SAMPLE", Comment = "ksknnina  lasklk  klsnklna ksaiopoellss ksdoasjdandanwdwqo sdnskandjasd  jskdnjksanda asndndiqwioqdwq", AppId = 1, ModuleId = 1, StatId=1, UserId=6, CreatedBy="vickyindiary@yahoo.com", TicketType="I", CreatedAt =  new DateTime(2021, 8, 1) }
            // );

            modelBuilder.Entity<TicketDetail>().ToTable("ticket_details");
            modelBuilder.Entity<TicketDetail>().HasKey(u => u.Id).HasName("PK_Ticket_details");  
            modelBuilder.Entity<TicketDetail>().Property(u => u.Id).HasColumnName("id").HasColumnType("int").UseMySqlIdentityColumn().IsRequired();  
            modelBuilder.Entity<TicketDetail>().Property(u => u.TicketId).HasColumnName("ticket_id").HasColumnType("int").IsRequired();   
            modelBuilder.Entity<TicketDetail>().Property(u => u.UserId).HasColumnName("user_id").HasColumnType("int").IsRequired(false);   
            modelBuilder.Entity<TicketDetail>().Property(u => u.Comment).HasColumnName("comment").HasColumnType("longtext").IsRequired();
            modelBuilder.Entity<TicketDetail>().Property(u => u.Flag).HasColumnName("flag").HasColumnType("tinyint(1)").IsRequired().HasDefaultValue(false); 
            modelBuilder.Entity<TicketDetail>().Property(u => u.Private).HasColumnName("private").HasColumnType("tinyint(1)").IsRequired().HasDefaultValue(false); 
            modelBuilder.Entity<TicketDetail>().Property(u => u.CreatedAt).HasColumnName("created_at").HasColumnType("datetime").IsRequired(false);
            modelBuilder.Entity<TicketDetail>().Property(u => u.UpdatedAt).HasColumnName("updated_at").HasColumnType("datetime").IsRequired(false);

            // modelBuilder.Entity<TicketDetail>().HasData(
            //     new { Id = 1, TicketId = 1, UserId= 4, Comment = "lorem ipsum dolor shit nyolibay kksdj nknop ksiola knoper low", CreatedAt =  new DateTime(2021, 8, 1), UpdatedAt= new DateTime(2021, 8, 1)   },
            //     new { Id = 2, TicketId = 1, Comment = "asdhjkahsdjas jasshdjkajksdas jashdjkahsjkd oashdasihsjskaslnsk", CreatedAt =  new DateTime(2021, 8, 1), UpdatedAt= new DateTime(2021, 8, 1)   },
            //     new { Id = 3, TicketId = 1, UserId= 4, Comment = "ksknnina  lasklk  klsnklna ksaiopoells;mlauw klnskoiskel aksnkadia mkaskks ", CreatedAt =  new DateTime(2021, 8, 1), UpdatedAt= new DateTime(2021, 8, 1)   }
            // );

            modelBuilder.Entity<TicketAssign>().ToTable("ticket_assigns");
            modelBuilder.Entity<TicketAssign>().HasKey(u => u.Id).HasName("PK_Ticket_assign");  
            modelBuilder.Entity<TicketAssign>().Property(u => u.Id).HasColumnName("id").HasColumnType("int").UseMySqlIdentityColumn().IsRequired();  
            modelBuilder.Entity<TicketAssign>().Property(u => u.TicketId).HasColumnName("ticket_id").HasColumnType("int").IsRequired();   
            modelBuilder.Entity<TicketAssign>().Property(u => u.TeamId).HasColumnName("team_id").HasColumnType("int").IsRequired(false);   
            modelBuilder.Entity<TicketAssign>().Property(u => u.TeamAt).HasColumnName("team_at").HasColumnType("datetime").IsRequired(false);
            modelBuilder.Entity<TicketAssign>().Property(u => u.UserId).HasColumnName("user_id").HasColumnType("int").IsRequired(false);
            modelBuilder.Entity<TicketAssign>().Property(u => u.UserAt).HasColumnName("user_at").HasColumnType("datetime").IsRequired(false);
            modelBuilder.Entity<TicketAssign>().Property(u => u.AssignType).HasColumnName("assign_type").HasColumnType("nvarchar(5)").IsRequired(false);
            modelBuilder.Entity<TicketAssign>().Property(u => u.Viewed).HasColumnName("viewed").HasColumnType("tinyint(1)").IsRequired().HasDefaultValue(false);
            modelBuilder.Entity<TicketAssign>().Property(u => u.ViewedAt).HasColumnName("viewed_at").HasColumnType("datetime").IsRequired(false);

            // modelBuilder.Entity<TicketAssign>().HasData(
            //     new { Id = 1, TicketId=1, UserId=2, UserAt= new DateTime(2021, 8, 1), AssignType="M", Viewed=true, ViewedAt= new DateTime(2021, 8, 1)},
            //     new { Id = 2, TicketId=1, UserId=4, TeamId=1, TeamAt= new DateTime(2021, 8, 1), AssignType="T", Viewed=true, ViewedAt= new DateTime(2021, 8, 1)},
            //     new { Id = 3, TicketId=1, UserId=6, UserAt= new DateTime(2021, 8, 1), AssignType="U", Viewed=true, ViewedAt= new DateTime(2021, 8, 1)},
            //     new { Id = 4, TicketId=2, UserId=2, UserAt= new DateTime(2021, 8, 1), AssignType="M", Viewed=false },
            //     new { Id = 5, TicketId=3, UserId=2, UserAt= new DateTime(2021, 8, 1), AssignType="M", Viewed=false },
            //     new { Id = 6, TicketId=4, UserId=3, UserAt= new DateTime(2021, 8, 1), AssignType="M", Viewed=false }
            // );

            modelBuilder.Entity<Media>().ToTable("medias");
            modelBuilder.Entity<Media>().HasKey(u => u.Id).HasName("PK_Media");  
            modelBuilder.Entity<Media>().Property(u => u.Id).HasColumnName("id").HasColumnType("int").UseMySqlIdentityColumn().IsRequired();  
            modelBuilder.Entity<Media>().Property(u => u.FileName).HasColumnName("file_name").HasColumnType("nvarchar(150)").IsRequired();   
            modelBuilder.Entity<Media>().Property(u => u.FileType).HasColumnName("file_type").HasColumnType("nvarchar(50)").IsRequired();   
            modelBuilder.Entity<Media>().Property(u => u.RelId).HasColumnName("rel_id").HasColumnType("int").IsRequired();
            modelBuilder.Entity<Media>().Property(u => u.RelType).HasColumnName("rel_type").HasColumnType("nvarchar(5)").IsRequired();
            modelBuilder.Entity<Media>().Property(u => u.TicketId).HasColumnName("ticket_id").HasColumnType("int").IsRequired(false);
            modelBuilder.Entity<Media>().Property(u => u.TicketDetailId).HasColumnName("ticket_detail_id").HasColumnType("int").IsRequired(false);
            modelBuilder.Entity<Media>().Property(u => u.CLogDetailId).HasColumnName("clog_detail_id").HasColumnType("int").IsRequired(false);
            modelBuilder.Entity<Media>().Property(u => u.KbaseId).HasColumnName("kbase_id").HasColumnType("int").IsRequired(false);
            modelBuilder.Entity<Media>().Ignore(u => u.File);

            // modelBuilder.Entity<Media>().HasData(
            //     new { Id = 1, FileName="Tickets/atc1.jpg", FileType=".pf", RelId=1, RelType="T" },
            //     new { Id = 2, FileName="Tickets/atc2.pdf", FileType=".pdf", RelId=1, RelType="T" },
            //     new { Id = 3, FileName="TicketDetails/atc3.jpeg", FileType=".jpg", RelId=1, RelType="TD" },
            //     new { Id = 4, FileName="TicketsDetails/atc4.xls", FileType=".xls", RelId=1, RelType="TD" }
            // );

            modelBuilder.Entity<KBase>().ToTable("kbases");
            modelBuilder.Entity<KBase>().HasKey(u => u.Id).HasName("PK_KBase");  
            modelBuilder.Entity<KBase>().Property(u => u.Id).HasColumnName("id").HasColumnType("int").UseMySqlIdentityColumn().IsRequired();  
            modelBuilder.Entity<KBase>().Property(u => u.Title).HasColumnName("title").HasColumnType("text").IsRequired();   
            modelBuilder.Entity<KBase>().Property(u => u.Body).HasColumnName("body").HasColumnType("text").IsRequired();   
            modelBuilder.Entity<KBase>().Property(u => u.AppId).HasColumnName("app_id").HasColumnType("int").IsRequired();
            modelBuilder.Entity<KBase>().Property(u => u.ModuleId).HasColumnName("module_id").HasColumnType("int").IsRequired();
            modelBuilder.Entity<KBase>().Property(u => u.UserId).HasColumnName("user_id").HasColumnType("int").IsRequired();
            modelBuilder.Entity<KBase>().Property(u => u.CreatedAt).HasColumnName("created_at").HasColumnType("datetime").IsRequired(false);
            modelBuilder.Entity<KBase>().Property(u => u.UpdatedAt).HasColumnName("updated_at").HasColumnType("datetime").IsRequired(false);

            modelBuilder.Entity<CLog>().ToTable("clogs");
            modelBuilder.Entity<CLog>().HasKey(u => u.Id).HasName("PK_CLog");  
            modelBuilder.Entity<CLog>().Property(u => u.Id).HasColumnName("id").HasColumnType("int").UseMySqlIdentityColumn().IsRequired();  
            modelBuilder.Entity<CLog>().Property(u => u.Version).HasColumnName("version").HasColumnType("nvarchar(100)").IsRequired();   
            modelBuilder.Entity<CLog>().Property(u => u.Desc).HasColumnName("desc").HasColumnType("text").IsRequired(false);   
            modelBuilder.Entity<CLog>().Property(u => u.AppId).HasColumnName("app_id").HasColumnType("int").IsRequired();
            modelBuilder.Entity<CLog>().Property(u => u.UserId).HasColumnName("user_id").HasColumnType("int").IsRequired();
            modelBuilder.Entity<CLog>().Property(u => u.CreatedAt).HasColumnName("created_at").HasColumnType("datetime").IsRequired(false);
            modelBuilder.Entity<CLog>().Property(u => u.UpdatedAt).HasColumnName("updated_at").HasColumnType("datetime").IsRequired(false);

            modelBuilder.Entity<CLogDetail>().ToTable("clog_details");
            modelBuilder.Entity<CLogDetail>().HasKey(u => u.Id).HasName("PK_CLog_Detail");  
            modelBuilder.Entity<CLogDetail>().Property(u => u.Id).HasColumnName("id").HasColumnType("int").UseMySqlIdentityColumn().IsRequired();  
            modelBuilder.Entity<CLogDetail>().Property(u => u.Title).HasColumnName("title").HasColumnType("nvarchar(200)").IsRequired();   
            modelBuilder.Entity<CLogDetail>().Property(u => u.Desc).HasColumnName("desc").HasColumnType("text").IsRequired(false);   
            modelBuilder.Entity<CLogDetail>().Property(u => u.CLogId).HasColumnName("clog_id").HasColumnType("int").IsRequired();
            modelBuilder.Entity<CLogDetail>().Property(u => u.ModuleId).HasColumnName("module_id").HasColumnType("int").IsRequired(false);
            modelBuilder.Entity<CLogDetail>().Property(u => u.CLogTypeId).HasColumnName("clog_type_id").HasColumnType("int").IsRequired();
            modelBuilder.Entity<CLogDetail>().Ignore(u => u.ImageName);


            modelBuilder.Entity<CLogType>().ToTable("clog_type");
            modelBuilder.Entity<CLogType>().HasKey(u => u.Id).HasName("PK_CLog_Type");  
            modelBuilder.Entity<CLogType>().Property(u => u.Id).HasColumnName("id").HasColumnType("int").UseMySqlIdentityColumn().IsRequired();  
            modelBuilder.Entity<CLogType>().Property(u => u.Name).HasColumnName("name").HasColumnType("nvarchar(20)").IsRequired();   
            modelBuilder.Entity<CLogType>().Property(u => u.Color).HasColumnName("color").HasColumnType("nvarchar(20)").IsRequired();   
            modelBuilder.Entity<CLogType>().Property(u => u.Desc).HasColumnName("desc").HasColumnType("text").IsRequired(false);
            modelBuilder.Entity<CLogType>().HasData(
                new { Id = 1, Name="New", Color="Blue"},
                new { Id = 2, Name="Fix", Color="Orange"},
                new { Id = 3, Name="Enhance", Color="Green" }
  
            );

            modelBuilder.Entity<Faq>().ToTable("faqs");
            modelBuilder.Entity<Faq>().HasKey(u => u.Id).HasName("PK_Faqs");  
            modelBuilder.Entity<Faq>().Property(u => u.Id).HasColumnName("id").HasColumnType("int").UseMySqlIdentityColumn().IsRequired();  
            modelBuilder.Entity<Faq>().Property(u => u.Question).HasColumnName("question").HasColumnType("text").IsRequired();   
            modelBuilder.Entity<Faq>().Property(u => u.Desc).HasColumnName("desc").HasColumnType("text").IsRequired();   
            modelBuilder.Entity<Faq>().Property(u => u.AppId).HasColumnName("app_id").HasColumnType("int").IsRequired();
            modelBuilder.Entity<Faq>().Property(u => u.ModuleId).HasColumnName("module_id").HasColumnType("int").IsRequired();
            modelBuilder.Entity<Faq>().Property(u => u.UserId).HasColumnName("user_id").HasColumnType("int").IsRequired();
            modelBuilder.Entity<Faq>().Property(u => u.CreatedAt).HasColumnName("created_at").HasColumnType("datetime").IsRequired(false);
            modelBuilder.Entity<Faq>().Property(u => u.UpdatedAt).HasColumnName("updated_at").HasColumnType("datetime").IsRequired(false);

            modelBuilder.Entity<ClientGroup>().ToTable("client_groups");
            modelBuilder.Entity<ClientGroup>().HasKey(u => u.Id).HasName("PK_ClientGroups");  
            modelBuilder.Entity<ClientGroup>().Property(u => u.Id).HasColumnName("id").HasColumnType("int").UseMySqlIdentityColumn().IsRequired();  
            modelBuilder.Entity<ClientGroup>().Property(u => u.Name).HasColumnName("name").HasColumnType("nvarchar(50)").IsRequired();   
            modelBuilder.Entity<ClientGroup>().Property(u => u.Domain).HasColumnName("domain").HasColumnType("nvarchar(50)").IsRequired();
            modelBuilder.Entity<ClientGroup>().Property(u => u.Desc).HasColumnName("desc").HasColumnType("text").IsRequired(false);   

            modelBuilder.Entity<ClientDetail>().ToTable("client_details");
            modelBuilder.Entity<ClientDetail>().HasKey(u => u.Id).HasName("PK_ClientDetails");  
            modelBuilder.Entity<ClientDetail>().Property(u => u.Id).HasColumnName("id").HasColumnType("int").UseMySqlIdentityColumn().IsRequired();  
            modelBuilder.Entity<ClientDetail>().Property(u => u.ClientGroupId).HasColumnName("client_group_id").HasColumnType("int").IsRequired();  
            modelBuilder.Entity<ClientDetail>().Property(u => u.Name).HasColumnName("name").HasColumnType("nvarchar(50)").IsRequired();   
            modelBuilder.Entity<ClientDetail>().Property(u => u.Domain).HasColumnName("domain").HasColumnType("nvarchar(50)").IsRequired();
            modelBuilder.Entity<ClientDetail>().Property(u => u.Desc).HasColumnName("desc").HasColumnType("text").IsRequired(false);   

         
            modelBuilder.Entity<Verification>().ToTable("verifications");
            modelBuilder.Entity<Verification>().HasKey(u => u.Id).HasName("PK_Verifications");  
            modelBuilder.Entity<Verification>().Property(u => u.Id).HasColumnName("id").HasColumnType("int").UseMySqlIdentityColumn().IsRequired();  
            modelBuilder.Entity<Verification>().Property(u => u.Code).HasColumnName("code").HasColumnType("nvarchar(20)").IsRequired();  
            modelBuilder.Entity<Verification>().Property(u => u.Verified).HasColumnName("verified").HasColumnType("tinyint(1)").IsRequired().HasDefaultValue(false);
            modelBuilder.Entity<Verification>().Property(u => u.ExpiredAt).HasColumnName("expired_at").HasColumnType("datetime").IsRequired();
            modelBuilder.Entity<Verification>().Property(u => u.Desc).HasColumnName("desc").HasColumnType("text").IsRequired(false);   
            modelBuilder.Entity<Verification>().Property(u => u.UserId).HasColumnName("user_id").HasColumnType("int").IsRequired(false);
            modelBuilder.Entity<Verification>().Property(u => u.SenderId).HasColumnName("sender_id").HasColumnType("int").IsRequired(false);
            modelBuilder.Entity<Verification>().Property(u => u.Email).HasColumnName("email").HasColumnType("nvarchar(50)").IsRequired(false);
            modelBuilder.Entity<Verification>().Property(u => u.CreatedAt).HasColumnName("created_at").HasColumnType("datetime").IsRequired(false);
            modelBuilder.Entity<Verification>().Property(u => u.UpdatedAt).HasColumnName("updated_at").HasColumnType("datetime").IsRequired(false); 
        
            modelBuilder.Entity<NotifRegister>().ToTable("notif_registers");
            modelBuilder.Entity<NotifRegister>().HasKey(u => u.Id).HasName("PK_Notif_registers");  
            modelBuilder.Entity<NotifRegister>().Property(u => u.Id).HasColumnName("id").HasColumnType("int").UseMySqlIdentityColumn().IsRequired();  
            modelBuilder.Entity<NotifRegister>().Property(u => u.UserId).HasColumnName("user_id").HasColumnType("int").IsRequired(false);  
            modelBuilder.Entity<NotifRegister>().Property(u => u.SenderId).HasColumnName("sender_id").HasColumnType("int").IsRequired(false);  
            modelBuilder.Entity<NotifRegister>().Property(u => u.FcmToken).HasColumnName("fcm_token").HasColumnType("text").IsRequired();  
            modelBuilder.Entity<NotifRegister>().Property(u => u.Os).HasColumnName("os").HasColumnType("nvarchar(50)").IsRequired(false);
            modelBuilder.Entity<NotifRegister>().Property(u => u.OsVersion).HasColumnName("os_version").HasColumnType("nvarchar(50)").IsRequired(false);
            modelBuilder.Entity<NotifRegister>().Property(u => u.Browser).HasColumnName("browser").HasColumnType("nvarchar(50)").IsRequired(false);
            modelBuilder.Entity<NotifRegister>().Property(u => u.BrowserVersion).HasColumnName("browser_version").HasColumnType("nvarchar(50)").IsRequired(false);
            modelBuilder.Entity<NotifRegister>().Property(u => u.CreatedAt).HasColumnName("created_at").HasColumnType("datetime").IsRequired(false);
            modelBuilder.Entity<NotifRegister>().Property(u => u.UpdatedAt).HasColumnName("updated_at").HasColumnType("datetime").IsRequired(false); 
            
            modelBuilder.Entity<Notif>().ToTable("notifs");
            modelBuilder.Entity<Notif>().HasKey(u => u.Id).HasName("PK_Notifs");  
            modelBuilder.Entity<Notif>().Property(u => u.Id).HasColumnName("id").HasColumnType("int").UseMySqlIdentityColumn().IsRequired();  
            modelBuilder.Entity<Notif>().Property(u => u.NotifRegisterId).HasColumnName("notif_register_id").HasColumnType("int").IsRequired();  
            modelBuilder.Entity<Notif>().Property(u => u.Viewed).HasColumnName("viewed").HasColumnType("tinyint(1)").IsRequired().HasDefaultValue(false);
            modelBuilder.Entity<Notif>().Property(u => u.Title).HasColumnName("title").HasColumnType("text").IsRequired();
            modelBuilder.Entity<Notif>().Property(u => u.Message).HasColumnName("message").HasColumnType("longtext").IsRequired(false);   
            modelBuilder.Entity<Notif>().Property(u => u.Link).HasColumnName("link").HasColumnType("text").IsRequired(false);
            modelBuilder.Entity<Notif>().Property(u => u.NtfType).HasColumnName("ntf_type").HasColumnType("nvarchar(50)").IsRequired(false);
            modelBuilder.Entity<Notif>().Property(u => u.CreatedAt).HasColumnName("created_at").HasColumnType("datetime").IsRequired(false);
            modelBuilder.Entity<Notif>().Property(u => u.UpdatedAt).HasColumnName("updated_at").HasColumnType("datetime").IsRequired(false);
        }  
    }  
}  