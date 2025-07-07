using AutoMapper;
using HallOfFameAPI.Data.Repositories;
using HallOfFameAPI.Data.Entities;
using HallOfFameAPI.DTOs;


namespace HallOfFameAPI.Services;

public class PersonService : IPersonService
{
    private readonly IPersonRepository _personRepository;
    private readonly IMapper _mapper;

    public PersonService(IPersonRepository personRepository, IMapper mapper)
    {
        _personRepository = personRepository;
        _mapper = mapper;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<PersonResponseDto>> GetAllPersonsAsync()
    {
        var persons = await _personRepository.GetAllPersonAsync();
        return _mapper.Map<IEnumerable<PersonResponseDto>>(persons);
    }
    
    /// <inheritdoc/>
    public async Task<PersonResponseDto> GetPersonByIdAsync(long id)
    {
        var person = await _personRepository.GetPersonByIdAsync(id);
        var response = _mapper.Map<PersonResponseDto>(person);
        return response;
    }

    /// <inheritdoc/>
    public async Task<PersonResponseDto> AddPersonAsync(PersonCreateDto personDto)
    {
        var person = _mapper.Map<Person>(personDto);
        await _personRepository.AddPersonAsync(person);
        return _mapper.Map<PersonResponseDto>(person);
    }

    /// <inheritdoc/>
    public async Task<PersonResponseDto> UpdatePersonAsync(long id, PersonUpdateDto personDto)
    {
        var existingPerson = await _personRepository.GetPersonByIdAsync(id);
        if (existingPerson == null) return null;
        
        var person = _mapper.Map<Person>(personDto);
        
        existingPerson.Name = personDto.Name;
        existingPerson.DisplayName = personDto.DisplayName;
        _personRepository.RemoveSkills(existingPerson.Skills);
        existingPerson.Skills = person.Skills.ToList();
        
        var updatePerson = await _personRepository.UpdatePersonAsync(existingPerson);
        var responseDto = _mapper.Map<PersonResponseDto>(updatePerson);
        return responseDto;
    }

    /// <inheritdoc/>
    public async Task DeletePersonAsync(long id)
    {
        await _personRepository.DeletePersonAsync(id);
    }
}