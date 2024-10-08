using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DMAWS_T2305M_Tran_Dinh_Lan.Models 
{
    public class Project
    {
        public int ProjectId { get; set; } // Primary key, auto-generated

        [Required, StringLength(150, MinimumLength = 2)]
        public string ProjectName { get; set; } // Tên dự án phải dài từ 2 đến 150 ký tự

        [Required]
        public DateTime ProjectStartDate { get; set; } // Ngày bắt đầu dự án

        [CustomValidation(typeof(Project), nameof(ValidateProjectDates))]
        public DateTime? ProjectEndDate { get; set; } // Ngày kết thúc dự án, có thể null nếu dự án đang thực hiện

        // Không bắt buộc khi tạo dự án, chỉ cần khi có nhân viên tham gia
        public virtual ICollection<ProjectEmployee>? ProjectEmployees { get; set; } = new List<ProjectEmployee>(); // Khởi tạo một danh sách trống

        // Custom validation to ensure ProjectStartDate < ProjectEndDate
        public static ValidationResult ValidateProjectDates(DateTime? endDate, ValidationContext context)
        {
            var instance = context.ObjectInstance as Project;
            if (endDate.HasValue && instance.ProjectStartDate >= endDate.Value)
            {
                return new ValidationResult("ProjectStartDate must be earlier than ProjectEndDate.");
            }
            return ValidationResult.Success;
        }
    }
}
