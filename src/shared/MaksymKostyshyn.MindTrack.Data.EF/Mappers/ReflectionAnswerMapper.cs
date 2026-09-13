using MaksymKostyshyn.MindTrack.Data.EF.Entities;
using MaksymKostyshyn.MindTrack.Domain;

namespace MaksymKostyshyn.MindTrack.Data.EF.Mappers;

public static class ReflectionAnswerMapper
{
    public static ReflectionAnswerEntity ToEntity(this ReflectionAnswer reflectionAnswer)
    {
        return new ReflectionAnswerEntity
        {
            Id = reflectionAnswer.Id,
            JournalEntryId = reflectionAnswer.JournalEntryId,
            Questions = reflectionAnswer.Questions,
            Answers = reflectionAnswer.Answers,
            PreviousAnswerId = reflectionAnswer.PreviousAnswerId,
            AnsweredAt = reflectionAnswer.AnsweredAt
        };
    }

    public static ReflectionAnswer ToDomain(this ReflectionAnswerEntity entity)
    {
        return new ReflectionAnswer(
            entity.Id,
            entity.JournalEntryId,
            entity.Questions,
            entity.Answers,
            entity.PreviousAnswerId,
            entity.AnsweredAt);
    }
}
