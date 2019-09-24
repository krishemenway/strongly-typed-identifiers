using System.Collections.Generic;
using StronglyTyped.IntIds;

namespace ExampleService
{
	public class Team
	{
		public Id<Team> TeamId { get; set; }
		public string Name { get; set; }
		public IReadOnlyList<Person> TeamMembers { get; set; }
	}
}
