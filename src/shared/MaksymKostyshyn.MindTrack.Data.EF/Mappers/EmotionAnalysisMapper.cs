using MaksymKostyshyn.MindTrack.Data.EF.Entities;
using MaksymKostyshyn.MindTrack.Domain;

namespace MaksymKostyshyn.MindTrack.Data.EF.Mappers;

public static class EmotionAnalysisMapper
{
    public static EmotionAnalysesEntity ToEntity(this EmotionAnalysis emotionAnalysis)
    {
        return new EmotionAnalysesEntity
        {
            Id = emotionAnalysis.Id,
            JournalEntryId = emotionAnalysis.JournalEntryId,
            DominantEmotion = emotionAnalysis.DominantEmotion,
            IntonationScore = emotionAnalysis.IntonationScore,
            KeyPhrasesJson = emotionAnalysis.KeyPhrasesJson,
            AnalyzedAt = emotionAnalysis.AnalyzedAt
        };
    }

    public static EmotionAnalysis ToDomain(this EmotionAnalysesEntity entity)
    {
        return new EmotionAnalysis(
            entity.Id,
            entity.JournalEntryId,
            entity.DominantEmotion,
            entity.IntonationScore,
            entity.KeyPhrasesJson,
            entity.AnalyzedAt);
    }
}
