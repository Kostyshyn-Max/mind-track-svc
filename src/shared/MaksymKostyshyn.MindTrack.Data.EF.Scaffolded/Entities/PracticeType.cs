using System;
using System.Collections.Generic;

namespace MaksymKostyshyn.MindTrack.Data.EF.Scaffolded.Entities;

public partial class PracticeType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Practice> Practices { get; set; } = new List<Practice>();
}
