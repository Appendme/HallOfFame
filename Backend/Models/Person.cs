using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
	public class Person
	{
		public long Id { get; set; }

		[Required(ErrorMessage = "Enter the name of person")]
		public string Name { get; set; }

		[Required(ErrorMessage = "Enter the display name of person")]
		public string DisplayName { get; set; }

		public List<Skill> Skills { get; set; }
	}
}
