using AElf.Indexing.Elasticsearch;
using AElfIndexer.Client;
using Nest;

namespace Points.Indexer.Plugin.Entities;

public class UserReferralCountIndex : AElfIndexerClientEntity<string>, IIndexBuild
{
    [Keyword] public string Domain { get; set; }
    
    [Keyword] public string DappId { get; set; }  
    
    [Keyword] public string Referrer { get; set; }
    
    public long InviteeNumber { get; set; }

    public long CreateTime { get; set; }
    
    public long UpdateTime { get; set; }
}