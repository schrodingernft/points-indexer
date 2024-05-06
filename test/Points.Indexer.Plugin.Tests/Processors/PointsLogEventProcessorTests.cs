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

        var pointsDetailList = new PointsChangedDetails();
        pointsDetailList.PointsDetails.Add( new PointsChangedDetail
        {
            Domain = "test.dapp.io",
            PointsReceiver = Address.FromBase58("xsnQafDAhNTeYcooptETqWnYBksFGGXxfcQyJJ5tmu6Ak9ZZt"),
            IncomeSourceType = IncomeSourceType.Inviter,
            PointsName = "TEST-1",
            Balance = 10000000
        });
        pointsDetailList.PointsDetails.Add(new PointsChangedDetail
        {
            Domain = "test.dapp.io",
            PointsReceiver = Address.FromBase58("2NxwCPAGJr4knVdmwhb1cK7CkZw5sMJkRDLnT7E2GoDP2dy5iZ"),
            IncomeSourceType = IncomeSourceType.Kol,
            PointsName = "TEST-2",
            Balance = 20000000
        });
        pointsDetailList.PointsDetails.Add(new PointsChangedDetail
        {
            Domain = "test.dapp.io",
            PointsReceiver = Address.FromBase58("2NxwCPAGJr4knVdmwhb1cK7CkZw5sMJkRDLnT7E2GoDP2dy5iZ"),
            IncomeSourceType = IncomeSourceType.Inviter,
            PointsName = "TEST-6",
            Balance = 40000000
        });
        
        var pointsUpdated = new PointsChanged()
        {
            PointsChangedDetails = pointsDetailList
        };
        
        var logEvent = MockLogEventInfo(pointsUpdated.ToLogEvent());
        
        var updatedProcessor = GetRequiredService<PointsRecordedLogEventProcessor>();
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

        var pointsRecordList = new PointsChangedDetails();
        pointsRecordList.PointsDetails.Add( new PointsChangedDetail
        {
            Domain = "test.dapp.io",
            PointsReceiver = Address.FromBase58("xsnQafDAhNTeYcooptETqWnYBksFGGXxfcQyJJ5tmu6Ak9ZZt"),
            IncomeSourceType = IncomeSourceType.Inviter,
            PointsName = "TEST-1",
            IncreaseAmount = 100000,
            IncreaseValue = "100000",
            ActionName = "Join",
            DappId = HashHelper.ComputeFrom("Schrodinger"),
            Balance = 40000000,
            BalanceValue = "40000000"
        });
        pointsRecordList.PointsDetails.Add(new PointsChangedDetail
        {
            Domain = "test.dapp.io",
            PointsReceiver = Address.FromBase58("2NxwCPAGJr4knVdmwhb1cK7CkZw5sMJkRDLnT7E2GoDP2dy5iZ"),
            IncomeSourceType = IncomeSourceType.Kol,
            PointsName = "TEST-2",
            IncreaseAmount = 200000,
            IncreaseValue = "200000",
            ActionName = "Increase",
            DappId = HashHelper.ComputeFrom("Schrodinger"),
            Balance = 50000000,
            BalanceValue = "50000000"

        });
        pointsRecordList.PointsDetails.Add(new PointsChangedDetail
        {
            Domain = "test.dapp.io",
            PointsReceiver = Address.FromBase58("2NxwCPAGJr4knVdmwhb1cK7CkZw5sMJkRDLnT7E2GoDP2dy5iZ"),
            IncomeSourceType = IncomeSourceType.Inviter,
            PointsName = "TEST-6",
            IncreaseAmount = 400000,
            IncreaseValue = "400000",
            ActionName = "Mint",
            DappId = HashHelper.ComputeFrom("Schrodinger"),
            Balance = 60000000,
            BalanceValue = "60000000"
        });
        
        var pointsRecorded = new PointsChanged()
        {
            PointsChangedDetails = pointsRecordList
        };
        
        var logEvent = MockLogEventInfo(pointsRecorded.ToLogEvent());
        
        var recordedProcessor = GetRequiredService<PointsRecordedLogEventProcessor>();
        var actionIndexRepository = GetRequiredService<IAElfIndexerClientEntityRepository<AddressPointsSumByActionIndex, LogEventInfo>>();
        var logIndexRepository = GetRequiredService<IAElfIndexerClientEntityRepository<AddressPointsLogIndex, LogEventInfo>>();
        var symbolIndexRepository = GetRequiredService<IAElfIndexerClientEntityRepository<AddressPointsSumBySymbolIndex, LogEventInfo>>();

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
        
        
        var pointsSumBySymbol = await Query.GetPointsSumBySymbol(symbolIndexRepository, objectMapper,
            new GetPointsSumBySymbolDto()
            {
                StartTime = DateTime.Now.AddHours(-1),
                EndTime = DateTime.Now.AddHours(1) 
            });
        pointsSumBySymbol.TotalRecordCount.ShouldBe(2);
    }
}



