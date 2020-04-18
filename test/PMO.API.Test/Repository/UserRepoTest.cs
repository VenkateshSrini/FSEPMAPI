using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.TestDriver;
using Raven.Client.Documents.Session;
using Microsoft.Extensions.Logging;
using PMO.API.Repository;
using PMO.API.DomainModel;
using PMO.API.Messages;
[assembly: CollectionBehavior(DisableTestParallelization = true)]
namespace PMO.API.Test.Repository
{

    public class UserRepoTest:IClassFixture<DocumentStoreClassFixture>
    {
        private readonly IDocumentStore DocStore;
        public UserRepoTest(DocumentStoreClassFixture fixture) => DocStore = fixture.Store;
        
        private ILogger<UserRepo> createUserLogger()
        {
            var loggerFactory = new LoggerFactory();
            return loggerFactory.CreateLogger<UserRepo>();
        }
        [Fact]
        public async Task CURDUserTest()
        {
            var pmoUser = new PMOUser
            {
                EmployeeId = "E001",
                LastName = "L1",
                FirstName = "F1"
            };
           
                using (var session = DocStore.OpenAsyncSession())
                {
                    var logger = createUserLogger();
                    var userRepo = new UserRepo(session, logger);
                    var result = await userRepo.AddUser(pmoUser);

                    Assert.True(result.Item1);
                    pmoUser.LastName = "Last";
                    result = await userRepo.EditUser(pmoUser);
                    Assert.True(result.Item1);
                    var searchCrit = new UserSearchCriteria
                    {
                        EmployeeID = "E001",
                        LastName = "L1",
                        FirstName = "F1"
                    };
                   

                    var delResult = await userRepo.DeleteUser(pmoUser);
                    Assert.True(delResult);

                }
            
        }
        [Fact]
        public async Task RetrieveUserTest()
        {
            var pmoUser = new PMOUser
            {
                EmployeeId = "E001",
                LastName = "L1",
                FirstName = "F1"
            };
            
                using (var session = DocStore.OpenAsyncSession())
                {
                    var logger = createUserLogger();
                    var userRepo = new UserRepo(session, logger);
                    var addresult = await userRepo.AddUser(pmoUser);
                    var searchCrit = new UserSearchCriteria
                    {
                        EmployeeID = "E001",
                        LastName = "L1",
                        FirstName = "F1"
                    };
                    var getResultAny = await userRepo.GetAllUserMatchAnyCriteria(searchCrit);
                    Assert.Single(getResultAny);
                    var getResultAll = await userRepo.GetAllUser();
                    Assert.Single(getResultAll);
                    var getResultByEmpId = await userRepo.GetUserByEmployeeId(searchCrit.EmployeeID);
                    Assert.NotNull(getResultByEmpId);
                    Assert.Equal(searchCrit.EmployeeID, getResultByEmpId.EmployeeId);
                    
            }
            
        }
    }
}
