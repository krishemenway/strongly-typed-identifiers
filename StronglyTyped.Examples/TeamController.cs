using Microsoft.AspNetCore.Mvc;
using StronglyTyped.IntIds;
using System.Collections.Generic;

namespace ExampleService
{
	[ApiController]
	public class TeamController : ControllerBase
	{
		public TeamController(ITeamStore teamStore = null)
		{
			_teamStore = teamStore ?? new TeamStore();
		}

		[Route("Team/FindById")]
		[ProducesResponseType(200, Type = typeof(TeamResponse))]
		public ActionResult<TeamResponse> FindById([FromQuery] Id<Team> teamId)
		{
			if (!_teamStore.TryFind(teamId, out var team))
			{
				return NotFound();
			}

			return new TeamResponse
			{
				TeamId = teamId,
				Name = team.Name,
				TeamMembers = team.TeamMembers,
			};
		}

		private readonly ITeamStore _teamStore;
	}

	public class TeamResponse
	{
		public Id<Team> TeamId { get; set; }
		public string Name { get; set; }
		public IReadOnlyList<Person> TeamMembers { get; set; }
	}
}
