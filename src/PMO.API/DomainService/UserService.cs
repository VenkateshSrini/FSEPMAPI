using Microsoft.Extensions.Logging;
using PMO.API.DomainModel;
using PMO.API.Messages;
using PMO.API.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using AutoMapper;

namespace PMO.API.DomainService
{
    public class UserService:IUserService
    {
        private readonly IUserRepo userRepo;
        private readonly IProjectTaskRepo projectTaskRepo;
        private readonly ILogger<UserService> usLogger;
        private readonly IMapper mapper;
        public UserService(IUserRepo userRepo, IProjectTaskRepo projectTaskRepo, ILogger<UserService> logger,
            IMapper mapper)
        {
            this.userRepo = userRepo;
            this.projectTaskRepo = projectTaskRepo;
            usLogger = logger;
            this.mapper = mapper;
        }

        public async Task<Tuple<bool, string>> Add(UserAddMsg userAdd)
        {
            
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(userAdd);
            var validationResults = new List<ValidationResult>();
            if (Validator.TryValidateObject(userAdd, validationContext, validationResults))
            {
                var pmoUser = mapper.Map<PMOUser>(userAdd);
                return await userRepo.AddUser(pmoUser);
            }
            return new Tuple<bool, string>(false, "");
        }

        public async Task<bool> Delete(string EmployeeId)
        {
            var pmoUser = await userRepo.GetUserByEmployeeId(EmployeeId);
            var projUserCount = await projectTaskRepo.GetProjectCountByPM(pmoUser.Id);
            if (projUserCount > 0)
                throw new ApplicationException("Cannot Delete. User asociated with one more projects");
            var tskUserCount = await projectTaskRepo.GetTaskCountByUser(pmoUser.Id);
            if (tskUserCount >0 )
                throw new ApplicationException("Cannot Delete. User asociated with one more tasks");
            return await userRepo.DeleteUser(pmoUser);


        }

        public async  Task<Tuple<bool, string>> Edit(UserModMsg userMod)
        {
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(userMod);
            var validationResults = new List<ValidationResult>();
            if (Validator.TryValidateObject(userMod, validationContext, validationResults))
            {
                var pmoUser = mapper.Map<PMOUser>(userMod);
                return await userRepo.EditUser(pmoUser);
            }
            return new Tuple<bool, string>(false, "");

        }

        public async Task<List<PMOUser>> GetAllUser()
        {
            return await userRepo.GetAllUser();
        }

        public async Task<List<PMOUser>> GetUserByCriteria(UserSearchCriteria userSearchCriteria)
        {
            return await userRepo.GetAllUserMatchAnyCriteria(userSearchCriteria);
        }

        public async Task<PMOUser> GetUserByEmployeeId(string employeeId)
        {
            return await userRepo.GetUserByEmployeeId(employeeId);
        }
    }
}
