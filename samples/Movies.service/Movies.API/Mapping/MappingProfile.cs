using AutoMapper;
using Elasticsearch.Net;

namespace Movies.API.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
            : base("Movies.API.MappingProfile")
        {
        }
    }
}
