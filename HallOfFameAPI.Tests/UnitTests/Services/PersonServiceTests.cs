using HallOfFameAPI.Services;
using HallOfFameAPI.Data.Repositories;
using HallOfFameAPI.Data.Entities;
using Moq;

namespace HallOfFameAPI.Tests.UnitTests.Services;

public class PersonServiceTests
{
    private readonly Mock<IPersonRepository> _mockRepo;
    private readonly PersonService _service;

    public PersonServiceTests()
    {
        _mockRepo = new Mock<IPersonRepository>();
        _service = new PersonService(_mockRepo.Object);
    }

    [Fact]
    public async Task GetPersonByIdAsync_ReturnsPerson_WhenExists()
    {
        // Arrange
        var expectedPerson = new Person { Id = 1, Name = "Test" };
        _mockRepo.Setup(r => r.GetPersonByIdAsync(1)).ReturnsAsync(expectedPerson);

        // Act
        var result = await _service.GetPersonByIdAsync(1);

        // Assert
        Assert.Equal(expectedPerson, result);
        _mockRepo.Verify(r => r.GetPersonByIdAsync(1), Times.Once);
    }

    [Fact]
    public async Task GetPersonByIdAsync_ReturnsNull_WhenNotExists()
    {
        // Arrange
        _mockRepo.Setup(r => r.GetPersonByIdAsync(1)).ReturnsAsync((Person)null);

        // Act
        var result = await _service.GetPersonByIdAsync(1);

        // Assert
        Assert.Null(result);
    }
}