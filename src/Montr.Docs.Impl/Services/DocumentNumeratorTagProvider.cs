﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Montr.Docs.Models;
using Montr.MasterData.Services;

namespace Montr.Docs.Impl.Services
{
	public class DocumentNumberTagProvider : INumberTagProvider
	{
		public bool Supports(string entityTypeCode, out string[] supportedTags)
		{
			if (entityTypeCode == DocumentType.EntityTypeCode)
			{
				supportedTags = new []
				{
					"{Company}",
					"{Year2}",
					"{Year4}" 
				};

				return true;
			}

			supportedTags = null;
			return false;
		}

		public Task Resolve(string entityTypeCode, Guid enityUid,
			IEnumerable<string> tags, IDictionary<string, string> values, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}
	}
}