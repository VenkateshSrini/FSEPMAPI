using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PMO.API.DomainService;
using PMO.API.Messages;

namespace PMO.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjTaskService projService;
        private ILogger<ProjectController> projLogger;
        public ProjectController(IProjTaskService projService, ILogger<ProjectController> logger)
        {
            this.projService = projService;
            projLogger = logger;
        }
        [HttpGet]
        [Route("GetAllActiveProject")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ProjectListing>> GetAllActiveProject()
        {
            var results = await projService.GetAllActiveProject();
            if (results.Count == 0)
                return NotFound("No active projects found");
            return Ok(results);
        }
        [HttpGet]
        [Route("GetProjectByName")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ProjectListing>> GetProjectByName(string prjId)
        {
            if (string.IsNullOrWhiteSpace(prjId))
                return BadRequest("project id is empty");
            var result = await projService.GetProjectByName(prjId);
            if (result.Count == 0)
                return NotFound("No project with given name found");
            return Ok(result);
        }
        [HttpPost]
        [Route("AddProject")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<bool>>AddProject([FromBody]ProjectAdd projectAdd)
        {
            if (projectAdd == null)
            {
                ModelState.AddModelError("ParameterEmpty", "Input parameter are all empty");
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await projService.AddProject(projectAdd);
            if (result.Item1)
                return Created($"api/Project/{result.Item2}", result.Item1);
            else
                return StatusCode(500, "Unable to create project");
        }
        [HttpPut]
        [Route("EditProject")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        public async Task<ActionResult<bool>> EditProject([FromBody]ProjectMod projectMod)
        {
            if (projectMod == null)
            {
                ModelState.AddModelError("ParameterEmpty", "Input parameter are all empty");
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await projService.EditProject(projectMod);
            if (result.Item1)
                return Accepted();
            else
                return StatusCode(500, "Unable to edit project");
        }
        [HttpPut]
        [Route("SuspendProject")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        public async Task<ActionResult<bool>>SuspendProject(string projId)
        {
            if (string.IsNullOrWhiteSpace(projId))
                return BadRequest("project id is empty");
            var result = await projService.SuspendProject(projId);
            if (result)
                return Accepted();
            else
                return StatusCode(500, "Unable to suspend project");
        }
        [HttpGet]
        [Route("GetAllActiveTask")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<TaskListing>> GetAllActiveTask(string projId)
        {
            if (string.IsNullOrWhiteSpace(projId))
                return BadRequest("project id is empty");
            var result = await projService.GetAllActiveTask(projId);
            if (result.Count > 0)
                return Ok(result);
            else
                return NotFound("No Active task found");
        }
        [HttpPost]
        [Route("AddTask")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<bool>>AddTask([FromBody]TaskAdd taskAdd)
        {
            if (taskAdd == null)
            {
                ModelState.AddModelError("ParameterEmpty", "Input parameter are all empty");
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await projService.AddTask(taskAdd);
            if (result.Item1)
                return Created($"api/Task/{result.Item2}", result.Item1);
            else
                return StatusCode(500, "Unable to create project");

        }
        [HttpPut]
        [Route("EditTask")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        public async Task<ActionResult<bool>>EditTask([FromBody]TaskMod taskMod)
        {
            if (taskMod == null)
            {
                ModelState.AddModelError("ParameterEmpty", "Input parameter are all empty");
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await projService.EditTask(taskMod);
            if (result.Item1)
                return Accepted();
            else
                return StatusCode(500, "Unable to edit task");
        }
        [HttpPut]
        [Route("EndTask")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        public async Task<ActionResult<bool>>EndTask(string projId, string tskId)
        {
            if (string.IsNullOrWhiteSpace(projId) || (string.IsNullOrWhiteSpace(tskId)))
                return BadRequest("project id and task id cannot be empty");
            var result = await projService.EndTask(projId, tskId);
            if (result.Item1)
                return Accepted();
            else
                return StatusCode(500, "Unable to end task");
        }
    }
}