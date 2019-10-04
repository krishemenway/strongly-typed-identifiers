using System;
using System.Data;
using static Dapper.SqlMapper;

namespace StronglyTyped.IntIds.Dapper
{
	/// <summary>Type handler for registering identifiers in Dapper queries</summary>
	/// <typeparam name="TModel">Type the identifier is for (e.g. Person, Team)</typeparam>
	public class TypeHandlerForIdOf<TModel> : TypeHandler<Id<TModel>>
	{
		public override Id<TModel> Parse(object value)
		{
			if (value is int valueAsInt)
			{
				return new Id<TModel>(valueAsInt);
			}

			throw new Exception($"Tried to convert type from ({value.GetType()}) to int");
		}

		public override void SetValue(IDbDataParameter parameter, Id<TModel> value)
		{
			parameter.DbType = DbType.Int32;
			parameter.Value = value.Value;
		}

		/// <summary>Register identifier with Dapper</summary>
		public static void Register()
		{
			AddTypeHandler(new TypeHandlerForIdOf<TModel>());
		}
	}
}
