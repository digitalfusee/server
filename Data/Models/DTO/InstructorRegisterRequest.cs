using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Data.Models.DTO
{
    public class InstructorRegisterRequest
    {
        [Required, MinLength(2), MaxLength(100), Display(Name = "First Name")]
        public string FirstName { get; set; } = string.Empty;
        [Required, MinLength(2), MaxLength(100), Display(Name = "Last Name")]
        public string LastName { get; set; } = string.Empty;
        [MaxLength(50)]
        public string Username { get; set; } = string.Empty;
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
        public string? ProfilePicture { get; set; }
        [DataType(DataType.Password)]
        [Required, MinLength(6, ErrorMessage = "Please enter at least 6 characters.")]
        public string Password { get; set; } = string.Empty;
        [Required, Compare("Password", ErrorMessage = "Passwords did not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;
        [Phone, PersonalData]
        public string PhoneNumber { get; set; } = string.Empty;
        [Required]
        public string Address { get; set; } = string.Empty;
        [Required]
        public string City { get; set; } = string.Empty;
        [Required]
        public string PostalCode { get; set; } = string.Empty;
        [Required]
        public string Province { get; set; } = string.Empty;
        public string Country { get; set; } = "Canada";
    }
}
