# StronglyTyped Identifiers

## What is this? Why do I want this?

This repository includes a series of different projects that provide generically typed identifiers that can replace some common data types. For example, the ```StronglyTyped.GuidIds``` library includes an ```Id<T>``` struct that can be used for typing Guid based identifiers in your code. In addition to the GuidIds project, there exists IntIds, LongIds, and StringIds projects as well. These should be used if you desire an extra level of type safety for identifier variables, fields, properties, etc. It also improves code readability by allowing you to communicate the content of a variable using both the type name as well as in the variable name.

We can take a class like this:

```csharp
internal class Person {
	public Guid PersonId { get; }
	public Guid TeamId { get; }
	public Guid MostRecentInteractionId { get; }
}
```

And write it like this instead:

```csharp
internal class Person {
	public Id<Person> PersonId { get; }
	public Id<Team> TeamId { get; }
	public Id<Interaction> MostRecentInteractionId { get; }
}
```


## Improves code readability!

```csharp
var teamIds = new List<Guid>();

var teamsByTeamId = new Dictionary<Guid, Team>();

var memberIdsByTeamId = new Dictionary<Guid, IReadOnlyList<Guid>();
```

Or using Id instead of Guid

```csharp
var teamIds = new List<Id<Team>>();

var teamsByTeamId = new Dictionary<Id<Team>, Team>();

var memberIdsByTeamId = new Dictionary<Id<Team>, IReadOnlyList<Id<Member>>();
```


## Catch coding mistakes earlier!

```csharp
interface TeamMemberStore {
	Guid CreateTeamMember(Guid teamId, Guid personId, Guid interactionId)
}

_teamMemberStore.CreateTeamMember(team.TeamId, person.PersonId, userSession.InteractionId);
// Compiles Fine! Works fine at runtime since everything was done right!

_teamMemberStore.CreateTeamMember(person.PersonId, team.TeamId, userSession.InteractionId);
// Compiles Fine!
//  Best case it fails at runtime because of a foreign key was configured on the database and it's fairly obvious what we did wrong.
//  Worst case it doesn't work and we waste a whole lot of time trying to figure out why we can't get joining a team to work only to realize we made this stupid mistake.
```

Or using Id instead of Guid

```csharp
interface TeamMemberStore {
	Id<TeamMember> CreateTeamMember(Id<Team> teamId, Id<Person> personId, Id<Interaction> interactionId)
}

_teamMemberStore.CreateTeamMember(team.TeamId, person.PersonId, userSession.InteractionId);
// Compiles Fine! Works fine at runtime since everything was done right!

_teamMemberStore.CreateTeamMember(person.PersonId, team.TeamId, userSession.InteractionId);
// Does not compile! We don't even have to run the application to catch this type is error. A red, swiggly underline shows us the problem!
```