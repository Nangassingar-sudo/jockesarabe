namespace JokesWebApp.Models
{
    public class Joke
    {
        public int Id { get; set; }
        public int JokeQuestionId { get; set; }
        public int JokeAnswerId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public int LikesCount { get; set; } = 0;

        // Navigation properties
        public JokeQuestion? JokeQuestion { get; set; }
        public JokeAnswer? JokeAnswer { get; set; }
        public Microsoft.AspNetCore.Identity.IdentityUser? User { get; set; }
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
