using Microsoft.Extensions.DependencyInjection;
using Orleans;
using Points.Indexer.TestBase;
using Volo.Abp;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Points.Indexer.Orleans.TestBase;

[DependsOn(typeof(AbpAutofacModule),
    typeof(AbpTestBaseModule),
    typeof(PointsIndexerTestBaseModule)
)]
public class PointsIndexerOrleansTestBaseModule : AbpModule
{
    private ClusterFixture _fixture;

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        // if (_fixture == null)
        //     _fixture = new ClusterFixture();
        var _fixture = new ClusterFixture();
        context.Services.AddSingleton<ClusterFixture>(_fixture);
        context.Services.AddSingleton<IClusterClient>(sp => _fixture.Cluster.Client);
    }
}