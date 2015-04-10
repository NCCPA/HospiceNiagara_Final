using HospiceNiagara.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

//Andrei Library for Users & Roles

namespace HospiceNiagara
{
    public class HelperClass
    {

        private ApplicationDbContext db = new ApplicationDbContext();
        private UserManager<ApplicationUser> UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
        private RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));

        /// <summary>
        /// Returns a single role name for a user by their GUID ID (String)
        /// </summary>
        /// <param name="id">User ID</param>
        /// <returns>string</returns>
        public string roleNameByUserId(string id)
        {
            //declare variables
            string roleName = "";

            //Get all the Roles that the Person is in, only 1 can be assinged
            var userID = UserManager.FindById(id).Roles;

            //Search through Collection of Roles and find the only one there...
            foreach (var item in userID)
            {
                roleName = roleManager.FindById(item.RoleId).Name;
                break; //Exit once found
            }

            //Send to ViewBag so it can be read in view
            return roleName;
        }

        /// <summary>
        /// returnd RoleID (String) using a userID (GUID) string
        /// </summary>
        /// <param name="id">GUID ApplicationUser ID</param>
        /// <returns>string roleID (GUID)</returns>
        public string roleIdByUserId(string id)
        {
            //declare variables
            string roleName = "";
          
            //Get all the Roles that the Person is in, only 1 can be assinged
            var userID = UserManager.FindById(id).Roles;

            //Search through Collection of Roles and find the only one there...
            foreach (var item in userID)
            {
                roleName = roleManager.FindById(item.RoleId).Id;
                break; //Exit once found
            }

            //Send to ViewBag so it can be read in view
            return roleName;
        }

        /// <summary>
        /// Get all Roles in a ToList List<SelectListItem> 
        /// </summary>
        /// <returns>Roles in a ToList of Type SelectListItem</returns>
        public List<SelectListItem> getRolesList()
        {
            //Get all roles from db
            SelectList roles = new SelectList(db.Roles.OrderBy(x => x.Name), "ID", "Name");

            //assign roles to a list & return
            return roles.ToList();            
        }

        /// <summary>
        /// Get all Roles in a ToList List With a Selected Value<SelectListItem> 
        /// </summary>
        /// <returns>Roles in a ToList of Type SelectListItem</returns>
        public List<SelectListItem> getRolesList(string id)
        {
            string selectedValue = roleIdByUserId(id);

            //Get all roles from db
            SelectList roles = new SelectList(db.Roles.OrderBy(x => x.Name), "ID", "Name", selectedValue);

            //assign roles to a list & return
            return roles.ToList();
        }

        /// <summary>
        /// Use this in Edit to change a users role, takes current user ID and new RoleID
        /// </summary>
        /// <param name="id">UserID GUID string</param>
        /// <param name="roleID">New RoleID String GUID</param>
        public void ChangeRoles (string id, string roleID)
        {
            string result = "Success";

            //Try to Change catch any errors
            try
            {
                //Check to see if new role same as current
                if (roleNameByUserId(id) != roleManager.FindById(roleID).Name)
                {

                    //Remove From Old Role
                    UserManager.RemoveFromRole(id, roleNameByUserId(id));

                    //Add to new Role
                    UserManager.AddToRole(id, roleManager.FindById(roleID).Name);

                   
                }                
            }
            catch(Exception e)
            {
                result = e.Message + " -- Failure";
            }                
        }

        /// <summary>
        /// Returns the current user in databases Email
        /// </summary>
        /// <param name="id">UserID</param>
        /// <returns>string email</returns>
        public string CurrentEmail(string id)
        {
            return UserManager.FindById(id).Email;
        }

        /// <summary>
        /// Checks to see if an email exists in the database
        /// </summary>
        /// <param name="email">string of an email that is to be checked</param>
        /// <returns>true if email exists, false if email does not exist</returns>
        public bool EmailExist(string email)
        {
            var userSelected = db.Users.FirstOrDefault(x => x.Email == email);

            if (userSelected != null)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// When updating the current user, do they own the current email being re-added
        /// </summary>
        /// <param name="id">UserID</param>
        /// <param name="email">Email being inputted</param>
        /// <returns>Returns true if the emails match therefor no email changes, false means its a new email</returns>
        public bool CurrentEmail(string id, string email)
        {
            //get currently owned email
            string currentEmail = UserManager.FindById(id).Email;

            //Check and see its the the same email being submitted
            if (currentEmail == email)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        

        /// <summary>
        /// Get role id as a string by the role name
        /// </summary>
        /// <param name="name">Role Name</param>
        /// <returns>String of roleID</returns>
        public string GetRoleValueByName(string name)
        {
            try
            {
                string roleID = roleManager.FindByName(name).Id;
                return roleID;
            }
            catch (Exception)
            {
                return ""; //Incase volunteer get removed in the future
            }
        }

        public string GetRandomPasswordUsingGUID(int length)
        {
            // Get the GUID
            string guidResult = System.Guid.NewGuid().ToString();

            // Remove the hyphens
            guidResult = guidResult.Replace("-", string.Empty);

            // Make sure length is valid
            if (length <= 0 || length > guidResult.Length)
                throw new ArgumentException("Length must be between 1 and " + guidResult.Length);

            // Return the first length bytes
            return guidResult.Substring(0, length);
        }       
    }
}