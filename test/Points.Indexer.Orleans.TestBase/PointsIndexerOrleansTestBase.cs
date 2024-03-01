using Orleans.TestingHost;
using Points.Indexer.TestBase;
using Volo.Abp.Modularity;

namespace Points.Indexer.Orleans.TestBase;

public abstract class PointsIndexerOrleansTestBase<TStartupModule>:PointsIndexerTestBase<TStartupModule> 
    where TStartupModule : IAbpModule
{
    protected readonly TestCluster Cluster;

    public PointsIndexerOrleansTestBase()
    {
        Cluster = GetRequiredService<ClusterFixture>().Cluster;
    }
}