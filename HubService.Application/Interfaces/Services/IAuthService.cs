﻿using HubService.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HubService.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<ResultDTO> RegisterUser(UserModelDto user);
        Task<ResultDTO> Login(UserModelDto user);
        LoginResponseDto GenerateToeknString(UserModelDto user);
    }
}
