using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using HospiceNiagara.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Net.Mail;
using System.Net;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace HospiceNiagara.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AccountController : Controller
    {
        private ApplicationUserManager _userManager;        
        private HelperClass helperClass = new HelperClass();


        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        private ApplicationSignInManager _signInManager;

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set { _signInManager = value; }
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {               
                return View(model);
            }

            //if user not activated dont let them login

            ApplicationUser userObj = UserManager.FindByEmail(model.Email);

            if (userObj != null)
            {
                if (!userObj.isActive)
                {
                    ModelState.AddModelError("PermissionDenied", "Your Account is deactivated, contact Hospice Niagara admin for further information");
                    return View(model);
                }
            }


            // current email is not confirmed in the database
            if (helperClass.isValidUser(model.Email,model.Password))
            {
                return View("ConfirmEmail", model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);


            //break down the result status with custom login first time verification
            if (result == SignInStatus.Success)
            {
                    return RedirectToLocal(returnUrl);                                          
            }
            if (result == SignInStatus.LockedOut)
            {
                return View("Lockout");
            }
            if (result == SignInStatus.RequiresVerification)
            {
                return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
            }


            ModelState.AddModelError("", "Invlaid Login attempt");
            return View(model);

            /*
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
            */
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            var user = await UserManager.FindByIdAsync(await SignInManager.GetVerifiedUserIdAsync());
            if (user != null)
            {
                var code = await UserManager.GenerateTwoFactorTokenAsync(user.Id, provider);
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }



            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        [HttpPost]        
        public JsonResult addRole(string[] selectedRoles)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            
            var roles = db.Roles.ToList();
            foreach(var r in selectedRoles)
            {
                foreach(var x in roles.ToList())
                {
                    if (x.Id == r || x.Name == "SuperAdmin")
                    {
                        roles.Remove(x);
                    }
                }
            }
            return Json(roles);
        }

        [HttpPost]        
        public JsonResult SubRoles(string roleID)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var subRoles = db.SubRoles.Where(x => x.RoleID == roleID);
            var subRolesList = subRoles.ToList();
            return Json(subRolesList);
            
        }

        //
        // GET: /Account/Register
        [Authorize(Roles="Admin")]       
        public ActionResult Register()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            //Get all roles from db
            //When creating user majority is volunteer so make it selected value
            
            HelperClass helperClass = new HelperClass();

            //allow selected drop down list to be volunteer            
            SelectList roles = new SelectList(db.Roles.OrderBy(x => x.Name).Where(x => x.Name != "SuperAdmin"), "ID", "Name");
            SelectListItem allOption = new SelectListItem() { Value = "0", Text = "Select A Main Role" };

            //assign roles to a list
            var rolelist = roles.ToList();
            rolelist.Insert(0, allOption);
            ViewBag.RolesList = rolelist;            
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]        
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model, string[] roleID, string[] subRolesList)
        {            
            ApplicationDbContext db = new ApplicationDbContext();
                        
            SelectList roles = new SelectList(db.Roles.OrderBy(x => x.Name), "ID", "Name");            
            //assign roles to a list
            var rolelist = roles.ToList();            
            ViewBag.RolesList = rolelist;      

            if (ModelState.IsValid)
            {
                
                HelperClass helperClass = new HelperClass();              

                //check email to see if its already registered, if it is add error and return view
                if (helperClass.EmailExist(model.Email))
                {                   
                        //there is another email in the database with that name already add error to model
                        ModelState.AddModelError("Email", "E-Mail already registered");                                                      
                        return View(model);                    
                }

                //Generate Random Password
                string generatedPassword = System.Web.Security.Membership.GeneratePassword(8, 2);
                                

                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,                    
                    LastName = model.LastName,
                    PhoneNumber = model.PhoneNumber,
                    PhoneExt = model.PhoneExt,
                    IsContact = model.IsContact,
                    Position = model.Position,
                    PositionDescription = model.PositionDescription,
                    Bio = model.Bio,
                    isActive = true
                };

                var result = await UserManager.CreateAsync(user, generatedPassword);
                if (result.Succeeded)
                {
                    //once a user is created send them an email
                    SmtpClient client = new SmtpClient("smtp-mail.outlook.com", 587);
                    client.Credentials = new NetworkCredential("morozoandrei@outlook.com","Shad0w!59");
                    client.EnableSsl = true;

                    MailMessage mailMessage = new MailMessage();
                    mailMessage.From = new MailAddress("morozoandrei@outlook.com");
                    mailMessage.To.Add(model.Email);
                    mailMessage.Subject = "Hospice Niagara Account setup";

                    string bodyMessage = "<h1>Hello " + model.FirstName + " " + model.LastName + "</h1>" +
                       "<p>Welcome to Hospice Niagara your account is ready</p>" +
                       "<p>Login Portal: http://www.hospiceniagara/portal.com</p>" +
                       "<p>Temporary Password: " + generatedPassword + "</p>" +
                       "<p>Check out your profile to see if all information is correct</p>";

                    mailMessage.IsBodyHtml = true;
                    mailMessage.Body = bodyMessage;


                    client.Send(mailMessage);
                    

                   

                    var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
                    var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));


                    

                    
                    var userID = userManager.FindByEmail(model.Email).Id;

                    for (int i = 0; i < roleID.Length; i++)
                    {
                        var roleName = roleManager.FindById(roleID[i]).Name;
                        userManager.AddToRole(userID, roleName);
                    }

                    //grab current User
                    ApplicationUser userToUpdate = db.Users.Where(x => x.Id == userID).Single();

                    //set up variables to user and check each other
                    var selectedSubRoles = new HashSet<string>(subRolesList);
                    var userSubRoles = new HashSet<int>(userToUpdate.SubRoles.Select(x => x.ID));

                    foreach (var subRole in db.SubRoles)
                    {
                        if (selectedSubRoles.Contains(subRole.ID.ToString()))
                        {
                            if (!userSubRoles.Contains(subRole.ID))
                            {
                                userToUpdate.SubRoles.Add(subRole);
                            }
                        }
                    }

                    db.SaveChanges();

                    //use for later 
                    /*
                     else {
                     *  if (userSubRoles.Contains(subRole.ID))
                     *      {
                     *          userToUpdate.SubRoles.Remove(subRole);
                     *      }
                     * }
                    */
                    return RedirectToAction("Index", "AdminMembers");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        [HttpGet]
        public ActionResult ConfirmEmail([Bind(Include = "id,Email")] LoginViewModel model, string oldPassword, string newPassword)
        {
          return View(model);
        }

        //
        // POST: /Account/ConfirmEmail
        [AllowAnonymous]
        [HttpPost]
        public ActionResult ConfirmEmail(LoginViewModel model, string oldPassword, string password, string passwordConfirmed)
        {
            //Make sure fields all work nicely
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            
            if(password != passwordConfirmed)
            {
                ModelState.AddModelError("Password", "New password and confirm password do not match");
                return View(model);
            }

            //Check to make sure password contains 1 uppcase or 1 symbol
            Regex r = new Regex(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[$@$!%*#?&])[A-Za-z\d$@$!%*#?&]{6,}$");
                if (!r.Match(password).Success)
                {
                    ModelState.AddModelError("passwordFormat", "Password must contain 1 uppercase, digit and symbol.");
                    return View(model);
                }

            else //everything is good change user password
            {
                var user = UserManager.Find(model.Email,oldPassword); //find user

                if(user != null) //make sure he is inside the database once more
                {
                    var result = UserManager.ChangePassword(user.Id, oldPassword, password); //change password and get result
                    
                    if (result.Succeeded)
                    {
                        //to update EmailConfirmedField
                        ApplicationDbContext db = new ApplicationDbContext();

                        var updateUser = db.Users.Where(x => x.Id == user.Id).SingleOrDefault();
                        updateUser.EmailConfirmed = true;
                        db.SaveChanges();                       
                        
                        return View("Login");
                    }                                      
                }
                else
                {
                    ModelState.AddModelError("ErrorOldPassword", "Old Password entered Incorrectly or Email does not exist anymore");
                    return View(model);
                }
            }
            
            //if we got this far dammit!                        
            return View(model);
        }


        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }            

            //first check if the email is registered with us            
            ApplicationDbContext db = new ApplicationDbContext();

            var existingUser = db.Users.Where(x => x.Email == model.Email);

            if (existingUser.Count() == 0)
            {
                ModelState.AddModelError("no-email", "This E-mail is not registered with us, check spelling");
                return View(model);
            }
            else //email exists change password to new generated email
            {
                var randomPassword = System.Web.Security.Membership.GeneratePassword(8, 2);
                var resetUser = UserManager.FindByEmail(model.Email);

                UserManager.RemovePassword(resetUser.Id);
                UserManager.AddPassword(resetUser.Id, randomPassword);
                
                //removed confirmed, this will make them change their password
                var linqUser = db.Users.Where(m => m.Id == resetUser.Id).SingleOrDefault();
                linqUser.EmailConfirmed = false;
                db.SaveChanges();

                //Send email with the new password
                //once a user is created send them an email
                SmtpClient client = new SmtpClient("smtp-mail.outlook.com", 587);
                client.Credentials = new NetworkCredential("morozoandrei@outlook.com", "Shad0w!59");
                client.EnableSsl = true;

                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress("morozoandrei@outlook.com");
                mailMessage.To.Add(model.Email);
                mailMessage.Subject = "Hospice Niagara Account Password Reset";

                string bodyMessage = "<h1>Hello " + resetUser.FirstName + " " + resetUser.LastName + "</h1>" +
                   "<p>New Password is listed below, once entered you will be promted to change it</p>" +
                   "<p>Login Portal: http://www.hospiceniagara/portal.com</p>" +
                   "<p>Temporary Password: " + randomPassword + "</p>";
                   

                mailMessage.IsBodyHtml = true;
                mailMessage.Body = bodyMessage;
                client.Send(mailMessage);

                return RedirectToAction("Login");
            }            
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // GET: /Account/LogOff               
        [AllowAnonymous]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Login", "Account");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }


        [HttpPost]
        public JsonResult doesEmailNameExist(string Email)
        {  
            using(ApplicationDbContext db = new ApplicationDbContext())
            {
                try
                {
                    var email = db.Users.Single(m => m.Email == Email);
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
                catch(Exception)
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
            }
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }        



        #endregion
    }
}