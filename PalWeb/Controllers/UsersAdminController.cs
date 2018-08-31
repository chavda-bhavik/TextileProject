using PalWeb.App_Start;
using PalWeb.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Pal.Services;
using Pal.Entities.Models;
using Repository.Pattern.UnitOfWork;
namespace PalWeb.Controllers
{
    public class UsersAdminController : BaseAdminController
    {
		public UsersAdminController()
		{
		}

		//private IBusinessRoleService _businessRoleService;
		private IUnitOfWorkAsync _unitOfWork;
		public UsersAdminController(ApplicationUserManager userManager, ApplicationRoleManager roleManager, IUnitOfWorkAsync unitOfWork /*,IBusinessRoleService businessRoleService*/)
		{
			UserManager = userManager;
			RoleManager = roleManager;
			//_businessRoleService = businessRoleService;
			_unitOfWork = unitOfWork;
		}

		private ApplicationUserManager _userManager;
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

		private ApplicationRoleManager _roleManager;
		public ApplicationRoleManager RoleManager
		{
			get
			{
				return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
			}
			private set
			{
				_roleManager = value;
			}
		}

		//
		// GET: /Users/
		public async Task<ActionResult> Index()
		{
			return View(await UserManager.Users.ToListAsync());
		}

		//
		// GET: /Users/Details/5
		public async Task<ActionResult> Details(string id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			var user = await UserManager.FindByIdAsync(id);

			ViewBag.RoleNames = await UserManager.GetRolesAsync(user.Id);

			return View(user);
		}

		//
		// GET: /Users/Create
		public async Task<ActionResult> Create()
		{
			//Get the list of Roles
			ViewBag.RoleId = new SelectList(await RoleManager.Roles.ToListAsync(), "Name", "Name");
			return View();
		}

		//
		// POST: /Users/Create
		[HttpPost]
		public async Task<ActionResult> Create(RegisterViewModel userViewModel, int[] businessRole, params string[] selectedRoles)
		{
			if (ModelState.IsValid)
			{
				try
				{


					var user = new ApplicationUser { UserName = userViewModel.Email, Email = userViewModel.Email};

					var adminresult = await UserManager.CreateAsync(user, userViewModel.Password);


					//Add User to the selected Roles 
					if (adminresult.Succeeded)
					{
						if (selectedRoles != null)
						{
							var result = await UserManager.AddToRolesAsync(user.Id, selectedRoles);
							if (!result.Succeeded)
							{
								ModelState.AddModelError("", result.Errors.First());
								ViewBag.RoleId = new SelectList(await RoleManager.Roles.ToListAsync(), "Name", "Name");
								return View();
							}
						}

						//if (userViewModel.CarehomeId.HasValue)
						//{
						//	var claim = new System.Security.Claims.Claim("CarehomeId", userViewModel.CarehomeId.GetValueOrDefault().ToString());
						//	var claimResult = await UserManager.AddClaimAsync(user.Id, claim);
						//	if (!claimResult.Succeeded)
						//	{
						//		ModelState.AddModelError("", claimResult.Errors.First());
						//		return View();
						//	}
						//}
						//if (businessRole.Length > 0)
						//{
						//	for (int i = 0; i < businessRole.Length; i++)
						//	{
						//		BusinessRole Br = new BusinessRole();
						//		Br.UserId = user.Id;
						//		int BusinessRoleTypeID = 0;
						//		int.TryParse(businessRole[i].ToString(), out BusinessRoleTypeID);
						//		Br.BusinessRoleTypeID = BusinessRoleTypeID;
						//		_businessRoleService.Insert(Br);
						//	}
						//	_unitOfWork.SaveChanges(User.Identity.Name);
						//}
					}
					else
					{
						ModelState.AddModelError("", adminresult.Errors.First());
						ViewBag.RoleId = new SelectList(RoleManager.Roles, "Name", "Name");
						return View();

					}


					return RedirectToAction("Index");
				}
				catch (Exception ex)
				{
					string s = ex.Message;
				}
			}
			ViewBag.RoleId = new SelectList(RoleManager.Roles, "Name", "Name");

			return View();
		}

		//
		// GET: /Users/Edit/1
		public async Task<ActionResult> Edit(string id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			var user = await UserManager.FindByIdAsync(id);
			if (user == null)
			{
				return HttpNotFound();
			}

			var userRoles = await UserManager.GetRolesAsync(user.Id);

			return View(new EditUserViewModel()
			{
				Id = user.Id,
				Email = user.Email,
				RolesList = RoleManager.Roles.ToList().Select(x => new SelectListItem()
				{
					Selected = userRoles.Contains(x.Name),
					Text = x.Name,
					Value = x.Name
				})
			});
		}

		//
		// POST: /Users/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Edit([Bind(Include = "Email,Id")] EditUserViewModel editUser, params string[] selectedRole)
		{
			if (ModelState.IsValid)
			{
				var user = await UserManager.FindByIdAsync(editUser.Id);
				if (user == null)
				{
					return HttpNotFound();
				}

				user.UserName = editUser.Email;
				user.Email = editUser.Email;

				var userRoles = await UserManager.GetRolesAsync(user.Id);

				selectedRole = selectedRole ?? new string[] { };

				var result = await UserManager.AddToRolesAsync(user.Id, selectedRole.Except(userRoles).ToArray<string>());

				if (!result.Succeeded)
				{
					ModelState.AddModelError("", result.Errors.First());
					return View();
				}
				result = await UserManager.RemoveFromRolesAsync(user.Id, userRoles.Except(selectedRole).ToArray<string>());

				if (!result.Succeeded)
				{
					ModelState.AddModelError("", result.Errors.First());
					return View();
				}
				return RedirectToAction("Index");
			}
			ModelState.AddModelError("", "Something failed.");
			return View();
		}

		//
		// GET: /Users/Delete/5
		public async Task<ActionResult> Delete(string id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			var user = await UserManager.FindByIdAsync(id);
			if (user == null)
			{
				return HttpNotFound();
			}
			return View(user);
		}

		//
		// POST: /Users/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> DeleteConfirmed(string id)
		{
			if (ModelState.IsValid)
			{
				if (id == null)
				{
					return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
				}

				var user = await UserManager.FindByIdAsync(id);
				if (user == null)
				{
					return HttpNotFound();
				}
				var result = await UserManager.DeleteAsync(user);
				if (!result.Succeeded)
				{
					ModelState.AddModelError("", result.Errors.First());
					return View();
				}
				return RedirectToAction("Index");
			}
			return View();
		}


		public ActionResult AddBusinessRole(string Userid)
		{
			return RedirectToAction("Create", "BusinessRole", new { Userid = Userid });
		}
	}
}