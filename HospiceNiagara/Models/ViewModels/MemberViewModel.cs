using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HospiceNiagara.Models.ViewModels
{
    public class MemberViewModel
    {
        [Key]
        public string id { get; set; }

        [Required(ErrorMessage = "Fist Name Cannot be empty")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name cannot be empty")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage= "E-Mail")]
        [Display(Name = "E-Mail")]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [Display(Name="Phone Number")]
        public string PhoneNumber { get; set; }

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
    }
}