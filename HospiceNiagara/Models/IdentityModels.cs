using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using HospiceNiagara.Models.DatabaseModels;
using System.Web.Mvc;
using HospiceNiagara.Models.ViewModels;
using System.Collections.Generic;

namespace HospiceNiagara.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        //Member Additional Information
        [Required(ErrorMessage = "Fist Name Cannot be empty")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [DisplayFormat(DataFormatString = "{0:###-###-####}")]
        [StringLength(10, ErrorMessage = "cannot Have more than 10 digits, do not input - or () ")]
        public override string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Last Name cannot be empty")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Phone Extension")]
        public string PhoneExt { get; set; }

        [Display(Name = "View in Contacts")]
        public bool IsContact { get; set; }

        [Display(Name = "Position Title")]
        public string Position { get; set; }

        [Display(Name = "Position Description")]
        public string PositionDescription { get; set; }

        [Display(Name = "Bio")]
        [StringLength(250, ErrorMessage = "Bio Cannot be More than 250 Charactes")]
        public string Bio { get; set; }

        //Fields for Profile Picture
        public byte[] ProfilePicture { get; set; }
        public string MimeType { get; set; }


        public virtual ICollection<SubRoles> SubRoles { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        //DataBase Models
        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<Deaths> Deaths { get; set; }
        public DbSet<Meetings> Meetings { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<Folder> Folders { get; set; }
        public DbSet<SubRoles> SubRoles { get; set; }




    }
}