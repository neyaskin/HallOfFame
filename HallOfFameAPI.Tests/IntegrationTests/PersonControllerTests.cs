using System.Net;
using System.Net.Http.Json;
using HallOfFameAPI.DTOs;
using Microsoft.AspNetCore.Mvc.Testing;

namespace HallOfFameAPI.Tests.IntegrationTests;

public class PersonsControllerIntegrationTests : IClassFixture<TestWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly TestWebApplicationFactory<Program> _factory;

    public PersonsControllerIntegrationTests(TestWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task GetAllPersons()
    {
        var response = await _client.GetAsync("api/v1/persons");
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetPersonById_WhenPersonExists()
    {
        var person = new PersonCreateDto
        {
            Name = "Test test",
            DisplayName = "TT",
            Skills = new List<SkillDto> { new() { Name = "xUnit", Level = 4 } }
        };

        var createResponse = await _client.PostAsJsonAsync("/api/v1/persons", person);
        var createdPerson = await createResponse.Content.ReadFromJsonAsync<PersonResponseDto>();
        var response = await _client.GetAsync($"/api/v1/persons/{createdPerson!.Id}");

        Assert.Equal(response.StatusCode, HttpStatusCode.OK);

        var fetchedPerson = await response.Content.ReadFromJsonAsync<PersonResponseDto>();

        Assert.Equal(fetchedPerson.Name, "Test test");
    }

    [Fact]
    public async Task GetPersonById_WhenPersonDoesNotExist()
    {
        var response = await _client.GetAsync("/api/v1/persons/999");

        Assert.Equal(response.StatusCode, HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task AddPerson_WhenDataIsValid()
    {
        var person = new PersonCreateDto
        {
            Name = "Test data",
            DisplayName = "TD",
            Skills = new List<SkillDto>
            {
                new() { Name = "C#", Level = 9 },
                new() { Name = "Golang", Level = 8 }
            }
        };

        var response = await _client.PostAsJsonAsync("/api/v1/persons", person);

        Assert.Equal(response.StatusCode, HttpStatusCode.Created);
        var createdPerson = await response.Content.ReadFromJsonAsync<PersonResponseDto>();

        Assert.NotNull(createdPerson);
        Assert.Equal(createdPerson.Name, "Test data");
    }

    [Fact]
    public async Task AddPersonAsync_WhenDataIsInvalid()
    {
        var invalidPerson = new PersonCreateDto
        {
            Name = "",
            DisplayName = "TD",
            Skills = new List<SkillDto>
            {
                new() { Name = "C#", Level = 10 },
                new() { Name = "Golang", Level = 8 }
            }
        };

        var response = await _client.PostAsJsonAsync("/api/v1/persons", invalidPerson);

        Assert.Equal(response.StatusCode, HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task AddPersonAsync_WhenSkillsNotExist()
    {
        var invalidPerson = new PersonCreateDto
        {
            Name = "",
            DisplayName = "TD",
            Skills = new List<SkillDto>()
        };

        var response = await _client.PostAsJsonAsync("/api/v1/persons", invalidPerson);

        Assert.Equal(response.StatusCode, HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task UpdatePersonAsync_WhenDataIsValid()
    {
        var createPerson = new PersonCreateDto
        {
            Name = "Original Name",
            DisplayName = "Original",
            Skills = new List<SkillDto> { new() { Name = "Original Skill", Level = 5 } }
        };

        var createResponse = await _client.PostAsJsonAsync("/api/v1/persons", createPerson);
        var createdPerson = await createResponse.Content.ReadFromJsonAsync<PersonResponseDto>();

        var updatePerson = new PersonUpdateDto
        {
            Name = "New Name",
            DisplayName = "New",
            Skills = new List<SkillDto>
            {
                new() { Name = "New Skill", Level = 8 },
                new() { Name = "Another Skill", Level = 7 }
            }
        };

        var response = await _client.PutAsJsonAsync($"/api/v1/persons/{createdPerson!.Id}", updatePerson);

        Assert.Equal(response.StatusCode, HttpStatusCode.NoContent);

        var getResponse = await _client.GetAsync($"/api/v1/persons/{createdPerson.Id}");
        var updatedPerson = await getResponse.Content.ReadFromJsonAsync<PersonResponseDto>();
        Assert.Equal(updatedPerson.Name, "New Name");
        Assert.Equal(updatedPerson.Skills[1].Name, "Another Skill");
    }

    [Fact]
    public async Task UpdatePersonAsync_WhenPersonDoesNotExist()
    {
        var updatePerson = new PersonUpdateDto
        {
            Name = "New Name",
            DisplayName = "New",
            Skills = new List<SkillDto>
            {
                new() { Name = "New Skill", Level = 8 },
                new() { Name = "Another Skill", Level = 7 }
            }
        };

        var response = await _client.PutAsJsonAsync("/api/v1/persons/999", updatePerson);

        Assert.Equal(response.StatusCode, HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeletePersonAsync_WhenPersonExists()
    {
        var person = new PersonCreateDto
        {
            Name = "Delete",
            DisplayName = "Delete",
            Skills = new List<SkillDto>
            {
                new() { Name = "Test", Level = 1 }
            }
        };

        var createResponse = await _client.PostAsJsonAsync("/api/v1/persons", person);
        var createdPerson = await createResponse.Content.ReadFromJsonAsync<PersonResponseDto>();

        var responseDeletePerson = await _client.DeleteAsync($"/api/v1/persons/{createdPerson!.Id}");

        Assert.Equal(responseDeletePerson.StatusCode, HttpStatusCode.NoContent);

        var responseGetPerson = await _client.GetAsync($"/api/v1/persons/{createdPerson.Id}");
        Assert.Equal(responseGetPerson.StatusCode, HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task DeletePersonAsync_WhenPersonDoesNotExist()
    {
        var response = await _client.DeleteAsync("/api/v1/persons/999");

        Assert.Equal(response.StatusCode, HttpStatusCode.NoContent);
    }
}