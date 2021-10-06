using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RuS.Application.Features.Projects.Commands.AddEdit;
using RuS.Application.Features.Projects.Commands.Delete;
using RuS.Application.Features.Projects.Queries.Export;
using RuS.Application.Features.Projects.Queries.GetAll;
using RuS.Application.Features.Projects.Queries.GetById;
using RuS.Shared.Constants.Permission;
using System.Threading.Tasks;

namespace RuS.Server.Controllers.v1.Project
{
    public class ProjectsController : BaseApiController<ProjectsController>
    {
        /// <summary>
        /// Get All Projects
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Projects.View)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var projects = await _mediator.Send(new GetAllProjectsQuery());
            return Ok(projects);
        }

        /// <summary>
        /// Get a Project By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 Ok</returns>
        [Authorize(Policy = Permissions.Projects.View)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var project = await _mediator.Send(new GetProjectByIdQuery() { Id = id });
            return Ok(project);
        }

        /// <summary>
        /// Create/Update a Project
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Projects.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditProjectCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        /// <summary>
        /// Delete a Project
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Projects.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeleteProjectCommand { Id = id }));
        }

        /// <summary>
        /// Search Project and Export to Excel
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        [Authorize(Policy = Permissions.Projects.Export)]
        [HttpGet("export")]
        public async Task<IActionResult> Export(string searchString = "")
        {
            return Ok(await _mediator.Send(new ExportProjectsQuery(searchString)));
        }
    }
}
