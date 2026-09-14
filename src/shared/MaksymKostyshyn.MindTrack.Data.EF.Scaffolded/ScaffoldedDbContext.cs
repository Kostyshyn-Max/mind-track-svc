using System;
using System.Collections.Generic;
using MaksymKostyshyn.MindTrack.Data.EF.Scaffolded.Entities;
using Microsoft.EntityFrameworkCore;

namespace MaksymKostyshyn.MindTrack.Data.EF.Scaffolded;

public partial class ScaffoldedDbContext : DbContext
{
    public ScaffoldedDbContext(DbContextOptions<ScaffoldedDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Achievement> Achievements { get; set; }

    public virtual DbSet<AiresponseStyle> AiresponseStyles { get; set; }

    public virtual DbSet<Alert> Alerts { get; set; }

    public virtual DbSet<AssignedPractice> AssignedPractices { get; set; }

    public virtual DbSet<EmotionAnalysis> EmotionAnalyses { get; set; }

    public virtual DbSet<JournalEntry> JournalEntries { get; set; }

    public virtual DbSet<Practice> Practices { get; set; }

    public virtual DbSet<PracticeStatus> PracticeStatuses { get; set; }

    public virtual DbSet<PracticeType> PracticeTypes { get; set; }

    public virtual DbSet<ReflectionAnswer> ReflectionAnswers { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserAchievement> UserAchievements { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Achievement>(entity =>
        {
            entity.ToTable("Achievement");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<AiresponseStyle>(entity =>
        {
            entity.ToTable("AIResponseStyle");
        });

        modelBuilder.Entity<Alert>(entity =>
        {
            entity.ToTable("Alert");

            entity.HasIndex(e => e.PatientId, "IX_Alert_PatientId");

            entity.HasIndex(e => e.TherapistId, "IX_Alert_TherapistId");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Patient).WithMany(p => p.AlertPatients)
                .HasForeignKey(d => d.PatientId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Therapist).WithMany(p => p.AlertTherapists)
                .HasForeignKey(d => d.TherapistId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<AssignedPractice>(entity =>
        {
            entity.HasIndex(e => new { e.JournalEntryId, e.PracticeId }, "IX_AssignedPractices_JournalEntryId_PracticeId").IsUnique();

            entity.HasIndex(e => e.PracticeId, "IX_AssignedPractices_PracticeId");

            entity.HasIndex(e => e.PracticeStatusId, "IX_AssignedPractices_PracticeStatusId");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.JournalEntry).WithMany(p => p.AssignedPractices).HasForeignKey(d => d.JournalEntryId);

            entity.HasOne(d => d.Practice).WithMany(p => p.AssignedPractices)
                .HasForeignKey(d => d.PracticeId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.PracticeStatus).WithMany(p => p.AssignedPractices).HasForeignKey(d => d.PracticeStatusId);
        });

        modelBuilder.Entity<EmotionAnalysis>(entity =>
        {
            entity.HasIndex(e => e.JournalEntryId, "IX_EmotionAnalyses_JournalEntryId").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.JournalEntry).WithOne(p => p.EmotionAnalysis).HasForeignKey<EmotionAnalysis>(d => d.JournalEntryId);
        });

        modelBuilder.Entity<JournalEntry>(entity =>
        {
            entity.ToTable("JournalEntry");

            entity.HasIndex(e => e.PatientId, "IX_JournalEntry_PatientId");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.HasOne(d => d.Patient).WithMany(p => p.JournalEntries).HasForeignKey(d => d.PatientId);
        });

        modelBuilder.Entity<Practice>(entity =>
        {
            entity.ToTable("Practice");

            entity.HasIndex(e => e.PracticeTypeId, "IX_Practice_PracticeTypeId");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.PracticeType).WithMany(p => p.Practices).HasForeignKey(d => d.PracticeTypeId);
        });

        modelBuilder.Entity<PracticeStatus>(entity =>
        {
            entity.ToTable("PracticeStatus");
        });

        modelBuilder.Entity<PracticeType>(entity =>
        {
            entity.ToTable("PracticeType");
        });

        modelBuilder.Entity<ReflectionAnswer>(entity =>
        {
            entity.HasIndex(e => e.JournalEntryId, "IX_ReflectionAnswers_JournalEntryId");

            entity.HasIndex(e => e.PreviousAnswerId, "IX_ReflectionAnswers_PreviousAnswerId");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.JournalEntry).WithMany(p => p.ReflectionAnswers).HasForeignKey(d => d.JournalEntryId);

            entity.HasOne(d => d.PreviousAnswer).WithMany(p => p.InversePreviousAnswer).HasForeignKey(d => d.PreviousAnswerId);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.HasIndex(e => e.AiresponseStyleId, "IX_User_AIResponseStyleId");

            entity.HasIndex(e => e.Email, "IX_User_Email").IsUnique();

            entity.HasIndex(e => e.RoleId, "IX_User_RoleId");

            entity.HasIndex(e => e.TherapistId, "IX_User_TherapistId");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.AiresponseStyleId).HasColumnName("AIResponseStyleId");

            entity.HasOne(d => d.AiresponseStyle).WithMany(p => p.Users)
                .HasForeignKey(d => d.AiresponseStyleId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Therapist).WithMany(p => p.InverseTherapist).HasForeignKey(d => d.TherapistId);
        });

        modelBuilder.Entity<UserAchievement>(entity =>
        {
            entity.ToTable("UserAchievement");

            entity.HasIndex(e => e.AchievementId, "IX_UserAchievement_AchievementId");

            entity.HasIndex(e => new { e.PatientId, e.AchievementId }, "IX_UserAchievement_PatientId_AchievementId").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Achievement).WithMany(p => p.UserAchievements).HasForeignKey(d => d.AchievementId);

            entity.HasOne(d => d.Patient).WithMany(p => p.UserAchievements).HasForeignKey(d => d.PatientId);
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.ToTable("UserRole");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
