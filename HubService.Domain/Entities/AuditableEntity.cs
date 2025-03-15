using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HubService.Domain.Entities
{
    public class AuditableEntity : BaseEntity
    {
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public DateTime? UpdateDate { get; set; }
        public string CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public virtual DateTime? DateDeleted { get; set; }
        public virtual string? DeletedBy { get; set; }
    }
}
