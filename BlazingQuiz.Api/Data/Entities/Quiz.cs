using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazingQuiz.Api.Data.Entities
{
    public class Quiz
    {
        [Key]
        public Guid Id { get; set; }

        public int CategotyId { get; set; }

        [MaxLength(30)]
        public string Name { get; set; }

        public int TimeInMinutes { get; set; }

        public int TotalQuestions { get; set; }

        public bool IsActive { get; set; }

        [ForeignKey(nameof(CategotyId))]
        public virtual Category Category { get; set; }

        public ICollection<Question> Questions { get; set; }
    }
}
