using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HubService.Domain.Entities
{
    public class BetEntity : AuditableEntity
    {
        public decimal Amount { get; set; }
        public DateTime PlacedAt { get; set; }
    }
}
