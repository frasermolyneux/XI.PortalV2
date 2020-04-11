﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElCamino.AspNetCore.Identity.AzureTable.Model;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Azure.Cosmos.Table.Queryable;
using XI.Portal.Data.Auth;
using XI.Portal.Users.Models;

namespace XI.Portal.Users.Repository
{
    public class UsersRepository : IUsersRepository
    {
        private readonly ApplicationAuthDbContext _authContext;

        public UsersRepository(ApplicationAuthDbContext authContext)
        {
            _authContext = authContext ?? throw new ArgumentNullException(nameof(authContext));
        }

        public async Task<List<UserListEntryViewModel>> GetUsers()
        {
            var query = (from user in _authContext.UserTable.CreateQuery<IdentityUser>()
                where user.Email != ""
                select user).AsTableQuery();

            var results = new List<UserListEntryViewModel>();

            TableContinuationToken continuationToken = null;
            do
            {
                var queryResult = await query.ExecuteSegmentedAsync(continuationToken);
                foreach (var entity in queryResult)
                    results.Add(new UserListEntryViewModel
                    {
                        Id = entity.Id,
                        Username = entity.UserName,
                        Email = entity.Email
                    });

                continuationToken = queryResult.ContinuationToken;
            } while (continuationToken != null);

            return results;
        }
    }
}