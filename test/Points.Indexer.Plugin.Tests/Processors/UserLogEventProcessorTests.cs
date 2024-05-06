using AElf;
using AElf.Contracts.Whitelist;
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

public sealed class UserLogEventProcessorTests : PointsIndexerPluginTestBase
{
    public UserLogEventProcessorTests()
    {
    }


    [Fact]
    public async Task HandleJoinedProcessor()
    {
        var context = MockLogEventContext();
        var state = await MockBlockState(context);
        var joined = new Joined()
        {
            Domain = "test.dapp.io",
            Registrant = Address.FromBase58("2NxwCPAGJr4knVdmwhb1cK7CkZw5sMJkRDLnT7E2GoDP2dy5iZ"),
            DappId = HashHelper.ComputeFrom("XUnit"),
        };
        var logEvent = MockLogEventInfo(joined.ToLogEvent());
        
        var joinedProcessor = GetRequiredService<JoinedLogEventProcessor>();
        var userRepository = GetRequiredService<IAElfIndexerClientEntityRepository<OperatorUserIndex, LogEventInfo>>();
        var objectMapper = GetRequiredService<IObjectMapper>();

        await joinedProcessor.HandleEventAsync(logEvent, context);
        await BlockStateSetSaveDataAsync<LogEventInfo>(state);
        
        var userPager = await Query.QueryUserAsync(userRepository, objectMapper, new OperatorUserRequestDto());
        userPager.ShouldNotBeNull();
        userPager.TotalRecordCount.ShouldBe(1);
        userPager.Data.ShouldNotBeEmpty();
        
        // Repeat execution, there is only one piece of data in the library.
        await joinedProcessor.HandleEventAsync(logEvent, context);
        await BlockStateSetSaveDataAsync<LogEventInfo>(state);
        
        userPager = await Query.QueryUserAsync(userRepository, objectMapper, new OperatorUserRequestDto());
        userPager.ShouldNotBeNull();
        userPager.TotalRecordCount.ShouldBe(1);
        userPager.Data.ShouldNotBeEmpty();
    }


    [Fact]
    public async Task HandleJoinedProcessorQuery()
    {
        await HandleJoinedProcessor();

        var ts = DateTime.UtcNow.ToUtcMilliSeconds();
        await Task.Delay(200);
        
        
        var context = MockLogEventContext();
        var state = await MockBlockState(context);
        var joined = new Joined()
        {
            Domain = "test.dapp.io",
            Registrant = Address.FromBase58("xsnQafDAhNTeYcooptETqWnYBksFGGXxfcQyJJ5tmu6Ak9ZZt"),
            DappId = HashHelper.ComputeFrom("XUnit"),
        };
        var logEvent = MockLogEventInfo(joined.ToLogEvent());
        
        var joinedProcessor = GetRequiredService<JoinedLogEventProcessor>();
        var userRepository = GetRequiredService<IAElfIndexerClientEntityRepository<OperatorUserIndex, LogEventInfo>>();
        var objectMapper = GetRequiredService<IObjectMapper>();

        await joinedProcessor.HandleEventAsync(logEvent, context);
        await BlockStateSetSaveDataAsync<LogEventInfo>(state);
        
        var userPager = await Query.QueryUserAsync(userRepository, objectMapper, new OperatorUserRequestDto
        {
            DomainIn = new List<string>{"test.dapp.io"},
            AddressIn = new List<string>{"xsnQafDAhNTeYcooptETqWnYBksFGGXxfcQyJJ5tmu6Ak9ZZt"}
            // CreateTimeGtEq = ts
            // CreateTimeLt = ts
        });
        userPager.ShouldNotBeNull();
        userPager.TotalRecordCount.ShouldBe(1);
        userPager.Data.ShouldNotBeEmpty();
    }

}