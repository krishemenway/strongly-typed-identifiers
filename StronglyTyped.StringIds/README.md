# StronglyTyped.stringIds

## What is this? Why do I want this?

This library includes an ```Id<T>``` struct that can be used for typing string based identifiers in your code. This should be used if you desire an extra level of type safety for identifier variables, fields, properties, etc. In addition, it provides improved code readability by allowing you to communicate the content of a variable using both the type name as well as in the variable name.

We can take a class like this:

```csharp
internal class Person {
	public string PersonId { get; }
	public string TeamId { get; }
	public string MostRecentInteractionId { get; }
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
var teamIds = new List<string>();

var teamsByTeamId = new Dictionary<string, Team>();

var memberIdsByTeamId = new Dictionary<string, IReadOnlyList<string>();
```

Or using Id instead of string

```csharp
var teamIds = new List<Id<Team>>();

var teamsByTeamId = new Dictionary<Id<Team>, Team>();

var memberIdsByTeamId = new Dictionary<Id<Team>, IReadOnlyList<Id<Member>>();
```


## Catch coding mistakes earlier!

```csharp
interface TeamMemberStore {
	string CreateTeamMember(string teamId, string personId, string interactionId)
}

_teamMemberStore.CreateTeamMember(team.TeamId, person.PersonId, userSession.InteractionId);
// Compiles Fine! Works fine at runtime since everything was done right!

_teamMemberStore.CreateTeamMember(person.PersonId, team.TeamId, userSession.InteractionId);
// Compiles Fine!
//  Best case it fails at runtime because of a foreign key was configured on the database and it's fairly obvious what we did wrong.
//  Worst case it doesn't work and we waste a whole lot of time trying to figure out why we can't get joining a team to work only to realize we made this stupid mistake.
```

Or using Id instead of string

```csharp
interface TeamMemberStore {
	Id<TeamMember> CreateTeamMember(Id<Team> teamId, Id<Person> personId, Id<Interaction> interactionId)
}

_teamMemberStore.CreateTeamMember(team.TeamId, person.PersonId, userSession.InteractionId);
// Compiles Fine! Works fine at runtime since everything was done right!

_teamMemberStore.CreateTeamMember(person.PersonId, team.TeamId, userSession.InteractionId);
// Does not compile! We don't even have to run the application to catch this type is error. A red, swiggly underline shows us the problem!
```