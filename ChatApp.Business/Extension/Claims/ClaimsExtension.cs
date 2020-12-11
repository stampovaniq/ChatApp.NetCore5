using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace ChatApp.Business.Extension.Claims
{
    public static class ClaimsExtension
    {
        public static Guid GetLoggedInUserId(this ClaimsPrincipal principal)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));

            return new Guid(principal.FindFirstValue(ClaimTypes.NameIdentifier));
        }

        public static string GetLoggedInUserName(this ClaimsPrincipal principal)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));

            return principal.FindFirstValue(ClaimTypes.Name);
        }

        public static string GetLoggedInUserEmail(this ClaimsPrincipal principal)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));

            return principal.FindFirstValue(ClaimTypes.Email);
        }

    }
}
