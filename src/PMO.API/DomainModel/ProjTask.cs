using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMO.API.DomainModel
{
    public class ProjTask
    {
        public string Id { get; set; } = "|";
        public string Name { get; set; }
        public string ParentTaskId { get; set; }
        public int Priority { get; set; }
        public DateTime Start { get; set; }
        public DateTime EndDate { get; set; }
        public string TaskOwnerId { get; set; }
    }
}
