using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using DM = ChatApp.DataAccess.Models.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace ChatApp.Business.Core.Authentication
{
    public class ApplicationUser
    {
        private readonly IServiceProvider _provider;

        public ApplicationUser(IServiceProvider provider)
        {
            this._provider = provider;
        }

        #region Public Methods

        public async Task<bool> HasUser(string userName)
        {
            return await GetUser(userName) != null;
        }

        public async Task<bool> HasUser(Guid id)
        {
            using (IServiceScope scope = this._provider.CreateScope())
            {
                return await this.GetUserByIdAsync(scope, id) != null;
            }
        }

        public async Task<DM.ApplicationUser> GetUser(string userName)
        {
            using (IServiceScope scope = this._provider.CreateScope())
            {
                return await this.GetUserByNameAsync(scope, userName);
            }
        }

        public List<DM.ApplicationUser> GetUsers()
        {
            using (IServiceScope scope = this._provider.CreateScope())
            {
                return scope.ServiceProvider.GetRequiredService<UserManager<DM.ApplicationUser>>().Users.ToList();
            }
        }

        public async Task<bool> Activated(Guid id, bool isActive)
        {

            return await this.SetEmailConfirmationAsync(id, isActive);

        }

        public async Task<SignInResult> PasswordSignInAsync(string userName, string password, bool isPersistent = default(bool), bool lockoutOnFailure = default(bool))
        {
            using (IServiceScope scope = this._provider.CreateScope())
            {
                DM.ApplicationUser user = await this.GetUserByNameAsync(scope, userName);

                return await scope.ServiceProvider.GetRequiredService<SignInManager<DM.ApplicationUser>>().PasswordSignInAsync(user, password, isPersistent: false, lockoutOnFailure: true);
            }

        }

        public async Task SignOutAsync()
        {
            using (IServiceScope scope = this._provider.CreateScope())
            {
                await scope.ServiceProvider.GetRequiredService<SignInManager<DM.ApplicationUser>>().SignOutAsync();
            }

        }

        public async Task<IdentityResult> CreateAsync(dynamic user)
        {
            using (IServiceScope scope = this._provider.CreateScope())
            {
                DM.ApplicationUser applicationUser = new DM.ApplicationUser()
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    UserName = user.UserName
                };
                return await scope.ServiceProvider.GetRequiredService<UserManager<DM.ApplicationUser>>().CreateAsync(applicationUser, user.Password);
            }
        }

        public async Task<IdentityResult> AddToRoleAsync(string userName, string role)
        {
            using (IServiceScope scope = this._provider.CreateScope())
            {
                DM.ApplicationUser user = await this.GetUserByNameAsync(scope, userName);

                return await scope.ServiceProvider.GetRequiredService<UserManager<DM.ApplicationUser>>().AddToRoleAsync(user, role);
            }
        }

        public async Task<IdentityResult> DeleteAsync(string userName)
        {
            using (IServiceScope scope = this._provider.CreateScope())
            {
                DM.ApplicationUser user = await this.GetUserByNameAsync(scope, userName);

                return await scope.ServiceProvider.GetRequiredService<UserManager<DM.ApplicationUser>>().DeleteAsync(user);
            }
        }

        #endregion

        #region Private Methods

        private async Task<DM.ApplicationUser> GetUserByNameAsync(IServiceScope scope, string userName)
        {
            return await scope.ServiceProvider.GetRequiredService<UserManager<DM.ApplicationUser>>().FindByNameAsync(userName);
        }

        private async Task<DM.ApplicationUser> GetUserByIdAsync(IServiceScope scope, Guid id)
        {
            string userId = id.ToString();

            return await scope.ServiceProvider.GetRequiredService<UserManager<DM.ApplicationUser>>().FindByIdAsync(userId);
        }

        private async Task<bool> SetEmailConfirmationAsync(Guid id, bool isConfirm)
        {
            bool isSucceeded = default(bool);

            using (IServiceScope scope = this._provider.CreateScope())
            {

                DM.ApplicationUser user = await this.GetUserByIdAsync(scope, id);

                if (user != null)
                {
                    user.EmailConfirmed = isConfirm;

                    UserManager<DM.ApplicationUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<DM.ApplicationUser>>();

                    string token = await userManager.GenerateEmailConfirmationTokenAsync(user); /*without token  EmailConfirmed can't be updated */

                    IdentityResult result = await userManager.UpdateAsync(user); /* await userManager.ConfirmEmailAsync(user, token); */

                    isSucceeded = result.Succeeded;
                }

            }

            return isSucceeded;
        }

        #endregion
    }
}
