﻿using Microsoft.Extensions.DependencyInjection;
using Montr.Modularity;

namespace Kompany
{
	public class Module : IModule
	{
		public void ConfigureServices(IServiceCollection services)
		{
			// no-op, to load assembly in domain
		}
	}
}
