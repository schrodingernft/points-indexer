using Volo.Abp.Application.Dtos;

namespace Points.Indexer.Plugin.GraphQL;

public class GetRankingListInput : PagedAndSortedResultRequestDto
{
    public string Keyword { get; set; }
    public string DappId { get; set; }
    public SortingKeywordType SortingKeyWord { get; set; }
    public override string Sorting { get; set; } = "DESC";
}

public enum SortingKeywordType
{
    FirstSymbolAmount,
    SecondSymbolAmount,
    FiveSymbolAmount,
}