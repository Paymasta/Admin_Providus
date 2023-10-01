using Microsoft.IdentityModel.Tokens;
using PayMasta.ViewModel.Account;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Utilities
{
    public class JwtTokenUtils
    {
        private string JwtKey = AppSetting.JwtKey;
        private double JwtExpireDays = Convert.ToDouble(AppSetting.JwtExpireDays);
        private string JwtIssuer = AppSetting.JwtIssuer;

        public string GenerateToken(LoginResponse user)
        {
            // return GenerateToken(user.FirstName, user.UserGuid.ToString(), user.UserId);
            var claims = new List<Claim>{
                new Claim(JwtRegisteredClaimNames.Typ, user.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, user.UserGuid.ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, user.MobileNumber.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.FirstName+" "+user.LastName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(JwtExpireDays);

            var token = new JwtSecurityToken(
                JwtIssuer,
                JwtIssuer,
                claims,
                expires: expires,
                signingCredentials: creds
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public LoginResponse GetCurrentUser(HttpRequestMessage request)
        {
            var result = new LoginResponse();
            try
            {
                var rawToken = request.Headers.GetValues("Authorization").FirstOrDefault();
                var token = rawToken.ToString().Substring("Bearer ".Length);
                string secret = "db3OIsj+BXE9NZDy0t8W3TcNekrF+2d/1sFnWG4HnV8TZY30iTOdtVWJG8abWvB1GlOgJuQZdcF2Luqm/hccMw==";

                var key = Encoding.ASCII.GetBytes(secret);
                var handler = new JwtSecurityTokenHandler();
                var tokenSecure = handler.ReadToken(token) as SecurityToken;

                var validations = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
                var time = tokenSecure.ValidTo;
                if (time.Date > DateTime.UtcNow)
                {
                    var claims = handler.ValidateToken(token, validations, out tokenSecure);
                    result.UserGuid = Guid.Parse(claims.Claims.FirstOrDefault(claim => claim.Type == "jti").Value);
                    result.UserId = Convert.ToInt64(claims.Claims.FirstOrDefault(claim => claim.Type == "typ").Value);
                    result.MobileNumber = Convert.ToString(claims.Claims.FirstOrDefault(claim => claim.Type == "iat").Value);
                    result.FirstName = claims.Claims.FirstOrDefault(claim => claim.Type == "NameIdentifier").Value;

                }

            }
            catch (Exception ex)
            {

            }

            return result;
        }

      
    }
}
