using System.ComponentModel.DataAnnotations;

namespace BlazingQuiz.Api.Data.Entities
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(30)]
        public string Name { get; set; }
    }
}
