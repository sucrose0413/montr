﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Montr.Modularity
{
	public static class ModularityServiceCollectionExtensions
	{
		public static ICollection<IModule> AddModules(this IServiceCollection services, IConfiguration configuration, ILogger logger)
		{
			var allAssemblies = AppDomain.CurrentDomain.GetAssemblies();

			foreach (var file in Directory.EnumerateFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll"))
			{
				if (allAssemblies.FirstOrDefault(
						x => string.Equals(x.Location, file, StringComparison.OrdinalIgnoreCase)) == null)
				{
					if (logger.IsEnabled(LogLevel.Information))
					{
						logger.LogInformation($"Preloading assembly from {file}");
					}

					Assembly.LoadFrom(file);
				}
			}

			var types = new List<Type>();
			var result = new List<IModule>();

			foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				types.AddRange(assembly.GetTypes()
					.Where(x => x.IsClass && x.IsAbstract == false && typeof(IModule).IsAssignableFrom(x)));
			}

			// todo: order by module dependencies
			foreach (var type in types)
			{
				var module = (IModule)Activator.CreateInstance(type);

				if (logger.IsEnabled(LogLevel.Information))
				{
					logger.LogInformation($"Initializing module {module}");
				}

				module.ConfigureServices(configuration, services);

				result.Add(module);
			}

			return result.AsReadOnly();
		}
	}
}