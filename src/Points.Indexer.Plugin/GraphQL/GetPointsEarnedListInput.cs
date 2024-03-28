using Volo.Abp.Application.Dtos;

namespace Points.Indexer.Plugin.GraphQL;

public class GetPointsEarnedListInput : PagedAndSortedResultRequestDto
{
    public override string Sorting { get; set; } = "DESC";
    public string Address { get; set; }
    public string DappId { get; set; }
    public OperatorRole Type { get; set; }
    public SortingKeywordType SortingKeyWord { get; set; }
}

public enum OperatorRole
{
    Inviter,
    Kol,
    User,
    All = -1
}