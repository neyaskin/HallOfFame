using HallOfFameAPI.DTOs;
using HallOfFameAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace HallOfFameAPI.Controllers;

/// <summary>
///     Контроллер для управления пользователями.
/// </summary>
[Route("api/v1/persons")]
[ApiController]
public class PersonsController : ControllerBase
{
    private readonly ILogger<PersonsController> _logger;

    /// <summary>
    ///     <seealso cref="IPersonService" />
    /// </summary>
    private readonly IPersonService _personService;


    public PersonsController(IPersonService personService, ILogger<PersonsController> logger)
    {
        _personService = personService;
        _logger = logger;
    }

    /// <summary>
    ///     Метод для получения всех пользователей.
    /// </summary>
    /// <returns>Список пользователей.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PersonResponseDto>>> GetAllPersons()
    {
        var response = await _personService.GetAllPersonsAsync();
        _logger.LogInformation("Get all persons. Success");
        return Ok(response);
    }

    /// <summary>
    ///     Метод для получения пользователя.
    /// </summary>
    /// <param name="id">Идентификатор пользователя.</param>
    /// <returns>Пользователь. <seealso cref="PersonResponseDto" /></returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<PersonResponseDto>> GetPersonById(long id)
    {
        var response = await _personService.GetPersonByIdAsync(id);

        if (response == null) return NotFound();
        _logger.LogInformation("Get person by id. Success");
        return Ok(response);
    }

    /// <summary>
    ///     Метод для добавления пользователя.
    /// </summary>
    /// <param name="personDto">Данные пользователя. <seealso cref="PersonCreateDto" /></param>
    /// <returns>Пользователь. <seealso cref="PersonResponseDto" /></returns>
    [HttpPost]
    public async Task<ActionResult<PersonResponseDto>> AddPersonAsync([FromBody] PersonCreateDto? personDto)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogError("Add person. Invalid model state");
            return BadRequest(ModelState);
        }

        var result = await _personService.AddPersonAsync(personDto);

        if (!result.IsSuccess)
        {
            _logger.LogError($"Add person. {result.ErrorMessage}");
            return BadRequest(result.ErrorMessage);
        }

        _logger.LogInformation("Add person. Success");
        return CreatedAtAction(nameof(GetPersonById), new { id = result.Data.Id }, result.Data);
    }

    /// <summary>
    ///     Метод обновления данных пользователя.
    /// </summary>
    /// <param name="id">Идентификатор пользователя.</param>
    /// <param name="personDto">Данные пользователя. <seealso cref="PersonUpdateDto" /></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePersonAsync(long id, [FromBody] PersonUpdateDto personDto)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogError("Update Person. Invalid model state");
            return BadRequest(ModelState);
        }

        var result = await _personService.UpdatePersonAsync(id, personDto);

        if (!result.IsSuccess)
        {
            _logger.LogError($"Update person. {result.ErrorMessage}");
            return result.ErrorMessage == "Person not found" ? NotFound() : BadRequest(result.ErrorMessage);
        }

        _logger.LogInformation("Update person. Success");
        return NoContent();
    }

    /// <summary>
    ///     Метод удаления пользователя.
    /// </summary>
    /// <param name="id">Идентификатор пользователя.</param>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePersonAsync(long id)
    {
        var deletedPerson = await _personService.GetPersonByIdAsync(id);

        if (deletedPerson == null)
        {
            _logger.LogError("Delete Person. Person Dto is null");
            return NotFound();
        }

        await _personService.DeletePersonAsync(id);
        _logger.LogInformation("Delete person. Success");
        return NoContent();
    }
}