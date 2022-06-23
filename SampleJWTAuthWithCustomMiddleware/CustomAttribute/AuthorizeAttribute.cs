using System;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SampleJWTAuthWithCustomMiddleware.CustomAttribute
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = (User)context.HttpContext.Items["User"];

            if (user == null)
                context.Result = new UnauthorizedResult();
                //context.Result = new JsonResult(new { message = "Unauthorize"}) {StatusCode = StatusCodes.Status401Unauthorized};
        }
    }
}
