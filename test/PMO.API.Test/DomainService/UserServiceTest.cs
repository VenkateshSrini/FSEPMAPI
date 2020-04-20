﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using PMO.API.Repository;
using PMO.API.DomainModel;
using PMO.API.Messages;
using AutoMapper;
using PMO.API.DomainService;
using Microsoft.Extensions.Logging;

namespace PMO.API.Test.DomainService
{
    public class UserServiceTest
    {
        private ILogger<UserService> createUserLogger()
        {
            var loggerFactory = new LoggerFactory();
            return loggerFactory.CreateLogger<UserService>();
        }
        [Fact]
        public async Task AddUserTest()
        {
            var result = new Tuple<bool, string>(true, "Usr/1");
            var userAdd = new UserAddMsg {
                EmployeeId = "EP001",
                FirstName = "F1",
                LastName = "L2"
            };
            var pmoUser = new PMOUser
            {
                EmployeeId = userAdd.EmployeeId,
                FirstName = userAdd.FirstName,
                LastName = userAdd.LastName
            };
            var mockUserRepo = new Mock<IUserRepo>();
            var mockProjectTaskRepo = new Mock<IProjectTaskRepo>();
            var logger = createUserLogger();
            mockUserRepo.Setup(usrRepo => usrRepo.AddUser(It.IsAny<PMOUser>()))
                                               .Returns(Task.FromResult(result));
            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(map => map.Map<PMOUser>(It.IsAny<UserAddMsg>())).Returns(pmoUser);
            var userService = new UserService(mockUserRepo.Object, mockProjectTaskRepo.Object, logger,
                mockMapper.Object);
            var addResult = await userService.Add(userAdd);
            Assert.True(addResult.Item1);
            Assert.Equal("Usr/1", addResult.Item2);
        }
        [Fact]
        public async Task EditUserTest()
        {
            var result = new Tuple<bool, string>(true, "Usr/1");
            var userMod = new UserModMsg
            {
                EmployeeId = "EP001",
                FirstName = "F1",
                LastName = "L2"

            };
            var pmoUser = new PMOUser
            {
                EmployeeId = userMod.EmployeeId,
                FirstName = userMod.FirstName,
                LastName = userMod.LastName
            };
            var mockUserRepo = new Mock<IUserRepo>();
            var mockProjectTaskRepo = new Mock<IProjectTaskRepo>();
            var logger = createUserLogger();
            mockUserRepo.Setup(usrRepo => usrRepo.EditUser(It.IsAny<PMOUser>()))
                                              .Returns(Task.FromResult(result));
            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(map => map.Map<PMOUser>(It.IsAny<UserModMsg>())).Returns(pmoUser);
            var userService = new UserService(mockUserRepo.Object, mockProjectTaskRepo.Object, logger,
               mockMapper.Object);
            var modResult = await userService.Edit(userMod);
            Assert.True(modResult.Item1);
            Assert.Equal("Usr/1", modResult.Item2);
        }
        [Theory]
        [InlineData(0, 0)]
        [InlineData(1, 0)]
        [InlineData(0, 1)]
        public async Task Delete(int projUserCount, int taskUserCount)
        {
            var pmoUser = new PMOUser
            {
                EmployeeId = "EP001",
                FirstName = "F1",
                LastName = "L2",
                Id = "Usr/1"

            };
            var mockUserRepo = new Mock<IUserRepo>();
            var mockProjectTaskRepo = new Mock<IProjectTaskRepo>();
            var logger = createUserLogger();
            mockUserRepo.Setup(userRepo => userRepo.GetUserByEmployeeId(It.IsAny<string>()))
                        .Returns(Task.FromResult(pmoUser));
            mockUserRepo.Setup(userRepo => userRepo.DeleteUser(pmoUser)).Returns(Task.FromResult(true));
            var mockMapper = new Mock<IMapper>();
            mockProjectTaskRepo.Setup(projTskRepo => projTskRepo.GetProjectCountByPM(It.IsAny<string>()))
                               .Returns(Task.FromResult(projUserCount));
            mockProjectTaskRepo.Setup(projTskRepo => projTskRepo.GetTaskCountByUser(It.IsAny<string>()))
                               .Returns(Task.FromResult(taskUserCount));
            var userService = new UserService(mockUserRepo.Object, mockProjectTaskRepo.Object, logger,
               mockMapper.Object);

            if (projUserCount > 0)
                await Assert.ThrowsAsync<ApplicationException>(async () => await userService.Delete("EP001"));
            else if (taskUserCount > 0)
                await Assert.ThrowsAsync<ApplicationException>(async () => await userService.Delete("EP001"));
            else
            {
                var delResult = await userService.Delete("EP001");
                Assert.True(delResult);
            }
        }
        [Fact]
        public async Task GetAllUserTest()
        {
            var pmoUser = new PMOUser
            {
                EmployeeId = "EP001",
                FirstName = "F1",
                LastName = "L2",
                Id = "Usr/1"

            };
            var mockUserRepo = new Mock<IUserRepo>();
            var mockProjectTaskRepo = new Mock<IProjectTaskRepo>();
            var mockMapper = new Mock<IMapper>();
            var logger = createUserLogger();
            mockUserRepo.Setup(usrRepo => usrRepo.GetAllUser()).Returns(Task.FromResult(
                new List<PMOUser> { pmoUser }));
            var userService = new UserService(mockUserRepo.Object, mockProjectTaskRepo.Object, logger,
           mockMapper.Object);
            var result = await userService.GetAllUser();
            Assert.NotEmpty(result);
        }
        [Fact]
        public async Task GetUserByCriteriaTest()
        {
            var searchCrit = new UserSearchCriteria
            {
                EmployeeID = "EP001",
                FirstName = "F1",
                LastName = "L2"
            };
            var pmoUser = new PMOUser
            {
                EmployeeId = "EP001",
                FirstName = "F1",
                LastName = "L2",
                Id = "Usr/1"

            };
            var mockUserRepo = new Mock<IUserRepo>();
            var mockProjectTaskRepo = new Mock<IProjectTaskRepo>();
            var mockMapper = new Mock<IMapper>();
            var logger = createUserLogger();
            mockUserRepo.Setup(usrRepo => usrRepo.GetAllUserMatchAnyCriteria(It.IsAny<UserSearchCriteria>()))
                        .Returns(Task.FromResult(new List<PMOUser> { pmoUser }));
            var userService = new UserService(mockUserRepo.Object, mockProjectTaskRepo.Object, logger,
          mockMapper.Object);
            var result = await userService.GetUserByCriteria(searchCrit);
            Assert.NotEmpty(result);
        }
        [Fact]
        public async Task GetUserByEmployeeIdTest()
        {
            var pmoUser = new PMOUser
            {
                EmployeeId = "EP001",
                FirstName = "F1",
                LastName = "L2",
                Id = "Usr/1"

            };
            var mockUserRepo = new Mock<IUserRepo>();
            var mockProjectTaskRepo = new Mock<IProjectTaskRepo>();
            var mockMapper = new Mock<IMapper>();
            var logger = createUserLogger();
            mockUserRepo.Setup(usrRepo => usrRepo.GetUserByEmployeeId(It.IsAny<string>()))
                        .Returns(Task.FromResult(pmoUser));
            var userService = new UserService(mockUserRepo.Object, mockProjectTaskRepo.Object, logger,
          mockMapper.Object);
            var result = await userService.GetUserByEmployeeId("EP001");
            Assert.NotNull(result);
        }
    }
}
