using AutoMapper;
using HallOfFameAPI.Data.Entities;
using HallOfFameAPI.DTOs;

namespace HallOfFameAPI.Configurations;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<PersonCreateDto, Person>();
        CreateMap<SkillDto, Skill>();

        CreateMap<Person, PersonResponseDto>();
        CreateMap<Skill, SkillDto>();

        CreateMap<PersonUpdateDto, Person>();
        CreateMap<SkillDto, Skill>();
    }
}