using MaksymKostyshyn.MindTrack.Domain;

namespace MaksymKostyshyn.MindTrack.Data.Abstractions.Repositories;

public interface IReflectionAnswerRepository
{
    public Task<ReflectionAnswer?> AddReflectionAnswer(ReflectionAnswer reflectionAnswer, CancellationToken cancellationToken);
}
