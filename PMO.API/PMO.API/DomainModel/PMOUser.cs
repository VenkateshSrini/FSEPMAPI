using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMO.API.DomainModel
{
    public class PMOUser
    {
        public string Id { get; set; } = "Usr|";
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmployeeId { get; set; }
    }
}
