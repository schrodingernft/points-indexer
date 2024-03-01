using AElf.Indexing.Elasticsearch;
using AElfIndexer.Client;
using Nest;

namespace Points.Indexer.Plugin.Entities;

public class OperatorUserIndex : AElfIndexerClientEntity<string>, IIndexBuild
{
    [Keyword] public string Domain { get; set; }
    
    [Keyword] public string Address { get; set; }
    
    [Keyword] public string DappName { get; set; }

    public long CreateTime { get; set; }
}