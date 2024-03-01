using System.ComponentModel.DataAnnotations;

namespace Points.Indexer.Plugin.GraphQL;

public class GetPointsSumByActionDto
{ 
    [Required]public string DappName { get; set; }
    [Required]public string Domain { get; set; }
    [Required]public string Address { get; set; }
}