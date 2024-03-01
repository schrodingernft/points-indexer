using AElfIndexer.Client.Handlers;
using AutoMapper;
using Points.Indexer.Plugin.GraphQL;
using Points.Indexer.Plugin.Entities;

namespace Points.Indexer.Plugin;

public class PointsIndexerClientAutoMapperProfile : Profile
{
    public PointsIndexerClientAutoMapperProfile()
    {
        CreateMap<OperatorDomainIndex, OperatorDomainDto>();
        CreateMap<AddressPointsSumBySymbolIndex, PointsSumBySymbolDto>();
        CreateMap<AddressPointsSumByActionIndex, PointsSumByActionDto>();
        CreateMap<AddressPointsLogIndex, AddressPointsLogDto>();
    }
}