﻿using System.Collections.Generic;
using System.Linq;
using LinqToDB.Configuration;
using Microsoft.Extensions.Configuration;

namespace Montr.Data.Linq2Db
{
	public class DbSettingsProvider : ILinqToDBSettings
	{
		private readonly IConfiguration _configuration;

		public IEnumerable<IDataProviderSettings> DataProviders => Enumerable.Empty<IDataProviderSettings>();

		public string DefaultConfiguration => "Default";

		public string DefaultDataProvider => "PostgreSQL"; // "SqlServer";

		public DbSettingsProvider(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public IEnumerable<IConnectionStringSettings> ConnectionStrings
		{
			get
			{
				yield return new ConnectionStringSettings
				{
					Name = DefaultConfiguration,
					ProviderName = DefaultDataProvider,
					ConnectionString = _configuration.GetConnectionString("Default")
				};
			}
		}

		public class ConnectionStringSettings : IConnectionStringSettings
		{
			public string ConnectionString { get; set; }

			public string Name { get; set; }

			public string ProviderName { get; set; }

			public bool IsGlobal => false;
		}
	}
}