using System;
using System.Collections.Generic;
using System.Linq;

namespace StronglyTyped.GuidIds
{
	public static class IdOfTExtensions
	{
		/// <summary>Converts an IEnumerable of Ids to Guids.</summary>
		/// <typeparam name="TModel">Type for identifier</typeparam>
		/// <param name="setOfIdOfT">Set of identifiers to convert to guids</param>
		/// <returns>Set of guid values from identifiers</returns>
		public static IReadOnlyList<Guid> AsGuidList<TModel>(this IEnumerable<Id<TModel>> setOfIdOfT)
		{
			return setOfIdOfT.Select(identifier => identifier.Value).ToList();
		}

		/// <summary>Converts an IEnumerable of Guids to Ids.</summary>
		/// <typeparam name="TModel">Type for identifier</typeparam>
		/// <param name="setOfGuids">Set of guids to convert to identifiers</param>
		/// <returns>Set of identifiers from the given guids</returns>
		public static IReadOnlyList<Id<TModel>> AsIdList<TModel>(this IEnumerable<Guid> setOfGuids)
		{
			return setOfGuids.Select(guid => new Id<TModel>(guid)).ToList();
		}
	}
}
