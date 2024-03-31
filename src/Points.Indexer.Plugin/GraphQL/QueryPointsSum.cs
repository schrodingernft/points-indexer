using System.Linq.Expressions;
using AElfIndexer.Client;
using AElfIndexer.Grains.State.Client;
using GraphQL;
using Nest;
using Points.Contracts.Point;
using Points.Indexer.Plugin.Entities;
using Volo.Abp.ObjectMapping;

namespace Points.Indexer.Plugin.GraphQL;

public partial class Query
{
    [Name("getRankingList")]
    public static async Task<PointsSumListDto> GetRankingList(
        [FromServices] IAElfIndexerClientEntityRepository<AddressPointsSumBySymbolIndex, LogEventInfo> repository,
        [FromServices] IObjectMapper objectMapper,
        GetRankingListInput input)
    {
        var mustQuery = new List<Func<QueryContainerDescriptor<AddressPointsSumBySymbolIndex>, QueryContainer>>();
        
        if (!input.Keyword.IsNullOrWhiteSpace())
        {
            var shouldQuery = new List<Func<QueryContainerDescriptor<AddressPointsSumBySymbolIndex>, QueryContainer>>();
            shouldQuery.Add(q => q.Term(i => i.Field(f => f.Domain).Value(input.Keyword)));
            shouldQuery.Add(q => q.Term(i => i.Field(f => f.Address).Value(input.Keyword)));
            mustQuery.Add(q => q.Bool(b => b.Should(shouldQuery)));
        }

        mustQuery.Add(q => q.Terms(i =>
            i.Field(f => f.DappId).Terms(input.DappId)));
        mustQuery.Add(q => q.Term(i =>
            i.Field(f => f.Role).Value(IncomeSourceType.Kol)));
        
        QueryContainer Filter(QueryContainerDescriptor<AddressPointsSumBySymbolIndex> f) =>
            f.Bool(b => b.Must(mustQuery));
        
        var sortType = input.Sorting == "DESC" ? SortOrder.Descending : SortOrder.Ascending;
        var recordList = await repository.GetListAsync(Filter, sortType: sortType,
            sortExp: GetSortBy(input.SortingKeyWord), skip: input.SkipCount, limit: input.MaxResultCount);
        var dataList = objectMapper.Map<List<AddressPointsSumBySymbolIndex>, List<PointsSumDto>>(recordList.Item2);
        return new PointsSumListDto
        {
            Data = dataList,
            TotalCount = recordList.Item1
        };
    }
    
    
    [Name("getPointsEarnedList")]
    public static async Task<PointsSumListDto> GetPointsEarnedList(
        [FromServices] IAElfIndexerClientEntityRepository<AddressPointsSumBySymbolIndex, LogEventInfo> repository,
        [FromServices] IObjectMapper objectMapper,
        GetPointsEarnedListInput input)
    {
        var mustQuery = new List<Func<QueryContainerDescriptor<AddressPointsSumBySymbolIndex>, QueryContainer>>();
        
        mustQuery.Add(q => q.Terms(i =>
            i.Field(f => f.Address).Terms(input.Address)));
        mustQuery.Add(q => q.Terms(i =>
            i.Field(f => f.DappId).Terms(input.DappId)));

        if (input.Type == OperatorRole.All)
        {
            var shouldQuery = new List<Func<QueryContainerDescriptor<AddressPointsSumBySymbolIndex>, QueryContainer>>();
            shouldQuery.Add(q => q.Term(i => i.Field(f => f.Role).Value(OperatorRole.Kol)));
            shouldQuery.Add(q => q.Term(i => i.Field(f => f.Role).Value(OperatorRole.Inviter)));
            mustQuery.Add(q => q.Bool(b => b.Should(shouldQuery)));
        }
        else
        {
            mustQuery.Add(q => q.Term(i =>
                i.Field(f => f.Role).Value(input.Type)));
        }
        
        QueryContainer Filter(QueryContainerDescriptor<AddressPointsSumBySymbolIndex> f) => f.Bool(b => b.Must(mustQuery));

        var sortType = input.Sorting == "DESC" ? SortOrder.Descending : SortOrder.Ascending;
        var recordList = await repository.GetListAsync(Filter, sortType: sortType,
            sortExp: GetSortBy(input.SortingKeyWord), skip: input.SkipCount, limit: input.MaxResultCount);
        
        var dataList = objectMapper.Map<List<AddressPointsSumBySymbolIndex>, List<PointsSumDto>>(recordList.Item2);
        return new PointsSumListDto
        {
            Data = dataList,
            TotalCount = recordList.Item1
        };
    }
    
    private static Expression<Func<AddressPointsSumBySymbolIndex, object>> GetSortBy(SortingKeywordType sortingKeyWord)
    {
        return sortingKeyWord switch
        {
            SortingKeywordType.FirstSymbolAmount => a => a.FirstSymbolAmount,
            SortingKeywordType.SecondSymbolAmount => a => a.SecondSymbolAmount,
            SortingKeywordType.FiveSymbolAmount => a => a.FiveSymbolAmount,
            _ => a => a.FirstSymbolAmount
        };
    }
}