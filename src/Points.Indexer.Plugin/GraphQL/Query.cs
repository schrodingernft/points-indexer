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
        GetOperatorDomainDto dto)
    {
        if (dto.Domain.IsNullOrWhiteSpace()) return null;
        var domainIndex = await repository.GetAsync(dto.Domain.ToMd5());
        if (domainIndex == null) return null;

        return objectMapper.Map<OperatorDomainIndex, OperatorDomainDto>(domainIndex);
    }
    
    [Name("checkDomainApplied")]
    public static async Task<List<string>> CheckDomainApplied(
        [FromServices] IAElfIndexerClientEntityRepository<OperatorDomainIndex, LogEventInfo> repository,
        [FromServices] IObjectMapper objectMapper,
        CheckDomainAppliedDto dto)
    {
        if (dto.DomainList.IsNullOrEmpty()) return null;
        
        var mustQuery = new List<Func<QueryContainerDescriptor<OperatorDomainIndex>, QueryContainer>>();
        mustQuery.Add(q => q.Terms(i => i.Field(f => f.Domain)
            .Terms(dto.DomainList)));
        
        QueryContainer Filter(QueryContainerDescriptor<OperatorDomainIndex> f) =>
            f.Bool(b => b.Must(mustQuery));
        var domainIndexList = await repository.GetListAsync(Filter);

        return domainIndexList.Item2.Select(i => i.Domain).ToList();
    }
}