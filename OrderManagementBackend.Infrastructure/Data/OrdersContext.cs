using Microsoft.EntityFrameworkCore;
using OrderManagementBackend.Domain;
using OrderManagementBackend.Domain.Common;
using System.Reflection;

namespace OrderManagementBackend.Infrastructure.Data
{
    public class OrdersContext : DbContext
    {
        private readonly ICurrentUserProvider _currentUserProvider;

        public OrdersContext(DbContextOptions<OrdersContext> options, ICurrentUserProvider currentUserProvider) : base(options)
        {
            _currentUserProvider = currentUserProvider;
        }

        public DbSet<Product> Product { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderProduct> OrderProduct { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            StampAuditableEntities();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            StampAuditableEntities();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void StampAuditableEntities()
        {
            var now = DateTime.UtcNow;
            var currentUser = _currentUserProvider.GetCurrentUser();

            foreach (var entry in ChangeTracker.Entries<IAuditableEntity>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = now;
                    entry.Entity.CreatedBy = currentUser;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = now;
                    entry.Entity.UpdatedBy = currentUser;
                }
            }
        }
    }
}
