using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;

namespace HospiceNiagara.Models.DatabaseModels
{
    public class SubRoles
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public string RoleID { get; set; }
        public virtual ICollection<IdentityRole> Roles { get; set; }
    }
}