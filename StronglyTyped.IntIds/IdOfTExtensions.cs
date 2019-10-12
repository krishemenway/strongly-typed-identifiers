using System.Collections.Generic;
using System.Linq;

namespace StronglyTyped.IntIds
{
	public static class IdOfTExtensions
	{
		/// <summary>Converts an IEnumerable of Ids to Ints.</summary>
		/// <typeparam name="TModel">Type for identifier</typeparam>
		/// <param name="setOfIdOfT">Set of identifiers to convert to ints</param>
		/// <returns>Set of int values from identifiers</returns>
		public static IReadOnlyList<int> AsIntList<TModel>(this IEnumerable<Id<TModel>> setOfIdOfT)
		{
			return setOfIdOfT.Select(identifier => identifier.Value).ToList();
		}

		/// <summary>Converts an IEnumerable of Ints to Ids.</summary>
		/// <typeparam name="TModel">Type for identifier</typeparam>
		/// <param name="setOfInts">Set of ints to convert to identifiers</param>
		/// <returns>Set of identifiers from the given ints</returns>
		public static IReadOnlyList<Id<TModel>> AsIdList<TModel>(this IEnumerable<int> setOfInts)
		{
			return setOfInts.Select(integer => new Id<TModel>(integer)).ToList();
		}
	}
}
