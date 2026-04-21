using Microsoft.AspNetCore.Identity;
using JokesWebApp.Models;

namespace JokesWebApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<Joke> Jokes { get; set; } = new List<Joke>();
    }
}
