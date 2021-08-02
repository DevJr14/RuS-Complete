using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RuS.Application.Features.Sites.Commands.AddEdit;
using RuS.Application.Features.Sites.Commands.Delete;
using RuS.Application.Features.Sites.Queries.Export;
using RuS.Application.Features.Sites.Queries.GetAllPaged;
using RuS.Application.Features.Sites.Queries.GetById;
using RuS.Shared.Constants.Permission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RuS.Server.Controllers.v1.Core
{
    public class SitesController : BaseApiController<SitesController>
    {
        /// <summary>
        /// Create/Update a Site
        /// </summary>
        /// <param name="command">
        /// <summary>
        /// Recieves input values
        /// </summary>
        /// </param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Sites.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditSiteCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        /// <summary>
        /// Get All Sites. Data will be paged.
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchString"></param>
        /// <param name="orderBy"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Sites.View)]
        [HttpGet]
        public async Task<IActionResult> GetAll(int pageNumber, int pageSize, string searchString, string orderBy = null)
        {
            var companies = await _mediator.Send(new GetAllSitesQuery(pageNumber, pageSize, searchString, orderBy));
            return Ok(companies);
        }

        /// <summary>
        /// Get a Site By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 Ok</returns>
        [Authorize(Policy = Permissions.Sites.View)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var site = await _mediator.Send(new GetSiteByIdQuery() { Id = id });
            return Ok(site);
        }

        /// <summary>
        /// Delete a Site
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK response</returns>
        [Authorize(Policy = Permissions.Sites.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeleteSiteCommand { Id = id }));
        }

        /// <summary>
        /// Search Sites and Export to Excel
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Sites.Export)]
        [HttpGet("export")]
        public async Task<IActionResult> Export(string searchString = "")
        {
            return Ok(await _mediator.Send(new ExportSitesQuery(searchString)));
        }
    }
}
