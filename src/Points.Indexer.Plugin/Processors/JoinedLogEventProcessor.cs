using AElfIndexer.Client;
using AElfIndexer.Client.Handlers;
using AElfIndexer.Grains.State.Client;
using Points.Indexer.Plugin.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Orleans.Runtime;
using Points.Contracts.Point;
using Volo.Abp.ObjectMapping;

namespace Points.Indexer.Plugin.Processors;

public class JoinedLogEventProcessor : AElfLogEventProcessorBase<Joined, LogEventInfo>
{
    private readonly IObjectMapper _objectMapper;
    private readonly ContractInfoOptions _contractInfoOptions;
    private readonly IAElfIndexerClientEntityRepository<OperatorUserIndex, LogEventInfo> _operatorUserRepository;
    private readonly ILogger<JoinedLogEventProcessor> _logger;

    public JoinedLogEventProcessor(ILogger<JoinedLogEventProcessor> logger,
        IObjectMapper objectMapper,
        IOptionsSnapshot<ContractInfoOptions> contractInfoOptions,
        IAElfIndexerClientEntityRepository<OperatorUserIndex, LogEventInfo> operatorUserRepository) : base(logger)
    {
        _logger = logger;
        _contractInfoOptions = contractInfoOptions.Value;
        _objectMapper = objectMapper;
        _operatorUserRepository = operatorUserRepository;
    }

    public override string GetContractAddress(string chainId)
    {
        return _contractInfoOptions.ContractInfos.First(c => c.ChainId == chainId).PointsContractAddress;
    }

    protected override async Task HandleEventAsync(Joined eventValue, LogEventContext context)
    {
        try
        {
            _logger.Debug("JoinedEvent: {eventValue} context: {context}", JsonConvert.SerializeObject(eventValue),
                JsonConvert.SerializeObject(context));
            var id = IdGenerateHelper.GetId(eventValue.DappId.ToHex(), eventValue.Registrant.ToBase58());
            if (await _operatorUserRepository.GetAsync(id) != null)
            {
                _logger.LogWarning("User {User} of {DApp} exists", eventValue.Registrant.ToBase58(), eventValue.DappId.ToHex());
                return;
            }

            var user = new OperatorUserIndex
            {
                Id = id,
                Domain = eventValue.Domain,
                Address = eventValue.Registrant.ToBase58(),
                DappName = eventValue.DappId.ToHex(),
                CreateTime = context.BlockTime.ToUtcMilliSeconds()
            };

            _objectMapper.Map(context, user);
            await _operatorUserRepository.AddOrUpdateAsync(user);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "HandleEventAsync error");
        }

    }
}