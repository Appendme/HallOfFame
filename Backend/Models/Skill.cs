using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Backend.Models
{
	public class Skill
	{
		[Required(ErrorMessage = "Enter the name of the skill")]
		public string Name { get; set; }

		[Range(1, 10, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
		public byte Level { get; set; }

		// Make PersonId not null.
		[JsonIgnore]
		public long PersonId { get; set; }

		// Add navigation propeties.
		[JsonIgnore]
		public Person Person { get; set; }
	}
}
