using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models.DTO
{
    public class InstructorLoginResponse : Status
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? PostalCode { get; set; }
        public string? Province { get; set; }
        public string? Country { get; set; }
        public string? ProfilePicture { get; set; }
        public bool IsInstructor { get; set; }
        // public string Token { get; set; }
        // public string RefreshToken { get; set; }
        // public DateTime? Expiration { get; set; }
    }
}
