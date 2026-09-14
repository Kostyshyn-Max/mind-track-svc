using System;
using System.Collections.Generic;

namespace MaksymKostyshyn.MindTrack.Data.EF.Scaffolded.Entities;

public partial class ReflectionAnswer
{
    public Guid Id { get; set; }

    public Guid JournalEntryId { get; set; }

    public string Questions { get; set; } = null!;

    public string Answers { get; set; } = null!;

    public Guid? PreviousAnswerId { get; set; }

    public DateTime AnsweredAt { get; set; }

    public virtual ICollection<ReflectionAnswer> InversePreviousAnswer { get; set; } = new List<ReflectionAnswer>();

    public virtual JournalEntry JournalEntry { get; set; } = null!;

    public virtual ReflectionAnswer? PreviousAnswer { get; set; }
}
