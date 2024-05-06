using AElfIndexer.Client.GraphQL;

namespace Points.Indexer.Plugin.GraphQL;

public class PointsIndexerPluginSchema : AElfIndexerClientSchema<Query>
{
    public PointsIndexerPluginSchema(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }
}