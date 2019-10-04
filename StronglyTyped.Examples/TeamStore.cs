using Dapper;
using StronglyTyped.IntIds;
using StronglyTyped.IntIds.Dapper;
using System.Linq;

namespace ExampleService
{
	public interface ITeamStore
	{
		bool TryFind(Id<Team> personId, out Team person);
	}

	public class TeamStore : ITeamStore
	{
		static TeamStore()
		{
			TypeHandlerForIdOf<Team>.Register();
		}

		public TeamStore(ITeamMemberStore teamMemberStore = null)
		{
			_teamMemberStore = teamMemberStore ?? new TeamMemberStore();
		}

		public bool TryFind(Id<Team> teamId, out Team team)
		{
			using (var connection = Database.CreateConnection())
			{
				const string sql = @"
					SELECT
						team_id as teamid,
						name
					FROM
						public.team
					WHERE
						team_id = @teamId";

				team = connection
					.Query<TeamRecord>(sql, new { teamId })
					.Select(CreateTeam)
					.SingleOrDefault();

				return team != null;
			}
		}

		private Team CreateTeam(TeamRecord record)
		{
			return new Team
			{
				TeamId = record.TeamId,
				Name = record.Name,
				TeamMembers = _teamMemberStore.FindTeamMembers(record.TeamId),
			};
		}

		public class TeamRecord
		{
			public Id<Team> TeamId { get; set; }
			public string Name { get; set; }
		}

		private readonly ITeamMemberStore _teamMemberStore;
	}
}
