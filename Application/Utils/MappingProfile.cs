using AutoMapper;
using URLshortner.Dtos;
using URLshortner.Models;

namespace URLshortner.Utils;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, RegisterRequest>().ReverseMap();
        CreateMap<User, UserResponse>().ReverseMap();
    }
}
