namespace MaksymKostyshyn.MindTrack.Domain.Models;

public record UserSummary(Guid Id, string FullName, string Email, string AIStyleResponse, int EarnedPoints);
