using System;
using System.Collections.Generic;

namespace MaksymKostyshyn.MindTrack.Data.EF.Scaffolded.Entities;

public partial class AssignedPractice
{
    public Guid Id { get; set; }

    public Guid JournalEntryId { get; set; }

    public Guid PracticeId { get; set; }

    public int PracticeStatusId { get; set; }

    public DateTime StartedAt { get; set; }

    public DateTime? CompletedAt { get; set; }

    public virtual JournalEntry JournalEntry { get; set; } = null!;

    public virtual Practice Practice { get; set; } = null!;

    public virtual PracticeStatus PracticeStatus { get; set; } = null!;
}
