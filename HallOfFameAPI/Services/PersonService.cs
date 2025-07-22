using AutoMapper;
using HallOfFameAPI.Data.Entities;
using HallOfFameAPI.Data.Repositories;
using HallOfFameAPI.DTOs;
using HallOfFameAPI.Validators;

namespace HallOfFameAPI.Services;

public class PersonService : IPersonService
{
    private readonly IMapper _mapper;
    private readonly IPersonRepository _personRepository;
    private readonly IPersonValidator _validator;

    public PersonService(IPersonRepository personRepository, IMapper mapper, IPersonValidator validator)
    {
        _personRepository = personRepository;
        _mapper = mapper;
        _validator = validator;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<PersonResponseDto>> GetAllPersonsAsync()
    {
        var persons = await _personRepository.GetAllPersonAsync();
        return _mapper.Map<IEnumerable<PersonResponseDto>>(persons);
    }

    /// <inheritdoc />
    public async Task<PersonResponseDto> GetPersonByIdAsync(long id)
    {
        var person = await _personRepository.GetPersonByIdAsync(id);
        return _mapper.Map<PersonResponseDto>(person);
    }

    /// <inheritdoc />
    public async Task<ServiceResult<PersonResponseDto>> AddPersonAsync(PersonCreateDto personDto)
    {
        var validationResult = _validator.Validate(personDto);
        if (!validationResult.IsValid)
            return ServiceResult<PersonResponseDto>.Failure(validationResult.ErrorMessage);

        var person = _mapper.Map<Person>(personDto);
        await _personRepository.AddPersonAsync(person);
        return ServiceResult<PersonResponseDto>.Success(_mapper.Map<PersonResponseDto>(person));
    }

    /// <inheritdoc />
    public async Task<ServiceResult<PersonResponseDto>> UpdatePersonAsync(long id, PersonUpdateDto personDto)
    {
        var validationResult = _validator.Validate(personDto);
        if (!validationResult.IsValid)
            return ServiceResult<PersonResponseDto>.Failure(validationResult.ErrorMessage);

        var existingPerson = await _personRepository.GetPersonByIdAsync(id);
        if (existingPerson == null)
            return ServiceResult<PersonResponseDto>.Failure("Person not found");

        var person = _mapper.Map<Person>(personDto);

        existingPerson.Name = personDto.Name;
        existingPerson.DisplayName = personDto.DisplayName;
        _personRepository.RemoveSkills(existingPerson.Skills);
        existingPerson.Skills = person.Skills.ToList();

        var updatePerson = await _personRepository.UpdatePersonAsync(existingPerson);
        return ServiceResult<PersonResponseDto>.Success(_mapper.Map<PersonResponseDto>(updatePerson));
    }

    /// <inheritdoc />
    public async Task DeletePersonAsync(long id)
    {
        await _personRepository.DeletePersonAsync(id);
    }
}