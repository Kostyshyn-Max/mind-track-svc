using System;
using System.Collections.Generic;

namespace MaksymKostyshyn.MindTrack.Data.EF.Scaffolded.Entities;

public partial class Achievement
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int PointsReward { get; set; }

    public virtual ICollection<UserAchievement> UserAchievements { get; set; } = new List<UserAchievement>();
}
