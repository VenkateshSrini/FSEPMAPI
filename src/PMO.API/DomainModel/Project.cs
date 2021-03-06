﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMO.API.DomainModel
{
    public class Project
    {
        public string Id { get; set; } = "P|";
        public string Title { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int Priority { get; set; }
        public string PMId { get; set; }
        public int Status { get; set; }
        public int MaxTaskCount { get; set; }=0;
        public List<ProjTask> ProjectTasks { get; set; } = new List<ProjTask>();
    }
}
