using AElfIndexer.Client;
using AElfIndexer.Grains.State.Client;
using Points.Indexer.Plugin.Entities;
using GraphQL;
using Nest;
using Points.Indexer.Plugin.GraphQL.Dto;
using Volo.Abp.Application.Dtos;
using Volo.Abp.ObjectMapping;

namespace Points.Indexer.Plugin.GraphQL;

public partial class Query
{
    [Name("QueryUser")]
    public static async Task<OperatorUserPagerDto> QueryUser(
        [FromServices] IAElfIndexerClientEntityRepository<OperatorUserIndex, LogEventInfo> repository,
        [FromServices] IObjectMapper objectMapper,
        OperatorUserRequestDto dto)
    {
        var mustQuery = new List<Func<QueryContainerDescriptor<OperatorUserIndex>, QueryContainer>>();

        if (!dto.Domain.IsNullOrEmpty())
            mustQuery.Add(q => q.Terms(i => i.Field(f => f.Domain).Terms(dto.Domain)));

        if (!dto.Address.IsNullOrEmpty())
            mustQuery.Add(q => q.Terms(i => i.Field(f => f.Address).Terms(dto.Address)));

        if (!dto.DappName.IsNullOrEmpty())
            mustQuery.Add(q => q.Terms(i => i.Field(f => f.DappName).Terms(dto.DappName)));

        if (dto.CreateTimeLt != null)
            mustQuery.Add(q => q.LongRange(i => i.Field(f => f.CreateTime).LessThan(dto.CreateTimeLt)));

        if (dto.CreateTimeGtEq != null)
            mustQuery.Add(q => q.LongRange(i => i.Field(f => f.CreateTime).GreaterThanOrEquals(dto.CreateTimeGtEq)));

        QueryContainer Filter(QueryContainerDescriptor<OperatorUserIndex> f) => f.Bool(b => b.Must(mustQuery));
        var (total, list) = await repository.GetListAsync(Filter,
            sortExp: s => s.CreateTime, sortType: SortOrder.Descending,
            skip: dto.SkipCount, limit: dto.MaxResultCount);

        var dataList = objectMapper.Map<List<OperatorUserIndex>, List<OperatorUserDto>>(list);
        return new OperatorUserPagerDto
        {
            TotalRecordCount = total,
            Data = dataList
        };
    }
}