namespace MarketPlaceService.Utils
{
    using Microsoft.IdentityModel.Tokens;
    using System;
    using System.Collections;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;

    public class TokVal
    {

        public static String? GetIdFromClaim(IDictionary<String, String> claims) {

            var has_id = claims.TryGetValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", out String? id);

            if(has_id && id != null)
            {
                return id;
            }

            return null;
        }
        
        public static IDictionary<String, String> ValidateJwt(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("super-long-sercret-for-testing-because-it-needs-to-be-this-is-dumb")); // Replace with your secret key

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateIssuer = false,
                ValidIssuer = "Iamanissuer",
                ValidateAudience = false,
                ValidAudience = "Iamanaudience",
                ValidateLifetime = false,
                ClockSkew = TimeSpan.Zero // Optional: set clock skew to zero
            };

            try
            {
                ClaimsPrincipal claimsPrincipal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);

                Console.WriteLine("Token is valid. Claims:");
                var claims = new Dictionary<String, String>();
                foreach (var claim in claimsPrincipal.Claims)
                {
                    claims.Add(claim.Type, claim.Value.ToString());
                }

                return claims;
            }
            catch (Exception ex)
            {
                
                throw new Exception("Invalid token", ex);
            }
        }
    }
}
