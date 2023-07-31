using FluentNHibernate.Automapping;
using LinkShorteningSystem.Domain.Dtos;
using LinkShorteningSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkShorteningSystem.Infrastructure.Mapper
{
    public sealed class Map : AutoMapper.Profile
    {
        public Map()
        {
            CreateMap<Link, LinkDto>().ReverseMap();

        }
    }
}
