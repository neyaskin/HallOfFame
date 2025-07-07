using HallOfFameAPI.DTOs;
using HallOfFameAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace HallOfFameAPI.Controllers;

/// <summary>
/// Контроллер для управления пользователями.
/// </summary>
[Route("api/persons")]
[ApiController]
public class PersonsController : ControllerBase
{
    /// <summary>
    /// <seealso cref="IPersonService"/>
    /// </summary>
    private readonly IPersonService _personService;


    public PersonsController(IPersonService personService)
    {
        _personService = personService;
    }

    /// <summary>
    /// Метод для получения всех пользователей.
    /// </summary>
    /// <returns>Список пользователей.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PersonResponseDto>>> GetAllPersons()
    {
        var response = await _personService.GetAllPersonsAsync();
        return Ok(response);
    }

    /// <summary>
    /// Метод для получения пользователя.
    /// </summary>
    /// <param name="id">Идентификатор пользователя.</param>
    /// <returns>Пользователь. <seealso cref="PersonResponseDto"/></returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<PersonResponseDto>> GetPersonById(long id)
    {
        var response = await _personService.GetPersonByIdAsync(id);

        if (response == null)
        {
            return NotFound();
        }

        return Ok(response);
    }

    /// <summary>
    /// Метод для добавления пользователя.
    /// </summary>
    /// <param name="personDto">Данные пользователя. <seealso cref="PersonCreateDto"/></param>
    /// <returns>Пользователь. <seealso cref="PersonResponseDto"/></returns>
    [HttpPost]
    public async Task<ActionResult<PersonResponseDto>> AddPersonAsync([FromBody] PersonCreateDto? personDto)
    {
        if (personDto == null)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        var person = await _personService.AddPersonAsync(personDto);
        return CreatedAtAction(nameof(GetPersonById), new { id = person.Id }, person);
    }

    /// <summary>
    /// Метод обновления данных пользователя.
    /// </summary>
    /// <param name="id">Идентификатор пользователя.</param>
    /// <param name="personDto">Данные пользователя. <seealso cref="PersonUpdateDto"/></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePersonAsync(long id, [FromBody] PersonUpdateDto personDto)
    {
        if (personDto == null)
        {
            return BadRequest("Person is null");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest("Invalid person");
        }

        var updatedPerson = await _personService.UpdatePersonAsync(id, personDto);

        if (updatedPerson == null)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Метод удаления пользователя.
    /// </summary>
    /// <param name="id">Идентификатор пользователя.</param>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePersonAsync(long id)
    {
        var deletedPerson = await _personService.GetPersonByIdAsync(id);

        if (deletedPerson == null)
        {
            return NotFound();
        }

        await _personService.DeletePersonAsync(id);

        return NoContent();
    }
}