using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace Continental.API.Core.Helpers
{
    public static class TokenHelper
    {
        public static string ExtraerClaimDelToken(string token, string claimType)
        {
            var handler       = new JwtSecurityTokenHandler();
            var tokenSecurity = handler.ReadToken(token) as JwtSecurityToken;

            return tokenSecurity.Claims.First(claim => claim.Type == claimType).Value;
        }
    }
}
