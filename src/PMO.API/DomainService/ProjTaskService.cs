using AutoMapper;
using Microsoft.Extensions.Logging;
using PMO.API.DomainModel;
using PMO.API.Messages;
using PMO.API.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PMO.API.DomainService
{
    public class ProjTaskService : IProjTaskService
    {
        private readonly IProjectTaskRepo projectTaskRepo;
        private readonly ILogger<ProjTaskService> logger;
        private readonly IMapper mapper;
        public ProjTaskService(IProjectTaskRepo projectTaskRepo,
            ILogger<ProjTaskService> logger,IMapper mapper)
        {
            this.projectTaskRepo = projectTaskRepo;
            this.logger = logger;
            this.mapper = mapper;
            
        }
        public async Task<Tuple<bool, string>> AddProject(ProjectAdd project)
        {
            if ((project.EndDate > DateTime.MinValue) && (project.StartDate > DateTime.MinValue)
                    && (project.StartDate > project.EndDate))
                return new Tuple<bool, string>(false, "start date greater than end date");
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(project);
            var validationResults = new List<ValidationResult>();
            if (Validator.TryValidateObject(project, validationContext, validationResults))
            {
                var projectDO = mapper.Map<Project>(project);
                return await projectTaskRepo.AddProject(projectDO);
            }
            return new Tuple<bool, string>(false, "validation failures");
        }

        public async Task<Tuple<bool, string>> EditProject(ProjectMod project)
        {
            if ((project.EndDate > DateTime.MinValue) && (project.StartDate > DateTime.MinValue)
                    && (project.StartDate > project.EndDate))
                return new Tuple<bool, string>(false, "start date greater than end date");
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(project);
            var validationResults = new List<ValidationResult>();
            if (Validator.TryValidateObject(project, validationContext, validationResults))
            {
                var projectDO = mapper.Map<Project>(project);
                return await projectTaskRepo.EditProject(projectDO);
            }
            return new Tuple<bool, string>(false, "validation failures");
        }

        public async Task<List<ProjectListing>> GetAllActiveProject()
        {
            var projUserVO = await projectTaskRepo.GetAllActiveProject();
            var results = mapper.Map<List<ProjectListing>>(projUserVO);
            return results;
        }

        public async Task<List<ProjectListing>> GetProjectByName(string projectName)
        {
            var projUserVO = await projectTaskRepo.GetProjectByName(projectName);
            var results = mapper.Map<List<ProjectListing>>(projUserVO);
            return results;
        }

        public async Task<bool> SuspendProject(string projectId)
        {
            return await projectTaskRepo.SuspendProject(projectId);
        }
    }
}
