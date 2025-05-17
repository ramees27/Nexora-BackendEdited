using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Application.DTO
{
    public class CounselorAddDTO
    {
        [Required(ErrorMessage = "Full name is required.")]
        [StringLength(100, ErrorMessage = "Full name can't be longer than 100 characters.")]
        public string full_name { get; set; }

        [Required(ErrorMessage = "Specialization is required.")]
        [Display(Name = "Comma-separated specializations (e.g., Data Science,Web Development)")]
        public string specialization { get; set; } // comma-separated string

        [Required(ErrorMessage = "Short bio is required.")]
        [StringLength(1000, ErrorMessage = "Short bio can't exceed 1000 characters.")]
        public string short_bio { get; set; }

        [Required(ErrorMessage = "Mobile number is required.")]
        [Phone(ErrorMessage = "Invalid mobile number format.")]
        [StringLength(20, ErrorMessage = "Mobile number can't exceed 20 digits.")]
        public string mobile_number { get; set; }

        [Required(ErrorMessage = "Experience is required.")]
        [Range(0, 50, ErrorMessage = "Experience must be between 0 and 50 years.")]
        public int experience { get; set; }

        [Required(ErrorMessage = "Hourly rate is required.")]
        [Range(0, 10000, ErrorMessage = "Hourly rate must be a positive number.")]
        public int hourly_rate { get; set; }

        [Required(ErrorMessage = "UPI ID is required.")]
        [StringLength(100, ErrorMessage = "UPI ID can't exceed 100 characters.")]
        public string upi_id { get; set; }

        public IFormFile? ProfileImage { get; set; }

    }
}