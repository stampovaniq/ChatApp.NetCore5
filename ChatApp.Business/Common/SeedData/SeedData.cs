using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ChatApp.Business.Common.Enums;
using ChatApp.DataAccess.Models.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace ChatApp.Business.Common.SeedData
{
    public class SeedData
    {
        private readonly IServiceProvider _provider;

        private ApplicationUser User { get; set; }

        private string Role => Enum.GetName(typeof(EnumApplicationRoles), EnumApplicationRoles.Administrator);

        private const string userPassword = "12345678";

        public SeedData(IServiceProvider provider)
        {
            this._provider = provider;
            this.User = new ApplicationUser() { FirstName = "Petar", LastName = "Petrov", Email = "test@test.com", UserName = "Admin" };
        }

        public async Task InitSeedDataAsync()
        {
            using (IServiceScope scope = this._provider.CreateScope())
            {
                await this.CreateRoles(scope);
                await this.CreateAdmin(scope);
            }
        }

        private async Task CreateRoles(IServiceScope scope)
        {
            RoleManager<IdentityRole<Guid>> role = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
            bool isAdminRoleExist = await role.RoleExistsAsync("Administrator");
            if (!isAdminRoleExist) await role.CreateAsync(new IdentityRole<Guid>("Administrator"));
            bool isUserRoleExist = await role.RoleExistsAsync("User");
            if (!isUserRoleExist) await role.CreateAsync(new IdentityRole<Guid>("User"));
        }

        private async Task CreateAdmin(IServiceScope scope)
        {
            UserManager<ApplicationUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            if (!this.HasUserAdmin(userManager).Result)
            {
                IdentityResult result = await userManager.CreateAsync(this.User, userPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(this.User, this.Role);
                }
            }
        }

        private async Task<bool> HasUserAdmin(UserManager<ApplicationUser> userManager)
        {
            return ((await userManager.FindByNameAsync(this.User.UserName) != null) && (await userManager.FindByEmailAsync(this.User.Email) != null));
        }
    }
}
