using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RuS.Application.Features.Companies.Queries.GetById;
using RuS.Application.Features.Employees.Commands.AddEdit;
using RuS.Application.Features.Employees.Commands.Delete;
using RuS.Application.Features.Employees.Queries.Export;
using RuS.Application.Features.Employees.Queries.GetAllPaged;
using RuS.Shared.Constants.Permission;
using System.Threading.Tasks;

namespace RuS.Server.Controllers.v1.Core
{
    public class EmployeesController : BaseApiController<EmployeesController>
    {    
        /// <summary>
        /// Create/Update a Employee
        /// </summary>
        /// <param name="command">
        /// <summary>
        /// Recieves input values
        /// </summary>
        /// </param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Employees.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditEmployeeCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        /// <summary>
        /// Get All Employees
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchString"></param>
        /// <param name="orderBy"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Employees.View)]
        [HttpGet]
        public async Task<IActionResult> GetAll(int pageNumber, int pageSize, string searchString, string orderBy = null)
        {
            var employees = await _mediator.Send(new GetAllEmployeesQuery(pageNumber, pageSize, searchString, orderBy));
            return Ok(employees);
        }

        /// <summary>
        /// Get a Employee By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 Ok</returns>
        [Authorize(Policy = Permissions.Employees.View)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var employee = await _mediator.Send(new GetCompanyByIdQuery() { Id = id });
            return Ok(employee);
        }

        /// <summary>
        /// Delete a Employees
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK response</returns>
        [Authorize(Policy = Permissions.Employees.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeleteEmployeeCommand { Id = id }));
        }

        /// <summary>
        /// Search Employees and Export to Excel
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Employees.Export)]
        [HttpGet("export")]
        public async Task<IActionResult> Export(string searchString = "")
        {
            return Ok(await _mediator.Send(new ExportEmployeesQuery(searchString)));
        }
    }
}
