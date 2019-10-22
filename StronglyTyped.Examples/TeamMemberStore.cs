using Dapper;
using System.Collections.Generic;
using System.Linq;
using Int = StronglyTyped.IntIds;
using Guid = StronglyTyped.GuidIds;
using Long = StronglyTyped.LongIds;
using GuidDapper = StronglyTyped.GuidIds.Dapper;
using IntDapper = StronglyTyped.IntIds.Dapper;

namespace ExampleService
{
	public interface ITeamMemberStore
	{
		IReadOnlyList<Person> FindTeamMembers(Int.Id<Team> teamId);
	}

	public class TeamMemberStore : ITeamMemberStore
	{
		static TeamMemberStore()
		{
			GuidDapper.TypeHandlerForIdOf<Person>.Register();
			IntDapper.TypeHandlerForIdOf<Team>.Register();
		}

		public IReadOnlyList<Person> FindTeamMembers(Int.Id<Team> teamId)
		{
			using (var connection = Database.CreateConnection())
			{
				const string sql = @"
					SELECT
						tm.team_member_id as teammemberid,
						tm.team_id as teamid,
						p.person_id as personid,
						p.first_name as firstname,
						p.last_name as lastname
					FROM public.team_member tm
					INNER JOIN public.person p
						ON tm.person_id = p.person_id
					WHERE
						tm.team_id = @TeamId";

				return connection
					.Query<TeamMemberRecord>(sql, new { teamId })
					.Select(CreatePerson)
					.ToList();
			}
		}

		private Person CreatePerson(TeamMemberRecord record)
		{
			return new Person
			{
				PersonId = record.PersonId,
				FirstName = record.FirstName,
				LastName = record.LastName,
			};
		}

		public class TeamMemberRecord
		{
			public Long.Id<TeamMember> TeamMemberId { get; set; }

			public Int.Id<Team> TeamId { get; set; }
			public Guid.Id<Person> PersonId { get; set; }

			public string FirstName { get; set; }
			public string LastName { get; set; }
		}

		public struct TeamMember
		{
		}
	}
}
