﻿namespace Contests.App.Areas.Admin.Controllers
{
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using AutoMapper.QueryableExtensions;
    using Contest.App;
    using Data.UnitOfWork;
    using Microsoft.AspNet.Identity;
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
            var users = this.ContestsData.Users.All()
                .Where(u => u.Id != this.UserProfile.Id)
                .Project()
                .To<UserViewModel>()
                .ToList();

            foreach (var user in users)
            {
                var role = this.UserManager.GetRoles(user.Id).FirstOrDefault();
                user.Role = role;
            }

            users = users
                .OrderBy(u => u.Role)
                .ThenBy(u => u.FullName)
                .ToList();

            return View(users);
        }

        // GET: Admin/Users/Edit
        public ActionResult EditDetails(string id)
        {
            var user = this.ContestsData.Users.All()
                .Where(u => u.Id == id)
                .Project()
                .To<UserEditBindingModel>()
                .FirstOrDefault();

            if (user == null)
            {
                this.AddToastMessage("Error", "Non-existing user.", ToastType.Error);
                return this.RedirectToAction("Index");
            }

            return View(user);
        }

        // POST: Admin/Users/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditDetails(string id, UserEditBindingModel model)
        {
            if (this.ModelState.IsValid)
            {
                var user = this.ContestsData.Users.Find(id);
                if (user == null)
                {
                    this.AddToastMessage("Error", "Non-existing user.", ToastType.Error);
                    return this.RedirectToAction("Index");
                }

                user.FullName = model.FullName;
                user.Email = model.Email;
                user.PhoneNumber = model.PhoneNumber;

                var result = this.UserManager.Update(user);

                //this.ContestsData.SaveChanges();
                
                this.AddToastMessage("Success", "User edited.", ToastType.Success);
                return this.RedirectToAction("Index");
            }

            return View(model);
        }

        // GET: Admin/Users/Edit
        public ActionResult ChangeRole(string id)
        {
            var user = this.ContestsData.Users.All()
                .Where(u => u.Id == id)
                .Project()
                .To<ChangeRoleBindingModel>()
                .FirstOrDefault();

            if (user == null)
            {
                this.AddToastMessage("Error", "Non-existing user.", ToastType.Error);
                return this.RedirectToAction("Index");
            }

            user.Role = this.UserManager.GetRoles(id).FirstOrDefault();

            return View(user);
        }

        // GET: Admin/Users/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeRole(string id, ChangeRoleBindingModel model)
        {
            var user = this.ContestsData.Users.Find(id);

            if (user == null)
            {
                this.AddToastMessage("Error", "Non-existing user.", ToastType.Error);
                return this.RedirectToAction("Index");
            }

            var roles = this.UserManager.GetRoles(id);

            if (roles.Contains(model.Role))
            {
                this.AddToastMessage("Info", "No role change needed.", ToastType.Info);
                return this.RedirectToAction("Index");
            }

            foreach (var role in roles)
            {
                var a = this.UserManager.RemoveFromRole(user.Id, role);
            }

            var result = this.UserManager.AddToRole(user.Id, model.Role);

            if (result.Succeeded)
            {
                this.AddToastMessage("Success", "Role changed.", ToastType.Success);
            }
            else
            {
                this.AddToastMessage("Error", "Error changing user role.", ToastType.Error);
            }

            return this.RedirectToAction("Index");
        }
    }
}