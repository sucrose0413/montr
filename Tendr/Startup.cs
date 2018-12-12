﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Montr.Data.Linq2Db;
using Montr.Metadata.Controllers;
using Montr.Modularity;
using Montr.Web;

namespace Tendr
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddLinq2Db(
				Configuration.GetSection("ConnectionString").Get<ConnectionStringSettings>());

			services.Configure<CookiePolicyOptions>(options =>
			{
				// This lambda determines whether user consent for non-essential cookies is needed for a given request.
				options.CheckConsentNeeded = context => true;
				options.MinimumSameSitePolicy = SameSiteMode.None;
			});

			services.AddMvc()
				.SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
				.AddApplicationPart(typeof(MetadataController).Assembly)
				.AddJsonOptions(options =>
				{
					options.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.None;
					options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter(true));
					options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
				});

			services.AddOpenIdAuthentication(
				Configuration.GetSection("OpenId").Get<OpenIdOptions>());

			services.AddModules();
		}

		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			app.UseWhen(
				context => !context.Request.Path.StartsWithSegments("/api"),
				a => a.UseExceptionHandler("/Home/Error")
			);
			app.UseHsts();

			app.UseStaticFiles();
			app.UseCookiePolicy();

			app.UseAuthentication();

			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "default",
					template: "{controller=Home}/{action=Index}/{id?}");
			});
		}
	}
}
