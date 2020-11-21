﻿using ComputerService.Core.Dto.Request;
using ComputerService.Core.Dto.Response;
using ComputerService.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ComputerService.Core.Interfaces.Services
{
    public interface IRepairService
    {
        Task<int> CreateRepairAsync(CancellationToken cancelationToken);

        Task<RepairDetailsResponse> GetRepairAsync(int id, CancellationToken cancellationToken);
    }
}
