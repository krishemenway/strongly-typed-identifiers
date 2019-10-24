# StronglyTyped.LongIds.Dapper

## What is this? Why do I want this?

This is a companion library for integrating ```Id<T>``` from ```StronglyTyped.LongIds``` as a usable type in a Dapper query.

The simplest and worst performing method of accomplishing this is to call ```DapperIdRegistrar.RegisterAll(Assembly.GetExecutingAssembly())``` one time before any Dapper queries have an opprotunity to run. This method requires using ```System.Reflection``` to scan all the types in the provided assemblies to find instances of ```Id<T>``` and register them with Dapper.

If you want to avoid this method and get a little bit of one-time performance back, you can register each ```Id<T>``` individually by executing this static method like so: ```DapperIdRegistrar.RegisterTypeHandlerForId<Person>()```.

Check out this example:

```csharp
using Dapper;
using StronglyTyped.LongIds;
using StronglyTyped.LongIds.Dapper;

public class PersonStore
{
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

internal static class Database
{
	static Database()
	{
		DapperIdRegistrar.RegisterAll(Assembly.GetExecutingAssembly());
	}

	internal static IDbConnection CreateConnection()
	{
		var connection = new NpgsqlConnection(ConnectionString);
		connection.Open();
		return connection;
	}

	private string ConnectionString { get; set; }
}
```