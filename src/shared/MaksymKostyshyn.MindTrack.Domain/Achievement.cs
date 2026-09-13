namespace MaksymKostyshyn.MindTrack.Domain;

public class Achievement(
    Guid id,
    string name,
    string description,
    int pointsReward)
{
    public Guid Id { get; private set; } = id;
    public string Name { get; private set; } = name;
    public string Description { get; private set; } = description;
    public int PointsReward { get; private set; } = pointsReward;
}
