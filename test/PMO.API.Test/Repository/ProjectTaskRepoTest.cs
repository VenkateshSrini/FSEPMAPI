using Microsoft.Extensions.Logging;
using PMO.API.DomainModel;
using PMO.API.Repository;
using Raven.Client.Documents;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
namespace PMO.API.Test.Repository
{
    [Collection("RepoUnitTest")]
    public class ProjectTaskRepoTest
    {
        private readonly IDocumentStore DocStore;
        public ProjectTaskRepoTest(DocumentStoreClassFixture fixture) { 
            DocStore = fixture.Store;
            ManageUserCollection().GetAwaiter().GetResult();
        }
        private ILogger<ProjectTaskRepo> createProjectTaskLogger()
        {
            var loggerFactory = new LoggerFactory();
            return loggerFactory.CreateLogger<ProjectTaskRepo>();
        }
        private ILogger<UserRepo> createUserLogger()
        {
            var loggerFactory = new LoggerFactory();
            return loggerFactory.CreateLogger<UserRepo>();
        }
        protected async  Task ManageUserCollection()
        {
            
            var pmoUser1 = new PMOUser
            {
                EmployeeId = "EP001",
                LastName = "L1",
                FirstName = "F1"
            };
            var pmoUser2 = new PMOUser
            {
                EmployeeId = "EP002",
                LastName = "L2",
                FirstName = "F2"
            };
            var pmoUser3 = new PMOUser
            {
                EmployeeId = "EP003",
                LastName = "L3",
                FirstName = "F3"
            };
            var pmoUser4 = new PMOUser
            {
                EmployeeId = "E004",
                LastName = "L4",
                FirstName = "F4"
            };
            var pmoUser5 = new PMOUser
            {
                EmployeeId = "EP005",
                LastName = "L5",
                FirstName = "F5"
            };
            using (var session = DocStore.OpenAsyncSession())
            {
                var userCollectionLock = await session.Query<UserCollectionLock>()
                                                      .FirstOrDefaultAsync();
                if ((userCollectionLock == default) || (!userCollectionLock.IsUserCollectionAdded))
                {
                    var logger = createUserLogger();
                    var userRepo = new UserRepo(session, logger);
                    var result = await userRepo.AddUser(pmoUser1);
                    result = await userRepo.AddUser(pmoUser2);
                    result = await userRepo.AddUser(pmoUser3);
                    result = await userRepo.AddUser(pmoUser4);
                    result = await userRepo.AddUser(pmoUser5);
                    userCollectionLock = new UserCollectionLock { IsUserCollectionAdded = true };
                    await session.StoreAsync(userCollectionLock);
                    await session.SaveChangesAsync();
                }

            }
        }
        [Fact]
        public async Task CUDProjectTest()
        {
            var project = new Project {
                End = DateTime.Today.AddDays(1),
                PMId="E001",
                Priority=1,
                Start=DateTime.Today,
                Status = 0,
                Title = "Project A"
            };
            var logger = createProjectTaskLogger();
            using (var session = DocStore.OpenAsyncSession())
            {
                var projTaskRepo = new ProjectTaskRepo(session, logger);
                var projAddResult = await projTaskRepo.AddProject(project);
                Assert.True(projAddResult.Item1);
                Assert.NotEmpty(projAddResult.Item2);
            }

        }
    }
}
