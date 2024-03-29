using AElf;
using AElfIndexer.Client;
using AElfIndexer.Client.Handlers;
using AElfIndexer.Grains.State.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Orleans.Runtime;
using Points.Contracts.Point;
using Points.Indexer.Plugin.Entities;
using Volo.Abp.ObjectMapping;

namespace Points.Indexer.Plugin.Processors;

public class ReferralAcceptedLogEventProcessor : AElfLogEventProcessorBase<ReferralAccepted, LogEventInfo>
{
    private readonly IObjectMapper _objectMapper;
    private readonly ContractInfoOptions _contractInfoOptions;
    private readonly IAElfIndexerClientEntityRepository<UserReferralRecordIndex, LogEventInfo> _referralRecordRepository;
    private readonly IAElfIndexerClientEntityRepository<UserReferralCountIndex, LogEventInfo> _referralCountRepository;
    private readonly ILogger<ReferralAcceptedLogEventProcessor> _logger;
    
    public ReferralAcceptedLogEventProcessor(ILogger<ReferralAcceptedLogEventProcessor> logger, 
        IObjectMapper objectMapper,
        IAElfIndexerClientEntityRepository<UserReferralRecordIndex, LogEventInfo> referralRecordRepository,
        IAElfIndexerClientEntityRepository<UserReferralCountIndex, LogEventInfo> referralCountRepository,
        IOptionsSnapshot<ContractInfoOptions> contractInfoOptions
    ) : base(logger)
    {
        _referralRecordRepository = referralRecordRepository;
        _referralCountRepository = referralCountRepository;
        _logger = logger;
        _contractInfoOptions = contractInfoOptions.Value;
        _objectMapper = objectMapper;
    }
    
    public override string GetContractAddress(string chainId)
    {
        return _contractInfoOptions.ContractInfos.First(c => c.ChainId == chainId).PointsContractAddress;
    }

    protected override async Task HandleEventAsync(ReferralAccepted eventValue, LogEventContext context)
    {
        _logger.Info("ReferralAcceptedEvent: {eventValue} context: {context}",JsonConvert.SerializeObject(eventValue), 
            JsonConvert.SerializeObject(context));
        
        var rawRecordId = IdGenerateHelper.GetId(eventValue.DappId.ToHex(), eventValue.Referrer.ToBase58(), 
            eventValue.Invitee.ToBase58());
        var recordId = HashHelper.ComputeFrom(rawRecordId).ToHex();
        var recordIndex = await _referralRecordRepository.GetFromBlockStateSetAsync(recordId, context.ChainId);
        if (recordIndex != null) 
        {
            _logger.Info("invite record already exist: {index}",JsonConvert.SerializeObject(recordIndex));
            return;
        }
        recordIndex = new UserReferralRecordIndex
        {
            Id = recordId,
            Domain = eventValue.Domain,
            DappId = eventValue.DappId.ToHex(),
            Referrer = eventValue.Referrer?.ToBase58() ?? string.Empty,
            Invitee = eventValue.Invitee?.ToBase58() ?? string.Empty,
            Inviter = eventValue.Inviter?.ToBase58() ?? string.Empty,
            CreateTime = context.BlockTime.ToUtcMilliSeconds()
        };
        _objectMapper.Map(context, recordIndex);
        await _referralRecordRepository.AddOrUpdateAsync(recordIndex);
        
        var rawCountId = IdGenerateHelper.GetId(eventValue.DappId.ToHex(), eventValue.Referrer.ToBase58());
        var countId = HashHelper.ComputeFrom(rawCountId).ToHex();
        var countIndex = await _referralCountRepository.GetFromBlockStateSetAsync(countId, context.ChainId) ?? new UserReferralCountIndex
        {
            Id = countId,
            Domain = eventValue.Domain,
            DappId = eventValue.DappId.ToHex(),
            Referrer = eventValue.Referrer?.ToBase58() ?? string.Empty,
            InviteeNumber = 0,
            CreateTime = context.BlockTime.ToUtcMilliSeconds(),
            UpdateTime = context.BlockTime.ToUtcMilliSeconds()
        };
        countIndex.InviteeNumber += 1;
        _objectMapper.Map(context, countIndex);
        await _referralCountRepository.AddOrUpdateAsync(countIndex);

        _logger.Info("ReferralAcceptedEventProcessFinished");
    }
}