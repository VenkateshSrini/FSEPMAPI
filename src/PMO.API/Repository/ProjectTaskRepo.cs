using Microsoft.Extensions.Logging;
using PMO.API.DomainModel;
using Raven.Client.Documents.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMO.API.Repository
{
    public class ProjectTaskRepo : IProjectTaskRepo
    {
        private IAsyncDocumentSession asyncDocumentSession;
        private ILogger<ProjectTaskRepo> logger;
        public ProjectTaskRepo(IAsyncDocumentSession asyncDocumentSession, ILogger<ProjectTaskRepo> logger)
        {
            this.asyncDocumentSession = asyncDocumentSession;
            this.logger = logger;
        }
        public Task<Tuple<bool, string>> AddProject(Project project)
        {
            throw new NotImplementedException();
        }

        public Task<Tuple<bool, string>> AddTask(string projectId, ProjTask projTask)
        {
            throw new NotImplementedException();
        }

        public Task<Tuple<bool, string>> EditProject(Project project)
        {
            throw new NotImplementedException();
        }

        public Task<Tuple<bool, string>> EditTask(string projectId, ProjTask projTask)
        {
            throw new NotImplementedException();
        }

        public Task<Tuple<bool, string>> EndTask(string projectId, string taskId)
        {
            throw new NotImplementedException();
        }

        public Task<ProjectUserVO> GetAllActiveProject()
        {
            throw new NotImplementedException();
        }

        public Task<TaskUserVO> GetAllActiveTask(string projectId)
        {
            throw new NotImplementedException();
        }

        public Task<ProjectUserVO> GetProjectByName(string projectName)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetProjectCountByPM(string pmId)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetTaskCountByUser(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SuspendProject(string ProjectId)
        {
            throw new NotImplementedException();
        }
    }
}
