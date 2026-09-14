using System;
using System.Collections.Generic;

namespace MaksymKostyshyn.MindTrack.Data.EF.Scaffolded.Entities;

public partial class Practice
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public int PracticeTypeId { get; set; }

    public int DifficultyLevel { get; set; }

    public int UnlockPointsRequired { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<AssignedPractice> AssignedPractices { get; set; } = new List<AssignedPractice>();

    public virtual PracticeType PracticeType { get; set; } = null!;
}
