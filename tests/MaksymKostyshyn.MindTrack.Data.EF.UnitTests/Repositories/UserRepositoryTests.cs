using MaksymKostyshyn.MindTrack.Data.Abstractions.Repositories;
using MaksymKostyshyn.MindTrack.Data.EF.Entities;
using MaksymKostyshyn.MindTrack.Data.EF.Repositories;
using MaksymKostyshyn.MindTrack.Data.EF.UnitTests.TestData;
using MaksymKostyshyn.MindTrack.Data.EF.UnitTests.TestSupport;
using MaksymKostyshyn.MindTrack.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MaksymKostyshyn.MindTrack.Data.EF.UnitTests.Repositories;

public class UserRepositoryTests
{
    private string _databaseName = null!;
    private MindTrackDbContext _dbContext = null!;
    private CapturingLogger<UserRepository> _logger = null!;
    private IUserRepository _userRepository = null!;

    [SetUp]
    public void Setup()
    {
        _databaseName = Guid.NewGuid().ToString();
        _dbContext = CreateContext();

        _dbContext.UserRole.AddRange(UserMockDataFactory.CreateUserRoles());
        _dbContext.AIResponseStyle.AddRange(UserMockDataFactory.CreateAIResponseStyles());
        _dbContext.SaveChanges();

        _logger = new CapturingLogger<UserRepository>();
        _userRepository = new UserRepository(_dbContext, _logger);
    }

    [TearDown]
    public void TearDown()
    {
        _dbContext.Dispose();
    }

    private MindTrackDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<MindTrackDbContext>()
            .UseInMemoryDatabase(_databaseName)
            .Options;

