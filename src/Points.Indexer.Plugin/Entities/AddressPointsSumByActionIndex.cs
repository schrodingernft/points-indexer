using AElf.Indexing.Elasticsearch;
using AElfIndexer.Client;
using Nest;
using Points.Contracts.Point;

namespace Points.Indexer.Plugin.Entities;

public class AddressPointsSumByActionIndex : AElfIndexerClientEntity<string>, IIndexBuild
{
    [Keyword] public override string Id { get; set; }
    [Keyword] public string Address { get; set; }
    [Keyword] public string Domain { get; set; }
    public IncomeSourceType Role { get; set; }
    [Keyword] public string DappId { get; set; }
    [Keyword] public string ActionName { get; set; } 
    [Keyword] public string Amount { get; set; }
    [Keyword] public string PointsName { get; set; }  

    public DateTime CreateTime { get; set; }
    public DateTime UpdateTime { get; set; }
}