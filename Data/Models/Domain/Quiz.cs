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
    [Table("quizzes")]
    public class Quiz
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
        [Required, MaxLength(100)]
        public string Title { get; set; } = string.Empty;
        [Column("description")]
        [Required, MaxLength(1000)]
        public string Description { get; set; } = string.Empty;
        [Column("questions", TypeName = "jsonb")]
        [InverseProperty(nameof(Question.Quiz))]
        [ForeignKey(nameof(Question.QuizId))]
        [Required]
        public List<Question> Questions { get; set; } = new List<Question>();
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
