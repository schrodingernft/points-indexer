using AElfIndexer.Client;
using AElfIndexer.Client.Handlers;
using AElfIndexer.Grains.State.Client;
using Points.Indexer.Plugin.GraphQL;
using Points.Indexer.Plugin.Handlers;
using Microsoft.Extensions.DependencyInjection;
using Points.Indexer.Plugin.Processors;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace Points.Indexer.Plugin;

[DependsOn(typeof(AElfIndexerClientModule), typeof(AbpAutoMapperModule))]
public class PointsIndexerPluginModule : AElfIndexerClientPluginBaseModule<PointsIndexerPluginModule,
    PointsIndexerPluginSchema, Query>
{
    protected override void ConfigureServices(IServiceCollection serviceCollection)
    {
        var configuration = serviceCollection.GetConfiguration();
        Configure<ContractInfoOptions>(configuration.GetSection("ContractInfo"));
    
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, AppliedLogEventProcessor>();
        serviceCollection.AddSingleton<IBlockChainDataHandler, PointsTransactionHandler>();
        
    }
    
    protected override string ClientId => "AElfIndexer_Points";
    protected override string Version => "588d070550b04439986f2d7a6f01eb0b";
}