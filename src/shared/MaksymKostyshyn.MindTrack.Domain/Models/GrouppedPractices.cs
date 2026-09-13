namespace MaksymKostyshyn.MindTrack.Domain.Models;

public class GrouppedPractices
{
    public string CategoryName { get; set; } = string.Empty;
    public int Count { get; set; }
    public IList<Practice> Practices = [];
}
