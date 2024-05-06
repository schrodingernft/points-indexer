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
        CreateMap<AddressPointsSumBySymbolIndex, PointsSumBySymbolDto>()
            .ForMember(t => t.FirstSymbolAmount, m => m.MapFrom(f => f.FirstSymbolAmount ?? "0"))
            .ForMember(t => t.SecondSymbolAmount, m => m.MapFrom(f => f.SecondSymbolAmount ?? "0"))
            .ForMember(t => t.ThirdSymbolAmount, m => m.MapFrom(f => f.ThirdSymbolAmount ?? "0"))
            .ForMember(t => t.FourSymbolAmount, m => m.MapFrom(f => f.FourSymbolAmount ?? "0"))
            .ForMember(t => t.FiveSymbolAmount, m => m.MapFrom(f => f.FiveSymbolAmount ?? "0"))
            .ForMember(t => t.SixSymbolAmount, m => m.MapFrom(f => f.SixSymbolAmount ?? "0"))
            .ForMember(t => t.SevenSymbolAmount, m => m.MapFrom(f => f.SevenSymbolAmount ?? "0"))
            .ForMember(t => t.EightSymbolAmount, m => m.MapFrom(f => f.EightSymbolAmount ?? "0"))
            .ForMember(t => t.NineSymbolAmount, m => m.MapFrom(f => f.NineSymbolAmount ?? "0"));
        CreateMap<AddressPointsSumByActionIndex, PointsSumByActionDto>();
        CreateMap<AddressPointsLogIndex, AddressPointsLogDto>();
        CreateMap<LogEventContext, OperatorUserIndex>().ReverseMap();
        CreateMap<OperatorUserIndex, OperatorUserDto>().ReverseMap();
        CreateMap<PointsChangedDetail, AddressPointsLogIndex>().ForMember(destination => destination.Address,
            opt => opt.MapFrom(source => source.PointsReceiver.ToBase58())).ForMember(destination => destination.Role,
            opt => opt.MapFrom(source => source.IncomeSourceType)).ForMember(destination => destination.DappId,
            opt => opt.MapFrom(source => source.DappId.ToHex()));
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
            .ForMember(t => t.FirstSymbolAmount, m => m.MapFrom(f => f.FirstSymbolAmount ?? "0"))
            .ForMember(t => t.SecondSymbolAmount, m => m.MapFrom(f => f.SecondSymbolAmount ?? "0"))
            .ForMember(t => t.ThirdSymbolAmount, m => m.MapFrom(f => f.ThirdSymbolAmount ?? "0"))
            .ForMember(t => t.FourSymbolAmount, m => m.MapFrom(f => f.FourSymbolAmount ?? "0"))
            .ForMember(t => t.FiveSymbolAmount, m => m.MapFrom(f => f.FiveSymbolAmount ?? "0"))
            .ForMember(t => t.SixSymbolAmount, m => m.MapFrom(f => f.SixSymbolAmount ?? "0"))
            .ForMember(t => t.SevenSymbolAmount, m => m.MapFrom(f => f.SevenSymbolAmount ?? "0"))
            .ForMember(t => t.EightSymbolAmount, m => m.MapFrom(f => f.EightSymbolAmount ?? "0"))
            .ForMember(t => t.NineSymbolAmount, m => m.MapFrom(f => f.NineSymbolAmount ?? "0"))
            ;
    }
}