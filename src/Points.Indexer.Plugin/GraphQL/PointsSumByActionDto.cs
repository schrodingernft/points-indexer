using Points.Indexer.Plugin.Entities;

namespace Points.Indexer.Plugin.GraphQL;

public class PointsSumByActionDto
{
    public string Id { get; set; }
    public string Address { get; set; }
    public string Domain { get; set; }
    public Role Role { get; set; }
    public string DappName { get; set; }
    public string Action { get; set; } 
    public decimal Amount { get; set; }
    public string SymbolName { get; set; }  

    public DateTime CreateTime { get; set; }
    public DateTime UpdateTime { get; set; }
}

public class PointsSumByActionDtoList
{
    public long TotalRecordCount { get; set; }

    public List<PointsSumByActionDto> Data { get; set; }
}