using AElf.Indexing.Elasticsearch;
using AElfIndexer.Client;
using Nest;

namespace Points.Indexer.Plugin.Entities;

public class OperatorDomainIndex : AElfIndexerClientEntity<string>, IIndexBuild
{
    [Keyword] public override string Id { get; set; }  
    
    [Keyword] public string Domain { get; set; }
    
    [Keyword] public string DepositAddress { get; set; }
    
    [Keyword] public string InviterAddress { get; set; }
    
    [Keyword] public string DappId { get; set; }  
    
    public DateTime CreateTime { get; set; }  
}