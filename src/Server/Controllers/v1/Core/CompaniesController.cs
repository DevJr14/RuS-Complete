using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RuS.Application.Features.Companies.Commands.AddEdit;
using RuS.Application.Features.Companies.Commands.Delete;
using RuS.Application.Features.Companies.Queries.Export;
using RuS.Application.Features.Companies.Queries.GetAll;
using RuS.Application.Features.Companies.Queries.GetAllPaged;
using RuS.Application.Features.Companies.Queries.GetById;
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
        [Authorize(Policy = Permissions.Companies.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditCompanyCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        /// <summary>
        /// Get All Paged Companies
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchString"></param>
        /// <param name="orderBy"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Companies.View)]
        [HttpGet("all-paged")]
        public async Task<IActionResult> GetAllPaged(int pageNumber, int pageSize, string searchString, string orderBy = null)
        {
            var companies = await _mediator.Send(new GetAllPagedCompaniesQuery(pageNumber, pageSize, searchString, orderBy));
            return Ok(companies);
        }

        /// <summary>
        /// Get All Companies
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Companies.View)]
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var companies = await _mediator.Send(new GetAllCompaniesQuery());
            return Ok(companies);
        }

        /// <summary>
        /// Get a Company By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 Ok</returns>
        [Authorize(Policy = Permissions.Companies.View)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var company = await _mediator.Send(new GetCompanyByIdQuery() { Id = id });
            return Ok(company);
        }

        /// <summary>
        /// Delete a Company
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK response</returns>
        [Authorize(Policy = Permissions.Companies.Delete)]
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
        [Authorize(Policy = Permissions.Companies.Export)]
        [HttpGet("export")]
        public async Task<IActionResult> Export(string searchString = "")
        {
            return Ok(await _mediator.Send(new ExportCompaniesQuery(searchString)));
        }

    }
}
