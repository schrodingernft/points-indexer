using Volo.Abp.Application.Dtos;

namespace Points.Indexer.Plugin.GraphQL.Dto;


public class OperatorUserPagerDto
{
    public long TotalRecordCount { get; set; }
    public List<OperatorUserDto> Data { get; set; }
}

public class OperatorUserDto
{
    public string Id { get; set; }
    public string Domain { get; set; }
    public string Address { get; set; }
    public string DappName { get; set; }
    public long CreateTime { get; set; }
}

public class OperatorUserRequestDto : PagedResultRequestDto
{
    public List<string> DomainIn { get; set; }
    public List<string> AddressIn { get; set; }
    public List<string> DappNameIn { get; set; }
    public long? CreateTimeLt { get; set; }
    public long? CreateTimeGtEq { get; set; }
}