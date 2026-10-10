using App.Common.Interfaces;
using Domain.Common;
using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Infra.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options), IApplicationDbContext
{
    public DbSet<Cluster> Clusters => Set<Cluster>();

    public DbSet<Building> Buildings => Set<Building>();

    public DbSet<ClusterAdmin> ClusterAdmins => Set<ClusterAdmin>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        // globally restrict deletion for all foreign key relationships
        var referencingForeignKeys = builder.Model.GetEntityTypes()
            .Where(e => typeof(BaseAuditableEntity).IsAssignableFrom(e.ClrType))
            .SelectMany(e => e.GetReferencingForeignKeys());
        foreach (var relationship in referencingForeignKeys)
        {
            relationship.DeleteBehavior = DeleteBehavior.Restrict;
        }

        base.OnModelCreating(builder);
        
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
