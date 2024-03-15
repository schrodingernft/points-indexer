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

public class PointsUpdatedLogEventProcessor : AElfLogEventProcessorBase<PointsUpdated, LogEventInfo>
{
    private readonly IObjectMapper _objectMapper;
    private readonly ContractInfoOptions _contractInfoOptions;
    private readonly IAElfIndexerClientEntityRepository<AddressPointsSumBySymbolIndex, LogEventInfo> _addressPointsSumBySymbolIndexRepository;
    private readonly ILogger<PointsUpdatedLogEventProcessor> _logger;
    
    public PointsUpdatedLogEventProcessor(ILogger<PointsUpdatedLogEventProcessor> logger, 
        IObjectMapper objectMapper,
        IAElfIndexerClientEntityRepository<AddressPointsSumBySymbolIndex, LogEventInfo> addressPointsSumBySymbolIndexRepository,
        IOptionsSnapshot<ContractInfoOptions> contractInfoOptions
        ) : base(logger)
    {
        _addressPointsSumBySymbolIndexRepository = addressPointsSumBySymbolIndexRepository;
        _logger = logger;
        _contractInfoOptions = contractInfoOptions.Value;
        _objectMapper = objectMapper;
    }
    
    public override string GetContractAddress(string chainId)
    {
        return _contractInfoOptions.ContractInfos.First(c => c.ChainId == chainId).PointsContractAddress;
    }
    
    protected override async Task HandleEventAsync(PointsUpdated eventValue, LogEventContext context)
    {
        _logger.Debug("PointsUpdated: {eventValue} context: {context}",JsonConvert.SerializeObject(eventValue), 
            JsonConvert.SerializeObject(context));

        foreach (var pointsState in eventValue.PointStateList.PointStates)
        {
            var raw = IdGenerateHelper.GetId(pointsState.Address, pointsState.Domain, pointsState.IncomeSourceType, pointsState.PointName);
            var id = HashHelper.ComputeFrom(raw).ToHex();
            var pointsIndex = await _addressPointsSumBySymbolIndexRepository.GetFromBlockStateSetAsync(id, context.ChainId);
            if (pointsIndex != null)
            {
                if (pointsIndex.UpdateTime > context.BlockTime)
                {
                    return;
                }
                _objectMapper.Map(context, pointsIndex);
                var hasUpdate = UpdatePoint(pointsState, pointsIndex, out var newIndex);
                if (!hasUpdate)
                {
                    continue;
                }

                newIndex.UpdateTime = context.BlockTime;
                await _addressPointsSumBySymbolIndexRepository.AddOrUpdateAsync(newIndex);
            }
            else
            {
                pointsIndex = _objectMapper.Map<PointsState, AddressPointsSumBySymbolIndex>(pointsState);
                _objectMapper.Map(context, pointsIndex);
                var hasUpdate = UpdatePoint(pointsState, pointsIndex, out var newIndex);
                if (!hasUpdate)
                {
                    continue;
                }
                
                newIndex.Id = id;
                newIndex.CreateTime = context.BlockTime;
                newIndex.UpdateTime = context.BlockTime;
                await _addressPointsSumBySymbolIndexRepository.AddOrUpdateAsync(newIndex);
            }
        }
    }

    private static bool UpdatePoint(PointsState  pointsState, AddressPointsSumBySymbolIndex originIndex, out AddressPointsSumBySymbolIndex newIndex)
    {
        newIndex = originIndex;
        var symbol = pointsState.PointName;
        var amount = pointsState.Balance;
        if (symbol.EndsWith("-1"))
        {
            newIndex.FirstSymbolAmount = amount;
        } else if (symbol.EndsWith("-2"))
        {
            newIndex.SecondSymbolAmount = amount;
        } else if (symbol.EndsWith("-3"))
        {
            newIndex.ThirdSymbolAmount = amount;
        }else if (symbol.EndsWith("-4"))
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

