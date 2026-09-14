using MaksymKostyshyn.MindTrack.Data.EF.Entities;
using Microsoft.EntityFrameworkCore;

namespace MaksymKostyshyn.MindTrack.Data.EF.UnitTests.Entities;

public class JournalEntryConcurrencyTests
{
    private string _databaseName = null!;

    [SetUp]
    public void Setup()
    {
        _databaseName = Guid.NewGuid().ToString();
    }

    private MindTrackDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<MindTrackDbContext>()
            .UseInMemoryDatabase(_databaseName)
            .Options;

        return new MindTrackDbContext(options);
    }

    [Test]
    public async Task SaveChangesAsync_RowVersionChangedSincePropertyWasRead_ThrowsConcurrencyException()
    {
        // Arrange
        var patientId = Guid.NewGuid();
        var journalEntryId = Guid.NewGuid();

        await using (var seedContext = CreateContext())
        {
            seedContext.UserRole.Add(new UserRoleEntity { Id = 3, Name = "Patient" });
            seedContext.AIResponseStyle.Add(new AIResponseStyleEntity { Id = 1, Name = "Standard" });
            seedContext.User.Add(new UserEntity
            {
                Id = patientId,
                Email = "patient@example.com",
                PasswordHash = "hash",
                FullName = "Test Patient",
                RoleId = 3,
                IsVulnerable = false,
                AIResponseStyleId = 1,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });
            seedContext.JournalEntry.Add(new JournalEntryEntity
            {
                Id = journalEntryId,
                PatientId = patientId,
                VideoUrl = "https://example.com/video.mp4",
                RecordedAt = DateTime.UtcNow,
                MotivationalMessage = "Keep going!",
                Tips = "Breathe."
            });

            await seedContext.SaveChangesAsync();
        }

        // Act
        await using var context = CreateContext();
        var journalEntry = await context.JournalEntry.SingleAsync(entry => entry.Id == journalEntryId);

        journalEntry.Tips = "Updated without knowing someone else already changed this row.";

        context.Entry(journalEntry).Property(entry => entry.RowVersion).OriginalValue =
            [1, 2, 3, 4, 5, 6, 7, 8];

        // Assert
        Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () => await context.SaveChangesAsync());
    }
}
