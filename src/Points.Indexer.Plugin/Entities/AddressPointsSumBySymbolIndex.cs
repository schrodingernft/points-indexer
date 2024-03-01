using AElf.Indexing.Elasticsearch;
using AElfIndexer.Client;
using Nest;

namespace Points.Indexer.Plugin.Entities;

public class AddressPointsSumBySymbolIndex : AElfIndexerClientEntity<string>, IIndexBuild
{
    [Keyword] public override string Id { get; set; }
    [Keyword] public string Address { get; set; }
    public string Domain { get; set; }
    public Role Role { get; set; }
    public string DappName { get; set; }
    public string Symbol1 { get; set; } 
    public string Symbol2 { get; set; }
    public string Symbol3 { get; set; } 
    
    public DateTime CreateTime { get; set; }
    public DateTime UpdateTime { get; set; }
}

public enum Role
{
    
}