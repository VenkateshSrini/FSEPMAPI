using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PMO.API.DomainModel;
using PMO.API.DomainService;
using PMO.API.Messages;

namespace PMO.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly ILogger<UserController> userLogger;
        public UserController(IUserService userService, ILogger<UserController>logger)
        {
            this.userService = userService;
            this.userLogger = logger;
        }
        [HttpGet]
        [Route("SearchUser")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<PMOUser>>>SearchUser(string empId, string lName, string fName)
        {
            var searchCriteria = new UserSearchCriteria
            {
                EmployeeID = empId,
                LastName = lName,
                FirstName = fName
            };
            var results = await userService.GetUserByCriteria(searchCriteria);
            if (results.Count == 0)
                return NotFound("No User found");
            else
                return Ok(results);
        }
        [HttpGet]
        [Route("GetEmployeeById")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<PMOUser>> GetEmployeeById(string empId)
        {
            if (string.IsNullOrWhiteSpace(empId))
                return BadRequest("employee id is empty");
            var results = await userService.GetUserByEmployeeId(empId);
            if (results == default)
                return NotFound("User not found");
            return Ok(results);
        }
        [HttpGet]
        [Route("GetAllEmployee")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<PMOUser>>> GetAllEmployee()
        {
            var results = await userService.GetAllUser();
        if (results.Count==0)
                return NotFound("User not found");
            return Ok(results);
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>>Post([FromBody]UserAddMsg userAdd)
        {
            if (userAdd == null)
            {
                ModelState.AddModelError("ParameterEmpty", "Input parameter are all empty");
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var usr = await userService.GetUserByEmployeeId(userAdd.EmployeeId);
            if (usr!=default)
            {
                ModelState.AddModelError("EMployeeIdExist", "User already exist");
                return BadRequest(ModelState);
            }

            var result = await userService.Add(userAdd);
            if (result.Item1)
            {
                return Created($"api/Task/{result.Item2}", result.Item1);
            }
            return StatusCode(500, "Unable to create user");
        }
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> Put([FromBody]UserModMsg userMod)
        {
            if (userMod == null)
            {
                ModelState.AddModelError("ParameterEmpty", "Input parameter are all empty");
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await userService.Edit(userMod);
            if (result.Item1)
            {
                return Accepted();
            }
            else
                return StatusCode(500, "Unable to edit user");

        }
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> Delete(string empId)
        {
            if (string.IsNullOrWhiteSpace(empId))
                return BadRequest("employee id is empty");
            var result = await userService.Delete(empId);
            if (result)
                return NoContent();
            else
                return StatusCode(500, "User could not be deleted");
        }

    }
    
}