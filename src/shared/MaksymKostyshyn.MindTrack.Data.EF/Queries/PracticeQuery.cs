using MaksymKostyshyn.MindTrack.Data.Abstractions.Queries;
using MaksymKostyshyn.MindTrack.Data.EF.Mappers;
using MaksymKostyshyn.MindTrack.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MaksymKostyshyn.MindTrack.Data.EF.Queries;

public class PracticeQuery(MindTrackDbContext dbContext, ILogger<PracticeQuery> logger) : IPracticeQuery
{
    private readonly MindTrackDbContext _dbContext = dbContext;
    private readonly ILogger<PracticeQuery> _logger = logger;

    public async Task<IList<GrouppedPractices>?> GetAllPracticesGrouppedByCategory(CancellationToken cancellationToken)
    {
        try
        {
            var practices = await _dbContext.Practice
                .GroupBy(practice => practice.PracticeType)
                .Select(group => new GrouppedPractices 
                    { 
                        CategoryName = group.Key.ToString()!,
                        Count = group.Count(),
                        Practices = group.ToList().Select(practice => practice.ToDomain()).ToList() 
                    }
                )
                .ToListAsync(cancellationToken);

            return practices;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to retrieve practices");

            return null;
        }
    }
}
