using Dapper;
using Microsoft.Extensions.Configuration;
using StronglyTyped.GuidIds;
using StronglyTyped.GuidIds.Dapper;
using System.ComponentModel.DataAnnotations.Schema;
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
			if (Program.Settings.GetValue<bool>("UseEF"))
			{
				Id<Person>.NewId();
				using (var context = new EntityFrameworkContext())
				{
					person = context.Person.Select(CreatePerson).SingleOrDefault(x => x.PersonId == personId);
				}
			}
			else
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
				}
			}

				return person != null;
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
			[Column("person_id", TypeName = "uuid")]
			public Id<Person> PersonId { get; set; }

			[Column("first_name")]
			public string FirstName { get; set; }

			[Column("last_name")]
			public string LastName { get; set; }
		}
	}
}
