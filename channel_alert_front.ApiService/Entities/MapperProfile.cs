using AutoMapper;

using channel_alert_front.Shared.DataTransferObject;

namespace channel_alert_front.ApiService.Entities;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<User, UserDto>()
            .ForMember(userDto => userDto.Email, (opt) => opt.MapFrom(x => x.Email))
            .ForMember(userDto => userDto.Id, (opt) => opt.MapFrom(x => x.Id));

        CreateMap<AlertHistory, AlertHistoryDto>();
    }
}
