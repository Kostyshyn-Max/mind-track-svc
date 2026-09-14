namespace MaksymKostyshyn.MindTrack.Domain.Models;

public class AlertSeverityReport
{
    public string Severity { get; set; } = string.Empty;
    public int AlertCount { get; set; }
    public DateTime MostRecentAlert { get; set; }
    public int UnresolvedCount { get; set; }
}
