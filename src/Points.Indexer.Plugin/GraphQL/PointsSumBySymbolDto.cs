using Points.Indexer.Plugin.Entities;

namespace Points.Indexer.Plugin.GraphQL;

public class PointsSumBySymbolDto
{
    public string Id { get; set; }
    public string Address { get; set; }
    public string Domain { get; set; }
    public Role Role { get; set; }
    public string DappName { get; set; }
    public string Symbol1 { get; set; } 
    public string Symbol2 { get; set; }
    public string Symbol3 { get; set; } 
    
    public DateTime CreateTime { get; set; }
    public DateTime UpdateTime { get; set; }
}

public class PointsSumBySymbolDtoList
{
    public long TotalRecordCount { get; set; }

    public List<PointsSumBySymbolDto> Data { get; set; }
}