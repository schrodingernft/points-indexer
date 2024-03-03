using System.ComponentModel.DataAnnotations;
using Points.Contracts.Point;
using Points.Indexer.Plugin.Entities;

namespace Points.Indexer.Plugin.GraphQL;

public class GetPointsSumByActionDto
{ 
    [Required]public string DappId { get; set; }
    [Required]public string Address { get; set; }
    public string Domain { get; set; }
    public IncomeSourceType? Role { get; set; }
}