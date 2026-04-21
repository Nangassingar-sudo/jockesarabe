using System.ComponentModel.DataAnnotations;

namespace JokesWebApp.Models
{
    public class Comment
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(500)]
        public string Text { get; set; } = string.Empty;
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        [Required]
        public string UserId { get; set; } = string.Empty;
        
        [Required]
        public int JokeId { get; set; }

        // Navigation properties
        public Joke? Joke { get; set; }
        public Microsoft.AspNetCore.Identity.IdentityUser? User { get; set; }
    }
}
