using AElf;
using AElfIndexer.Client;
using AElfIndexer.Client.Handlers;
using AElfIndexer.Grains.State.Client;
using Points.Indexer.Plugin.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Orleans.Runtime;
using Volo.Abp.ObjectMapping;
using Points.Contracts.Point;

namespace Points.Indexer.Plugin.Processors;

public class AppliedLogEventProcessor : AElfLogEventProcessorBase<InviterApplied, LogEventInfo>
{
    private readonly IObjectMapper _objectMapper;
    private readonly ContractInfoOptions _contractInfoOptions;
    private readonly IAElfIndexerClientEntityRepository<OperatorDomainIndex, LogEventInfo> _operatorDomainRepository;
    private readonly ILogger<AppliedLogEventProcessor> _logger;
    
    public AppliedLogEventProcessor(ILogger<AppliedLogEventProcessor> logger, 
        IObjectMapper objectMapper,
        IAElfIndexerClientEntityRepository<OperatorDomainIndex, LogEventInfo> operatorDomainIndexRepository,
        IOptionsSnapshot<ContractInfoOptions> contractInfoOptions
        ) : base(logger)
    {
        _operatorDomainRepository = operatorDomainIndexRepository;
         _logger = logger;
        _contractInfoOptions = contractInfoOptions.Value;
        _objectMapper = objectMapper;
    }
    
    public override string GetContractAddress(string chainId)
    {
        return _contractInfoOptions.ContractInfos.First(c => c.ChainId == chainId).PointsContractAddress;
    }
    
    protected override async Task HandleEventAsync(InviterApplied eventValue, LogEventContext context)
    {
        _logger.Debug("AppliedEvent: {eventValue} context: {context}",JsonConvert.SerializeObject(eventValue), 
            JsonConvert.SerializeObject(context));
        
        var id = HashHelper.ComputeFrom(eventValue.Domain).ToHex();
        var domainIndex = await _operatorDomainRepository.GetFromBlockStateSetAsync(id, context.ChainId);
        
        if (domainIndex != null) 
        {
            _logger.Info("domain already exist: {index}",JsonConvert.SerializeObject(domainIndex));
            return;
        }

        domainIndex = new OperatorDomainIndex
        {
            Id = id,
            Domain = eventValue.Domain,
            DepositAddress = eventValue.Invitee.ToBase58(),
            DappId = eventValue.DappId.ToHex(),
            CreateTime = context.BlockTime
        };

        if (eventValue.Inviter != null)
        {
            domainIndex.InviterAddress = eventValue.Inviter.ToBase58();
        }
        _objectMapper.Map(context, domainIndex);
        
        await _operatorDomainRepository.AddOrUpdateAsync(domainIndex);
    }
}