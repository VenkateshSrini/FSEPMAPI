using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using PMO.API.DomainService;

using Microsoft.Extensions.Logging;
using PMO.API.Repository;
using PMO.API.DomainModel;
using AutoMapper;
using PMO.API.Messages;

namespace PMO.API.Test.DomainService
{
    public class ProjectTaskServiceTest
    {
        private ILogger<ProjTaskService>createProjServiceLogger()
        {
            var loggerFactory = new LoggerFactory();
            return loggerFactory.CreateLogger<ProjTaskService>();
        }
        [Fact]
        public async Task AddProject()
        {
            var projectMod = new ProjectAdd
            {
                EndDate = DateTime.Today.AddDays(2),
                StartDate = DateTime.Today,
                PMUsrId = "Usr/1",
                ProjectTitle = "Project A1",
                Priority =1

            };
            var project = new Project
            {
                End = projectMod.EndDate,
                MaxTaskCount = 0,
                PMId = projectMod.PMUsrId,
                Priority = projectMod.Priority,
                Start = projectMod.StartDate,
                Status = 0,
                Title = projectMod.ProjectTitle
            };
            var mockProjTaskRepo = new Mock<IProjectTaskRepo>();
            mockProjTaskRepo.Setup(repo => repo.AddProject(It.IsAny<Project>()))
                            .Returns(Task.FromResult(new Tuple<bool, string>(true, "P/1")));
            var mockLogger = createProjServiceLogger();
            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(mapper => mapper.Map<Project>(It.IsAny<ProjectAdd>())).Returns(project);
            var projectTaskService = new ProjTaskService(mockProjTaskRepo.Object, mockLogger, mockMapper.Object);
            var result = await projectTaskService.AddProject(projectMod);
            Assert.True( result.Item1);
            Assert.Equal("P/1", result.Item2);
        }
        [Fact]
        public async Task ModifyProject()
        {
            var projectMod = new ProjectMod
            {
                EndDate = DateTime.Today.AddDays(2),
                StartDate = DateTime.Today,
                PMUsrId = "Usr/1",
                ProjectTitle = "Project A1",
                Priority = 1,
                ProjId = "P/1"

            };
            var project = new Project
            {
                End = projectMod.EndDate,
                MaxTaskCount = 0,
                PMId = projectMod.PMUsrId,
                Priority = projectMod.Priority,
                Start = projectMod.StartDate,
                Status = 0,
                Id = projectMod.ProjId

            };
            var mockProjTaskRepo = new Mock<IProjectTaskRepo>();
            mockProjTaskRepo.Setup(repo => repo.EditProject(It.IsAny<Project>()))
                            .Returns(Task.FromResult(new Tuple<bool, string>(true, "P/1")));
            var mockLogger = createProjServiceLogger();
            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(mapper => mapper.Map<Project>(It.IsAny<ProjectMod>())).Returns(project);
            var projectTaskService = new ProjTaskService(mockProjTaskRepo.Object, mockLogger, mockMapper.Object);
            var result = await projectTaskService.EditProject(projectMod);
            Assert.True(result.Item1);
            Assert.Equal("P/1", result.Item2);
        }
        [Fact]
        public async Task GetAllActiveProjectTest()
        {
            var projectlsting = new ProjectListing
            {
                EndDate = DateTime.Today.AddDays(2),
                StartDate = DateTime.Today,
                PMUsrId = "Usr/1",
                ProjectTitle = "Project A1",
                Priority = 1,
                ProjId = "P/1",
                CompletedTaskCount=0,
                PMUsrName="John",
                TotalTaskCount=0

            };
            var projectlstings = new List<ProjectListing> { projectlsting };
            var projectList = new List<ProjectUserVO>
            {
                new ProjectUserVO
                {
                    Projects =new Project
                    {
                        End = projectlsting.EndDate,
                        MaxTaskCount = 0,
                        PMId = projectlsting.PMUsrId,
                        Priority = projectlsting.Priority,
                        Start = projectlsting.StartDate,
                        Status = 0,
                        Id = projectlsting.ProjId
                    },
                    UserName=projectlsting.PMUsrName

                }
            };
            var mockProjTaskRepo = new Mock<IProjectTaskRepo>();
            mockProjTaskRepo.Setup(repo => repo.GetAllActiveProject())
                           .Returns(Task.FromResult(projectList));
            var mockLogger = createProjServiceLogger();
            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(mapper => mapper.Map<List<ProjectListing>>(It.IsAny<List<ProjectUserVO>>()))
                      .Returns(projectlstings);
            var projectTaskService = new ProjTaskService(mockProjTaskRepo.Object, mockLogger, mockMapper.Object);
            var result = await projectTaskService.GetAllActiveProject();
            Assert.NotEmpty(result);
        }
        [Fact]
        public async Task GetProjectByNameTest()
        {
            var projectlsting = new ProjectListing
            {
                EndDate = DateTime.Today.AddDays(2),
                StartDate = DateTime.Today,
                PMUsrId = "Usr/1",
                ProjectTitle = "Project A1",
                Priority = 1,
                ProjId = "P/1",
                CompletedTaskCount = 0,
                PMUsrName = "John",
                TotalTaskCount = 0

            };
            var projectlstings = new List<ProjectListing> { projectlsting };
            var projectList = new List<ProjectUserVO>
            {
                new ProjectUserVO
                {
                    Projects =new Project
                    {
                        End = projectlsting.EndDate,
                        MaxTaskCount = 0,
                        PMId = projectlsting.PMUsrId,
                        Priority = projectlsting.Priority,
                        Start = projectlsting.StartDate,
                        Status = 0,
                        Id = projectlsting.ProjId
                    },
                    UserName=projectlsting.PMUsrName

                }
            };
            var mockProjTaskRepo = new Mock<IProjectTaskRepo>();
            mockProjTaskRepo.Setup(repo => repo.GetProjectByName(It.IsAny<string>()))
                            .Returns(Task.FromResult(projectList));
            var mockLogger = createProjServiceLogger();
            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(mapper => mapper.Map<List<ProjectListing>>(It.IsAny<List<ProjectUserVO>>()))
                      .Returns(projectlstings);
            var projectTaskService = new ProjTaskService(mockProjTaskRepo.Object, mockLogger, mockMapper.Object);
            var result = await projectTaskService.GetProjectByName("P/1");
            Assert.NotEmpty(result);
        }
        [Fact]
        public async Task SuspendProjectTest()
        {
            var mockProjTaskRepo = new Mock<IProjectTaskRepo>();
            mockProjTaskRepo.Setup(repo => repo.SuspendProject(It.IsAny<string>()))
                            .Returns(Task.FromResult(true));
            var mockLogger = createProjServiceLogger();
            var mockMapper = new Mock<IMapper>();
            var projectTaskService = new ProjTaskService(mockProjTaskRepo.Object, mockLogger, mockMapper.Object);
            var result = await projectTaskService.SuspendProject("P/1");
            Assert.True(result);
        }
    }
}
