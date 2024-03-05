using AElfIndexer.Client;
using AElfIndexer.Grains.State.Client;
using Points.Indexer.Plugin.Entities;
using GraphQL;
using Nest;
using Points.Indexer.Plugin.GraphQL.Dto;
using Volo.Abp.ObjectMapping;

namespace Points.Indexer.Plugin.GraphQL;

public partial class Query
{
    [Name("QueryUserAsync")]
    public static async Task<OperatorUserPagerDto> QueryUserAsync(
        [FromServices] IAElfIndexerClientEntityRepository<OperatorUserIndex, LogEventInfo> repository,
        [FromServices] IObjectMapper objectMapper,
        OperatorUserRequestDto input)
    {
        var mustQuery = new List<Func<QueryContainerDescriptor<OperatorUserIndex>, QueryContainer>>();

        if (!input.DomainIn.IsNullOrEmpty())
            mustQuery.Add(q => q.Terms(i => i.Field(f => f.Domain).Terms(input.DomainIn)));

        if (!input.AddressIn.IsNullOrEmpty())
            mustQuery.Add(q => q.Terms(i => i.Field(f => f.Address).Terms(input.AddressIn)));

        if (!input.DappNameIn.IsNullOrEmpty())
            mustQuery.Add(q => q.Terms(i => i.Field(f => f.DappName).Terms(input.DappNameIn)));

        if (input.CreateTimeLt != null)
            mustQuery.Add(q => q.LongRange(i => i.Field(f => f.CreateTime).LessThan(input.CreateTimeLt)));

        if (input.CreateTimeGtEq != null)
            mustQuery.Add(q => q.LongRange(i => i.Field(f => f.CreateTime).GreaterThanOrEquals(input.CreateTimeGtEq)));

        QueryContainer Filter(QueryContainerDescriptor<OperatorUserIndex> f) => f.Bool(b => b.Must(mustQuery));
        var (total, list) = await repository.GetListAsync(Filter,
            sortExp: s => s.CreateTime, sortType: SortOrder.Descending,
            skip: input.SkipCount, limit: input.MaxResultCount);

        var dataList = objectMapper.Map<List<OperatorUserIndex>, List<OperatorUserDto>>(list);
        return new OperatorUserPagerDto
        {
            TotalRecordCount = total,
            Data = dataList
        };
    }
}