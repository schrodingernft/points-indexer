namespace Points.Indexer.Plugin.GraphQL;

public class OperatorDomainDto
{
    public  string Id { get; set; }  
    
    public string Domain { get; set; }
    
    public string DepositAddress { get; set; }
    
    public string InviterAddress { get; set; }
    
    public string DappName { get; set; }  
    
    public DateTime CreateTime { get; set; }  
}