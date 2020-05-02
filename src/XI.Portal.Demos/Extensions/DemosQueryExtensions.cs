﻿using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using XI.CommonTypes;
using XI.Portal.Auth.Contract.Extensions;
using XI.Portal.Data.Legacy.Models;
using XI.Portal.Demos.Models;

namespace XI.Portal.Demos.Extensions
{
    public static class DemosQueryExtensions
    {
        public static IQueryable<Demoes> ApplyFilter(this IQueryable<Demoes> demos, DemosFilterModel filterModel)
        {
            demos = demos.Include(d => d.User).AsQueryable();

            if (filterModel.UserId != null && filterModel.GameTypes.Count != 0)
                demos = demos.Where(d => filterModel.GameTypes.Contains(d.Game) && d.UserId == filterModel.UserId).AsQueryable();
            else if (filterModel.GameTypes.Count != 0)
                demos = demos.Where(d => filterModel.GameTypes.Contains(d.Game)).AsQueryable();
            else if (filterModel.UserId != null)
                demos = demos.Where(d => d.UserId == filterModel.UserId).AsQueryable();

            if (!string.IsNullOrWhiteSpace(filterModel.FilterString))
                demos = demos.Where(d => d.Name.Contains(filterModel.FilterString) || d.User.UserName.Contains(filterModel.FilterString)).AsQueryable();

            switch (filterModel.Order)
            {
                case DemosFilterModel.OrderBy.GameTypeAsc:
                    demos = demos.OrderBy(d => d.Game).AsQueryable();
                    break;
                case DemosFilterModel.OrderBy.GameTypeDesc:
                    demos = demos.OrderByDescending(d => d.Game).AsQueryable();
                    break;
                case DemosFilterModel.OrderBy.NameAsc:
                    demos = demos.OrderBy(d => d.Name).AsQueryable();
                    break;
                case DemosFilterModel.OrderBy.NameDesc:
                    demos = demos.OrderByDescending(d => d.Name).AsQueryable();
                    break;
                case DemosFilterModel.OrderBy.DateAsc:
                    demos = demos.OrderBy(d => d.Date).AsQueryable();
                    break;
                case DemosFilterModel.OrderBy.DateDesc:
                    demos = demos.OrderByDescending(d => d.Date).AsQueryable();
                    break;
                case DemosFilterModel.OrderBy.UploadedByAsc:
                    demos = demos.OrderBy(d => d.User.UserName).AsQueryable();
                    break;
                case DemosFilterModel.OrderBy.UploadedByDesc:
                    demos = demos.OrderByDescending(d => d.User.UserName).AsQueryable();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            demos = demos.Skip(filterModel.SkipEntries).AsQueryable();

            if (filterModel.TakeEntries != 0) demos = demos.Take(filterModel.TakeEntries).AsQueryable();

            return demos;
        }
    }
}