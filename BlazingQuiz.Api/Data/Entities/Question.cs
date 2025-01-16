using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazingQuiz.Api.Data.Entities
{
    public class Question
    {
        public int Id { get; set; }
        public Guid QuizId { get; set; }

        [MaxLength(250)]
        public string Text { get; set; }

        [ForeignKey(nameof(QuizId))]

        public virtual Quiz Quiz { get; set; }

        public virtual ICollection<Option> Options { get; set; }


    }
}
