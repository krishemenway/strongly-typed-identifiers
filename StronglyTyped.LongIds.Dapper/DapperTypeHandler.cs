using System;
using System.Data;
using static Dapper.SqlMapper;

namespace StronglyTyped.LongIds.Dapper
{
	/// <summary>Type handler for registering identifiers in Dapper queries</summary>
	/// <typeparam name="TModel">Type the identifier is for (e.g. Person, Team)</typeparam>
	public class DapperTypeHandler<TModel> : TypeHandler<Id<TModel>>
	{
		public override Id<TModel> Parse(object value)
		{
			if (value is long valueAsLong)
			{
				return new Id<TModel>(valueAsLong);
			}

			throw new Exception($"Tried to convert type from ({value.GetType()}) to long");
		}

		public override void SetValue(IDbDataParameter parameter, Id<TModel> value)
		{
			parameter.DbType = DbType.Int64;
			parameter.Value = value.Value;
		}

		/// <summary>Register identifier with Dapper</summary>
		public static void Register()
		{
			AddTypeHandler(new DapperTypeHandler<TModel>());
		}
	}
}
