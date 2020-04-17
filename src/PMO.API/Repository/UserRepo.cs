using Microsoft.Extensions.Logging;
using PMO.API.DomainModel;
using PMO.API.Messages;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinqKit;

namespace PMO.API.Repository
{
    public class UserRepo : IUserRepo
    {
        private IAsyncDocumentSession asyncDocumentSession;
        private ILogger<UserRepo> logger;
        public UserRepo(IAsyncDocumentSession asyncDocumentSession,
            ILogger<UserRepo> logger)
        {
            this.asyncDocumentSession = asyncDocumentSession;
            this.logger = logger;
        }
        public async Task<Tuple<bool, string>> AddUser(PMOUser pmoUser)
        {
            
            try
            {
                await asyncDocumentSession.StoreAsync(pmoUser);
                await asyncDocumentSession.SaveChangesAsync();
                return new Tuple<bool, string>(true, pmoUser.Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteUser(PMOUser pmoUser)
        {
            var dbPmoUser = await asyncDocumentSession.Query<PMOUser>()
                                                      .Where(user => user.EmployeeId.CompareTo(pmoUser) == 0)
                                                      .FirstOrDefaultAsync();
            try
            {
                asyncDocumentSession.Delete<PMOUser>(pmoUser);
                await asyncDocumentSession.SaveChangesAsync();
                return true; 
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<Tuple<bool, string>> EditUser(PMOUser pmoUser)
        {
            var dbPmoUser = await asyncDocumentSession.Query<PMOUser>()
                                                      .Where(user => user.EmployeeId.CompareTo(pmoUser) == 0)
                                                      .FirstOrDefaultAsync();
            if (dbPmoUser == default)
                throw new ApplicationException("Provided user is not yet added in DB");
            dbPmoUser.EmployeeId = pmoUser.EmployeeId;
            dbPmoUser.FirstName = pmoUser.FirstName;
            dbPmoUser.LastName = pmoUser.LastName;
            try
            {
                await asyncDocumentSession.SaveChangesAsync();
                return new Tuple<bool, string>(true, dbPmoUser.Id);
            }
            catch(Exception ex)
            {
                throw ex;
            }

        }

        public async Task<List<PMOUser>> GetAllUser()
        {
            return await asyncDocumentSession.Query<PMOUser>().ToListAsync();
        }

        public async Task<List<PMOUser>> GetAllUserMatchAnyCriteria(UserSearchCriteria userSearchCriteria)
        {
            var predicate = PredicateBuilder.New<PMOUser>(false);
            if (!string.IsNullOrWhiteSpace(userSearchCriteria.EmployeeID))
                predicate = predicate.Or(user => user.EmployeeId.CompareTo(userSearchCriteria.EmployeeID) == 0);
            if (!string.IsNullOrWhiteSpace(userSearchCriteria.FirstName))
                predicate = predicate.Or(user => user.FirstName.CompareTo(userSearchCriteria.FirstName) == 0);
            if(!string.IsNullOrWhiteSpace(userSearchCriteria.LastName))
                predicate = predicate.Or(user => user.LastName.CompareTo(userSearchCriteria.LastName) == 0);
            return await asyncDocumentSession.Query<PMOUser>()
                                             .Where(predicate)
                                             .ToListAsync();

        }

        public async Task<PMOUser> GetUserByEmployeeId(string employeeId)
        {
            return await asyncDocumentSession.Query<PMOUser>()
                                             .Where(user => user.EmployeeId.CompareTo(employeeId) == 0)
                                             .FirstOrDefaultAsync();
        }
    }
}
