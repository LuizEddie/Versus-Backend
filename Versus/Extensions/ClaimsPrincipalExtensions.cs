using System.Security.Claims;
using Versus.Services.Exceptions;

namespace Versus.Extensions
{
    public static class ClaimsPrincipalExtensions
    {

        public static int GetUserId(this ClaimsPrincipal user)
        {
            var id = user.FindFirstValue(ClaimTypes.NameIdentifier);

            if(string.IsNullOrEmpty(id)) throw new TokenException("UserId não encontrado no token");

            return int.Parse(id);
        }
    }
}
