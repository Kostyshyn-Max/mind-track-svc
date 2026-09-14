using System;
using System.Collections.Generic;

namespace MaksymKostyshyn.MindTrack.Data.EF.Scaffolded.Entities;

public partial class AiresponseStyle
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
