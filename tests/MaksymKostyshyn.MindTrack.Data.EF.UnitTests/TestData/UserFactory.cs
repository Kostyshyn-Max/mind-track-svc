using MaksymKostyshyn.MindTrack.Domain;

namespace MaksymKostyshyn.MindTrack.Data.EF.UnitTests.TestData;

public static class UserFactory
{
    private static readonly DateTime DefaultTimestamp = new(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    public static User Create(
        Guid? id = null,
        string email = "test.user@example.com",
        string passwordHash = "hash",
        string fullName = "Test User",
        UserRole role = UserRole.User,
        Guid? therapistId = null,
        bool isVulnarable = false,
        AIResponseStyleType aiResponseStyleType = AIResponseStyleType.Standard,
        int earnedPoints = 0,
        bool isActive = true,
        DateTime? createdAt = null,
        DateTime? updatedAt = null)
    {
        return new User(
            id ?? Guid.NewGuid(),
            email,
            passwordHash,
            fullName,
            (int)role,
            therapistId ?? Guid.Empty,
            isVulnarable,
            (int)aiResponseStyleType,
            earnedPoints,
            isActive,
            createdAt ?? DefaultTimestamp,
            updatedAt ?? DefaultTimestamp);
    }
}
