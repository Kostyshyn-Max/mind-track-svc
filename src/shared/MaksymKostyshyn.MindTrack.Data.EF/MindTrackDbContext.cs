using MaksymKostyshyn.MindTrack.Data.EF.Entities;
using MaksymKostyshyn.MindTrack.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace MaksymKostyshyn.MindTrack.Data.EF;

public sealed class MindTrackDbContext : DbContext
{
    public DbSet<AIResponseStyleEntity> AIResponseStyle { get; set; }
    public DbSet<UserRoleEntity> UserRole { get; set; }
    public DbSet<PracticeTypeEntity> PracticeType { get; set; }
    public DbSet<PracticeStatusEntity> PracticeStatus { get; set; }
    public DbSet<UserEntity> User { get; set; }
    public DbSet<JournalEntryEntity> JournalEntry { get; set; }
    public DbSet<EmotionAnalysesEntity> EmotionAnalyses { get; set; }
    public DbSet<PracticeEntity> Practice { get; set; } 
    public DbSet<AssignedPractices> AssignedPractices { get; set; }
    public DbSet<ReflectionAnswerEntity> ReflectionAnswers { get; set; }
    public DbSet<AlertEntity> Alert { get; set; }
    public DbSet<AchievementEntity> Achievement { get; set; }
    public DbSet<UserAchievementEntity> UserAchievement { get; set; }
    
    public MindTrackDbContext(DbContextOptions<MindTrackDbContext> options)
        : base(options)
    {
       
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var aiResponseStyles = Enum.GetValues<AIResponseStyleType>()
            .Select(style => new AIResponseStyleEntity
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

        var practiceTypes = Enum.GetValues<PracticeType>()
            .Select(type => new PracticeTypeEntity
                {
                    Id = (int)type,
                    Name = type.ToString()
                }
            );

        var practiceStatuses = Enum.GetValues<PracticeStatus>()
            .Select(status => new PracticeStatusEntity
                {
                    Id = (int)status,
                    Name = status.ToString()
                }
            );

        modelBuilder.Entity<AIResponseStyleEntity>().HasData(aiResponseStyles);
        modelBuilder.Entity<UserRoleEntity>().HasData(userRoles);
        modelBuilder.Entity<PracticeTypeEntity>().HasData(practiceTypes);
        modelBuilder.Entity<PracticeStatusEntity>().HasData(practiceStatuses);

        modelBuilder.Entity<UserEntity>().HasIndex(u => u.Email).IsUnique();

        modelBuilder.Entity<UserEntity>()
            .HasOne(u => u.Role)
            .WithMany()
            .HasForeignKey(u => u.RoleId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<UserEntity>()
            .HasOne(u => u.AIResponseStyle)
            .WithMany()
            .HasForeignKey(u => u.AIResponseStyleId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<UserEntity>()
            .HasOne(u => u.Therapist)
            .WithMany()
            .HasForeignKey(u => u.TherapistId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<JournalEntryEntity>()
            .Property(entry => entry.RowVersion)
            .IsRowVersion();

        modelBuilder.Entity<EmotionAnalysesEntity>()
            .HasIndex(analysis => analysis.JournalEntryId)
            .IsUnique();

        modelBuilder.Entity<AssignedPractices>()
            .HasIndex(assignedPractice => new { assignedPractice.JournalEntryId, assignedPractice.PracticeId })
            .IsUnique();

        modelBuilder.Entity<AssignedPractices>()
            .HasOne(assignedPractice => assignedPractice.Practice)
            .WithMany()
            .HasForeignKey(assignedPractice => assignedPractice.PracticeId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ReflectionAnswerEntity>()
            .HasOne(r => r.ReflectionAnswer)
            .WithMany()
            .HasForeignKey(r => r.PreviousAnswerId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<AlertEntity>()
            .HasOne(alert => alert.Patient)
            .WithMany()
            .HasForeignKey(alert => alert.PatientId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<AlertEntity>()
            .HasOne(alert => alert.Therapist)
            .WithMany()
            .HasForeignKey(alert => alert.TherapistId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<UserAchievementEntity>()
            .HasIndex(userAchievement => new { userAchievement.PatientId, userAchievement.AchievementId })
            .IsUnique();
    }
}
