﻿using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Montr.Core.Models;
using Montr.Kompany.Models;
using Montr.Kompany.Queries;
using Montr.Web.Services;

namespace Montr.Kompany.Controllers
{
	// todo: remove, use mdm
	[Authorize, ApiController, Route("api/[controller]/[action]")]
	public class ContractorController : ControllerBase
	{
		private readonly IMediator _mediator;
		private readonly ICurrentUserProvider _currentUserProvider;

		public ContractorController(IMediator mediator, ICurrentUserProvider currentUserProvider)
		{
			_mediator = mediator;
			_currentUserProvider = currentUserProvider;
		}

		[HttpPost]
		public async Task<ActionResult<SearchResult<Company>>> List(ContractorSearchRequest request)
		{
			return await _mediator.Send(new GetContractorList
			{
				UserUid = _currentUserProvider.GetUserUid(),
				Request = request
			});
		}
	}
}