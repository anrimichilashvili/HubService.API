using HubService.Application.Interfaces.Services.HubService.Application.Interfaces.Services;
using HubService.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HubService.Infrastructure
{
    public class ApplicationDbContext: IdentityDbContext
    {
        private readonly IActiveUserService _user;
        public ApplicationDbContext(DbContextOptions options, IActiveUserService activeUserService)
            : base(options)
        {
            _user = activeUserService;
        }
        public DbSet<BetEntity> Bets { get; set; }


        public override int SaveChanges()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                Audition(entry);
            }

            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                Audition(entry);
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        private void Audition(EntityEntry entry)
        {
            var entity = entry.Entity;
            var entityType = entity.GetType();

            switch (entry.State)
            {
                case EntityState.Added:
                    SetPropertyIfExists(entity, entityType, "CreateDate", DateTime.Now);
                    SetPropertyIfExists(entity, entityType, "CreatedBy", _user?.UserId);
                    break;

                case EntityState.Modified:
                    SetPropertyIfExists(entity, entityType, "UpdateDate", DateTime.Now);
                    SetPropertyIfExists(entity, entityType, "UpdatedBy", _user?.UserId);

                    // Prevent changes to specific properties
                    SetIsModified(entry, "CreateDate", false);
                    SetIsModified(entry, "CreatedBy", false);
                    SetIsModified(entry, "DeletedBy", false);
                    SetIsModified(entry, "DateDeleted", false);
                    break;

                case EntityState.Deleted:
                    entry.State = EntityState.Unchanged;
                    SetPropertyIfExists(entity, entityType, "DateDeleted", DateTime.Now);
                    SetPropertyIfExists(entity, entityType, "DeletedBy", _user?.UserId);
                    break;
            }
        }


        private void SetPropertyIfExists(object entity, Type entityType, string propertyName, object? value)
        {
            var property = entityType.GetProperty(propertyName);
            if (property != null && property.CanWrite)
            {
                property.SetValue(entity, value);
            }
        }

        private void SetIsModified(EntityEntry entry, string propertyName, bool isModified)
        {
            if (entry.Properties.Any(p => p.Metadata.Name == propertyName))
            {
                entry.Property(propertyName).IsModified = isModified;
            }
        }
    }
}
