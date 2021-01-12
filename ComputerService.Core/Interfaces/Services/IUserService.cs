﻿using ComputerService.Core.Dto.Request;
using ComputerService.Core.Dto.Response;
using ComputerService.Core.Models;
using ComputerService.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ComputerService.Core.Interfaces.Services
{
    public interface IUserService
    {
        Task<UserModel> GetUserAsync(int userId, CancellationToken cancellationToken);
        Task<List<GetCustomersResponse>> GetUsersFromRoleAsync(string role, CancellationToken cancellationToken);
        Task<bool> CheckUserInRoleAsync(string role, CancellationToken cancellationToken);
        Task<string> CheckUserRoleAsync(CancellationToken cancellationToken);
        Task<List<GetUsersWithRolesResponse>> GetUsersWithRolesAsync(CancellationToken cancellationToken);
        Task<User> GetCurrentLoggedUserAsync(CancellationToken cancellationToken);
        Task UpdateUserInfoAsync(UpdateUserInfoRequest request, CancellationToken cancellationToken);
    }
}
