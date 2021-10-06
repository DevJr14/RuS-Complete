using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RuS.Application.Features.Clients.Commands.AddEdit;
using RuS.Application.Features.Clients.Commands.Delete;
using RuS.Application.Features.Clients.Queries.Export;
using RuS.Application.Features.Clients.Queries.GetAll;
using RuS.Application.Features.Clients.Queries.GetById;
using RuS.Shared.Constants.Permission;
using System.Threading.Tasks;

namespace RuS.Server.Controllers.v1.Project
{
    public class ClientsController : BaseApiController<ClientsController>
    {
        /// <summary>
        /// Get All Clients
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Clients.View)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var clients = await _mediator.Send(new GetAllClientsQuery());
            return Ok(clients);
        }

        /// <summary>
        /// Get a Clients By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 Ok</returns>
        [Authorize(Policy = Permissions.Clients.View)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var client = await _mediator.Send(new GetClientByIdQuery() { Id = id });
            return Ok(client);
        }

        /// <summary>
        /// Create/Update a Client
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Clients.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditClientCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        /// <summary>
        /// Delete a Client
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Clients.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeleteClientCommand { Id = id }));
        }

        /// <summary>
        /// Search Client and Export to Excel
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        [Authorize(Policy = Permissions.Clients.Export)]
        [HttpGet("export")]
        public async Task<IActionResult> Export(string searchString = "")
        {
            return Ok(await _mediator.Send(new ExportClientsQuery(searchString)));
        }
    }
}
