﻿using Microsoft.AspNetCore.Mvc;
using Tendr.Models;
using Tendr.Services;

namespace Tendr.Controllers
{
	[ApiController, Route("api/[controller]/[action]")]
	public class MetadataController : ControllerBase
	{
		private readonly IMetadataProvider _metadataProvider;

		public MetadataController(IMetadataProvider metadataProvider)
		{
			_metadataProvider = metadataProvider;
		}

		[HttpPost]
		public ActionResult<DataView> View(MetadataRequest request)
		{
			return _metadataProvider.GetView(request.ViewId);
		}
	}
}