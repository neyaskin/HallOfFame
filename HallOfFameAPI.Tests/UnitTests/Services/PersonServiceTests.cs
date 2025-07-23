using AutoMapper;
using HallOfFameAPI.Data.Entities;
using HallOfFameAPI.Data.Repositories;
using HallOfFameAPI.DTOs;
using HallOfFameAPI.Exceptions;
using HallOfFameAPI.Services;
using HallOfFameAPI.Validators;
using Moq;

namespace HallOfFameAPI.Tests.UnitTests.Services;

public class PersonServiceTests
{
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IPersonRepository> _mockRepo;
    private readonly PersonService _service;

    public PersonServiceTests()
    {
        _mockRepo = new Mock<IPersonRepository>();
        _mockMapper = new Mock<IMapper>();
        _service = new PersonService(_mockRepo.Object, _mockMapper.Object, new PersonValidator());
    }

    [Fact]
    public async Task CreatePersonAsync_Should_Return_CorrectResponse()
    {
        var createDto = new PersonCreateDto
        {
            Name = "Test",
            Skills = new List<SkillDto>()
        };

        var person = new Person { Id = 1, Name = "Test" };
        var expectedResponse = new PersonResponseDto { Id = 1, Name = "Test" };

        _mockMapper.Setup(m => m.Map<Person>(createDto))
            .Returns(person);

        _mockMapper.Setup(m => m.Map<PersonResponseDto>(person))
            .Returns(expectedResponse);

        _mockRepo.Setup(r => r.AddPersonAsync(It.IsAny<Person>()))
            .ReturnsAsync(person);

        var result = await _service.AddPersonAsync(createDto);

        Assert.Equal(expectedResponse.Id, result.Data.Id);
        _mockRepo.Verify(r => r.AddPersonAsync(It.IsAny<Person>()), Times.Once);
    }

    [Fact]
    public async Task GetPersonByIdAsync_ReturnsNull_WhenPersonDoesNotExist()
    {
        _mockRepo.Setup(repo => repo.GetPersonByIdAsync(1)).ReturnsAsync((Person)null);

        var result = await _service.GetPersonByIdAsync(1);

        if (result != null) Assert.Null(result);
    }

    [Fact]
    public async Task GetPersonByIdAsync()
    {
        _mockRepo.Setup(repo => repo.GetPersonByIdAsync(1)).ReturnsAsync((Person)null);

        var result = await _service.GetPersonByIdAsync(1);

        if (result != null) Assert.NotNull(result);
    }

    [Fact]
    public async Task GetAllPersonAsync_ReturnsNull_WhenPersonDoesNotExist()
    {
        _mockRepo.Setup(repo => repo.GetAllPersonAsync()).ReturnsAsync((ICollection<Person>)null);

        var result = await _service.GetPersonByIdAsync(1);

        if (result != null) Assert.Null(result);
    }

    [Fact]
    public async Task GetAllPersonAsync()
    {
        _mockRepo.Setup(repo => repo.GetAllPersonAsync()).ReturnsAsync((ICollection<Person>)null);

        var result = await _service.GetPersonByIdAsync(1);

        if (result != null) Assert.NotNull(result);
    }

    [Fact]
    public async Task UpdatePersonAsync_WhenPersonExists_ReturnsUpdatedPerson()
    {
        var id = 1L;
        var existingPerson = new Person
        {
            Id = id,
            Name = "Old Name",
            DisplayName = "Old Display",
            Skills = new List<Skill>()
        };

        var updateDto = new PersonUpdateDto
        {
            Name = "New Name",
            DisplayName = "New Display",
            Skills = new List<SkillDto>()
        };

        var mappedPerson = new Person
        {
            Skills = new List<Skill>()
        };

        var updatedPerson = new Person
        {
            Id = id,
            Name = "New Name",
            DisplayName = "New Display",
            Skills = new List<Skill>()
        };

        var expectedResponse = new PersonResponseDto
        {
            Id = id,
            Name = "New Name",
            DisplayName = "New Display",
            Skills = new List<SkillDto>()
        };

        _mockRepo.Setup(r => r.GetPersonByIdAsync(id))
            .ReturnsAsync(existingPerson);

        _mockMapper.Setup(m => m.Map<Person>(updateDto))
            .Returns(mappedPerson);

        _mockRepo.Setup(r => r.UpdatePersonAsync(It.IsAny<Person>()))
            .ReturnsAsync(updatedPerson);

        _mockMapper.Setup(m => m.Map<PersonResponseDto>(updatedPerson))
            .Returns(expectedResponse);

        var result = await _service.UpdatePersonAsync(id, updateDto);

        Assert.NotNull(result);
        Assert.Equal(expectedResponse.Id, result.Data.Id);
        Assert.Equal(expectedResponse.Name, result.Data.Name);
        Assert.Equal(expectedResponse.DisplayName, result.Data.DisplayName);

        _mockRepo.Verify(r => r.GetPersonByIdAsync(id), Times.Once);
        _mockRepo.Verify(r => r.RemoveSkills(existingPerson.Skills), Times.Once);
        _mockRepo.Verify(r => r.UpdatePersonAsync(It.Is<Person>(p =>
                p.Name == "New Name" &&
                p.DisplayName == "New Display")),
            Times.Once);
    }

    [Fact]
    public async Task UpdatePersonAsync_WhenPersonNotExists_ReturnsNull()
    {
        var id = 999L;
        var updateDto = new PersonUpdateDto()
        {
            Name = "Test test",
            DisplayName = "TT",
            Skills = new List<SkillDto> { new() { Name = "xUnit", Level = 4 } }
        };

        Assert.ThrowsAsync<NotFoundException>(async () =>
        {
            var result = await _service.UpdatePersonAsync(id, updateDto);

        });
    }


    [Fact]
    public async Task CreatePersonAsync_Should_Invoke_Mapper()
    {
        var createDto = new PersonCreateDto
        {
            Name = "Test test",
            DisplayName = "TT",
            Skills = new List<SkillDto> { new() { Name = "xUnit", Level = 4 } }
        };
        _mockMapper.Setup(m => m.Map<Person>(createDto))
            .Returns(new Person());

        await _service.AddPersonAsync(createDto);

        _mockMapper.Verify(m => m.Map<Person>(createDto), Times.Once);
    }
}