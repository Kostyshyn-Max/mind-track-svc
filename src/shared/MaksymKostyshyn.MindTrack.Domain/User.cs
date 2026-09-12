namespace MaksymKostyshyn.MindTrack.Domain;

public class User(
    Guid id,
    string email,
    string passwordHash,
    string fullName,
    int roleId,
    Guid therapistId,
    bool isVulnarable,
    int aiResponseStyleTypeId,
    int earnedPoints,
    bool isActive,
    DateTime createdAt,
    DateTime updatedAt)
{
    public Guid Id { get; private set; } = id;
    public string Email { get; private set; } = email;
    public string PasswordHash { get; private set; } = passwordHash;
    public string FullName { get; private set; } = fullName;
    public UserRole Role { get; private set; } = (UserRole)roleId;
    public Guid TherapishId { get; private set; } = therapistId;
    public bool IsVulnarable { get; private set; } = isVulnarable;
    public AIResponseStyleType AIResponseStyleType { get; private set; } = (AIResponseStyleType)aiResponseStyleTypeId;
    public int EarnedPoints { get; private set; } = earnedPoints;

    public bool IsActive { get; private set; } = isActive;
    public DateTime CreatedAt { get; private set; } = createdAt;
    public DateTime UpdatedAt { get; private set; } = updatedAt;
}
