﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Montr.Core.Models;

namespace Montr.Core.Services
{
	public interface IContentProvider
	{
		Task<Menu> GetMenu(string menuId);
	}

	public class DefaultContentProvider : IContentProvider
	{
		public async Task<Menu> GetMenu(string menuId)
		{
			var result = new Menu
			{
				Id = menuId,
				Items = new List<Menu>()
			};

			if (menuId == "TopMenu")
			{
				result.Items.Add(new Menu { Id = "p.0", Icon = "home", Route = "/" });
				// result.Items.Add(new Menu { Id = "login", Name = "Войти", Icon = "login", Route = "/account/login" });
				result.Items.Add(new Menu { Id = "register", Name = "Регистрация", Icon = "user-add", Route = "/account/register" });
				result.Items.Add(new Menu { Id = "p.1", Name = "Регистрация", Icon = "user-add", Route = "/register" });
				result.Items.Add(new Menu { Id = "p.2", Name = "Панель управления", Icon = "dashboard", Route = "/dashboard" });
			}

			if (menuId == "SideMenu")
			{
				result.Items.Add(new Menu { Id = "m.0", Name = "Панель управления", Icon = "dashboard", Route = "/dashboard" });
				result.Items.Add(new Menu { Id = "m.1", Name = "Процедуры", Icon = "project", Route = "/events" });

				result.Items.Add(new Menu
				{
					Id = "m.3",
					Name = "Администрирование",
					Icon = "setting",
					Items = new List<Menu>
					{
						new Menu {Id = "m.3.1", Name = "Справочники", Route = "/classifiers"},
						new Menu {Id = "m.3.2", Name = "Настройки", Route = "/settings"},
						new Menu {Id = "m.3.3", Name = "Локализация", Route = "/locales"},
						new Menu {Id = "hangfire", Name = "Hangfire Dashboard", Url = "/hangfire"}
					}
				});

				result.Items.Add(new Menu
				{
					Id = "tender",
					Name = "Промо",
					Icon = "global",
					Route = "/"
				});
			}

			return await Task.FromResult(result);
		}
	}
}