using System;
using System.Data;
using static Dapper.SqlMapper;

namespace StronglyTyped.StringIds.Dapper
{
	/// <summary>Type handler for registering identifiers in Dapper queries</summary>
	/// <typeparam name="TModel">Type the identifier is for (e.g. Person, Team)</typeparam>
	public class DapperTypeHandler<TModel> : TypeHandler<Id<TModel>>
	{
		public override Id<TModel> Parse(object value)
		{
			if (value is string valueAsString)
			{
				return new Id<TModel>(valueAsString);
			}

			throw new Exception($"Tried to convert type from ({value.GetType()}) to string");
		}

		public override void SetValue(IDbDataParameter parameter, Id<TModel> value)
		{
			parameter.DbType = DbType.String;
			parameter.Value = value.Value;
		}

		/// <summary>Register identifier with Dapper</summary>
		public static void Register()
		{
			AddTypeHandler(new DapperTypeHandler<TModel>());
		}
	}
}
