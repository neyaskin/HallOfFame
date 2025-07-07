using AutoMapper;
using HallOfFameAPI.Data.Entities;
using HallOfFameAPI.DTOs;

namespace HallOfFameAPI.Configurations;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<PersonCreateDto, Person>();
        CreateMap<SkillCreateDto, Skill>();

        CreateMap<Person, PersonResponseDto>();
        CreateMap<Skill, SkillResponseDto>();
        
        CreateMap<PersonUpdateDto, Person>();
        CreateMap<SkillResponseDto, Skill>();
    }
}