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

public class ReferralAcceptedLogEventProcessorTests : PointsIndexerPluginTestBase
{
    [Fact]
    public async Task HandleReferralAcceptedProcessor()
    {
        var context = MockLogEventContext();
        var state = await MockBlockState(context);
        var referralAcceptedEvent1 = new ReferralAccepted()
        {
            Domain = "test.dapp.io",
            Inviter = Address.FromBase58("2NxwCPAGJr4knVdmwhb1cK7CkZw5sMJkRDLnT7E2GoDP2dy5iZ"),
            Invitee = Address.FromBase58("xsnQafDAhNTeYcooptETqWnYBksFGGXxfcQyJJ5tmu6Ak9ZZt"),
            Referrer = Address.FromBase58("3yDz5oUiKHqhFJj1zPRmbLWFs9ErhGbWTwr9BR7uMXRMmJHMn"),
            DappId = HashHelper.ComputeFrom("Schrodinger"),
        };
        var logEvent1 = MockLogEventInfo(referralAcceptedEvent1.ToLogEvent());

        var processor = GetRequiredService<ReferralAcceptedLogEventProcessor>();
        var recordRepository = GetRequiredService<IAElfIndexerClientEntityRepository<UserReferralRecordIndex, LogEventInfo>>();
        var countRepository = GetRequiredService<IAElfIndexerClientEntityRepository<UserReferralCountIndex, LogEventInfo>>();
        var objectMapper = GetRequiredService<IObjectMapper>();

        await processor.HandleEventAsync(logEvent1, context);
        await BlockStateSetSaveDataAsync<LogEventInfo>(state);
        
        var records1 = await Query.GetUserReferralRecords(recordRepository, objectMapper, 
            new GetUserReferralRecordsDto()
        {
            ReferrerList = new List<string>
            {
                "3yDz5oUiKHqhFJj1zPRmbLWFs9ErhGbWTwr9BR7uMXRMmJHMn",
                "2NxwCPAGJr4knVdmwhb1cK7CkZw5sMJkRDLnT7E2GoDP2dy5iZ"
            }
        });
        records1.TotalRecordCount.ShouldBe(1);

        var counts1 = await Query.GetUserReferralCounts(countRepository, objectMapper, 
            new GetUserReferralCountsDto()
        {
            ReferrerList = new List<string>
            {
                "3yDz5oUiKHqhFJj1zPRmbLWFs9ErhGbWTwr9BR7uMXRMmJHMn",
                "2NxwCPAGJr4knVdmwhb1cK7CkZw5sMJkRDLnT7E2GoDP2dy5iZ"
            }
        });
        counts1.TotalRecordCount.ShouldBe(1);
        counts1.Data[0].InviteeNumber.ShouldBe(1);
        
        
        
        var referralAcceptedEvent2 = new ReferralAccepted()
        {
            Domain = "test.dapp.io",
            Inviter = Address.FromBase58("2NxwCPAGJr4knVdmwhb1cK7CkZw5sMJkRDLnT7E2GoDP2dy5iZ"),
            Invitee = Address.FromBase58("23GxsoW9TRpLqX1Z5tjrmcRMMSn5bhtLAf4HtPj8JX9BerqTqp"),
            Referrer = Address.FromBase58("3yDz5oUiKHqhFJj1zPRmbLWFs9ErhGbWTwr9BR7uMXRMmJHMn"),
            DappId = HashHelper.ComputeFrom("Schrodinger"),
        };
        var logEvent2 = MockLogEventInfo(referralAcceptedEvent2.ToLogEvent());
        await processor.HandleEventAsync(logEvent2, context);
        await BlockStateSetSaveDataAsync<LogEventInfo>(state);
        
        var records2 = await Query.GetUserReferralRecords(recordRepository, objectMapper, 
            new GetUserReferralRecordsDto()
            {
                ReferrerList = new List<string>
                {
                    "3yDz5oUiKHqhFJj1zPRmbLWFs9ErhGbWTwr9BR7uMXRMmJHMn",
                    "2NxwCPAGJr4knVdmwhb1cK7CkZw5sMJkRDLnT7E2GoDP2dy5iZ"
                }
            });
        records2.TotalRecordCount.ShouldBe(2);

        var counts2 = await Query.GetUserReferralCounts(countRepository, objectMapper, 
            new GetUserReferralCountsDto()
            {
                ReferrerList = new List<string>
                {
                    "3yDz5oUiKHqhFJj1zPRmbLWFs9ErhGbWTwr9BR7uMXRMmJHMn",
                    "2NxwCPAGJr4knVdmwhb1cK7CkZw5sMJkRDLnT7E2GoDP2dy5iZ"
                }
            });
        counts2.TotalRecordCount.ShouldBe(1);
        counts2.Data[0].InviteeNumber.ShouldBe(2);
    }
}