using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using HallOfFameAPI.DTOs;
using HallOfFameAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace HallOfFameAPI.Tests.IntegrationTests;

public class PersonsControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly Mock<IPersonService> _personServiceMock;
    private readonly HttpClient _client;

    public PersonsControllerIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _personServiceMock = new Mock<IPersonService>();

        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddScoped<IPersonService>(_ => _personServiceMock.Object);
            });
        });

        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task GetAllPersons_ReturnsOkWithPersons()
    {
        var expectedPersons = new List<PersonResponseDto>
        {
            new() { Id = 1, Name = "Test1", DisplayName = "Test1", Skills = new List<SkillResponseDto>() },
            new() { Id = 2, Name = "Test2", DisplayName = "Test2", Skills = new List<SkillResponseDto>() }
        };

        _personServiceMock.Setup(x => x.GetAllPersonsAsync())
            .ReturnsAsync(expectedPersons);

        var response = await _client.GetAsync("/api/persons");

        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        var persons = JsonSerializer.Deserialize<List<PersonResponseDto>>(content);

        Assert.Equal(expectedPersons.Count, persons?.Count);
    }

    [Fact]
    public async Task GetPersonById_ExistingId_ReturnsPerson()
    {
        var expectedPerson = new PersonResponseDto
        {
            Id = 1,
            Name = "Test",
            DisplayName = "Test",
            Skills = new List<SkillResponseDto>()
        };

        _personServiceMock
            .Setup(x => x.GetPersonByIdAsync(1))
            .ReturnsAsync(expectedPerson);

        var response = await _client.GetAsync("/api/persons/1");

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var person = JsonSerializer.Deserialize<PersonResponseDto>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        Assert.Equal(expectedPerson.Id, person.Id);
        Assert.Equal(expectedPerson.Name, person.Name);
        Assert.Equal(expectedPerson.DisplayName, person.DisplayName);
        Assert.Equal(expectedPerson.Skills.Count, person.Skills.Count);
    }

    [Fact]
    public async Task GetPersonById_NonExistingId_ReturnsNotFound()
    {
        _personServiceMock.Setup(x => x.GetPersonByIdAsync(999))!
            .ReturnsAsync((PersonResponseDto?)null);
        var response = await _client.GetAsync("/api/persons/999");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task AddPerson_ValidPerson_ReturnsCreated()
    {
        var testPerson = new PersonCreateDto 
        { 
            Name = "Test",
            DisplayName = "Test",
            Skills = new List<SkillCreateDto>()
        };

        var expectedPerson = new PersonResponseDto
        {
            Id = 1,
            Name = "Test",
            DisplayName = "Test",
            Skills = new List<SkillResponseDto>()
        };

        _personServiceMock
            .Setup(x => x.AddPersonAsync(It.Is<PersonCreateDto>(dto => 
                dto.Name == testPerson.Name &&
                dto.DisplayName == testPerson.DisplayName)))
            .ReturnsAsync(expectedPerson);

        var response = await _client.PostAsJsonAsync("/api/persons", testPerson);
    
        var responseContent = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.Equal($"/api/persons/{expectedPerson.Id}", response.Headers.Location?.PathAndQuery);

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    
        var actualPerson = JsonSerializer.Deserialize<PersonResponseDto>(responseContent, options);

        Assert.Equal(expectedPerson.Name, actualPerson.Name);
        Assert.Equal(expectedPerson.DisplayName, actualPerson.DisplayName);
        Assert.Empty(actualPerson.Skills);
    }
    
    [Fact]
    public async Task AddPerson_InvalidPerson_ReturnsValidationErrors()
    {
        var invalidPerson = new PersonCreateDto 
        { 
            Name = "",
            DisplayName = null,
            Skills = null
        };
        var response = await _client.PostAsJsonAsync("/api/persons", invalidPerson);
    
        Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task UpdatePerson_ExistingPerson_ReturnsNoContent()
    {
        var existingId = 1;
        var updateDto = new PersonUpdateDto
        {
            Name = "Updated Name",
            DisplayName = "Updated Display",
            Skills = new List<SkillResponseDto>()
        };

        _personServiceMock
            .Setup(x => x.UpdatePersonAsync(
                existingId, 
                It.Is<PersonUpdateDto>(dto => 
                    dto.Name == updateDto.Name &&
                    dto.DisplayName == updateDto.DisplayName)))
            .ReturnsAsync(new PersonResponseDto())
            .Verifiable();

        var response = await _client.PutAsJsonAsync($"/api/persons/{existingId}", updateDto);

        Assert.True(response.StatusCode == HttpStatusCode.NoContent);
        _personServiceMock.Verify();
    }
    
    [Fact]
    public async Task UpdatePerson_NonExistingPerson_ReturnsNotFound()
    {
        var updateDto = new PersonUpdateDto 
        { 
            Name = "Updated", 
            DisplayName = "Updated Person", 
            Skills = new List<SkillResponseDto>() 
        };
        
        _personServiceMock.Setup(x => x.UpdatePersonAsync(999, updateDto))
            .ReturnsAsync((PersonResponseDto?)null);

        var content = new StringContent(
            JsonSerializer.Serialize(updateDto),
            Encoding.UTF8,
            "application/json");
        var response = await _client.PutAsync("/api/persons/999", content);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
    
    [Fact]
    public async Task DeletePerson_ExistingPerson_ReturnsNoContent()
    {
        // Arrange
        _personServiceMock.Setup(x => x.GetPersonByIdAsync(1))
            .ReturnsAsync(new PersonResponseDto());
        
        _personServiceMock.Setup(x => x.DeletePersonAsync(1))
            .Returns(Task.CompletedTask);

        // Act
        var response = await _client.DeleteAsync("/api/persons/1");

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }
    
    [Fact]
    public async Task DeletePerson_NonExistingPerson_ReturnsNotFound()
    {
        // Arrange
        _personServiceMock.Setup(x => x.GetPersonByIdAsync(999))
            .ReturnsAsync((PersonResponseDto?)null);

        // Act
        var response = await _client.DeleteAsync("/api/persons/999");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}