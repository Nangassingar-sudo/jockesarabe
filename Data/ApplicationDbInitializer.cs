using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using JokesWebApp.Models;

namespace JokesWebApp.Data
{
    public static class ApplicationDbInitializer
    {
        public static async Task Initialize(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            context.Database.EnsureCreated();

            // Look for any users.
            if (context.Users.Any())
            {
                return;   // DB has been seeded
            }

            // Create default user
            var defaultUser = new IdentityUser
            {
                UserName = "test@example.com",
                Email = "test@example.com",
                EmailConfirmed = true
            };
            
            await userManager.CreateAsync(defaultUser, "Password123!");

            // Add sample joke questions
            var questions = new[]
            {
                new JokeQuestion { Text = "Why did the football coach go to the bank?" },
                new JokeQuestion { Text = "What do you call a football player with a helmet?" },
                new JokeQuestion { Text = "Why don't football players ever get hot?" },
                new JokeQuestion { Text = "What's a football player's favorite type of story?" },
                new JokeQuestion { Text = "Why did the striker bring string to the match?" }
            };

            context.JokeQuestions.AddRange(questions);
            await context.SaveChangesAsync();

            // Add sample joke answers
            var answers = new[]
            {
                new JokeAnswer { Text = "To get his quarterback!" },
                new JokeAnswer { Text = "Safe head!" },
                new JokeAnswer { Text = "Because they have lots of fans!" },
                new JokeAnswer { Text = "A tall tale!" },
                new JokeAnswer { Text = "To tie up the score!" }
            };

            context.JokeAnswers.AddRange(answers);
            await context.SaveChangesAsync();

            // Create some sample jokes
            var jokes = new[]
            {
                new Joke 
                { 
                    JokeQuestionId = questions[0].Id, 
                    JokeAnswerId = answers[0].Id, 
                    UserId = defaultUser.Id, 
                    LikesCount = 5 
                },
                new Joke 
                { 
                    JokeQuestionId = questions[1].Id, 
                    JokeAnswerId = answers[1].Id, 
                    UserId = defaultUser.Id, 
                    LikesCount = 3 
                },
                new Joke 
                { 
                    JokeQuestionId = questions[2].Id, 
                    JokeAnswerId = answers[2].Id, 
                    UserId = defaultUser.Id, 
                    LikesCount = 7 
                }
            };

            context.Jokes.AddRange(jokes);
            await context.SaveChangesAsync();
        }
    }
}
