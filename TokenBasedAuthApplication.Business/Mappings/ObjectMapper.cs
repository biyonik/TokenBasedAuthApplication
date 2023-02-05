using AutoMapper;
using TokenBasedAuthApplication.Business.Mappings.AutoMapper;

namespace TokenBasedAuthApplication.Business.Mappings;

public static class ObjectMapper
{
    private static readonly Lazy<IMapper> lazy = new Lazy<IMapper>(() =>
    {
        var config = new MapperConfiguration(c =>
        {
            c.AddProfile<MappingProfile>();
        });
        return config.CreateMapper();
    });

    public static IMapper Mapper = lazy.Value;
}