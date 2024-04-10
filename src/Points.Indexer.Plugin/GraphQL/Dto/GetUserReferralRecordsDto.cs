using Volo.Abp.Application.Dtos;

namespace Points.Indexer.Plugin.GraphQL.Dto;

public class GetUserReferralRecordsDto : PagedResultRequestDto
{
    public List<string> ReferrerList { get; set; }
}