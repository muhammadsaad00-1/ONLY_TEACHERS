using OT.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using OT.Areas.Identity.Data;
using ContactManager.Authorization;


namespace OT.Authorization
{
    public class PostIsOwnerAuthorizationHandler
                : AuthorizationHandler<OperationAuthorizationRequirement, Post>
    {
        UserManager<OTUser> _userManager;

        public PostIsOwnerAuthorizationHandler(UserManager<OTUser>
            userManager)
        {
            _userManager = userManager;
        }

        protected override Task
            HandleRequirementAsync(AuthorizationHandlerContext context,
                                   OperationAuthorizationRequirement requirement,
                                   Post resource)
        {
            if (context.User == null || resource == null)
            {
                return Task.CompletedTask;
            }

            // If not asking for CRUD permission, return.

            if (requirement.Name != Constants.CreateOperationName &&
                requirement.Name != Constants.ReadOperationName  &&
                requirement.Name !=Constants.UpdateOperationName  &&
                requirement.Name != Constants.DeleteOperationName )
            {
                return Task.CompletedTask;
            }

            if (resource.OwnerID == _userManager.GetUserId(context.User))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}