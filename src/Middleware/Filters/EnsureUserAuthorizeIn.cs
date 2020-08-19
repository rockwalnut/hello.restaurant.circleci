using System;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Threading.Tasks;
using hello.restaurant.api.Exceptions;
using hello.restaurant.api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

namespace hello.restaurant.api.Filters
{
    /* public abstract class ActionFilterAttribute : Attribute, IActionFilter, IFilterMetadata, 
                IAsyncActionFilter, IResultFilter, IAsyncResultFilter, IOrderedFilter
    {

    }*/

    /* 
    public class AsyncActionFilterExample : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // execute any code before the action executes
            var result = await next();
            // execute any code after the action executes
        }
    }*/

    public class EnsureUserAuthorizeInAsync : IAsyncActionFilter
    {

        private readonly IHttpContextAccessor _httpContextAccessor;

        // private readonly IUserService _userService;
        public EnsureUserAuthorizeInAsync(IHttpContextAccessor httpContextAccessor)

        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            //_userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }


        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var userHttpContextIsAuthenticated = true;
            var userIsAssignedAdGroup = true;

            //var 
            if (!userHttpContextIsAuthenticated)
            {
                throw new UserNotAuthenException();
            }

            if (!userIsAssignedAdGroup)
                throw new UserNotAuthorizeException();

            //return;

            // next() calls the action method.
            var resultContext = await next();
            // resultContext.Result is set.
            // Do something after the action executes.
        }
    }

    public class EnsureUserAuthorizeIn : ActionFilterAttribute
    {

        public EnsureUserAuthorizeIn()
        {

        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {

        }

    }
}

