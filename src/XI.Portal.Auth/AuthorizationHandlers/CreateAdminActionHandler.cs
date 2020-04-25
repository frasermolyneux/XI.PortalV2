﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using XI.CommonTypes;
using XI.Portal.Auth.AuthorizationRequirements;
using XI.Portal.Auth.Contract.Constants;
using XI.Portal.Players.Dto;

namespace XI.Portal.Auth.AuthorizationHandlers
{
    public class CreateAdminActionHandler : AuthorizationHandler<CreateAdminAction, AdminActionDto>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CreateAdminAction requirement, AdminActionDto resource)
        {
            if (context.User.HasClaim(claim => claim.Type == XtremeIdiotsClaimTypes.SeniorAdmin))
                context.Succeed(requirement);

            if (context.User.HasClaim(XtremeIdiotsClaimTypes.HeadAdmin, resource.GameType.ToString()))
                context.Succeed(requirement);

            if (context.User.HasClaim(XtremeIdiotsClaimTypes.GameAdmin, resource.GameType.ToString()))
                context.Succeed(requirement);

            switch (resource.Type)
            {
                case AdminActionType.Observation:
                    if (context.User.HasClaim(XtremeIdiotsClaimTypes.Moderator, resource.GameType.ToString()))
                        context.Succeed(requirement);
                    break;
                case AdminActionType.Warning:
                    if (context.User.HasClaim(XtremeIdiotsClaimTypes.Moderator, resource.GameType.ToString()))
                        context.Succeed(requirement);
                    break;
                case AdminActionType.Kick:
                    if (context.User.HasClaim(XtremeIdiotsClaimTypes.Moderator, resource.GameType.ToString()))
                        context.Succeed(requirement);
                    break;
            }

            return Task.CompletedTask;
        }
    }
}