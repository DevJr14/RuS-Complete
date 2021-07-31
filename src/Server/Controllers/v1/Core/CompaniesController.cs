using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RuS.Application.Features.Companies.Commands.AddEdit;
using RuS.Application.Features.Companies.Commands.Delete;
using RuS.Application.Features.Companies.Queries.Export;
using RuS.Application.Features.Companies.Queries.GetAllPaged;
using RuS.Shared.Constants.Permission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RuS.Server.Controllers.v1.Core
{
    public class CompaniesController : BaseApiController<CompaniesController>
    {
        /// <summary>
        /// Create/Update a Company
        /// </summary>
        /// <param name="command">
        /// <summary>
        /// Recieves input values
        /// </summary>
        /// </param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Brands.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditCompanyCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        /// <summary>
        /// Get All Companies
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchString"></param>
        /// <param name="orderBy"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Products.View)]
        [HttpGet]
        public async Task<IActionResult> GetAll(int pageNumber, int pageSize, string searchString, string orderBy = null)
        {
            var companies = await _mediator.Send(new GetAllCompaniesQuery(pageNumber, pageSize, searchString, orderBy));
            return Ok(companies);
        }

        /// <summary>
        /// Delete a Company
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK response</returns>
        [Authorize(Policy = Permissions.Products.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeleteCompanyCommand { Id = id }));
        }

        /// <summary>
        /// Search Companies and Export to Excel
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Products.Export)]
        [HttpGet("export")]
        public async Task<IActionResult> Export(string searchString = "")
        {
            return Ok(await _mediator.Send(new ExportCompaniesQuery(searchString)));
        }

    }
}
