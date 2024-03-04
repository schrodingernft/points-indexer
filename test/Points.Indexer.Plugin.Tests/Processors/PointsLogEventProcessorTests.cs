using AElf;
using AElf.CSharp.Core.Extension;
using AElf.Types;
using AElfIndexer.Client;
using AElfIndexer.Grains.State.Client;
using Google.Protobuf.Collections;
using Points.Contracts.Point;
using Points.Indexer.Plugin.Entities;
using Points.Indexer.Plugin.GraphQL;
using Points.Indexer.Plugin.GraphQL.Dto;
using Points.Indexer.Plugin.Processors;
using Shouldly;
using Volo.Abp.ObjectMapping;
using Xunit;

namespace Points.Indexer.Plugin.Tests.Processors;

public class PointsLogEventProcessorTests : PointsIndexerPluginTestBase
{
    [Fact]
    public async Task HandlePointsUpdatedProcessor()
    {
        var context = MockLogEventContext();
        var state = await MockBlockState(context);

        var pointsStateList = new PointsStateList();
        pointsStateList.PointStates.Add( new PointsState
        {
            Domain = "test.dapp.io",
            Address = Address.FromBase58("xsnQafDAhNTeYcooptETqWnYBksFGGXxfcQyJJ5tmu6Ak9ZZt"),
            IncomeSourceType = IncomeSourceType.Inviter,
            PointName = "TEST-1",
            Balance = 10000000
        });
        pointsStateList.PointStates.Add(new PointsState
        {
            Domain = "test.dapp.io",
            Address = Address.FromBase58("2NxwCPAGJr4knVdmwhb1cK7CkZw5sMJkRDLnT7E2GoDP2dy5iZ"),
            IncomeSourceType = IncomeSourceType.Kol,
            PointName = "TEST-2",
            Balance = 20000000
        });
        pointsStateList.PointStates.Add(new PointsState
        {
            Domain = "test.dapp.io",
            Address = Address.FromBase58("2NxwCPAGJr4knVdmwhb1cK7CkZw5sMJkRDLnT7E2GoDP2dy5iZ"),
            IncomeSourceType = IncomeSourceType.Inviter,
            PointName = "TEST-6",
            Balance = 40000000
        });
        
        var pointsUpdated = new PointsUpdated()
        {
            PointStateList = pointsStateList
        };
        
        var logEvent = MockLogEventInfo(pointsUpdated.ToLogEvent());
        
        var updatedProcessor = GetRequiredService<PointsUpdatedLogEventProcessor>();
        var symbolIndexRepository = GetRequiredService<IAElfIndexerClientEntityRepository<AddressPointsSumBySymbolIndex, LogEventInfo>>();
        var objectMapper = GetRequiredService<IObjectMapper>();

        await updatedProcessor.HandleEventAsync(logEvent, context);
        await BlockStateSetSaveDataAsync<LogEventInfo>(state);

        var pointsSumBySymbol = await Query.GetPointsSumBySymbol(symbolIndexRepository, objectMapper,
            new GetPointsSumBySymbolDto()
            {
                StartTime = DateTime.Now.AddHours(-1),
                EndTime = DateTime.Now.AddHours(1)
            });
        pointsSumBySymbol.TotalRecordCount.ShouldBe(2);
    }
    
    
    [Fact]
    public async Task HandlePointsRecordedProcessor()
    {
        var context = MockLogEventContext();
        var state = await MockBlockState(context);

        var pointsRecordList = new PointsDetailList();
        pointsRecordList.PointsDetails.Add( new PointsDetail
        {
            Domain = "test.dapp.io",
            PointerAddress = Address.FromBase58("xsnQafDAhNTeYcooptETqWnYBksFGGXxfcQyJJ5tmu6Ak9ZZt"),
            IncomeSourceType = IncomeSourceType.Inviter,
            PointsName = "TEST-1",
            Amount = 10000000,
            ActionName = "Join",
            DappId = HashHelper.ComputeFrom("Schrodinger"),
        });
        pointsRecordList.PointsDetails.Add(new PointsDetail
        {
            Domain = "test.dapp.io",
            PointerAddress = Address.FromBase58("2NxwCPAGJr4knVdmwhb1cK7CkZw5sMJkRDLnT7E2GoDP2dy5iZ"),
            IncomeSourceType = IncomeSourceType.Kol,
            PointsName = "TEST-2",
            Amount = 20000000,
            ActionName = "Increase",
            DappId = HashHelper.ComputeFrom("Schrodinger"),
        });
        pointsRecordList.PointsDetails.Add(new PointsDetail
        {
            Domain = "test.dapp.io",
            PointerAddress = Address.FromBase58("2NxwCPAGJr4knVdmwhb1cK7CkZw5sMJkRDLnT7E2GoDP2dy5iZ"),
            IncomeSourceType = IncomeSourceType.Inviter,
            PointsName = "TEST-6",
            Amount = 40000000,
            ActionName = "Mint",
            DappId = HashHelper.ComputeFrom("Schrodinger"),
        });
        
        var pointsRecorded = new PointsDetails()
        {
            PointDetailList = pointsRecordList
        };
        
        var logEvent = MockLogEventInfo(pointsRecorded.ToLogEvent());
        
        var recordedProcessor = GetRequiredService<PointsRecordedLogEventProcessor>();
        var actionIndexRepository = GetRequiredService<IAElfIndexerClientEntityRepository<AddressPointsSumByActionIndex, LogEventInfo>>();
        var logIndexRepository = GetRequiredService<IAElfIndexerClientEntityRepository<AddressPointsLogIndex, LogEventInfo>>();
        var objectMapper = GetRequiredService<IObjectMapper>();

        await recordedProcessor.HandleEventAsync(logEvent, context);
        await BlockStateSetSaveDataAsync<LogEventInfo>(state);
        
        
        var pointsSumByAction1 = await Query.GetPointsSumByAction(actionIndexRepository, objectMapper,
            new GetPointsSumByActionDto()
            {
                Domain = "test.dapp.io",
                Address = "2NxwCPAGJr4knVdmwhb1cK7CkZw5sMJkRDLnT7E2GoDP2dy5iZ",
                DappId = "abface2803f57fa10f032baa58f30f748ef99c2b95e56f2a1b6a6e06faacc8f6",
            });
        pointsSumByAction1.TotalRecordCount.ShouldBe(2);
        
        
        var pointsSumByAction2 = await Query.GetPointsSumByAction(actionIndexRepository, objectMapper,
            new GetPointsSumByActionDto()
            {
                Domain = "test.dapp.io",
                Address = "2NxwCPAGJr4knVdmwhb1cK7CkZw5sMJkRDLnT7E2GoDP2dy5iZ",
                DappId = "abface2803f57fa10f032baa58f30f748ef99c2b95e56f2a1b6a6e06faacc8f6",
                Role = IncomeSourceType.Kol
            });
        pointsSumByAction2.TotalRecordCount.ShouldBe(1);
        
        
        var addressLog = await Query.GetAddressPointLog(logIndexRepository, objectMapper,
            new GetAddressPointsLogDto()
            {
                Address = "2NxwCPAGJr4knVdmwhb1cK7CkZw5sMJkRDLnT7E2GoDP2dy5iZ",
                Role = IncomeSourceType.Kol
            });
        addressLog.TotalRecordCount.ShouldBe(1);
    }
}