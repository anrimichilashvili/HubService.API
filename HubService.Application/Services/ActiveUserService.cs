using HubService.Application.Interfaces.Services.HubService.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HubService.Application.Services
{
    public class ActiveUserService : IActiveUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ActiveUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string? UserId
        {
            get
            {
                return _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            }
        }
    }
}
