using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Identity;

namespace Data.Models.Domain
{
    [Table("instructors")]
    public class Instructor
    {
        [Column("id")]
        [Key]
        public int Id { get; set; }
        [Column("first_name")]
        [Required, MinLength(2), MaxLength(100), Display(Name = "First Name")]
        public string FirstName { get; set; } = string.Empty;
        [Column("last_name")]
        [Required, MinLength(2), MaxLength(100), Display(Name = "Last Name")]
        public string LastName { get; set; } = string.Empty;
        [Column("username")]
        [Required, MaxLength(50)]
        public string Username { get; set; } = string.Empty;
        [Column("email")]
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Column("phone_number")]
        [Required, Phone, PersonalData]
        public string PhoneNumber { get; set; } = string.Empty;
        [Column("address")]
        [Required, MaxLength(100), PersonalData]
        public string Address { get; set; } = string.Empty;
        [Column("city")]
        [Required, MaxLength(100), PersonalData]
        public string City { get; set; } = string.Empty;
        [Column("postal_code")]
        [Required, MaxLength(100), PersonalData]
        public string PostalCode { get; set; } = string.Empty;
        [Column("province")]
        [Required, MaxLength(100), PersonalData]
        public string Province { get; set; } = string.Empty;
        [Column("country")]
        [MaxLength(100), PersonalData]
        public string Country { get; set; } = "Canada";
        [Column("profile_picture")]
        public string? ProfilePicture { get; set; }
        [Column("bio")]
        [MaxLength(1000)]
        public string? Bio { get; set; }
        [Column("website")]
        public string? Website { get; set; }

        [Column("password_hash")]
        public byte[] PasswordHash { get; set; } = new byte[32];
        [Column("password_salt")]
        public byte[] PasswordSalt { get; set; } = new byte[32];
        [Column("is_instructor")]
        public bool IsInstructor { get; set; } = true;
        [Column("reset_token")]
        public string? PasswordResetToken { get; set; }
        [Column("verification_token")]
        public string? VerificationToken { get; set; }
        [Column("verified_at")]
        public DateTime? VerifiedAt { get; set; }
        [Column("reset_token_expires")]
        public DateTime? ResetTokenExpires { get; set; }
        [Column("created_at")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        [Column("updated_at")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? UpdatedAt { get; set; } = DateTime.Now;
    }
}