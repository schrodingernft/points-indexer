using AElf.Indexing.Elasticsearch;
using AElfIndexer.Client;
using Nest;

namespace Points.Indexer.Plugin.Entities;

public class UserReferralRecordIndex : AElfIndexerClientEntity<string>, IIndexBuild
{
    [Keyword] public string Domain { get; set; }
    
    [Keyword] public string DappId { get; set; }  
    
    [Keyword] public string Referrer { get; set; }
    
    [Keyword] public string Invitee { get; set; }
    
    [Keyword] public string Inviter { get; set; }

    public long CreateTime { get; set; }
}