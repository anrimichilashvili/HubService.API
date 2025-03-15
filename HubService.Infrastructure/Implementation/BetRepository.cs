using HubService.Application.Interfaces.Repositories;
using HubService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HubService.Infrastructure.Implementation
{
    public class BetRepository : Repository<BetEntity>, IBetRepository
    {
        public BetRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
