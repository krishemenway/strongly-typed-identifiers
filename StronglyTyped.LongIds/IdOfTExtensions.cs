using System.Collections.Generic;
using System.Linq;

namespace StronglyTyped.LongIds
{
	public static class IdOfTExtensions
	{
		/// <summary>Converts an IEnumerable of Ids to Longs.</summary>
		/// <typeparam name="TModel">Type for identifier</typeparam>
		/// <param name="setOfIdOfT">Set of identifiers to convert to longs</param>
		/// <returns>Set of long values from identifiers</returns>
		public static IReadOnlyList<long> AsLongList<TModel>(this IEnumerable<Id<TModel>> setOfIdOfT)
		{
			return setOfIdOfT.Select(identifier => identifier.Value).ToList();
		}

		/// <summary>Converts an IEnumerable of Longs to Ids.</summary>
		/// <typeparam name="TModel">Type for identifier</typeparam>
		/// <param name="setOfLongs">Set of longs to convert to identifiers</param>
		/// <returns>Set of identifiers from the given longs</returns>
		public static IReadOnlyList<Id<TModel>> AsIdList<TModel>(this IEnumerable<long> setOfLongs)
		{
			return setOfLongs.Select(longValue => new Id<TModel>(longValue)).ToList();
		}
	}
}
