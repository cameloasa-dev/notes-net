
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyOwnApi.Models;

namespace MyOwnApi.Context;

public class ApplicationDbContext : IdentityDbContext<User>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Note> Notes { get; set; }
    public DbSet<ApiKey> ApiKeys { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Note → User (1:N)
        builder.Entity<Note>()
            .HasOne(n => n.User)
            .WithMany(u => u.Notes)
            .HasForeignKey(n => n.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // ApiKey → User (1:1)
        builder.Entity<ApiKey>()
            .HasOne(k => k.User)
            .WithOne(u => u.APIKey)
            .HasForeignKey<ApiKey>(k => k.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
