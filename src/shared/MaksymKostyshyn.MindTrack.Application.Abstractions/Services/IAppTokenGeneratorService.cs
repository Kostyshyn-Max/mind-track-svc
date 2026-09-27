using MaksymKostyshyn.MindTrack.Domain;

namespace MaksymKostyshyn.MindTrack.Application.Abstractions.Services;

public interface IAppTokenGeneratorService
{
    public string GenerateToken(User user);
}
