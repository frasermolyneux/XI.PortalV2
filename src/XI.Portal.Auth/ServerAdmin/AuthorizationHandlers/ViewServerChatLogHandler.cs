﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using XI.Portal.Auth.Contract.Constants;
using XI.Portal.Auth.ServerAdmin.AuthorizationRequirements;
using XI.Portal.Servers.Dto;

namespace XI.Portal.Auth.ServerAdmin.AuthorizationHandlers
{
    public class ViewServerChatLogHandler : AuthorizationHandler<ViewServerChatLog, GameServerDto>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ViewServerChatLog requirement, GameServerDto resource)
        {
            if (context.User.HasClaim(claim => claim.Type == XtremeIdiotsClaimTypes.SeniorAdmin))
                context.Succeed(requirement);

            if (context.User.HasClaim(XtremeIdiotsClaimTypes.HeadAdmin, resource.GameType.ToString()))
                context.Succeed(requirement);

            if (context.User.HasClaim(XtremeIdiotsClaimTypes.GameAdmin, resource.GameType.ToString()))
                context.Succeed(requirement);

            if (context.User.HasClaim(XtremeIdiotsClaimTypes.Moderator, resource.GameType.ToString()))
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}