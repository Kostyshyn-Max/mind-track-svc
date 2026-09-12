using MaksymKostyshyn.MindTrack.Data.EF.Entities;

namespace MaksymKostyshyn.MindTrack.Data.EF.UnitTests.TestData;

public static class UserMockDataFactory
{
    public static readonly Guid TherapistAId = Guid.Parse("11111111-1111-1111-1111-111111111111");
    public static readonly Guid TherapistBId = Guid.Parse("22222222-2222-2222-2222-222222222222");

    public static List<UserRoleEntity> CreateUserRoles()
    {
        return
        [
            new UserRoleEntity { Id = 1, Name = "Administrator" },
            new UserRoleEntity { Id = 2, Name = "Therapist" },
            new UserRoleEntity { Id = 3, Name = "User" }
        ];
    }

    public static List<AIRepsonseStyleEntity> CreateAIResponseStyles()
    {
        return
        [
            new AIRepsonseStyleEntity { Id = 1, Name = "Standard" },
            new AIRepsonseStyleEntity { Id = 2, Name = "Cautious" }
        ];
    }

    public static List<UserEntity> CreateUsers()
    {
        var now = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        return
        [
            // TherapistAId's patients.
            new UserEntity
            {
                Id = Guid.Parse("aaaaaaaa-0001-0000-0000-000000000000"),
                Email = "charlie.patient@example.com",
                PasswordHash = "hash",
                FullName = "Charlie Patient",
                RoleId = 3,
                TherapishId = TherapistAId,
                IsVulnarable = false,
                AIResponseStyleId = 1,
                EarnedPoints = 40,
                IsActive = true,
                CreatedAt = now,
                UpdatedAt = now
            },
            new UserEntity
            {
                Id = Guid.Parse("aaaaaaaa-0003-0000-0000-000000000000"),
                Email = "bob.patient@example.com",
                PasswordHash = "hash",
                FullName = "Bob Patient",
                RoleId = 3,
                TherapishId = TherapistAId,
                IsVulnarable = false,
                AIResponseStyleId = 1,
                EarnedPoints = 80,
                IsActive = false,
                CreatedAt = now,
                UpdatedAt = now
            },

            new UserEntity
            {
                Id = Guid.Parse("aaaaaaaa-0004-0000-0000-000000000000"),
                Email = "dana.patient@example.com",
                PasswordHash = "hash",
                FullName = "Dana Patient",
                RoleId = 3,
                TherapishId = TherapistAId,
                IsVulnarable = false,
                AIResponseStyleId = 2,
                EarnedPoints = 60,
                IsActive = true,
                CreatedAt = now,
                UpdatedAt = now
            },

            // Inserted last (but alphabetically first) so the mock set can distinguish
            // "sort applied before paging" from "sort applied after paging" — see
            // UserQueryTests.GetPatientsFromTherapistAsync_MultiplePagesRequested_OrderingIsNotAppliedBeforePagination.
            new UserEntity
            {
                Id = Guid.Parse("aaaaaaaa-0002-0000-0000-000000000000"),
                Email = "alice.patient@example.com",
                PasswordHash = "hash",
                FullName = "Alice Patient",
                RoleId = 3,
                TherapishId = TherapistAId,
                IsVulnarable = true,
                AIResponseStyleId = 2,
                EarnedPoints = 120,
                IsActive = true,
                CreatedAt = now,
                UpdatedAt = now
            },

            // TherapistBId's patient — used to prove filtering doesn't leak across therapists.
            new UserEntity
            {
                Id = Guid.Parse("bbbbbbbb-0001-0000-0000-000000000000"),
                Email = "dave.patient@example.com",
                PasswordHash = "hash",
                FullName = "Dave Patient",
                RoleId = 3,
                TherapishId = TherapistBId,
                IsVulnarable = false,
                AIResponseStyleId = 2,
                EarnedPoints = 10,
                IsActive = true,
                CreatedAt = now,
                UpdatedAt = now
            },

            // Therapist entity itself — has no TherapishId of its own, used to prove it's excluded from its own patient list.
            new UserEntity
            {
                Id = TherapistAId,
                Email = "therapist.a@example.com",
                PasswordHash = "hash",
                FullName = "Therapist A",
                RoleId = 2,
                TherapishId = Guid.Empty,
                IsVulnarable = false,
                AIResponseStyleId = 1,
                EarnedPoints = 0,
                IsActive = true,
                CreatedAt = now,
                UpdatedAt = now
            }
        ];
    }
}
