# StronglyTyped.LongIds.Dapper

## What is this? Why do I want this?

This is a companion library for to integrate StronglyTyped.LongIds as a usable type in a Dapper query in an easy way. All you have to do is call ```TypeHandlerForIdOf<TypeForId>.Register()``` and Dapper will be able to serialize & deserialize ```Id<TypeForId>```.

Check out this example:

```csharp
using Dapper;
using StronglyTyped.LongIds;
using StronglyTyped.LongIds.Dapper;

public class PersonStore
{
	static PersonStore()
	{
		TypeHandlerForIdOf<Person>.Register();
	}

	public bool TryFind(Id<Person> personId, out Person person)
	{
		using (var connection = Database.CreateConnection())
		{
			const string sql = @"
				SELECT person_id as personid
				FROM public.person
				WHERE person_id = @personId";

			person = connection
				.Query<PersonRecord>(sql, new { personId })
				.Select(CreatePerson)
				.SingleOrDefault();
		}
	}

	private Person CreatePerson(PersonRecord record)
	{
		return new Person
		{
			PersonId = record.PersonId,
		};
	}

	public class PersonRecord
	{
		public Id<Person> PersonId { get; set; }
	}
}
```