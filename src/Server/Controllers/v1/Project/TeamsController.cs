using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Polly;
using RuS.Application.Features.Projects.Commands.AddEdit;
using RuS.Application.Features.Projects.Commands.Delete;
using RuS.Application.Features.Projects.Queries.Export;
using RuS.Application.Features.Projects.Queries.GetAll;
using RuS.Application.Features.Projects.Queries.GetById;
using RuS.Application.Features.Teams.Commands.AddEdit;
using RuS.Application.Features.Teams.Commands.Delete;
using RuS.Application.Features.Teams.Queries.Export;
using RuS.Application.Features.Teams.Queries.GetAll;
using RuS.Application.Features.Teams.Queries.GetById;
using RuS.Shared.Constants.Permission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RuS.Server.Controllers.v1.Project
{
    public class TeamsController : BaseApiController<TeamsController>
    {
        /// <summary>
        /// Get All Teams
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Teams.View)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var teams = await _mediator.Send(new GetAllTeamsQuery());
            return Ok(teams);
        }

        /// <summary>
        /// Get Project Teams
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Teams.View)]
        [HttpGet("project/{projectid}")]
        public async Task<IActionResult> GetAll(int projectId)
        {
            var teams = await _mediator.Send(new GetAllTeamsForProjectQuery() { ProjectId = projectId });
            return Ok(teams);
        }

        /// <summary>
        /// Get Team By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 Ok</returns>
        [Authorize(Policy = Permissions.Teams.View)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var team = await _mediator.Send(new GetTeamByIdQuery() { Id = id });
            return Ok(team);
        }

        /// <summary>
        /// Get Team Members
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 Ok</returns>
        [Authorize(Policy = Permissions.Teams.View)]
        [HttpGet("team-members/{id}")]
        public async Task<IActionResult> GetMembers(int id)
        {
            var teamMembers = await _mediator.Send(new GetTeamMembersQuery() { TeamId = id });
            return Ok(teamMembers);
        }

        /// <summary>
        /// Create/Update a Team
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Teams.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditTeamCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        /// <summary>
        /// Update Team Member(s) 
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Teams.Create)]
        [HttpPost("update-members")]
        public async Task<IActionResult> Post(UpdateTeamMembersCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        /// <summary>
        /// Add or Remove Team Member(s) 
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Teams.Create)]
        [HttpPost("members")]
        public async Task<IActionResult> Post(AddRemoveTeamMemberCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        /// <summary>
        /// Delete a Team
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Teams.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeleteTeamCommand { Id = id }));
        }

        /// <summary>
        /// Search Team and Export to Excel
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        [Authorize(Policy = Permissions.Teams.Export)]
        [HttpGet("export")]
        public async Task<IActionResult> Export(string searchString = "")
        {
            return Ok(await _mediator.Send(new ExportTeamsQuery(searchString)));
        }
    }
}
