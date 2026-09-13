using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaksymKostyshyn.MindTrack.Data.EF.Entities;

public class PracticeEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int PracticeTypeId { get; set; }
    [Range(0, 100)]
    public int DifficultyLevel { get; set; }
    public int UnlockPointsRequired { get; set; }
    public bool IsActive { get; set; }

    [ForeignKey(nameof(PracticeTypeId))]
    public PracticeTypeEntity PracticeType { get; set; } = null!;
} 