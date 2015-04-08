using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospiceNiagara.Models.ViewModels
{
    public class RolesViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<ApplicationUser> User { get; set; }

    }
}