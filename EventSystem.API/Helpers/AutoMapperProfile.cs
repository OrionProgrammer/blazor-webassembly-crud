using AutoMapper;
using EventSystem.Domain;
using EventSystem.Model;

namespace EventSystem.API.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Event, EventModel>().ReverseMap();
            CreateMap<Event, EventModel>();
        }
    }
}
