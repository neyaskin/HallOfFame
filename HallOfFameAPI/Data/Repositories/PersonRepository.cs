using HallOfFameAPI.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace HallOfFameAPI.Data.Repositories;

public class PersonRepository : IPersonRepository
{
    private readonly HallOfFameDbContext _context;

    public PersonRepository(HallOfFameDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Person>> GetAllPersonAsync()
    {
        return await _context.Persons.Include(p => p.Skills).ToListAsync();
    }

    /// <inheritdoc />
    public async Task<Person> GetPersonByIdAsync(long id)
    {
        return await _context.Persons
            .Include(p => p.Skills)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    /// <inheritdoc />
    public async Task<Person> AddPersonAsync(Person person)
    {
        await _context.Persons.AddAsync(person);
        await _context.SaveChangesAsync();
        return person;
    }

    /// <inheritdoc />
    public async Task<Person> UpdatePersonAsync(Person person)
    {
        _context.Persons.Update(person);
        await _context.SaveChangesAsync();
        return person;
    }

    /// <inheritdoc />
    public void RemoveSkills(ICollection<Skill> skills)
    {
        _context.Skills.RemoveRange(skills);
    }

    /// <inheritdoc />
    public async Task DeletePersonAsync(long id)
    {
        var person = await _context.Persons.FindAsync(id);
        if (person != null)
        {
            _context.Persons.Remove(person);
            await _context.SaveChangesAsync();
        }
    }
}