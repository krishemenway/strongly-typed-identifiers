using Npgsql;
using System.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace ExampleService
{
	internal static class Database
	{
		static Database()
		{
			StronglyTyped.GuidIds.Dapper.DapperIdRegistrar.RegisterAll(Assembly.GetExecutingAssembly());
			StronglyTyped.IntIds.Dapper.DapperIdRegistrar.RegisterAll(Assembly.GetExecutingAssembly());
			StronglyTyped.LongIds.Dapper.DapperIdRegistrar.RegisterAll(Assembly.GetExecutingAssembly());
		}

		internal static IDbConnection CreateConnection()
		{
			var connection = new NpgsqlConnection(ConnectionString.Value);
			connection.Open();
			return connection;
		}

		internal static string CreateConnectionString()
		{
			var connectionParams = new Dictionary<string, string>
				{
					{ "Host", Program.Settings.GetValue<string>("DatabaseHost") },
					{ "Port", Program.Settings.GetValue<int>("DatabasePort").ToString() },
					{ "Username", Program.Settings.GetValue<string>("DatabaseUser") },
					{ "Password", Program.Settings.GetValue<string>("DatabasePassword") },
					{ "Database", Program.Settings.GetValue<string>("DatabaseName") },
				};

			return string.Join(";", connectionParams.Select(param => $"{param.Key}={param.Value}"));
		}

		private static readonly Lazy<string> ConnectionString = new Lazy<string>(() => CreateConnectionString());
	}
}
