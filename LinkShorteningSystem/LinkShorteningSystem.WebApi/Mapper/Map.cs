using LinkShorteningSystem.Domain.Entities;
using LinkShorteningSystem.WebApi.Dtos;

namespace LinkShorteningSystem.WebApi.Mapper
{
    public sealed class Map : AutoMapper.Profile
    {
        public Map()
        {
            CreateMap<Link, LinkDto>().ReverseMap();

        }
    }
}
