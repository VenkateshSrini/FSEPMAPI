using PMO.API.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMO.API.Repository
{
    public interface IUserRepo
    {
        public Task<Tuple<bool, string>> AddUser(PMOUser pmoUser);
        public Task<Tuple<bool, string>> EditUser(PMOUser pmoUser);
        public Task<List<PMOUser>> GetAllUser();
        public Task<PMOUser> GetUserByEmployeeId(string employeeId);
    }
}
