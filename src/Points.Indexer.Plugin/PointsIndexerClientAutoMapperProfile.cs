using AElfIndexer.Client.Handlers;
using AutoMapper;
using Points.Contracts.Point;
using Points.Indexer.Plugin.GraphQL;
using Points.Indexer.Plugin.Entities;
using Points.Indexer.Plugin.GraphQL.Dto;

namespace Points.Indexer.Plugin;

public class PointsIndexerClientAutoMapperProfile : Profile
{
    public PointsIndexerClientAutoMapperProfile()
    {
        CreateMap<OperatorDomainIndex, OperatorDomainDto>();
        CreateMap<AddressPointsSumBySymbolIndex, PointsSumBySymbolDto>();
        CreateMap<AddressPointsSumByActionIndex, PointsSumByActionDto>();
        CreateMap<AddressPointsLogIndex, AddressPointsLogDto>();
        CreateMap<LogEventContext, OperatorUserIndex>().ReverseMap();
        CreateMap<OperatorUserIndex, OperatorUserDto>().ReverseMap();
        CreateMap<PointsChangedDetail, AddressPointsLogIndex>().
            ForMember(destination => destination.Address,
            opt => opt.MapFrom(source => source.PointsReceiver.ToBase58())).
            ForMember(destination => destination.Role,
                opt => opt.MapFrom(source => source.IncomeSourceType)).
            ForMember(destination => destination.DappId,
                opt => opt.MapFrom(source => source.DappId.ToHex())).
            ForMember(destination => destination.Amount,
                opt => opt.MapFrom(source => source.IncreaseAmount));;
        CreateMap<PointsChangedDetail, AddressPointsSumByActionIndex>().
            ForMember(destination => destination.Address,
            opt => opt.MapFrom(source => source.PointsReceiver.ToBase58())).
            ForMember(destination => destination.Role,
                opt => opt.MapFrom(source => source.IncomeSourceType)).
            ForMember(destination => destination.DappId,
                opt => opt.MapFrom(source => source.DappId.ToHex()));
        
        CreateMap<PointsChangedDetail, AddressPointsSumBySymbolIndex>().
            ForMember(destination => destination.Address,
                opt => opt.MapFrom(source => source.PointsReceiver.ToBase58())).
            ForMember(destination => destination.Role,
                opt => opt.MapFrom(source => source.IncomeSourceType)).
            ForMember(destination => destination.DappId,
                opt => opt.MapFrom(source => source.DappId.ToHex()));;
        CreateMap<LogEventContext, OperatorDomainIndex>().ReverseMap();
        CreateMap<LogEventContext, AddressPointsSumBySymbolIndex>().ReverseMap();
        CreateMap<LogEventContext, AddressPointsSumByActionIndex>().ReverseMap();
        CreateMap<LogEventContext, AddressPointsLogIndex>().ReverseMap();
        CreateMap<LogEventContext, OperatorDomainIndex>().ReverseMap();
        
        CreateMap<LogEventContext, UserReferralRecordIndex>().ReverseMap();
        CreateMap<LogEventContext, UserReferralCountIndex>().ReverseMap();
        CreateMap<UserReferralRecordIndex, UserReferralRecordsDto>().ReverseMap();
        CreateMap<UserReferralCountIndex, UserReferralCountsDto>().ReverseMap();
        
        CreateMap<AddressPointsSumBySymbolIndex, PointsSumDto>()
            .ForMember(t => t.UpdateTime, m => m.MapFrom(f => f.UpdateTime.ToUtcMilliSeconds()))
            .ForMember(t => t.DappName, m => m.MapFrom(f => f.DappId))
            ;
    }
}