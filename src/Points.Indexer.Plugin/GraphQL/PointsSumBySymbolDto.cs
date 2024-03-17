using Points.Contracts.Point;
using Points.Indexer.Plugin.Entities;

namespace Points.Indexer.Plugin.GraphQL;

public class PointsSumBySymbolDto
{
    public string Id { get; set; }
    public string Address { get; set; }
    public string Domain { get; set; }
    public IncomeSourceType Role { get; set; }
    public string DappId { get; set; }
    public long FirstSymbolAmount { get; set; } 
    public long SecondSymbolAmount { get; set; }
    public long ThirdSymbolAmount { get; set; }
    public long FourSymbolAmount { get; set; } 
    public long FiveSymbolAmount { get; set; } 
    public long SixSymbolAmount { get; set; } 
    public long SevenSymbolAmount { get; set; } 
    public long EightSymbolAmount { get; set; } 
    public long NineSymbolAmount { get; set; } 
    public DateTime CreateTime { get; set; }
    public DateTime UpdateTime { get; set; }
}

public class PointsSumBySymbolDtoList
{
    public long TotalRecordCount { get; set; }

    public List<PointsSumBySymbolDto> Data { get; set; }
}