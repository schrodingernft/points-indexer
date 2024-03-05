using AElf;
using AElfIndexer.Client;
using AElfIndexer.Grains.State.Client;
using Points.Indexer.Plugin.Entities;
using GraphQL;
using Nest;
using Volo.Abp.ObjectMapping;

namespace Points.Indexer.Plugin.GraphQL;

public partial class Query
{
    [Name("operatorDomainInfo")]
    public static async Task<OperatorDomainDto> OperatorDomainInfo(
        [FromServices] IAElfIndexerClientEntityRepository<OperatorDomainIndex, LogEventInfo> repository,
        [FromServices] IObjectMapper objectMapper,
        GetOperatorDomainDto input)
    {
        if (input.Domain.IsNullOrWhiteSpace()) return new OperatorDomainDto();
        var id = HashHelper.ComputeFrom(input.Domain).ToHex();
        var domainIndex = await repository.GetAsync(id);
        if (domainIndex == null) return new OperatorDomainDto();

        return objectMapper.Map<OperatorDomainIndex, OperatorDomainDto>(domainIndex);
    }
    
    [Name("checkDomainApplied")]
    public static async Task<DomainAppliedDto> CheckDomainApplied(
        [FromServices] IAElfIndexerClientEntityRepository<OperatorDomainIndex, LogEventInfo> repository,
        CheckDomainAppliedDto input)
    {
        if (input.DomainList.IsNullOrEmpty()) return new DomainAppliedDto();
        
        var mustQuery = new List<Func<QueryContainerDescriptor<OperatorDomainIndex>, QueryContainer>>();
        mustQuery.Add(q => q.Terms(i => i.Field(f => f.Domain)
            .Terms(input.DomainList)));
        
        QueryContainer Filter(QueryContainerDescriptor<OperatorDomainIndex> f) =>
            f.Bool(b => b.Must(mustQuery));
        var domainIndexList = await repository.GetListAsync(Filter);

        return new DomainAppliedDto()
        {
            DomainList = domainIndexList.Item2.Select(i => i.Domain).ToList()
        };
    }


    [Name("getPointsSumBySymbol")]
    public static async Task<PointsSumBySymbolDtoList> GetPointsSumBySymbol(
        [FromServices] IAElfIndexerClientEntityRepository<AddressPointsSumBySymbolIndex, LogEventInfo> repository,
        [FromServices] IObjectMapper objectMapper,
        GetPointsSumBySymbolDto input)
    {

        var mustQuery = new List<Func<QueryContainerDescriptor<AddressPointsSumBySymbolIndex>, QueryContainer>>();
        
        if (input.StartTime != DateTime.MinValue)
        {
            mustQuery.Add(q => q.DateRange(i =>
                i.Field(f => f.UpdateTime)
                    .GreaterThanOrEquals(input.StartTime)));
        }

        if (input.EndTime != DateTime.MinValue)
        {
            mustQuery.Add(q => q.DateRange(i => 
                i.Field(f => f.UpdateTime)
                    .LessThan(input.EndTime)));
        }

        QueryContainer Filter(QueryContainerDescriptor<AddressPointsSumBySymbolIndex> f) =>
            f.Bool(b => b.Must(mustQuery));

        var recordList = await repository.GetListAsync(Filter, skip: input.SkipCount, limit: input.MaxResultCount);

        var dataList = objectMapper.Map<List<AddressPointsSumBySymbolIndex>, List<PointsSumBySymbolDto>>(recordList.Item2);
        return new PointsSumBySymbolDtoList
        {
            Data = dataList,
            TotalRecordCount = recordList.Item1
        };
    }
    
    [Name("getPointsSumByAction")]
    public static async Task<PointsSumByActionDtoList> GetPointsSumByAction(
        [FromServices] IAElfIndexerClientEntityRepository<AddressPointsSumByActionIndex, LogEventInfo> repository,
        [FromServices] IObjectMapper objectMapper,
        GetPointsSumByActionDto input)
    {

        var mustQuery = new List<Func<QueryContainerDescriptor<AddressPointsSumByActionIndex>, QueryContainer>>();

        if (input.DappId != "")
        {
            mustQuery.Add(q => q.Term(i => i.Field(f => f.DappId).Value(input.DappId)));
        }
        
        if (input.Address != "")
        {
            mustQuery.Add(q => q.Term(i => i.Field(f => f.Address).Value(input.Address)));
        }
        
        if (input.Domain != "")
        {
            mustQuery.Add(q => q.Term(i => i.Field(f => f.Domain).Value(input.Domain)));
        }

        if (input.Role != null)
        {
            mustQuery.Add(q => q.Term(i => i.Field(f => f.Role).Value(input.Role)));

        }

        QueryContainer Filter(QueryContainerDescriptor<AddressPointsSumByActionIndex> f) =>
            f.Bool(b => b.Must(mustQuery));

        var recordList = await repository.GetListAsync(Filter);
        
        var dataList = objectMapper.Map<List<AddressPointsSumByActionIndex>, List<PointsSumByActionDto>>(recordList.Item2);
        return new PointsSumByActionDtoList
        {
            Data = dataList,
            TotalRecordCount = recordList.Item1
        };
    }
    
    [Name("getAddressPointLog")]
    public static async Task<AddressPointsLogDtoList> GetAddressPointLog(
        [FromServices] IAElfIndexerClientEntityRepository<AddressPointsLogIndex, LogEventInfo> repository,
        [FromServices] IObjectMapper objectMapper,
        GetAddressPointsLogDto input)
    {

        var mustQuery = new List<Func<QueryContainerDescriptor<AddressPointsLogIndex>, QueryContainer>>();
        
        mustQuery.Add(q => q.Term(i => i.Field(f => f.Role).Value(input.Role)));
        mustQuery.Add(q => q.Term(i => i.Field(f => f.Address).Value(input.Address)));

        QueryContainer Filter(QueryContainerDescriptor<AddressPointsLogIndex> f) =>
            f.Bool(b => b.Must(mustQuery));

        var recordList = await repository.GetListAsync(Filter);
        
        var dataList = objectMapper.Map<List<AddressPointsLogIndex>, List<AddressPointsLogDto>>(recordList.Item2);
        return new AddressPointsLogDtoList
        {
            Data = dataList,
            TotalRecordCount = recordList.Item1
        };
    }
}