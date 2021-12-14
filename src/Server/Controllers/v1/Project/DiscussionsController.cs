using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RuS.Application.Features.Discussions.Commands.AddEdit;
using RuS.Application.Features.Discussions.Commands.Delete;
using RuS.Application.Features.Discussions.Queries.GetAll;
using RuS.Application.Features.Discussions.Queries.GetById;
using RuS.Shared.Constants.Permission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RuS.Server.Controllers.v1.Project
{
    public class DiscussionsController : BaseApiController<DiscussionsController>
    {
        /// <summary>
        /// Get All Discussions For A Project
        /// </summary>
        /// <param name="projectid">
        /// </param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Discussions.View)]
        [HttpGet("all-for-project/{projectid}")]
        public async Task<IActionResult> GetAll(int projectid)
        {
            return Ok(await _mediator.Send(new GetAllDiscussionsForProjectQuery() {ProjectId = projectid }));
        }

        /// <summary>
        /// Get All Discussions For A Task
        /// </summary>
        /// <param name="taskid">
        /// </param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Discussions.View)]
        [HttpGet("all-for-task/{taskid}")]
        public async Task<IActionResult> GetAllForTask(int taskid)
        {
            return Ok(await _mediator.Send(new GetAllDiscussionsForTaskQuery() { TaskId = taskid }));
        }

        /// <summary>
        /// Get A Discussions By Id
        /// </summary>
        /// <param name="id">
        /// </param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Discussions.View)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _mediator.Send(new GetDiscussionByIdQuery() { Id = id }));
        }

        /// <summary>
        /// Create/Update Discussion
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 Ok</returns>
        [Authorize(Policy = Permissions.Discussions.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditDiscussionCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        /// <summary>
        /// Delete a Discussion
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Discussions.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeleteDiscussionCommand() { Id = id }));
        }
    }
}
