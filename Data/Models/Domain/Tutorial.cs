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
    [Table("tutorials")]
    public class Tutorial
    {
        [Column("id")]
        [Key]
        public int Id { get; set; }
        [Column("course_id")]
        [Required]
        [ForeignKey(nameof(CourseId))]
        public int CourseId { get; set; }
        [JsonIgnore]
        public Course? Course { get; set; }
        [Column("title")]
        [Required, MinLength(2), MaxLength(100), Display(Name = "Name")]
        public string Title { get; set; } = string.Empty;
        [Column("description")]
        [Required, MinLength(2), MaxLength(100), Display(Name = "Description")]
        public string Description { get; set; } = string.Empty;
        [Column("video_url")]
        [Required, MinLength(2), MaxLength(100), Display(Name = "VideoURL")]
        public string VideoURL { get; set; } = string.Empty;
        [Column("steps", TypeName = "jsonb")]
        [Required]
        public List<Step> Steps { get; set; } = new List<Step>();
        [Column("created_at")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        [Column("updated_at")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? UpdatedAt { get; set; } = DateTime.Now;
    }
    public class Step
    {
        [JsonPropertyName("stepNumber")]
        [Required]
        public int StepNumber { get; set; }
        [JsonPropertyName("step")]
        [Required]
        public string? StepDescription { get; set; }
    }

}
