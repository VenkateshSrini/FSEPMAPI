﻿using Castle.Core.Logging;
using Microsoft.Extensions.Logging;
using PMO.API.Controllers;
using Xunit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Moq;
using PMO.API.DomainService;
using PMO.API.Messages;
using PMO.API.DomainModel;
using Microsoft.AspNetCore.Mvc;

namespace PMO.API.Test.Controller
{
    public class UserControllerTest
    {
        private ILogger<UserController> createUserLogger()
        {
            var loggerFactory = new LoggerFactory();
            return loggerFactory.CreateLogger<UserController>();
        }
        [Fact]
        public async Task SearchUserTest()
        {
            var mockUserService = new Mock<IUserService>();
            var pmoUser = new PMOUser
            {
                EmployeeId = "EP001",
                FirstName = "F1",
                LastName = "L2"
            };
            mockUserService.Setup(usr => usr.GetUserByCriteria(It.IsAny<UserSearchCriteria>()))
                           .Returns(Task.FromResult(new List<PMOUser> { pmoUser }));
            var userLogger = createUserLogger();
            var usrController = new UserController(mockUserService.Object, userLogger);
            var actionResult = (await usrController.SearchUser("EP001", "F1", "L2")).Result as OkObjectResult;
            Assert.NotNull(actionResult);
            var result = actionResult.Value as List<PMOUser>;
            Assert.NotEmpty(result);

        }
        [Fact]
        public async Task GetEmployeeById()
        {
            var mockUserService = new Mock<IUserService>();
            var pmoUser = new PMOUser
            {
                EmployeeId = "EP001",
                FirstName = "F1",
                LastName = "L2"
            };
            mockUserService.Setup(usr => usr.GetUserByEmployeeId(It.IsAny<string>()))
                           .Returns(Task.FromResult(pmoUser));
            var userLogger = createUserLogger();
            var usrController = new UserController(mockUserService.Object, userLogger);
            var actionResult = (await usrController.GetEmployeeById("EP001")).Result as OkObjectResult;
            Assert.NotNull(actionResult);
            var result = actionResult.Value as PMOUser;
            Assert.NotNull(result);
        }
        [Fact]
        public async Task GetAllEmployee()
        {
            var mockUserService = new Mock<IUserService>();
            var pmoUser = new PMOUser
            {
                EmployeeId = "EP001",
                FirstName = "F1",
                LastName = "L2"
            };
            var pmoList = new List<PMOUser> { pmoUser };
            mockUserService.Setup(usr => usr.GetAllUser()).Returns(Task.FromResult(pmoList));
            var userLogger = createUserLogger();
            var usrController = new UserController(mockUserService.Object, userLogger);
            var actionResult = (await usrController.GetAllEmployee()).Result as OkObjectResult;
            Assert.NotNull(actionResult);
            var result = actionResult.Value as List<PMOUser>;
            Assert.NotEmpty(result);
        }
        [Fact]
        public async Task AddUser()
        {
            var mockUserService = new Mock<IUserService>();
            var usrAddMsg = new UserAddMsg
            {
                EmployeeId = "EP001",
                FirstName = "F1",
                LastName = "L2"
            };
            
            mockUserService.Setup(usr => usr.Add(It.IsAny<UserAddMsg>())).Returns(Task.FromResult(
                new Tuple<bool, string>(true, "Usr/1")));
            var userLogger = createUserLogger();
            var usrController = new UserController(mockUserService.Object, userLogger);
            var actionResult = (await usrController.Post(usrAddMsg)).Result as CreatedResult;
            Assert.NotNull(actionResult);
            Assert.Equal(201, actionResult.StatusCode);

        }
        [Fact]
        public async Task EditUser()
        {
            var mockUserService = new Mock<IUserService>();
            var usrModMsg = new UserModMsg
            {
                EmployeeId = "EP001",
                FirstName = "F1",
                LastName = "L2",
                Id = "Usr/1"
            };
            mockUserService.Setup(usr => usr.Edit(It.IsAny<UserModMsg>()))
                           .Returns(Task.FromResult(new Tuple<bool, string>(true, "Usr/1")));
            var userLogger = createUserLogger();
            var usrController = new UserController(mockUserService.Object, userLogger);
            var actionResult = (await usrController.Put(usrModMsg)).Result as AcceptedResult;
            Assert.NotNull(actionResult);
            Assert.Equal(202, actionResult.StatusCode);
        }
        [Fact]
        public async Task DeleteTest()
        {
            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(usr => usr.Delete(It.IsAny<string>())).Returns(Task.FromResult(true));
            var userLogger = createUserLogger();
            var usrController = new UserController(mockUserService.Object, userLogger);
            var actionResult = (await usrController.Delete("EP001")).Result as NoContentResult;
            Assert.NotNull(actionResult);
            Assert.Equal(204, actionResult.StatusCode);
        }
    }
}
