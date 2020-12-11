using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Authentication;
using System.Threading.Tasks;
using ChatApp.Business.Common.Enums;
using ChatApp.Business.Common.NavigationPages;
using ChatApp.Business.Core.Authentication;
using ChatApp.Business.Extension.ModelState;
using ChatApp.Models.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace ChatApp.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly NavigationPage _navigationPage;
        private readonly ApplicationUser _applicationUser;

        public AccountController(NavigationPage navigationPage, ApplicationUser applicationUser)
        {
            this._navigationPage = navigationPage;
            this._applicationUser = applicationUser;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(AuthenticationViewModel model, string returnUrl = null)
        {
            try
            {
                bool isExist = await this._applicationUser.HasUser(model.UserName);

                if (!isExist) throw new AuthenticationException(string.Format("{0} does not exist!", model.UserName));

                Microsoft.AspNetCore.Identity.SignInResult result = await this._applicationUser.PasswordSignInAsync(model.UserName, model.Password, isPersistent: false, lockoutOnFailure: true);

                if (result.Succeeded)
                {
                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction(nameof(HomeController.Index), this._navigationPage.Controller<HomeController>());
                    }
                }
                else
                {
                    if (result.IsNotAllowed) throw new AuthenticationException(string.Format("{0} is not allowed!", model.UserName));
                    if (result.IsLockedOut) throw new AuthenticationException(string.Format("{0} is locked out!", model.UserName));
                }
                throw new AuthenticationException("User or password incorrect!");
            }
            catch (AuthenticationException ex)
            {
                if (ModelState.TryAddModelException("authenticationException", ex))
                {
                    ModelState.ErrorMessage("authenticationException", model);
                }
            }
            catch (Exception ex)
            {
                if (ModelState.TryAddModelException("exception", ex))
                {
                    ModelState.CustomErrorMessage("Something went wrong!", model);
                }
            }

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    IdentityResult result = await this._applicationUser.CreateAsync(model);

                    if (result.Succeeded)
                    {
                        IdentityResult roleResult = await this._applicationUser.AddToRoleAsync(model.UserName, Enum.GetName(typeof(EnumApplicationRoles), EnumApplicationRoles.User));
                        if (roleResult.Succeeded)
                        {
                            model.MessageType = EnumMessageType.Success;
                            model.NotificationMessage = "Account created successfully!";
                        }
                        else
                        {
                            IdentityResult resultDelete = await this._applicationUser.DeleteAsync(model.UserName);
                            if (!resultDelete.Succeeded) throw new InvalidOperationException("Something went wrong!");
                        }
                    }
                    else
                    {
                        throw new InvalidOperationException("Failed to create account!");
                    }
                }
                else
                {
                    throw new InvalidOperationException("Invalid User Data!");
                }
            }
            catch (InvalidOperationException ex)
            {
                if (ModelState.TryAddModelException("invalidOperationException", ex))
                {
                    ModelState.ErrorMessage("invalidOperationException", model);
                }
            }
            catch (Exception ex)
            {
                if (ModelState.TryAddModelException("exception", ex))
                {
                    ModelState.CustomErrorMessage("Something went wrong!", model);
                }
            }

            return View("Register", model);
        }

        [HttpGet]
        public async Task<IActionResult> LogOut(string returnUrl = null)
        {
            await this._applicationUser.SignOutAsync();
            return Redirect(returnUrl);
        }
    }
}
