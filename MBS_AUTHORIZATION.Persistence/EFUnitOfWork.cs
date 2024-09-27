
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using MBS_AUTHORIZATION.Domain.Abstractions;
using MBS_AUTHORIZATION.Domain.Abstractions.Entities;

namespace MBS_AUTHORIZATION.Persistence;

public class EFUnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _dbContext;

    public EFUnitOfWork(ApplicationDbContext dbContext)
        => _dbContext = dbContext;

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        //ConvertDomainEventsToOutboxMessages();
        //UpdateAuditableEntities();
        await _dbContext.SaveChangesAsync();
    }

    async ValueTask IAsyncDisposable.DisposeAsync()
    => await _dbContext.DisposeAsync();

   
    private void UpdateAuditableEntities()
    {
        IEnumerable<EntityEntry<IAuditableEntity>> entries =
            _dbContext
                .ChangeTracker
                .Entries<IAuditableEntity>();

        foreach (EntityEntry<IAuditableEntity> entityEntry in entries)
        {
            if (entityEntry.State == EntityState.Added)
            {
                entityEntry.Property(a => a.CreatedOnUtc).CurrentValue = DateTime.UtcNow;
            }

            if (entityEntry.State == EntityState.Modified)
            {
                entityEntry.Property(a => a.ModifiedOnUtc).CurrentValue = DateTime.UtcNow;
            }
        }
    }
}