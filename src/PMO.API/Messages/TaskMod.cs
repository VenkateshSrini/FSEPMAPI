﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PMO.API.Messages
{
    public class TaskMod
    {
        [Required]
        public string ProjectId { get; set; }
        [Required]
        public string TaskId { get; set; }
        [Required]
        [StringLength(40, ErrorMessage = "Maxlength exceeded")]
        public string TaskDescription { get; set; }
        [Range(0, 30, ErrorMessage = "Min/Max value exceeded")]
        public int Priority { get; set; } = -1;
        public string ParentTaskId { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        public string TaskOwnerId { get; set; }
    }
}
