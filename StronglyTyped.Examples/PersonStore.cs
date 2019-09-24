using Dapper;
using StronglyTyped.GuidIds;
using StronglyTyped.GuidIds.Dapper;
using System.Linq;

namespace ExampleService
{
	public interface IPersonStore
	{
		bool TryFind(Id<Person> personId, out Person person);
	}

	public class PersonStore : IPersonStore
	{
		static PersonStore()
		{
			DapperTypeHandler<Person>.RegisterId();
		}

		public bool TryFind(Id<Person> personId, out Person person)
		{
			using (var connection = Database.CreateConnection())
			{
				const string sql = @"
					SELECT
						person_id as personid,
						first_name as firstname,
						last_name as lastname
					FROM
						public.person
					WHERE
						person_id = @personId";

				person = connection
					.Query<PersonRecord>(sql, new { personId })
					.Select(CreatePerson)
					.SingleOrDefault();

				return person != null;
			}
		}

		private Person CreatePerson(PersonRecord record)
		{
			return new Person
			{
				PersonId = record.PersonId,
				FirstName = record.FirstName,
				LastName = record.LastName,
			};
		}

		public class PersonRecord
		{
			public Id<Person> PersonId { get; set; }
			public string FirstName { get; set; }
			public string LastName { get; set; }
		}
	}
}
