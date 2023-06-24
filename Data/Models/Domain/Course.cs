using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace Data.Models.Domain
{
    [Table("courses")]
    public class Course
    {
        [Column("id")]
        [Key]
        public int Id { get; set; }
        [Column("instructor_id")]
        [Required]
        [ForeignKey(nameof(InstructorId))]
        public int InstructorId { get; set; }
        [JsonIgnore]
        public Instructor? Instructor { get; set; }
        [Column("title")]
        [Required]
        public string Title { get; set; } = string.Empty;
        [Column("description")]
        [Required]
        public string Description { get; set; } = string.Empty;
        [Column("image_url")]
        [Required]
        public string ImageURL { get; set; } = string.Empty;
        [Column("platform")]
        [Required]
        public string Platform { get; set; } = string.Empty;
        [Column("category")]
        [Required]
        public string Category { get; set; } = string.Empty;

        [Column("difficulty")]
        [Required]
        public string Difficulty { get; set; } = string.Empty;
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
