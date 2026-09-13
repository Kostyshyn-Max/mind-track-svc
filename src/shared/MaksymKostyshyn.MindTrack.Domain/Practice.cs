using MaksymKostyshyn.MindTrack.Domain.Enums;

namespace MaksymKostyshyn.MindTrack.Domain;

public class Practice(
    Guid id,
    string name,
    int practiceTypeId,
    int difficultyLevel,
    int unlockPointsRequired,
    bool isActive)
{
    public Guid Id { get; private set; } = id;
    public string Name { get; private set; } = name;
    public PracticeType PracticeType { get; private set; } = (PracticeType)practiceTypeId;
    public int DifficultyLevel { get; private set; } = difficultyLevel;
    public int UnlockPointsRequired { get; private set; } = unlockPointsRequired;
    public bool IsActive { get; private set; } = isActive;
}
