using AutoMapper;
using TokenBasedAuthApplication.Core.DTOs;
using TokenBasedAuthApplication.Core.Entities;

namespace TokenBasedAuthApplication.Business.Mappings.AutoMapper;

public class MappingProfile: Profile
{
    public MappingProfile()
    {
        CreateMap<ProductDto, Product>().ReverseMap();
        CreateMap<AppUserDto, AppUser>().ReverseMap();
    }
}