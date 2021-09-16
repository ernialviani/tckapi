using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using TicketingApi.Entities;
using System.Collections.Generic;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
    // private readonly IList<Role> _roles;

    // public AuthorizeAttribute(params Role[] roles)
    // {
    //     _roles = roles ?? new Role[] { };
    // }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if(context.HttpContext.Items["TokenType"] == null){
            context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
        }
        else if(context.HttpContext.Items["ErrorMessage"] != null){
            context.Result = new JsonResult(new { message = context.HttpContext.Items["ErrorMessage"] }) { StatusCode = StatusCodes.Status500InternalServerError };
        }
    }
}