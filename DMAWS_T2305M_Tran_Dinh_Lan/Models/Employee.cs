using System.ComponentModel.DataAnnotations;

namespace DMAWS_T2305M_Tran_Dinh_Lan.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; } // Primary key

        [Required, StringLength(150, MinimumLength = 2)]
        public string EmployeeName { get; set; } // Name: 2-150 characters

        [Required]
        [DataType(DataType.Date)]
        [CustomValidation(typeof(Employee), nameof(ValidateEmployeeDOB))]
        public DateTime EmployeeDOB { get; set; } // Employee must be over 16

        [Required]
        public string EmployeeDepartment { get; set; }

        public virtual ICollection<ProjectEmployee>? ProjectEmployees { get; set; }

        // Validation for EmployeeDOB to ensure age > 16 years
        public static ValidationResult ValidateEmployeeDOB(DateTime dob, ValidationContext context)
        {
            if (dob > DateTime.Now.AddYears(-16))
            {
                return new ValidationResult("Employee must be at least 16 years old.");
            }
            return ValidationResult.Success;
        }
    }
}