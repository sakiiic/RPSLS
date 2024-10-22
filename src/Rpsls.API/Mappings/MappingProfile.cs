using AutoMapper;
using Rpsls.API.Dto;
using Rpsls.API.Entities;

namespace Rpsls.API.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Choice, ChoiceDto>();
        }
    }
}