﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Montr.Data.Linq2Db;
using Montr.Tendr.Impl.QueryHandlers;
using Montr.Tendr.Models;
using Montr.Tendr.Queries;

namespace Montr.Tendr.Tests.QueryHandlers
{
	[TestClass]
	public class GetInvitationListHandlerTests
	{
		[TestMethod]
		public async Task GetInvitationList_ForNormalRequest_ReturnItems()
		{
			// arrange
			var cancellationToken = CancellationToken.None;
			var dbContextFactory = new DefaultDbContextFactory();

			var handler = new GetInvitationListHandler(dbContextFactory);

			// act
			var command = new GetInvitationList
			{
				UserUid = Guid.NewGuid(),
				Request = new InvitationSearchRequest
				{
				}
			};

			var result = await handler.Handle(command, cancellationToken);

			// assert
			Assert.IsNotNull(result);
			Assert.IsNotNull(result.Rows);
		}
	}
}