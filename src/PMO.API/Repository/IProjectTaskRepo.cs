using PMO.API.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMO.API.Repository
{
    public interface IProjectTaskRepo
    {
        Task<int> GetProjectCountByPM(string pmId);
        Task<int> GetTaskCountByUser(string userId);
        Task<ProjectUserVO> GetAllActiveProject();
        Task<TaskUserVO> GetAllActiveTask(string projectId);
        Task<ProjectUserVO> GetProjectByName(string projectName);
        Task<Tuple<bool, string>> AddProject(Project project);
        Task<Tuple<bool, string>> EditProject(Project project);
        Task<bool> SuspendProject(string ProjectId);
        Task<Tuple<bool, string>> AddTask(string projectId, ProjTask projTask);
        Task<Tuple<bool, string>> EditTask(string projectId, ProjTask projTask);
        Task<Tuple<bool, string>> EndTask(string projectId, string taskId);
    }
}
