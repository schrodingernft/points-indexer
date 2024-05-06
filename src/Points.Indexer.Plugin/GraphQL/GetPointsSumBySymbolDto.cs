using Volo.Abp.Application.Dtos;

namespace Points.Indexer.Plugin.GraphQL;

public class GetPointsSumBySymbolDto : PagedResultRequestDto
{
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
}