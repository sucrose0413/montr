﻿using System.Threading;
using System.Threading.Tasks;
using Montr.Automate.Models;
using Montr.Automate.Services;

namespace Montr.Automate.Impl.Services
{
	public class SetFieldAutomationActionProvider : IAutomationActionProvider
	{
		public Task Execute(AutomationAction automationAction, AutomationContext context, CancellationToken cancellationToken)
		{
			throw new System.NotImplementedException();
		}
	}
}