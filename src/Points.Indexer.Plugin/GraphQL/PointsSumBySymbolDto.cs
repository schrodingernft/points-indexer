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
    public string FirstSymbolAmount { get; set; } 
    public string SecondSymbolAmount { get; set; }
    public string ThirdSymbolAmount { get; set; }
    public string FourSymbolAmount { get; set; } 
    public string FiveSymbolAmount { get; set; } 
    public string SixSymbolAmount { get; set; } 
    public string SevenSymbolAmount { get; set; } 
    public string EightSymbolAmount { get; set; } 
    public string NineSymbolAmount { get; set; } 
    public DateTime CreateTime { get; set; }
    public DateTime UpdateTime { get; set; }
}

public class PointsSumBySymbolDtoList
{
    public long TotalRecordCount { get; set; }

    public List<PointsSumBySymbolDto> Data { get; set; }
}