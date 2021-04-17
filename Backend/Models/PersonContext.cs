using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Backend.Models
{
	public class PersonContext : DbContext
	{
		public PersonContext(DbContextOptions<PersonContext> options) : base(options)
		{
		}

		public DbSet<Person> Persons { get; set; }
		public DbSet<Skill> Skills { get; set; }

		public IIncludableQueryable<Person, List<Skill>> PersonsWithSkills => Persons.Include(s => s.Skills);

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Skill>()
				.HasKey(s => new { s.Name, s.PersonId });

			modelBuilder.Entity<Skill>()
				.HasOne(p => p.Person)
				.WithMany(s => s.Skills)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
