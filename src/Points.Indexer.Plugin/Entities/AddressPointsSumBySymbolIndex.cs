using System.Numerics;
using AElf.Indexing.Elasticsearch;
using AElfIndexer.Client;
using Nest;
using Points.Contracts.Point;

namespace Points.Indexer.Plugin.Entities;

public class AddressPointsSumBySymbolIndex : AElfIndexerClientEntity<string>, IIndexBuild
{
    [Keyword] public override string Id { get; set; }
    [Keyword] public string Address { get; set; }
    [Keyword] public string Domain { get; set; }
    [Keyword] public string DappId { get; set; }
    public IncomeSourceType Role { get; set; }
    [Keyword] public string FirstSymbolAmount { get; set; } 
    [Keyword] public string SecondSymbolAmount { get; set; }
    [Keyword] public string ThirdSymbolAmount { get; set; }

    [Keyword] public string FourSymbolAmount { get; set; }
    [Keyword] public string FiveSymbolAmount { get; set; }
    [Keyword] public string SixSymbolAmount { get; set; } 
    [Keyword] public string SevenSymbolAmount { get; set; } 
    [Keyword] public string EightSymbolAmount { get; set; } 
    [Keyword] public string NineSymbolAmount { get; set; } 

    public DateTime CreateTime { get; set; }
    public DateTime UpdateTime { get; set; }
}