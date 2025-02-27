﻿using System;
using MediatR;
using Montr.Metadata.Models;

namespace Montr.Docs.Queries
{
	public class GetDocumentMetadata : MetadataRequest, IRequest<DataView>
	{
		public Guid UserUid { get; set; }

		public Guid DocumentUid { get; set; }
	}
}
