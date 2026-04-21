using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using JokesWebApp.Models;

namespace JokesWebApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<JokeQuestion> JokeQuestions { get; set; }
        public DbSet<JokeAnswer> JokeAnswers { get; set; }
        public DbSet<Joke> Jokes { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure JokeQuestion
            builder.Entity<JokeQuestion>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Text).IsRequired().HasMaxLength(500);
            });

            // Configure JokeAnswer
            builder.Entity<JokeAnswer>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Text).IsRequired().HasMaxLength(500);
            });

            // Configure Joke
            builder.Entity<Joke>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.UserId).IsRequired();
                entity.Property(e => e.LikesCount).HasDefaultValue(0);

                // Relationships
                entity.HasOne(j => j.JokeQuestion)
                      .WithMany(jq => jq.Jokes)
                      .HasForeignKey(j => j.JokeQuestionId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(j => j.JokeAnswer)
                      .WithMany(ja => ja.Jokes)
                      .HasForeignKey(j => j.JokeAnswerId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne<IdentityUser>()
                      .WithMany()
                      .HasForeignKey(j => j.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure Comment
            builder.Entity<Comment>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Text).IsRequired().HasMaxLength(500);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("datetime('now')");

                // Relationships
                entity.HasOne(c => c.Joke)
                      .WithMany(j => j.Comments)
                      .HasForeignKey(c => c.JokeId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne<IdentityUser>()
                      .WithMany()
                      .HasForeignKey(c => c.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
