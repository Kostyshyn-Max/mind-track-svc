using MaksymKostyshyn.MindTrack.Data.EF;
using MaksymKostyshyn.MindTrack.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace MaksymKostyshyn.MindTrack.Data.EF.IntegrationTests.Queries;

public class EFQuerySqlGenerationTests
{
    private static MindTrackDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<MindTrackDbContext>()
            .UseSqlServer("Server=.;Database=Dummy;TrustServerCertificate=True;")
            .Options;

        return new MindTrackDbContext(options);
    }

    [Test]
    public void GetPatientsFromTherapistAsync_Query_TranslatesToSqlServerSql()
    {
        // Arrange
        using var dbContext = CreateContext();
        var therapistId = Guid.NewGuid();
        var page = 1;
        var pageSize = 10;

        // Act same LINQ shape as UserQuery.GetPatientsFromTherapistAsync
        var query = dbContext.User
            .Where(user => user.TherapistId.Equals(therapistId) && user.IsActive)
            .OrderBy(user => user.FullName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(user => new UserSummary(user.Id, user.FullName, user.Email, user.AIResponseStyle.Name, user.EarnedPoints));

        var sql = query.ToQueryString();
        TestContext.Out.WriteLine(sql);

        // Assert
        Assert.Pass();
    }

    [Test]
    public void GetJournalEntriesForPatientAsync_Query_TranslatesToSqlServerSql()
    {
        // Arrange
        using var dbContext = CreateContext();
        var patientId = Guid.NewGuid();
        var page = 2;
        var pageSize = 5;

        // Act same LINQ shape as JournalEntryQuery.GetJournalEntriesForPatientAsync
        var query = dbContext.JournalEntry
            .Where(entry => entry.PatientId == patientId)
            .OrderByDescending(entry => entry.RecordedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize);

        var sql = query.ToQueryString();
        TestContext.Out.WriteLine(sql);

        // Assert
        Assert.Pass();
    }

    [Test]
    public void GetByIdAsync_JournalEntryGraphQuery_TranslatesToSqlServerSql()
    {
        // Arrange
        using var dbContext = CreateContext();
        var journalEntryId = Guid.NewGuid();

        // Act same LINQ shape as JournalEntryRepository.GetByIdAsync 
        var query = dbContext.JournalEntry
            .Include(entry => entry.AssignedPractices)
                .ThenInclude(assignedPractice => assignedPractice.Practice)
            .Include(entry => entry.EmotionAnalysis)
            .Include(entry => entry.ReflectionAnswers)
            .Where(entry => entry.Id == journalEntryId)
            .Take(1);

        var sql = query.ToQueryString();
        TestContext.Out.WriteLine(sql);

        // Assert
        Assert.Pass();
    }
}
