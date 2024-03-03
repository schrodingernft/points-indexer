using AElf.Indexing.Elasticsearch;
using AElfIndexer.Client;
using Nest;
using Points.Contracts.Point;

namespace Points.Indexer.Plugin.Entities;

public class AddressPointsSumByActionIndex : AElfIndexerClientEntity<string>, IIndexBuild
{
    [Keyword] public override string Id { get; set; }
    [Keyword] public string Address { get; set; }
    public string Domain { get; set; }
    public IncomeSourceType Role { get; set; }
    public string DappId { get; set; }
    public string ActionName { get; set; } 
    public long Amount { get; set; }
    public string PointsName { get; set; }  

    public DateTime CreateTime { get; set; }
    public DateTime UpdateTime { get; set; }
}