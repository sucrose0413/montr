﻿using System.Threading.Tasks;
using Kompany.Models;
using Kompany.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Montr.Metadata.Models;
using Montr.Web.Services;

namespace Kompany.Controllers
{
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
		public async Task<ActionResult<DataResult<Company>>> List(ContractorSearchRequest request)
		{
			return await _mediator.Send(new GetContractorList
			{
				UserUid = _currentUserProvider.GetUserUid(),
				Request = request
			});
		}
	}
}