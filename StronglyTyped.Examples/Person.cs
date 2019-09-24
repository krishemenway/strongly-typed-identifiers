using StronglyTyped.GuidIds;

namespace ExampleService
{
	public class Person
	{
		public Id<Person> PersonId { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
	}
}
