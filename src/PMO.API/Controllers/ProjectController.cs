using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PMO.API.DomainService;

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

    }
}