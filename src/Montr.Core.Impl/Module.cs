﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Montr.Core.Impl.Services;
using Montr.Core.Services;
using Montr.Modularity;

namespace Montr.Core.Impl
{
	public class Module : IModule
	{
		public void ConfigureServices(IConfiguration configuration, IServiceCollection services)
		{
			services.AddSingleton<IAuditLogService, DbAuditLogService>();
		}
	}
}