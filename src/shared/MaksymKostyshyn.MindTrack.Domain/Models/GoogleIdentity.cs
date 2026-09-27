namespace MaksymKostyshyn.MindTrack.Domain.Models;

public record GoogleIdentity(string Email, bool EmailVerified, string Subject);
