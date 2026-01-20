using DrinkOrderSGIF.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DrinkOrderSGIF.Infrastructure.Data;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Team> Teams => Set<Team>();
    public DbSet<Drink> Drinks => Set<Drink>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Team>(entity =>
        {
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
        });

        modelBuilder.Entity<Drink>(entity =>
        {
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Price).IsRequired();
            entity.Property(e => e.KlipsPrice);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.Property(e => e.TeamName).IsRequired().HasMaxLength(200);
            entity.HasMany(e => e.Items)
                .WithOne(e => e.Order)
                .HasForeignKey(e => e.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.Property(e => e.DrinkName).IsRequired().HasMaxLength(200);
        });

        modelBuilder.Entity<Team>()
            .HasMany(e => e.Orders)
            .WithOne(e => e.Team)
            .HasForeignKey(e => e.TeamId);
    }
}
