using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RuS.Application.Features.Tasks.Commands.AddEdit;
using RuS.Application.Features.Tasks.Commands.Delete;
using RuS.Application.Features.Tasks.Queries.Export;
using RuS.Application.Features.Tasks.Queries.GetAll;
using RuS.Application.Features.Tasks.Queries.GetById;
using RuS.Shared.Constants.Permission;
using System.Threading.Tasks;

namespace RuS.Server.Controllers.v1.Project
{
    public class TasksController : BaseApiController<TasksController>
    {

        /// <summary>
        /// Get All Tasks
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Tasks.View)]
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var tasts = await _mediator.Send(new GetAllTasksQuery());
            return Ok(tasts);
        }

        /// <summary>
        /// Get All Tasks For a Project
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Tasks.View)]
        [HttpGet("all/project/{projectId}")]
        public async Task<IActionResult> GetAllForProject(int projectId)
        {
            var tasts = await _mediator.Send(new GetAllTasksForProjectQuery() { ProjectId = projectId});
            return Ok(tasts);
        }

        /// <summary>
        /// Get a Task By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 Ok</returns>
        [Authorize(Policy = Permissions.Tasks.View)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var task = await _mediator.Send(new GetTaskByIdQuery() { Id = id });
            return Ok(task);
        }

        /// <summary>
        /// Create/Update a Task
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Tasks.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditTaskCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        /// <summary>
        /// Delete a Task
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Tasks.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeleteTaskCommand { Id = id }));
        }

        /// <summary>
        /// Search Tasks and Export to Excel
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        [Authorize(Policy = Permissions.Tasks.Export)]
        [HttpGet("export")]
        public async Task<IActionResult> Export(string searchString = "")
        {
            return Ok(await _mediator.Send(new ExportTasksQuery(searchString)));
        }
    }
}
