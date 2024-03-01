using AElfIndexer.Client.Handlers;
using AutoMapper;
using Points.Indexer.Plugin.GraphQL;
using Points.Indexer.Plugin.Entities;
using Points.Indexer.Plugin.GraphQL.Dto;

namespace Points.Indexer.Plugin;

public class PointsIndexerClientAutoMapperProfile : Profile
{
    public PointsIndexerClientAutoMapperProfile()
    {
        CreateMap<OperatorDomainIndex, OperatorDomainDto>();
        CreateMap<LogEventContext, OperatorUserIndex>().ReverseMap();
        CreateMap<OperatorUserIndex, OperatorUserDto>().ReverseMap();
    }
}