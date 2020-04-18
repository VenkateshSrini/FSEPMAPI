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

namespace PMO.API.Test.Repository
{
    public class UserRepoTest : RavenTestDriver
    {
        protected override void PreInitialize(IDocumentStore documentStore)
        {
            documentStore.Conventions.MaxNumberOfRequestsPerSession = 50;
        }
        private IDocumentStore prepareDocumentStore()
        {
            ConfigureServer(new TestServerOptions
            {
                DataDirectory = @".\RavenDBTestDir"
            });
            var store = GetDocumentStore();
            return store;

        }
        private ILogger<UserRepo> createUserLogger()
        {
            var loggerFactory = new LoggerFactory();
            return loggerFactory.CreateLogger<UserRepo>();
        }
        [Fact]
        public async Task AddUserTest()
        {
            var pmoUser = new PMOUser
            {
                EmployeeId = "E001",
                LastName = "L1",
                FirstName = "F1"
            };
            using (var documentStore = prepareDocumentStore())
            {
                using (var session = documentStore.OpenAsyncSession())
                {
                    var logger = createUserLogger();
                    var userRepo = new UserRepo(session, logger);
                    var result = await  userRepo.AddUser(pmoUser);
                    Assert.True(result.Item1);
                }
            }
        }
    }
}
