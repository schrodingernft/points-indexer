using AElf.Indexing.Elasticsearch;
using AElfIndexer.Client;
using Nest;
using Points.Contracts.Point;

namespace Points.Indexer.Plugin.Entities;

public class AddressPointsSumBySymbolIndex : AElfIndexerClientEntity<string>, IIndexBuild
{
    [Keyword] public override string Id { get; set; }
    [Keyword] public string Address { get; set; }
    public string Domain { get; set; }
    public IncomeSourceType Role { get; set; }
    public long FirstSymbolAmount { get; set; } 
    public long SecondSymbolAmount { get; set; }
    public long ThirdSymbolAmount { get; set; } 
    
    public DateTime CreateTime { get; set; }
    public DateTime UpdateTime { get; set; }
}