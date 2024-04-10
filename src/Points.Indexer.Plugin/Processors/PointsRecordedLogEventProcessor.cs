using System.Numerics;
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

public class PointsRecordedLogEventProcessor : AElfLogEventProcessorBase<PointsChanged, LogEventInfo>
{
    private readonly IObjectMapper _objectMapper;
    private readonly ContractInfoOptions _contractInfoOptions;
    private readonly IAElfIndexerClientEntityRepository<AddressPointsSumByActionIndex, LogEventInfo> _addressPointsSumByActionIndexRepository;
    private readonly IAElfIndexerClientEntityRepository<AddressPointsLogIndex, LogEventInfo> _addressPointsLogIndexRepository;
    private readonly IAElfIndexerClientEntityRepository<AddressPointsSumBySymbolIndex, LogEventInfo> _addressPointsSumBySymbolIndexRepository;
    private readonly ILogger<PointsRecordedLogEventProcessor> _logger;
    
    public PointsRecordedLogEventProcessor(ILogger<PointsRecordedLogEventProcessor> logger, 
        IObjectMapper objectMapper,
        IAElfIndexerClientEntityRepository<AddressPointsSumByActionIndex, LogEventInfo> addressPointsSumByActionIndexRepository,
        IAElfIndexerClientEntityRepository<AddressPointsLogIndex, LogEventInfo> addressPointsLogIndexRepository,
        IAElfIndexerClientEntityRepository<AddressPointsSumBySymbolIndex, LogEventInfo> addressPointsSumBySymbolIndexRepository,
        IOptionsSnapshot<ContractInfoOptions> contractInfoOptions
        ) : base(logger)
    {
        _addressPointsSumByActionIndexRepository = addressPointsSumByActionIndexRepository;
        _addressPointsLogIndexRepository = addressPointsLogIndexRepository;
        _addressPointsSumBySymbolIndexRepository = addressPointsSumBySymbolIndexRepository;
        _logger = logger;
        _contractInfoOptions = contractInfoOptions.Value;
        _objectMapper = objectMapper;
    }
    
    public override string GetContractAddress(string chainId)
    {
        return _contractInfoOptions.ContractInfos.First(c => c.ChainId == chainId).PointsContractAddress;
    }
    
