﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using XI.Portal.Auth.AdminActions.AuthorizationRequirements;
using XI.Portal.Auth.Contract.Constants;

namespace XI.Portal.Auth.AdminActions.AuthorizationHandlers
{
    public class AccessAdminActionsHandler : AuthorizationHandler<AccessAdminActions>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AccessAdminActions requirement)
        {
            if (context.User.HasClaim(claim => claim.Type == XtremeIdiotsClaimTypes.SeniorAdmin))
                context.Succeed(requirement);

            if (context.User.HasClaim(claim => claim.Type == XtremeIdiotsClaimTypes.HeadAdmin))
                context.Succeed(requirement);

            if (context.User.HasClaim(claim => claim.Type == XtremeIdiotsClaimTypes.GameAdmin))
                context.Succeed(requirement);

            if (context.User.HasClaim(claim => claim.Type == XtremeIdiotsClaimTypes.Moderator))
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}