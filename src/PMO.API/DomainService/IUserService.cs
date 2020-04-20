using PMO.API.DomainModel;
using PMO.API.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMO.API.DomainService
{
    public interface IUserService
    {
        Task<Tuple<bool, string>> Add(UserAddMsg userAdd);
        Task<Tuple<bool, string>> Edit(UserModMsg userMod);
        Task<List<PMOUser>> GetAllUser();
        Task<List<PMOUser>> GetUserByCriteria(UserSearchCriteria userSearchCriteria);
        Task<PMOUser> GetUserByEmployeeId(string employeeId);
        Task<bool> Delete(string EmployeeId);


    }
}
