using System.Net;
using AutoMapper;
using HallOfFameAPI.Data.Entities;
using HallOfFameAPI.Data.Repositories;
using HallOfFameAPI.DTOs;
using HallOfFameAPI.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Moq;

namespace HallOfFameAPI.Tests.IntegrationTests;

public class PersonsControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public PersonsControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task CreatePersonAsync_Should_Return_MappedResponseDto()
    {
        // Arrange
        var mockRepo = new Mock<IPersonRepository>();
        var mockMapper = new Mock<IMapper>();
    
        var service = new PersonService(mockRepo.Object, mockMapper.Object);
        var createDto = new PersonCreateDto { Name = "Test" };
        var expectedPerson = new Person { Id = 1, Name = "Test" };
        var expectedResponse = new PersonResponseDto { Id = 1, Name = "Test" };

        mockMapper.Setup(m => m.Map<Person>(createDto))
            .Returns(expectedPerson);
              
        mockMapper.Setup(m => m.Map<PersonResponseDto>(expectedPerson))
            .Returns(expectedResponse);

        mockRepo.Setup(r => r.AddPersonAsync(expectedPerson))
            .ReturnsAsync(expectedPerson);

        // Act
        var result = await service.AddPersonAsync(createDto);

        // Assert
        Assert.Equal(expectedResponse.Id, result.Id);
        Assert.Equal(expectedResponse.Name, result.Name);
        mockRepo.Verify(r => r.AddPersonAsync(It.IsAny<Person>()), Times.Once);
    }

}