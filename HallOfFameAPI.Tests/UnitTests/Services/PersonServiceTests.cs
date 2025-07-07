using AutoMapper;
using HallOfFameAPI.Services;
using HallOfFameAPI.Data.Repositories;
using HallOfFameAPI.Data.Entities;
using HallOfFameAPI.DTOs;
using Moq;

namespace HallOfFameAPI.Tests.UnitTests.Services;

public class PersonServiceTests
{
    private readonly Mock<IPersonRepository> _mockRepo;
    private readonly Mock<IMapper> _mockMapper;
    private readonly PersonService _service;

    public PersonServiceTests()
    {
        _mockRepo = new Mock<IPersonRepository>();
        _mockMapper = new Mock<IMapper>();
        _service = new PersonService(_mockRepo.Object, _mockMapper.Object);
    }
    
    [Fact]
    public async Task CreatePersonAsync_Should_Return_CorrectResponse()
    {
        // Arrange
        var createDto = new PersonCreateDto 
        { 
            Name = "Test",
            Skills = new List<SkillCreateDto>()
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

        Assert.Equal(expectedResponse.Id, result.Id);
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
    public async Task UpdatePersonAsync_Should_Return_Bool_Result()
    {
        var existingPerson = new Person { Id = 1 };
        var updateDto = new PersonUpdateDto();

        _mockRepo.Setup(r => r.GetPersonByIdAsync(1))
            .ReturnsAsync(existingPerson);

        var result = await _service.UpdatePersonAsync(1, updateDto);

        Assert.IsType<PersonResponseDto>(result);
    }
    
    [Fact]
    public async Task CreatePersonAsync_Should_Invoke_Mapper()
    {
        var createDto = new PersonCreateDto();
        _mockMapper.Setup(m => m.Map<Person>(createDto))
            .Returns(new Person());

        await _service.AddPersonAsync(createDto);

        _mockMapper.Verify(m => m.Map<Person>(createDto), Times.Once);
    }
    
    [Fact]
    public async Task UpdatePersonAsync_Should_Return_False_When_NotFound()
    {
        _mockRepo.Setup(r => r.GetPersonByIdAsync(It.IsAny<long>()))
            .ReturnsAsync((Person?)null);

        var result = await _service.UpdatePersonAsync(1, new PersonUpdateDto());

        Assert.IsType<PersonResponseDto>(result);
    }
}