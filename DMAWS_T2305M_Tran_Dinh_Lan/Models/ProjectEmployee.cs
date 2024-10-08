using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DMAWS_T2305M_Tran_Dinh_Lan.Models 
{
    public class ProjectEmployee
    {
        [Required]
        public int EmployeeId { get; set; }

        [Required]
        public int ProjectId { get; set; }

        [Required]
        [StringLength(500)]
        public string Tasks { get; set; }

        public virtual Employee Employees { get; set; }
        public virtual Project Projects { get; set; }
    }
}


