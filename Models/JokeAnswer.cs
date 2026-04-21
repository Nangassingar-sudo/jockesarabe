namespace JokesWebApp.Models
{
    public class JokeAnswer
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;

        // Navigation property
        public ICollection<Joke> Jokes { get; set; } = new List<Joke>();
    }
}
