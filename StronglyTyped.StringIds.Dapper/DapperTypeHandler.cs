using System.Data;
using static Dapper.SqlMapper;

namespace StronglyTyped.StringIds.Dapper
{
	public class DapperTypeHandler<T> : TypeHandler<Id<T>>
	{
		public override Id<T> Parse(object value)
		{
			return new Id<T>((string)value);
		}

		public override void SetValue(IDbDataParameter parameter, Id<T> value)
		{
			parameter.DbType = DbType.String;
			parameter.Value = value.Value;
		}

		public static void RegisterId()
		{
			AddTypeHandler(new DapperTypeHandler<T>());
		}
	}
}
