using Microsoft.AspNetCore.Mvc;
using StronglyTyped.GuidIds;

namespace ExampleService
{
	[ApiController]
	public class PersonController : ControllerBase
	{
		public PersonController(IPersonStore personStore = null)
		{
			_personStore = personStore ?? new PersonStore();
		}

		[Route("Person/FindById")]
		[ProducesResponseType(200, Type = typeof(PersonResponse))]
		public ActionResult<PersonResponse> FindById([FromQuery] Id<Person> personId)
		{
			if (!_personStore.TryFind(personId, out var person))
			{
				return NotFound();
			}

			return new PersonResponse
			{
				PersonId = person.PersonId,
				FirstName = person.FirstName,
				LastName = person.LastName,
			};
		}

		private readonly IPersonStore _personStore;
	}

	public class PersonResponse
	{
		public Id<Person> PersonId { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
	}
}
