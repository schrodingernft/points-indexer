using System.ComponentModel.DataAnnotations;
using Points.Indexer.Plugin.Entities;

namespace Points.Indexer.Plugin.GraphQL;

public class GetAddressPointsLogDto
{
    [Required]public Role Role { get; set; }
    [Required]public string Address { get; set; }
}