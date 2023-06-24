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
    [Table("questions")]
    public class Question
    {
        [Column("id")]
        [Key]
        public int Id { get; set; }
        [Column("quiz_id")]
        [Required]
        [ForeignKey(nameof(QuizId))]
        public int QuizId { get; set; }
        [JsonIgnore]
        public Quiz? Quiz { get; set; }
        [Column("question")]
        [Required, MaxLength(1000)]
        public string QuestionText { get; set; } = string.Empty;
        [Column("choices", TypeName = "jsonb")]
        [Required]
        public List<Choice> Choices { get; set; } = new List<Choice>();
        [Column("created_at")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        [Column("updated_at")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? UpdatedAt { get; set; } = DateTime.Now;
    }

    public class Choice
    {
        [JsonPropertyName("text")]
        [Required, MaxLength(1000)]
        public string Text { get; set; } = string.Empty;
        [JsonPropertyName("isCorrect")]
        [Required]
        public bool IsCorrect { get; set; } = false;
    }
}
