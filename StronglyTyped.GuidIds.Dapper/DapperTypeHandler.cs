using System;
using System.Data;
using static Dapper.SqlMapper;

namespace StronglyTyped.GuidIds.Dapper
{
	public class DapperTypeHandler<T> : TypeHandler<Id<T>>
	{
		public override Id<T> Parse(object value)
		{
			return new Id<T>((Guid)value);
		}

		public override void SetValue(IDbDataParameter parameter, Id<T> value)
		{
			parameter.DbType = DbType.Guid;
			parameter.Value = value.Value;
		}

		public static void Register()
		{
			AddTypeHandler(new DapperTypeHandler<T>());
		}
	}
}
