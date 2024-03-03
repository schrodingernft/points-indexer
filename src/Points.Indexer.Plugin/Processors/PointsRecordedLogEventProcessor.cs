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

public class PointsRecordedLogEventProcessor : AElfLogEventProcessorBase<PointsRecorded, LogEventInfo>
{
    private readonly IObjectMapper _objectMapper;
    private readonly ContractInfoOptions _contractInfoOptions;
    private readonly IAElfIndexerClientEntityRepository<AddressPointsSumByActionIndex, LogEventInfo> _addressPointsSumByActionIndexRepository;
    private readonly IAElfIndexerClientEntityRepository<AddressPointsLogIndex, LogEventInfo> _addressPointsLogIndexRepository;
    private readonly ILogger<PointsRecordedLogEventProcessor> _logger;
    
    public PointsRecordedLogEventProcessor(ILogger<PointsRecordedLogEventProcessor> logger, 
        IObjectMapper objectMapper,
        IAElfIndexerClientEntityRepository<AddressPointsSumByActionIndex, LogEventInfo> addressPointsSumByActionIndexRepository,
        IAElfIndexerClientEntityRepository<AddressPointsLogIndex, LogEventInfo> addressPointsLogIndexRepository,
        IOptionsSnapshot<ContractInfoOptions> contractInfoOptions
        ) : base(logger)
    {
        _addressPointsSumByActionIndexRepository = addressPointsSumByActionIndexRepository;
        _addressPointsLogIndexRepository = addressPointsLogIndexRepository;
        _logger = logger;
        _contractInfoOptions = contractInfoOptions.Value;
        _objectMapper = objectMapper;
    }
    
    public override string GetContractAddress(string chainId)
    {
        return _contractInfoOptions.ContractInfos.First(c => c.ChainId == chainId).PointsContractAddress;
    }
    
    protected override async Task HandleEventAsync(PointsRecorded eventValue, LogEventContext context)
    {
        _logger.Debug("PointsRecorded: {eventValue} context: {context}",JsonConvert.SerializeObject(eventValue), 
            JsonConvert.SerializeObject(context));

        foreach (var pointsRecord in eventValue.PointsRecordList.PointsRecords)
        {
            var rawLogIndexId = IdGenerateHelper.GetId(context.TransactionId, pointsRecord.DappId.ToHex(),
                pointsRecord.PointerAddress.ToBase58(), pointsRecord.IncomeSourceType);
            var pointsLogIndexId = HashHelper.ComputeFrom(rawLogIndexId).ToHex();
            var pointsLogIndex = await _addressPointsLogIndexRepository.GetFromBlockStateSetAsync(pointsLogIndexId, context.ChainId);
            if (pointsLogIndex != null)
            {
                continue;
            }

            pointsLogIndex = _objectMapper.Map<PointsRecord, AddressPointsLogIndex>(pointsRecord);
            pointsLogIndex.Id = pointsLogIndexId;
            pointsLogIndex.CreateTime = context.BlockTime;
            _objectMapper.Map(context, pointsLogIndex);
            
            await _addressPointsLogIndexRepository.AddOrUpdateAsync(pointsLogIndex);
            
            
            var rawActionIndexId = IdGenerateHelper.GetId(pointsRecord.DappId.ToHex(), pointsRecord.PointerAddress.ToBase58(),
                pointsRecord.ActionName, pointsRecord.IncomeSourceType);
            var pointsActionIndexId = HashHelper.ComputeFrom(rawActionIndexId).ToHex();
            var pointsActionIndex = await _addressPointsSumByActionIndexRepository.GetFromBlockStateSetAsync(pointsActionIndexId, context.ChainId);
            if (pointsActionIndex != null)
            {
                pointsActionIndex.Amount += pointsRecord.Amount;
            }
            else
            {
                pointsActionIndex = _objectMapper.Map<PointsRecord, AddressPointsSumByActionIndex>(pointsRecord);
                pointsActionIndex.Id = pointsActionIndexId;
                pointsActionIndex.CreateTime = context.BlockTime;
                _objectMapper.Map(context, pointsActionIndex);
            }

            pointsActionIndex.UpdateTime = context.BlockTime;
            await _addressPointsSumByActionIndexRepository.AddOrUpdateAsync(pointsActionIndex);
        }
    }
}

