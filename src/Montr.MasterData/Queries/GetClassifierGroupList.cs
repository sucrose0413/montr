﻿using System;
using MediatR;
using Montr.Core.Models;
using Montr.MasterData.Models;

namespace Montr.MasterData.Queries
{
	public class GetClassifierGroupList : IRequest<SearchResult<ClassifierGroup>>
	{
		public Guid UserUid { get; set; }

		public ClassifierGroupSearchRequest Request { get; set; }
	}
}