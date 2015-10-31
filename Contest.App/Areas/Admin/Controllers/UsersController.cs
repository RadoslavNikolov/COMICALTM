namespace Contests.App.Areas.Admin.Controllers
{
    using System.Data.Entity;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;
    using AutoMapper.QueryableExtensions;
    using Contest.App;
    using Data;
    using Data.UnitOfWork;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.AspNet.Identity.Owin;
    using Models.BindingModels;
    using Models.ViewModels;
    using Toastr;

    public class UsersController : BaseAdminController
    {
        private ApplicationSignInManager _signInManager;
        private UserManager _userManager;

        public UsersController(IContestsData data)
            : base(data)
        {
        }

        public UsersController(IContestsData data, UserManager userManager, ApplicationSignInManager signInManager)
            : base(data)
        {
            this.SignInManager = signInManager;
            this.UserManager = userManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

        public UserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<UserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        
        // GET: Admin/Users
        public ActionResult Index()
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ContestsDbContext()));

            var adminsIds = roleManager
                .FindByName("Admin")
                .Users
                .Select(u => u.UserId);

            var usersIds = roleManager
                .FindByName("User")
                .Users
                .Select(u => u.UserId);

            var admins = this.ContestsData.Users.All()
                .Where(u => adminsIds.Contains(u.Id))
                .Project()
                .To<UserViewModel>()
                .AsEnumerable();

            var users = this.ContestsData.Users.All()
                .Where(u => usersIds.Contains(u.Id))
                .Project()
                .To<UserViewModel>();

            var adminsAndUsers = new AdminsAndUsersViewModel
            {
                Admins = admins,
                Users = users
            };

            return View(adminsAndUsers);
        }

        // GET: Admin/Users/Edit
        public ActionResult Edit(string id)
        {
            var user = this.ContestsData.Users.All()
                .Where(u => u.Id == id)
                .Project()
                .To<UserEditBindingModel>()
                .FirstOrDefault();

            if (user == null)
            {
                this.AddToastMessage("Error", "Non-existing user.", ToastType.Error);
                this.RedirectToAction("Index");
            }

            var role = this.UserManager.GetRoles(id).FirstOrDefault();
            user.Role = role;

            return View(user);
        }

        // POST: Admin/Users/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit()
        {
            return View();
        }
    }
}