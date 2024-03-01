using AElf.Indexing.Elasticsearch;
using AElfIndexer.Client;
using Nest;

namespace Points.Indexer.Plugin.Entities;

public class AddressPointsLogIndex : AElfIndexerClientEntity<string>, IIndexBuild
{
    [Keyword] public override string Id { get; set; }
    [Keyword] public string Address { get; set; }
    public string Domain { get; set; }
    public Role Role { get; set; }
    public string DappName { get; set; }
    public string Action { get; set; } 
    public decimal Amount { get; set; }
    public string SymbolName { get; set; }  

    public DateTime CreateTime { get; set; }
}
