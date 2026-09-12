using MaksymKostyshyn.MindTrack.Data.EF.Entities;
using MaksymKostyshyn.MindTrack.Domain;
using Microsoft.EntityFrameworkCore;

namespace MaksymKostyshyn.MindTrack.Data.EF;

public sealed class MindTrackDbContext : DbContext
{
    public DbSet<AIRepsonseStyleEntity> AIResponseStyle { get; set; }
    public DbSet<UserRoleEntity> UserRole { get; set; }
    public DbSet<UserEntity> User { get; set; }

    public MindTrackDbContext(DbContextOptions<MindTrackDbContext> options)
        : base(options)
    {
       
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var aiResponseStyles = Enum.GetValues<AIResponseStyleType>()
            .Select(style => new AIRepsonseStyleEntity 
                {
                    Id = (int)style,
                    Name = style.ToString()
                }
            );

        var userRoles = Enum.GetValues<UserRole>()
            .Select(role => new UserRoleEntity
                {
                    Id = (int)role,
                    Name = role.ToString() 
                }
            );

        modelBuilder.Entity<AIRepsonseStyleEntity>().HasData(aiResponseStyles);
        modelBuilder.Entity<UserRoleEntity>().HasData(userRoles);

        modelBuilder.Entity<UserEntity>().HasIndex(u => u.Email).IsUnique();

        modelBuilder.Entity<UserEntity>()
            .HasOne(u => u.Role)
            .WithMany()
            .HasForeignKey(u => u.RoleId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<UserEntity>()
            .HasOne(u => u.AIRepsonseStyle)
            .WithMany()
            .HasForeignKey(u => u.AIResponseStyleId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<UserEntity>()
            .HasOne(u => u.Therapist)
            .WithMany()
            .HasForeignKey(u => u.TherapishId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
