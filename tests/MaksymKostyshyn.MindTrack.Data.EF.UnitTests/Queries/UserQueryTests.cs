using MaksymKostyshyn.MindTrack.Data.EF.Queries;
using MaksymKostyshyn.MindTrack.Data.EF.UnitTests.TestData;
using MaksymKostyshyn.MindTrack.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace MaksymKostyshyn.MindTrack.Data.EF.UnitTests.Queries;

public class UserQueryTests
{
    private MindTrackDbContext _dbContext = null!;
    private UserQuery _userQuery = null!;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<MindTrackDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _dbContext = new MindTrackDbContext(options);

        _dbContext.UserRole.AddRange(UserMockDataFactory.CreateUserRoles());
        _dbContext.AIResponseStyle.AddRange(UserMockDataFactory.CreateAIResponseStyles());
        _dbContext.User.AddRange(UserMockDataFactory.CreateUsers());
        _dbContext.SaveChanges();

        _userQuery = new UserQuery(_dbContext, NullLogger<UserQuery>.Instance);
    }

    [Test]
    public async Task GetPatientsFromTherapistAsync_TherapistHasPatients_ReturnsOnlyThatTherapistsPatients()
    {
        // Arrange
        int page = 1;
        int pageSize = 10;

        // Act
        var patients = await _userQuery.GetPatientsFromTherapistAsync(UserMockDataFactory.TherapistAId, page, pageSize, CancellationToken.None);

        // Assert
        Assert.That(patients, Is.Not.Null);
        Assert.That(patients.Count, Is.GreaterThan(0));
        Assert.That(patients, Has.None.Matches<UserSummary>(user => user.Id == UserMockDataFactory.TherapistAId));
    }

    [Test]
    public async Task GetPatientsFromTherapistAsync_CancellationRequested_ReturnsNullInsteadOfThrowing()
    {
        // Arrange
        int page = 1;
        int pageSize = 10;

        using var cts = new CancellationTokenSource();
        cts.Cancel();

        // Act
        var patients = await _userQuery.GetPatientsFromTherapistAsync(UserMockDataFactory.TherapistAId, page, pageSize, cts.Token);

        // Assert
        Assert.That(patients, Is.Null);
    }

    [Test]
    public async Task GetPatientsFromTherapistAsync_TherapistHasPatients_ExcludesTherapistOwnRecord()
    {
        // Arrange
        int page = 1;
        int pageSize = 10;

        // Act
        var patients = await _userQuery.GetPatientsFromTherapistAsync(UserMockDataFactory.TherapistAId, page, pageSize, CancellationToken.None);

        // Assert
        Assert.That(patients, Is.Not.Null);
        Assert.That(patients, Has.None.Matches<UserSummary>(user => user.Id == UserMockDataFactory.TherapistAId));
        Assert.That(patients, Has.None.Matches<UserSummary>(user => user.FullName == "Therapist A"));
    }

    [Test]
    public async Task GetPatientsFromTherapistAsync_TherapistIdHasNoMatches_ReturnsEmptyList()
    {
        // Arrange
        int page = 1;
        int pageSize = 10;

        var unknownTherapistId = Guid.NewGuid();

        // Act
        var patients = await _userQuery.GetPatientsFromTherapistAsync(unknownTherapistId, page, pageSize, CancellationToken.None);

        // Assert
        Assert.That(patients, Is.Not.Null);
        Assert.That(patients, Is.Empty);
    }

    [Test]
    public async Task GetPatientsFromTherapistAsync_MatchingPatient_MapsEntityFieldsToUserSummary()
    {
        // Arrange
        var charlieId = Guid.Parse("aaaaaaaa-0001-0000-0000-000000000000");

        // Act
        var patients = await _userQuery.GetPatientsFromTherapistAsync(UserMockDataFactory.TherapistAId, 1, 10, CancellationToken.None);

        // Assert
        Assert.That(patients, Is.Not.Null);
        var charlie = patients.Single(p => p.Id == charlieId);
        Assert.That(charlie.FullName, Is.EqualTo("Charlie Patient"));
        Assert.That(charlie.Email, Is.EqualTo("charlie.patient@example.com"));
        Assert.That(charlie.EarnedPoints, Is.EqualTo(40));
    }

    [Test]
    public async Task GetPatientsFromTherapistAsync_MatchingPatient_IncludesAIResponseStyleName()
    {
        // Arrange
        var aliceId = Guid.Parse("aaaaaaaa-0002-0000-0000-000000000000");
        var charlieId = Guid.Parse("aaaaaaaa-0001-0000-0000-000000000000");

        // Act
        var patients = await _userQuery.GetPatientsFromTherapistAsync(UserMockDataFactory.TherapistAId, 1, 10, CancellationToken.None);

        // Assert
        Assert.That(patients, Is.Not.Null);
        Assert.That(patients.Single(p => p.Id == aliceId).AIStyleResponse, Is.EqualTo("Cautious"));
        Assert.That(patients.Single(p => p.Id == charlieId).AIStyleResponse, Is.EqualTo("Standard"));
    }

    [Test]
    public async Task GetPatientsFromTherapistAsync_FirstPageWithMoreDataAvailable_ReturnsFullPageSize()
    {
        // Arrange
        int page = 1;
        int pageSize = 2;

        // Act
        var patients = await _userQuery.GetPatientsFromTherapistAsync(UserMockDataFactory.TherapistAId, page, pageSize, CancellationToken.None);

        // Assert
        Assert.That(patients, Is.Not.Null);
        Assert.That(patients.Count, Is.EqualTo(2));
    }

    [Test]
    public async Task GetPatientsFromTherapistAsync_LastPage_ReturnsRemainingItemsOnly()
    {
        // Arrange
        int page = 2;
        int pageSize = 2;

        // Act
        var patients = await _userQuery.GetPatientsFromTherapistAsync(UserMockDataFactory.TherapistAId, page, pageSize, CancellationToken.None);

        // Assert
        Assert.That(patients, Is.Not.Null);
        Assert.That(patients.Count, Is.EqualTo(1));
    }

    [Test]
    public async Task GetPatientsFromTherapistAsync_PageBeyondAvailableData_ReturnsEmptyList()
    {
        // Arrange
        int page = 5;
        int pageSize = 2;

        // Act
        var patients = await _userQuery.GetPatientsFromTherapistAsync(UserMockDataFactory.TherapistAId, page, pageSize, CancellationToken.None);

        // Assert
        Assert.That(patients, Is.Not.Null);
        Assert.That(patients, Is.Empty);
    }

    [Test]
    public async Task GetPatientsFromTherapistAsync_PageSizeZero_ReturnsEmptyList()
    {
        // Arrange
        int page = 1;
        int pageSize = 0;

        // Act
        var patients = await _userQuery.GetPatientsFromTherapistAsync(UserMockDataFactory.TherapistAId, page, pageSize, CancellationToken.None);

        // Assert
        Assert.That(patients, Is.Not.Null);
        Assert.That(patients, Is.Empty);
    }

    [Test]
    public void GetPatientsFromTherapistAsync_NonPositivePage_DoesNotThrowAndSkipsNothing()
    {
        // Arrange
        int page = 0;
        int pageSize = 10;

        // Act & Assert
        IList<UserSummary>? patients = null;
        Assert.DoesNotThrowAsync(async () =>
            patients = await _userQuery.GetPatientsFromTherapistAsync(UserMockDataFactory.TherapistAId, page, pageSize, CancellationToken.None));

        Assert.That(patients, Is.Not.Null);
        Assert.That(patients!.Count, Is.EqualTo(3));
    }

    [Test]
    public async Task GetPatientsFromTherapistAsync_InactivePatientExists_IsExcludedFromResults()
    {
        // Arrange
        var bobId = Guid.Parse("aaaaaaaa-0003-0000-0000-000000000000");

        // Act
        var patients = await _userQuery.GetPatientsFromTherapistAsync(UserMockDataFactory.TherapistAId, 1, 10, CancellationToken.None);

        // Assert
        Assert.That(patients, Is.Not.Null);
        Assert.That(patients, Has.None.Matches<UserSummary>(p => p.Id == bobId));
    }

    [TearDown]
    public void TearDown()
    {
        _dbContext.Dispose();
    }

}
