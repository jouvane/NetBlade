using AutoMapper;

namespace Movies.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
            : base("Movies.Application.MappingProfile")
        {
            //this.CreateMap<TSource, TDestination>();
        }
    }
}
