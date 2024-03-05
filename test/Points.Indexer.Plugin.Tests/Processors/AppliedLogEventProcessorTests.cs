using AElf;
using AElf.CSharp.Core.Extension;
using AElf.Types;
using AElfIndexer.Client;
using AElfIndexer.Grains.State.Client;
using Points.Contracts.Point;
using Points.Indexer.Plugin.Entities;
using Points.Indexer.Plugin.GraphQL;
using Points.Indexer.Plugin.GraphQL.Dto;
using Points.Indexer.Plugin.Processors;
using Shouldly;
using Volo.Abp.ObjectMapping;
using Xunit;

namespace Points.Indexer.Plugin.Tests.Processors;

public class AppliedLogEventProcessorTests : PointsIndexerPluginTestBase
{
    [Fact]
    public async Task HandleAppliedProcessor()
    {
        var context = MockLogEventContext();
        var state = await MockBlockState(context);
        var joined = new InviterApplied()
        {
            Domain = "test.dapp.io",
            Inviter = Address.FromBase58("2NxwCPAGJr4knVdmwhb1cK7CkZw5sMJkRDLnT7E2GoDP2dy5iZ"),
            Invitee = Address.FromBase58("xsnQafDAhNTeYcooptETqWnYBksFGGXxfcQyJJ5tmu6Ak9ZZt"),
            DappId = HashHelper.ComputeFrom("Schrodinger"),
        };
        var logEvent = MockLogEventInfo(joined.ToLogEvent());

        var appliedProcessor = GetRequiredService<AppliedLogEventProcessor>();
        var domainRepository = GetRequiredService<IAElfIndexerClientEntityRepository<OperatorDomainIndex, LogEventInfo>>();
        var objectMapper = GetRequiredService<IObjectMapper>();

        await appliedProcessor.HandleEventAsync(logEvent, context);
        await BlockStateSetSaveDataAsync<LogEventInfo>(state);
        
        var domain = await Query.CheckDomainApplied(domainRepository, new CheckDomainAppliedDto()
        {
            DomainList = new List<string>()
            {
                "test3.dapp.io",
                "test.dapp.io",
                "test2.dapp.io"
            }
        });
        domain.DomainList.ShouldContain("test.dapp.io");
        domain.DomainList.ShouldNotContain("test2.dapp.io");

        var info = await Query.OperatorDomainInfo(domainRepository, objectMapper, new GetOperatorDomainDto()
        {
            Domain = "test.dapp.io",
        });
        info.DepositAddress.ShouldBe("xsnQafDAhNTeYcooptETqWnYBksFGGXxfcQyJJ5tmu6Ak9ZZt");
    }
}