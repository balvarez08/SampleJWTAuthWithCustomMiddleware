using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Services;

namespace SampleJWTAuthWithCustomMiddleware.Middleware
{
    public class JWTMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IUserService _userService;

        public JWTMiddleware(RequestDelegate next, IUserService userService)
        {
            _next = next;
            _userService = userService;
        }

        public async Task Invoke(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null) await AddUserToHttpContext(context, token);

            await _next(context);
        }

        private async Task AddUserToHttpContext(HttpContext context, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                //TODO: replace with much better key and also where it is stored
                var key = Encoding.ASCII.GetBytes("ThisIsMyCustomSecretKeyAuthenticationSampleKey");

                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero, //It forces token to expire exactly at the token expiration time instead of waiting 5 minutes
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                };

                tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = int.Parse(jwtToken.Claims.FirstOrDefault(x => x.Type == "userId")?.Value);

                context.Items["User"] = await _userService.GetByIdAsync(userId);
             
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
