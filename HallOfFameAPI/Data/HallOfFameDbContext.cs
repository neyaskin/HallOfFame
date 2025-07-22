using HallOfFameAPI.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace HallOfFameAPI.Data;

/// <summary>
///     Контекст базы данных
/// </summary>
public class HallOfFameDbContext : DbContext
{
    public HallOfFameDbContext(DbContextOptions<HallOfFameDbContext> options) : base(options)
    {
    }

    /// <summary>
    ///     Таблица с информацией о пользователях.
    /// </summary>
    public DbSet<Person?> Persons { get; set; }

    /// <summary>
    ///     Таблица с информацией о умениях.
    /// </summary>
    public DbSet<Skill> Skills { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Person>(entity =>
        {
            entity.Property(p => p.Name).HasMaxLength(100).IsRequired();
            entity.Property(p => p.DisplayName).HasMaxLength(100).IsRequired();
        });

        modelBuilder.Entity<Skill>(entity =>
        {
            entity.Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(s => s.Level)
                .IsRequired()
                .HasColumnName("level");
            entity.HasCheckConstraint("CK_Skill_Level", "Level BETWEEN 1 AND 10");
        });

        modelBuilder.Entity<Person>()
            .HasMany(p => p.Skills)
            .WithOne(s => s.Person)
            .HasForeignKey(s => s.PersonId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}