    protected override async Task HandleEventAsync(PointsChanged eventValue, LogEventContext context)
    {
        _logger.Debug("PointsRecorded: {eventValue} context: {context}",JsonConvert.SerializeObject(eventValue), 
            JsonConvert.SerializeObject(context));

        foreach (var pointsDetail in eventValue.PointsChangedDetails.PointsDetails)
        {
            var rawLogIndexId = IdGenerateHelper.GetId(context.TransactionId, pointsDetail.DappId.ToHex(),
                pointsDetail.PointsReceiver.ToBase58(), pointsDetail.IncomeSourceType, pointsDetail.ActionName, pointsDetail.PointsName);
            var pointsLogIndexId = HashHelper.ComputeFrom(rawLogIndexId).ToHex();
            var pointsLogIndex = await _addressPointsLogIndexRepository.GetFromBlockStateSetAsync(pointsLogIndexId, context.ChainId);
            if (pointsLogIndex != null)
            {
                _logger.Info("Duplicated event index: {index}", pointsLogIndex);
                continue;
            }

            pointsLogIndex = _objectMapper.Map<PointsChangedDetail, AddressPointsLogIndex>(pointsDetail);
            pointsLogIndex.Amount = pointsDetail.IncreaseValue?.Value ?? pointsDetail.IncreaseAmount.ToString();
            pointsLogIndex.Id = pointsLogIndexId;
            pointsLogIndex.CreateTime = context.BlockTime;
            _objectMapper.Map(context, pointsLogIndex);
            await _addressPointsLogIndexRepository.AddOrUpdateAsync(pointsLogIndex);
            
            
            var rawActionIndexId = IdGenerateHelper.GetId(pointsDetail.DappId.ToHex(), pointsDetail.PointsReceiver.ToBase58(),
                pointsDetail.Domain, pointsDetail.ActionName, pointsDetail.IncomeSourceType);
            var pointsActionIndexId = HashHelper.ComputeFrom(rawActionIndexId).ToHex();
            var pointsActionIndex = await _addressPointsSumByActionIndexRepository.GetFromBlockStateSetAsync(pointsActionIndexId, context.ChainId);
            var increaseValue = pointsDetail.IncreaseValue?.Value ?? pointsDetail.IncreaseAmount.ToString();
            if (pointsActionIndex != null)
            {
                var amount = BigInteger.Parse(pointsActionIndex.Amount) + BigInteger.Parse(increaseValue);
                pointsActionIndex.Amount = amount.ToString();
            }
            else
            {
                pointsActionIndex = _objectMapper.Map<PointsChangedDetail, AddressPointsSumByActionIndex>(pointsDetail);
                pointsActionIndex.Id = pointsActionIndexId;
                pointsActionIndex.Amount = increaseValue;
                pointsActionIndex.CreateTime = context.BlockTime;
            }
            _objectMapper.Map(context, pointsActionIndex);
            pointsActionIndex.UpdateTime = context.BlockTime;
            await _addressPointsSumByActionIndexRepository.AddOrUpdateAsync(pointsActionIndex);
            
            
            var rawSymboIndexlId = IdGenerateHelper.GetId(pointsDetail.DappId.ToHex(), pointsDetail.PointsReceiver.ToBase58(), 
                pointsDetail.Domain, pointsDetail.IncomeSourceType);
            var pointsSymbolIndexId = HashHelper.ComputeFrom(rawSymboIndexlId).ToHex();
            var pointsIndex = await _addressPointsSumBySymbolIndexRepository.GetFromBlockStateSetAsync(pointsSymbolIndexId, context.ChainId);
            if (pointsIndex != null)
            {
                if (pointsIndex.UpdateTime > context.BlockTime)
                {
                    continue;
                }
                _objectMapper.Map(context, pointsIndex);
                var needUpdated = UpdatePoint(pointsDetail, pointsIndex, out var newIndex);
                if (!needUpdated)
                {
                    continue;
                }

                newIndex.UpdateTime = context.BlockTime;
                await _addressPointsSumBySymbolIndexRepository.AddOrUpdateAsync(newIndex);
            }
            else
            {
                pointsIndex = _objectMapper.Map<PointsChangedDetail, AddressPointsSumBySymbolIndex>(pointsDetail);
                _objectMapper.Map(context, pointsIndex);
                var needUpdated = UpdatePoint(pointsDetail, pointsIndex, out var newIndex);
                if (!needUpdated)
                {
                    continue;
                }
                
                newIndex.Id = pointsSymbolIndexId;
                newIndex.CreateTime = context.BlockTime;
                newIndex.UpdateTime = context.BlockTime;
                await _addressPointsSumBySymbolIndexRepository.AddOrUpdateAsync(newIndex);
            }
        }
    }
    
    private static bool UpdatePoint(PointsChangedDetail  pointsState, AddressPointsSumBySymbolIndex originIndex, out AddressPointsSumBySymbolIndex newIndex)
    {
        newIndex = originIndex;
        var symbol = pointsState.PointsName;
        var amount = pointsState.BalanceValue?.Value ?? pointsState.Balance.ToString();
        if (symbol.EndsWith("-1"))
        {
            newIndex.FirstSymbolAmount = amount;
        } else if (symbol.EndsWith("-2"))
        {
            newIndex.SecondSymbolAmount = amount;
        } else if (symbol.EndsWith("-3"))
        {
            newIndex.ThirdSymbolAmount = amount;
        }
        else if (symbol.EndsWith("-4"))
        {
            newIndex.FourSymbolAmount = amount;
        }
        else if (symbol.EndsWith("-5"))
        {
            newIndex.FiveSymbolAmount = amount;
        }
        else if (symbol.EndsWith("-6"))
        {
            newIndex.SixSymbolAmount = amount;
        }
        else if (symbol.EndsWith("-7"))
        {
            newIndex.SevenSymbolAmount = amount;
        }
        else if (symbol.EndsWith("-8"))
        {
            newIndex.EightSymbolAmount = amount;
        }
        else if (symbol.EndsWith("-9"))
        {
            newIndex.NineSymbolAmount = amount;
        }
        else
        {
            return false;
        }

        return true;
    }
}

