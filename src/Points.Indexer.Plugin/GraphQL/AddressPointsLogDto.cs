using Points.Contracts.Point;
using Points.Indexer.Plugin.Entities;

namespace Points.Indexer.Plugin.GraphQL;

public class AddressPointsLogDto
{
    public string Id { get; set; }
    public string Address { get; set; }
    public string Domain { get; set; }
    public IncomeSourceType Role { get; set; }
    public string DappId { get; set; }
    public string ActionName { get; set; } 
    public long Amount { get; set; }
    public string PointsName { get; set; }  

    public DateTime CreateTime { get; set; }
}

public class AddressPointsLogDtoList
{
    public long TotalRecordCount { get; set; }

    public List<AddressPointsLogDto> Data { get; set; }
}