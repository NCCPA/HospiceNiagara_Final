namespace HospiceNiagara.Migrations
{
    using HospiceNiagara.Models;
    using HospiceNiagara.Models.DatabaseModels;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<HospiceNiagara.Models.ApplicationDbContext>
    {
        private UserManager<ApplicationUser> UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
        private RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));

        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "HospiceNiagara.Models.ApplicationDbContext";
        }

        protected override void Seed(HospiceNiagara.Models.ApplicationDbContext context)
        {
            ApplicationUser newUser = new ApplicationUser ();
            newUser.UserName = "morozoandrei@gmail.com";
            newUser.Email = "morozoandrei@gmail.com";
            newUser.FirstName = "Andrei";
            newUser.LastName = "Morozov";
            newUser.PhoneNumber = "9053413711";
            UserManager.Create(newUser, "Pass!23");
           

            //Add Announcement
            var announcement = new List<Announcement>
            {
                new Announcement {Title = "Movie Night and Raffle Draw!", Description="Random Movie Choosen from a hat of everyone's choice", isVisible=true, Date = DateTime.Now},
                new Announcement {Title = "Movie Night and Raffle Draw!", Description="Random Movie Choosen from a hat of everyone's choice", isVisible=true, Date = DateTime.Now},
                new Announcement {Title = "Movie Night and Raffle Draw!", Description="Random Movie Choosen from a hat of everyone's choice", isVisible=true, Date = DateTime.Now},
                new Announcement {Title = "Movie Night and Raffle Draw!", Description="Random Movie Choosen from a hat of everyone's choice", isVisible=true, Date = DateTime.Now},
                new Announcement {Title = "Movie Night and Raffle Draw!", Description="Random Movie Choosen from a hat of everyone's choice", isVisible=true, Date = DateTime.Now},
                new Announcement {Title = "Movie Night and Raffle Draw!", Description="Random Movie Choosen from a hat of everyone's choice", isVisible=true, Date = DateTime.Now},
                new Announcement {Title = "Movie Night and Raffle Draw!", Description="Random Movie Choosen from a hat of everyone's choice", isVisible=true, Date = DateTime.Now},
                new Announcement {Title = "Movie Night and Raffle Draw!", Description="Random Movie Choosen from a hat of everyone's choice", isVisible=true, Date = DateTime.Now},
                new Announcement {Title = "Movie Night and Raffle Draw!", Description="Random Movie Choosen from a hat of everyone's choice", isVisible=true, Date = DateTime.Now}
            };
            announcement.ForEach(a => context.Announcements.AddOrUpdate(x => x.ID, a));
            context.SaveChanges();


            //Deaths
            var Death = new List<Deaths> 
            { 
               new Deaths { Name="Joe Smith", Date = Convert.ToDateTime("2014-12-16"), Location = "Community Client", Note = "Volunteer: Ted Tennant", Visible= true},
               new Deaths { Name="Rachel Jones", Date = Convert.ToDateTime("2015-12-14"), Location = "The Stabler Centre", Note = "Room 4", Visible=true},
               new Deaths { Name="Mary Brown", Date = Convert.ToDateTime("2015-12-08"), Location = "NN Outreach Team", Note = "", Visible=true},
               new Deaths { Name="Sally Williams", Date = Convert.ToDateTime("2015-11-30"), Location = "NS Outreach Team", Note = "", Visible=true}
            };            
            Death.ForEach(d => context.Deaths.AddOrUpdate(x => x.Name, d));
            context.SaveChanges();

            //Add Meetings
            var meetings = new List<Meetings>
            {
                new Meetings { Date = DateTime.Today, Name = "Heart Day", Description = "Doctors and spiritual leaders meeting", StartTime="10:00 AM", EndTime="1:30 PM"},
                new Meetings { Date = DateTime.Today, Name = "Heart Day", Description = "Doctors and spiritual leaders meeting", StartTime="10:00 AM", EndTime="1:30 PM"},
                new Meetings { Date = DateTime.Today, Name = "Heart Day", Description = "Doctors and spiritual leaders meeting", StartTime="10:00 AM", EndTime="1:30 PM"},
                new Meetings { Date = DateTime.Today, Name = "Heart Day", Description = "Doctors and spiritual leaders meeting", StartTime="10:00 AM", EndTime="1:30 PM"},
                new Meetings { Date = DateTime.Today, Name = "Heart Day", Description = "Doctors and spiritual leaders meeting", StartTime="10:00 AM", EndTime="1:30 PM"},
                new Meetings { Date = DateTime.Today, Name = "Heart Day", Description = "Doctors and spiritual leaders meeting", StartTime="10:00 AM", EndTime="1:30 PM"},
                new Meetings { Date = DateTime.Today, Name = "Heart Day", Description = "Doctors and spiritual leaders meeting", StartTime="10:00 AM", EndTime="1:30 PM"}
            };
            meetings.ForEach(a => context.Meetings.AddOrUpdate(x => x.ID, a));
            context.SaveChanges();
        }
    }
}