        return new MindTrackDbContext(options);
    }

    private UserEntity SeedUser(Guid id, string email = "seeded.user@example.com", bool isActive = true, string fullName = "Seeded User")
    {
        var entity = new UserEntity
        {
            Id = id,
            Email = email,
            PasswordHash = "hash",
            FullName = fullName,
            RoleId = (int)UserRole.User,
            TherapishId = Guid.Empty,
            IsVulnarable = false,
            AIResponseStyleId = (int)AIResponseStyleType.Standard,
            EarnedPoints = 0,
            IsActive = isActive,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _dbContext.User.Add(entity);
        _dbContext.SaveChanges();

        return entity;
    }

    [Test]
    public async Task CreateUserAsync_ValidUser_PersistsAndReturnsMappedUser()
    {
        // Arrange
        var user = UserFactory.Create();

        // Act
        var result = await _userRepository.CreateUserAsync(user, CancellationToken.None);

        // Assert
        Assert.That(result, Is.Not.Null);

        await using var verifyContext = CreateContext();
        var persisted = await verifyContext.User.FindAsync(user.Id);
        Assert.That(persisted, Is.Not.Null);
    }

    [Test]
    public async Task CreateUserAsync_ValidUser_ReturnedUserMatchesInputFields()
    {
        // Arrange
        var user = UserFactory.Create(fullName: "Jane Doe", email: "jane.doe@example.com", earnedPoints: 25);

        // Act
        var result = await _userRepository.CreateUserAsync(user, CancellationToken.None);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Id, Is.EqualTo(user.Id));
        Assert.That(result.FullName, Is.EqualTo("Jane Doe"));
        Assert.That(result.Email, Is.EqualTo("jane.doe@example.com"));
        Assert.That(result.EarnedPoints, Is.EqualTo(25));
    }

    [Test]
    public async Task CreateUserAsync_DuplicateEmail_DoesNotThrowOnInMemoryProvider()
    {
        // Arrange
        var first = UserFactory.Create(email: "duplicate@example.com");
        await _userRepository.CreateUserAsync(first, CancellationToken.None);

        var second = UserFactory.Create(email: "duplicate@example.com");

        // Act & Assert
        User? result = null;
        Assert.DoesNotThrowAsync(async () => result = await _userRepository.CreateUserAsync(second, CancellationToken.None));
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public async Task CreateUserAsync_DuplicateId_ReturnsNullAndLogsWarning()
    {
        // Arrange
        var sharedId = Guid.NewGuid();
        SeedUser(sharedId, email: "existing@example.com");

        var duplicate = UserFactory.Create(id: sharedId, email: "different@example.com");

        // Act
        var result = await _userRepository.CreateUserAsync(duplicate, CancellationToken.None);

        // Assert
        Assert.That(result, Is.Null);
        Assert.That(_logger.Entries, Has.Some.Matches<(LogLevel Level, Exception? Exception, string Message)>(
            entry => entry.Level == LogLevel.Warning));
    }

    [Test]
    public async Task CreateUserAsync_CancellationRequested_ReturnsNullInsteadOfThrowing()
    {
        // Arrange
        using var cts = new CancellationTokenSource();
        cts.Cancel();
        var user = UserFactory.Create();

        // Act
        var result = await _userRepository.CreateUserAsync(user, cts.Token);

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task CreateUserAsync_UnknownRoleOrAIResponseStyleId_DoesNotThrowOnInMemoryProvider()
    {
        // Arrange
        var user = UserFactory.Create(role: (UserRole)999, aiResponseStyleType: (AIResponseStyleType)999);

        // Act & Assert
        User? result = null;
        Assert.DoesNotThrowAsync(async () => result = await _userRepository.CreateUserAsync(user, CancellationToken.None));
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public async Task DeleteUserAsync_ExistingUser_SetsIsActiveFalseWithoutRemovingRow()
    {
        // Arrange
        var id = Guid.NewGuid();
        SeedUser(id, isActive: true);

        // Act
        await _userRepository.DeleteUserAsync(id, CancellationToken.None);

        // Assert
        await using var verifyContext = CreateContext();
        var persisted = await verifyContext.User.FindAsync(id);
        Assert.That(persisted, Is.Not.Null);
        Assert.That(persisted!.IsActive, Is.False);
    }

    [Test]
    public async Task DeleteUserAsync_UnknownUserId_LogsWarningAndCompletesWithoutThrowing()
    {
        // Arrange
        var unknownId = Guid.NewGuid();

        // Act & Assert
        Assert.DoesNotThrowAsync(async () => await _userRepository.DeleteUserAsync(unknownId, CancellationToken.None));
        Assert.That(_logger.Entries, Has.Some.Matches<(LogLevel Level, Exception? Exception, string Message)>(
            entry => entry.Level == LogLevel.Warning));
    }

    [Test]
    public async Task DeleteUserAsync_AlreadyInactiveUser_RemainsInactiveAndDoesNotThrow()
    {
        // Arrange
        var id = Guid.NewGuid();
        SeedUser(id, isActive: false);

        // Act & Assert
        Assert.DoesNotThrowAsync(async () => await _userRepository.DeleteUserAsync(id, CancellationToken.None));

        await using var verifyContext = CreateContext();
        var persisted = await verifyContext.User.FindAsync(id);
        Assert.That(persisted!.IsActive, Is.False);
    }

    [Test]
    public async Task DeleteUserAsync_CancellationRequested_CompletesWithoutThrowing()
    {
        // Arrange
        var id = Guid.NewGuid();
        SeedUser(id, isActive: true);

        using var cts = new CancellationTokenSource();
        cts.Cancel();

        // Act & Assert
        Assert.DoesNotThrowAsync(async () => await _userRepository.DeleteUserAsync(id, cts.Token));
        Assert.That(_logger.Entries, Has.Some.Matches<(LogLevel Level, Exception? Exception, string Message)>(
            entry => entry.Level == LogLevel.Warning));
    }

    [Test]
    public async Task DeleteUserAsync_QueryThrows_LogsWarningAndSwallowsException()
    {
        // Arrange
        var disposedContext = CreateContext();
        await disposedContext.DisposeAsync();

        var capturingLogger = new CapturingLogger<UserRepository>();
        var repository = new UserRepository(disposedContext, capturingLogger);

        // Act & Assert
        Assert.DoesNotThrowAsync(async () => await repository.DeleteUserAsync(Guid.NewGuid(), CancellationToken.None));
        Assert.That(capturingLogger.Entries, Has.Some.Matches<(LogLevel Level, Exception? Exception, string Message)>(
            entry => entry.Level == LogLevel.Warning));
    }

    [Test]
    public async Task GetByIdAsync_ExistingUser_ReturnsMappedUser()
    {
        // Arrange
        var id = Guid.NewGuid();
        SeedUser(id, fullName: "Existing User");

        // Act
        var result = await _userRepository.GetByIdAsync(id, CancellationToken.None);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Id, Is.EqualTo(id));
        Assert.That(result.FullName, Is.EqualTo("Existing User"));
    }

    [Test]
    public async Task GetByIdAsync_UnknownUserId_ReturnsNull()
    {
        // Act
        var result = await _userRepository.GetByIdAsync(Guid.NewGuid(), CancellationToken.None);

        // Assert
        Assert.That(result, Is.Null);
        Assert.That(_logger.Entries, Is.Empty);
    }

    [Test]
    public async Task GetByIdAsync_InactiveUser_IsStillReturned()
    {
        // Arrange
        var id = Guid.NewGuid();
        SeedUser(id, isActive: false);

        // Act
        var result = await _userRepository.GetByIdAsync(id, CancellationToken.None);

        // Asser
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.IsActive, Is.False);
    }

    [Test]
    public async Task GetByIdAsync_QueryThrows_ReturnsNullAndLogsWarning()
    {
        // Arrange
        var disposedContext = CreateContext();
        await disposedContext.DisposeAsync();

        var capturingLogger = new CapturingLogger<UserRepository>();
        var repository = new UserRepository(disposedContext, capturingLogger);

        // Act
        var result = await repository.GetByIdAsync(Guid.NewGuid(), CancellationToken.None);

        // Assert
        Assert.That(result, Is.Null);
        Assert.That(capturingLogger.Entries, Has.Some.Matches<(LogLevel Level, Exception? Exception, string Message)>(
            entry => entry.Level == LogLevel.Warning));
    }

    [Test]
    public async Task GetByIdAsync_CancellationRequested_TokenIsNotHonoredByFindAsync()
    {
        // Arrange
        var id = Guid.NewGuid();
        SeedUser(id);

        using var cts = new CancellationTokenSource();
        cts.Cancel();

        // Act
        var result = await _userRepository.GetByIdAsync(id, cts.Token);

        // Assert
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public async Task UpdateUserAsync_ExistingUser_PersistsUpdatedFields()
    {
        // Arrange
        var id = Guid.NewGuid();
        SeedUser(id, fullName: "Old Name");
        _dbContext.ChangeTracker.Clear();

        var updatedUser = UserFactory.Create(id: id, fullName: "New Name", earnedPoints: 99);

        // Act
        await _userRepository.UpdateUserAsync(updatedUser, CancellationToken.None);

        // Assert
        await using var verifyContext = CreateContext();
        var persisted = await verifyContext.User.FindAsync(id);
        Assert.That(persisted, Is.Not.Null);
        Assert.That(persisted!.FullName, Is.EqualTo("New Name"));
        Assert.That(persisted.EarnedPoints, Is.EqualTo(99));
    }

    [Test]
    public async Task UpdateUserAsync_UnknownUserId_BehaviorOnSaveChanges()
    {
        // Arrange
        var user = UserFactory.Create();

        // Act & Assert
        Assert.DoesNotThrowAsync(async () => await _userRepository.UpdateUserAsync(user, CancellationToken.None));
        Assert.That(_logger.Entries, Has.Some.Matches<(LogLevel Level, Exception? Exception, string Message)>(
            entry => entry.Level == LogLevel.Warning));

        await using var verifyContext = CreateContext();
        var persisted = await verifyContext.User.FindAsync(user.Id);
        Assert.That(persisted, Is.Null);
    }

    [Test]
    public async Task UpdateUserAsync_CancellationRequested_CompletesWithoutThrowing()
    {
        // Arrange
        var id = Guid.NewGuid();
        SeedUser(id);
        _dbContext.ChangeTracker.Clear();

        using var cts = new CancellationTokenSource();
        cts.Cancel();

        var updatedUser = UserFactory.Create(id: id, fullName: "Should Not Persist");

        // Act & Assert
        Assert.DoesNotThrowAsync(async () => await _userRepository.UpdateUserAsync(updatedUser, cts.Token));
        Assert.That(_logger.Entries, Has.Some.Matches<(LogLevel Level, Exception? Exception, string Message)>(
            entry => entry.Level == LogLevel.Warning));
    }

    [Test]
    public async Task UpdateUserAsync_SaveChangesThrows_LogsWarningAndSwallowsException()
    {
        // Arrange
        var disposedContext = CreateContext();
        await disposedContext.DisposeAsync();

        var capturingLogger = new CapturingLogger<UserRepository>();
        var repository = new UserRepository(disposedContext, capturingLogger);
        var user = UserFactory.Create();

        // Act & Assert
        Assert.DoesNotThrowAsync(async () => await repository.UpdateUserAsync(user, CancellationToken.None));
        Assert.That(capturingLogger.Entries, Has.Some.Matches<(LogLevel Level, Exception? Exception, string Message)>(
            entry => entry.Level == LogLevel.Warning));
    }

    [Test]
    public async Task CreateUserAsync_ThenGetByIdAsync_ReturnsSameData()
    {
        // Arrange
        var user = UserFactory.Create(fullName: "Round Trip User", earnedPoints: 15);

        // Act
        await _userRepository.CreateUserAsync(user, CancellationToken.None);
        _dbContext.ChangeTracker.Clear();
        var fetched = await _userRepository.GetByIdAsync(user.Id, CancellationToken.None);

        // Assert
        Assert.That(fetched, Is.Not.Null);
        Assert.That(fetched!.FullName, Is.EqualTo("Round Trip User"));
        Assert.That(fetched.EarnedPoints, Is.EqualTo(15));
    }

    [Test]
    public async Task UpdateUserAsync_ThenGetByIdAsync_ReflectsChanges()
    {
        // Arrange
        var id = Guid.NewGuid();
        SeedUser(id, fullName: "Before Update");
        _dbContext.ChangeTracker.Clear();

        var updatedUser = UserFactory.Create(id: id, fullName: "After Update");

        // Act
        await _userRepository.UpdateUserAsync(updatedUser, CancellationToken.None);
        _dbContext.ChangeTracker.Clear();
        var fetched = await _userRepository.GetByIdAsync(id, CancellationToken.None);

        // Assert
        Assert.That(fetched, Is.Not.Null);
        Assert.That(fetched!.FullName, Is.EqualTo("After Update"));
    }

    [Test]
    public async Task DeleteUserAsync_ThenGetByIdAsync_StillReturnsUserButInactive()
    {
        // Arrange
        var id = Guid.NewGuid();
        SeedUser(id, isActive: true);
        _dbContext.ChangeTracker.Clear();

        // Act
        await _userRepository.DeleteUserAsync(id, CancellationToken.None);
        _dbContext.ChangeTracker.Clear();
        var fetched = await _userRepository.GetByIdAsync(id, CancellationToken.None);

        // Assert
        Assert.That(fetched, Is.Not.Null);
        Assert.That(fetched!.IsActive, Is.False);
    }
}
