using BlazingQuiz.Api.Data.Entities;
using BlazingQuiz.Shared.Enum;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BlazingQuiz.Api.Data
{
    public class QuizContext : DbContext
    {

        private readonly IPasswordHasher<User> _passwordHasher;
        public QuizContext(DbContextOptions<QuizContext> options ,IPasswordHasher<User> passwordHasher) : base(options)
        {
            _passwordHasher = passwordHasher;
        }

        public DbSet<User> Users { get; set; }
        public DbSet<StudentQuiz> StudentQuizzes { get; set; }
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Option> Options { get; set; }

        public DbSet<Question> Questions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var adminUser = new User
            {
                Id = 1,
                Name = "Abdo Khaled",
                Email = "abdo05301601@gmail.com",
                Phone = "777192677",
                Role = nameof(UserRole.Admin),
                IsApproved = true
            };
            adminUser.PasswordHash = _passwordHasher.HashPassword(adminUser, "Ali777192677");

            modelBuilder.Entity<User>().HasData(adminUser);
        }


    }

}
