namespace HospiceNiagara.Migrations
{
    using HospiceNiagara.Models.DatabaseModels;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<HospiceNiagara.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "HospiceNiagara.Models.ApplicationDbContext";
        }

        protected override void Seed(HospiceNiagara.Models.ApplicationDbContext context)
        {

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
        }
    }
}
