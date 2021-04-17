using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

/*
 Что еще можно сделать, но не сделал:
	Если в методах NotFound(string), BadRequest(string) возвращать что-то то лучше перегрузить эти функции для возврата Json обхекта.
	В методе PutPerson при возврате NotFound можно вернуть объект, который не нашли.
 */

/*
 What else can be done but didn't:
	If in methods NotFound(string), BadRequest(string) return something it's better to override these functions to return a Json object.
	In method PutPerson when returning NotFound it's possible to return an object that was not found.
 */

namespace Backend.Controllers
{
	[ApiController]
	[Route("api/v1/")]
	public class PersonController : ControllerBase
	{
		private readonly PersonContext _context;

		public PersonController(PersonContext context)
		{
			_context = context;
		}

		[HttpGet("persons")]
		[ProducesResponseType(200, Type = typeof(List<Models.Person>))]
		[ProducesResponseType(404)]
		public async Task<ActionResult<List<Models.Person>>> GetPersons()
		{
			var persons = await _context.PersonsWithSkills.AsNoTracking().ToListAsync();

			if (persons.Count == 0)
			{
				return NotFound();
			}

			return Ok(persons);
		}

		[HttpGet("person/{id}")]
		[ProducesResponseType(200, Type = typeof(Models.Person))]
		[ProducesResponseType(404)]
		public async Task<ActionResult<Models.Person>> GetPerson(long id)
		{
			var person = await _context.PersonsWithSkills.FirstOrDefaultAsync(p => p.Id == id);

			if (person == null)
			{
				return NotFound();
			}

			return Ok(person);
		}

		[HttpPost("person")]
		[ProducesResponseType(200, Type = typeof(Models.Person))]
		[ProducesResponseType(400)]
		[ProducesResponseType(404)]
		public async Task<ActionResult<Models.Person>> PostPerson(Models.Person person)
		{
			if (person.Id != 0)
			{
				return BadRequest("Id must be zero or undefined");
			}

			_context.Persons.Add(person);

			await _context.SaveChangesAsync();

			return Ok(person);
		}

		[HttpPut("person/{id}")]
		[ProducesResponseType(200)]
		[ProducesResponseType(400)]
		[ProducesResponseType(404)]
		public async Task<ActionResult> PutPerson(long id, Models.Person person)
		{
			if (person.Id != 0)
			{
				return BadRequest("Id in payload must be zero or undefined");
			}
			else
			{
				person.Id = id;
			}

			_context.Entry(person).State = EntityState.Modified;

			person.Skills?.ForEach(skill =>
			{
				skill.PersonId = id;
				_context.Entry(skill).State = EntityState.Modified;
			});

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException e)
			{
				foreach (var entry in e.Entries)
				{
					if (entry.Entity is Person || entry.Entity is Skill)
					{
						if (entry.GetDatabaseValues() == null)
						{
							return NotFound();
						}
					}
				}

				throw;
			}

			return Ok();
		}

		[HttpDelete("person/{id}")]
		[ProducesResponseType(200)]
		[ProducesResponseType(404)]
		public async Task<ActionResult> DeletePerson(long id)
		{
			var person = await _context.PersonsWithSkills.FirstOrDefaultAsync(s => s.Id == id);

			if (person == null)
			{
				return NotFound();
			}

			_context.Persons.Remove(person);

			await _context.SaveChangesAsync();

			return Ok();
		}
	}
}