// curl 'http://172.31.11.5:8113/AElfIndexer_Points/PointsIndexerPluginSchema/graphql' \
// -H 'content-type: application/json' \
// --data-raw '{"query":"query{operatorDomainInfo(input:{domain:\"hope.wang.com\"}){domain, depositAddress, inviterAddress, dappId, createTime}}"}' \
// --compressed
//
//
// curl 'http://172.31.11.5:8113/AElfIndexer_Points/PointsIndexerPluginSchema/graphql' \
// -H 'content-type: application/json' \
// --data-raw '{"query":"query{checkDomainApplied(input:{domainList:[\"hope.wang.com\",\"xxxx\"]}){domainList}}"}' \
// --compressed
//
//
// curl 'http://172.31.11.5:8113/AElfIndexer_Points/PointsIndexerPluginSchema/graphql' \
// -H 'content-type: application/json' \
// --data-raw '{"query":"query{getPointsSumBySymbol(input:{startTime:\"2024-03-04T00:00:00\",endTime:\"2024-03-06T00:00:00\", skipCount:0, maxResultCount:100}){totalRecordCount, data{domain, firstSymbolAmount}}}"}' \
// --compressed
//
//
// curl 'http://172.31.11.5:8113/AElfIndexer_Points/PointsIndexerPluginSchema/graphql' \
// -H 'content-type: application/json' \
// --data-raw '{"query":"query{getPointsSumByAction(input:{dappId:\"5bb8592717834522278d11805dac3f042df1bcf02cdce461968daf2e1b543172\",address:\"23GxsoW9TRpLqX1Z5tjrmcRMMSn5bhtLAf4HtPj8JX9BerqTqp\",role:\"USER\"domain:\"hope.wang.com\"}){totalRecordCount, data{role}}}"}' \
// --compressed
//
//
//
//
// curl --location --request GET 'http://172.31.46.30:9200/aelfindexer_points-017dacc6540a4d7aa6369ef1e9000314.operatoruserindex/_search' \
// --header 'Content-Type: application/json' \
// --data '{
// "query": {
// "match_all": {}
// },
// "size":100
// }
// '
//
// curl --location --request GET 'http://172.31.46.30:9200/aelfindexer_points-017dacc6540a4d7aa6369ef1e9000314.addresspointslogindex/_search' \
// --header 'Content-Type: application/json' \
// --data '{
// "query": {
//     "match_all": {}
// },
// "size":100
// }
// '
//
// curl --location --request GET 'http://172.31.46.30:9200/aelfindexer_points-017dacc6540a4d7aa6369ef1e9000314.addresspointssumbyactionindex/_search' \
// --header 'Content-Type: application/json' \
// --data '{
// "query": {
//     "match_all": {}
// },
// "size":100
// }
// '
//
//
// curl --location --request GET 'http://172.31.46.30:9200/aelfindexer_points-017dacc6540a4d7aa6369ef1e9000314.addresspointssumbysymbolindex/_search' \
// --header 'Content-Type: application/json' \
// --data '{
// "query": {
//     "match_all": {}
// },
// "size":100
// }
// '