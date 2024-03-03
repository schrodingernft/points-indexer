using System.ComponentModel.DataAnnotations;
using Points.Contracts.Point;
using Points.Indexer.Plugin.Entities;

namespace Points.Indexer.Plugin.GraphQL;

public class GetAddressPointsLogDto
{
    [Required]public IncomeSourceType Role { get; set; }
    [Required]public string Address { get; set; }
}