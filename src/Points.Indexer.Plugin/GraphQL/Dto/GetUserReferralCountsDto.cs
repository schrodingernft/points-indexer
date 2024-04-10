using Volo.Abp.Application.Dtos;

namespace Points.Indexer.Plugin.GraphQL.Dto;

public class GetUserReferralCountsDto : PagedResultRequestDto
{
    public List<string> ReferrerList { get; set; }
}