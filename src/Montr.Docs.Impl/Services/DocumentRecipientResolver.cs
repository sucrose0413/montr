﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Montr.Automate.Models;
using Montr.Automate.Services;
using Montr.Core.Services;
using Montr.Docs.Models;
using Montr.Idx.Models;

namespace Montr.Docs.Impl.Services
{
	public class DocumentRecipientResolver : IRecipientResolver
	{
		public class KnownTypes
		{
			public const string Requester = "requester";
		}

		private readonly IRepository<User> _userRepository;

		public DocumentRecipientResolver(IRepository<User> userRepository)
		{
			_userRepository = userRepository;
		}

		public async Task<Recipient> Resolve(string recipient, AutomationContext automationContext, CancellationToken cancellationToken)
		{
			if (recipient == KnownTypes.Requester)
			{
				var document = (Document) automationContext.Entity;

				var searchResult = await _userRepository.Search(
					new UserSearchRequest { UserName = document.CreatedBy }, cancellationToken);

				var user = searchResult.Rows.SingleOrDefault();

				if (user != null)
				{
					return new Recipient { Email = user.Email };
				}
			}

			return null;
		}
	}
}
