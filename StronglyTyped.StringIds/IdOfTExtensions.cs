using System.Collections.Generic;
using System.Linq;

namespace StronglyTyped.StringIds
{
	public static class IdOfTExtensions
	{
		/// <summary>Converts an IEnumerable of Ids to strings.</summary>
		/// <typeparam name="TModel">Type for identifier</typeparam>
		/// <param name="setOfIdOfT">Set of identifiers to convert to strings</param>
		/// <returns>Set of string values from identifiers</returns>
		public static IReadOnlyList<string> AsStringList<TModel>(this IEnumerable<Id<TModel>> setOfIdOfT)
		{
			return setOfIdOfT.Select(identifier => identifier.Value).ToList();
		}

		/// <summary>Converts an IEnumerable of strings to Ids.</summary>
		/// <typeparam name="TModel">Type for identifier</typeparam>
		/// <param name="setOfStrings">Set of strings to convert to identifiers</param>
		/// <returns>Set of identifiers from the given strings</returns>
		public static IReadOnlyList<Id<TModel>> AsIdList<TModel>(this IEnumerable<string> setOfStrings)
		{
			return setOfStrings.Select(stringValue => new Id<TModel>(stringValue)).ToList();
		}
	}
}
