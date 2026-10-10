namespace MaksymKostyshyn.MindTrack.Domain.Models;

public class PagedResult<T>(IList<T> items, int totalCount)
{
    public IList<T> Items { get; private set; } = items;
    public int TotalCount { get; private set; } = totalCount;
}
